using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace AkkDictionaryApp
{
    public partial class MainWindow : Window
    {
        private readonly string _dbPath;
        private readonly string _settingsPath;
        private readonly ObservableCollection<EntryRow> _results = new();
        private bool _reverseSearch = false;
        private AppSettings _settings;

        // Debounce
        private CancellationTokenSource _searchCts = new();

        // TTS
        private readonly SpeechSynthesizer _tts = new();

        // Current detail-pane state (for copy / speak / bookmark)
        private string _currentHeadword = string.Empty;
        private string _currentPos = string.Empty;
        private readonly List<string> _currentDefinitions = new();

        public MainWindow()
        {
            _settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
            _settings = AppSettings.Load(_settingsPath);
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionary.db");

            InitializeComponent();

            ResultsList.ItemsSource = _results;
            ApplySettings();
            _ = LoadEntryCountAsync();
        }

        // ─── Settings / Theme ────────────────────────────────────────────────

        private void ApplySettings()
        {
            try
            {
                this.FontFamily = new FontFamily(string.IsNullOrWhiteSpace(_settings.UiFontFamily) ? "Myanmar Text" : _settings.UiFontFamily);
                this.FontSize = 14 * _settings.FontScale;
                ReverseSearchToggle.IsChecked = _settings.DefaultReverseSearch;
                ReverseSearchMenu.IsChecked   = _settings.DefaultReverseSearch;
                _reverseSearch = _settings.DefaultReverseSearch;

                if (_settings.RememberWindow)
                {
                    if (_settings.WindowWidth  > 200) Width  = _settings.WindowWidth.Value;
                    if (_settings.WindowHeight > 100) Height = _settings.WindowHeight.Value;
                }

                if (_settings.ListColumnWidth > 100)
                    ListColumn.Width = new GridLength(_settings.ListColumnWidth);

                // Apply theme
                ApplyTheme(_settings.DarkMode);
                DarkModeMenu.IsChecked = _settings.DarkMode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Settings] Apply failed: {ex.Message}");
            }
        }

        private void ApplyTheme(bool dark)
        {
            var dicts = Application.Current.Resources.MergedDictionaries;
            dicts.Clear();
            dicts.Add(new ResourceDictionary
            {
                Source = new Uri(dark ? "Themes/DarkTheme.xaml" : "Themes/LightTheme.xaml", UriKind.Relative)
            });
        }

        private void DarkModeMenu_Click(object sender, RoutedEventArgs e)
        {
            _settings.DarkMode = DarkModeMenu.IsChecked;
            ApplyTheme(_settings.DarkMode);
            AppSettings.Save(_settings, _settingsPath);
        }

        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow { Owner = this };
            if (win.ShowDialog() == true)
            {
                _settings = AppSettings.Load(_settingsPath);
                ApplySettings();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_settings.RememberWindow)
            {
                _settings.WindowWidth  = Width;
                _settings.WindowHeight = Height;
            }
            _settings.ListColumnWidth = ListColumn.Width.Value;
            AppSettings.Save(_settings, _settingsPath);
            _tts.Dispose();
        }

        // ─── Keyboard ────────────────────────────────────────────────────────

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.L && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SearchBox.Focus();
                SearchBox.SelectAll();
                e.Handled = true;
            }
        }

        // ─── Search (debounced) ───────────────────────────────────────────────

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                _ = LoadSuggestionsAsync(string.Empty, CancellationToken.None);
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Cancel any in-flight search
            _searchCts.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            try
            {
                await Task.Delay(300, token);
                var q = SearchBox.Text?.Trim() ?? string.Empty;
                await LoadResultsAsync(q, token);
                if (!token.IsCancellationRequested)
                    await LoadSuggestionsAsync(q, token);
            }
            catch (OperationCanceledException) { /* newer keystroke took over */ }
        }

        private async Task LoadSuggestionsAsync(string query, CancellationToken token)
        {
            if (_reverseSearch || !_settings.EnableSuggestions)
            {
                SuggestList.Visibility = Visibility.Collapsed;
                return;
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                if (_settings.SearchHistory.Count > 0)
                {
                    SuggestList.ItemsSource = new ObservableCollection<string>(_settings.SearchHistory.Take(10));
                    SuggestList.Visibility = Visibility.Visible;
                }
                else
                {
                    SuggestList.Visibility = Visibility.Collapsed;
                }
                return;
            }

            if (!File.Exists(_dbPath)) { SuggestList.Visibility = Visibility.Collapsed; return; }

            using var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = _dbPath }.ToString());
            await conn.OpenAsync(token);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT DISTINCT display_headword FROM entries
                                WHERE search_key LIKE @starts
                                ORDER BY display_headword LIMIT 10";
            cmd.Parameters.AddWithValue("@starts", Utils.SearchKey(query) + "%");
            using var rdr = await cmd.ExecuteReaderAsync(token);
            var list = new ObservableCollection<string>();
            while (await rdr.ReadAsync(token)) list.Add(rdr.GetString(0));
            SuggestList.ItemsSource  = list;
            SuggestList.Visibility   = list.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private async Task LoadResultsAsync(string query, CancellationToken token)
        {
            _results.Clear();
            ClearDetailPane();

            UpdateQueryLabel(query, 0, loading: true);
            FuzzyBorder.Visibility = Visibility.Collapsed;

            if (!File.Exists(_dbPath)) { UpdateQueryLabel(query, 0); return; }

            using var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = _dbPath }.ToString());
            await conn.OpenAsync(token);

            var cmd = conn.CreateCommand();
            if (!_reverseSearch)
            {
                cmd.CommandText = @"
WITH q AS (SELECT @q AS q)
SELECT display_headword, pos, COUNT(*) AS senses, 1 AS rank
  FROM entries, q WHERE search_key = q.q
  GROUP BY display_headword, pos
UNION ALL
SELECT display_headword, pos, COUNT(*) AS senses, 2 AS rank
  FROM entries, q WHERE search_key LIKE (q.q || '%') AND search_key <> q.q
  GROUP BY display_headword, pos
UNION ALL
SELECT display_headword, pos, COUNT(*) AS senses, 3 AS rank
  FROM entries, q WHERE search_key LIKE ('%' || q.q || '%') AND search_key NOT LIKE (q.q || '%')
  GROUP BY display_headword, pos
ORDER BY rank, display_headword
LIMIT 400;";
                cmd.Parameters.AddWithValue("@q", Utils.SearchKey(query));
            }
            else
            {
                cmd.CommandText = @"SELECT display_headword, pos, COUNT(*) AS senses
                                    FROM entries WHERE definition LIKE @mm
                                    GROUP BY display_headword, pos
                                    ORDER BY display_headword LIMIT 400";
                cmd.Parameters.AddWithValue("@mm", "%" + query + "%");
            }

            using var reader = await cmd.ExecuteReaderAsync(token);
            while (await reader.ReadAsync(token))
            {
                var hw = reader.GetString(0);
                _results.Add(new EntryRow
                {
                    DisplayHeadword = hw,
                    Pos             = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Senses          = reader.GetInt32(2),
                    IsBookmarked    = _settings.Bookmarks.Contains(hw)
                });
            }

            UpdateQueryLabel(query, _results.Count);

            // Fuzzy fallback when no results found
            if (_results.Count == 0 && !string.IsNullOrWhiteSpace(query) && !_reverseSearch)
                _ = LoadFuzzyAsync(query);
        }

        private async Task LoadFuzzyAsync(string query)
        {
            var suggestions = await Utils.FuzzySearchAsync(_dbPath, query, maxDistance: 2);
            if (suggestions.Count == 0) return;

            FuzzyText.Inlines.Clear();
            FuzzyText.Inlines.Add(new Run("Did you mean: "));
            bool first = true;
            foreach (var s in suggestions)
            {
                if (!first) FuzzyText.Inlines.Add(new Run("  ·  "));
                first = false;
                var captured = s;
                var link = new Hyperlink(new Run(s)) { Foreground = (Brush)FindResource("TextFuzzy") };
                link.Click += (_, __) => { SearchBox.Text = captured; };
                FuzzyText.Inlines.Add(link);
            }
            FuzzyBorder.Visibility = Visibility.Visible;
        }

        private void UpdateQueryLabel(string query, int count, bool loading = false)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                QueryLabelBorder.Visibility = Visibility.Collapsed;
                StatusText.Text = "Ready";
                return;
            }

            QueryLabelBorder.Visibility = Visibility.Visible;
            if (loading)
            {
                QueryLabel.Text = $"Searching for \"{query}\"…";
                StatusText.Text = "Searching…";
            }
            else
            {
                QueryLabel.Text = count == 0
                    ? $"No results for \"{query}\""
                    : $"{count} result{(count == 1 ? "" : "s")} for \"{query}\"";
                StatusText.Text = count == 0 ? $"No results" : $"{count} results";
            }
        }

        private async Task LoadEntryCountAsync()
        {
            if (!File.Exists(_dbPath)) { EntryCountText.Text = "No database"; return; }
            try
            {
                using var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = _dbPath }.ToString());
                await conn.OpenAsync();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM entries";
                var count = (long)(await cmd.ExecuteScalarAsync())!;
                EntryCountText.Text = $"{count:N0} entries";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DB] Count failed: {ex.Message}");
            }
        }

        // ─── Result selection → Detail pane ─────────────────────────────────

        private async void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ResultsList.SelectedItem is not EntryRow row) { ClearDetailPane(); return; }
            if (!File.Exists(_dbPath)) return;

            _currentHeadword = row.DisplayHeadword;
            _currentPos      = row.Pos ?? string.Empty;
            _currentDefinitions.Clear();

            // Update header
            DetailHeadword.Text = row.DisplayHeadword;
            DetailPos.Text      = string.IsNullOrEmpty(row.Pos) ? string.Empty : row.Pos;
            DetailHeader.Visibility = Visibility.Visible;

            // Bookmark button star
            UpdateBookmarkButton(row.IsBookmarked);

            // Load definitions
            using var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = _dbPath }.ToString());
            await conn.OpenAsync();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT definition FROM entries
                                WHERE display_headword = @hw
                                  AND (pos = @pos OR (@pos = '' AND (pos IS NULL OR pos = '')))
                                ORDER BY definition LIMIT 500";
            cmd.Parameters.AddWithValue("@hw",  row.DisplayHeadword);
            cmd.Parameters.AddWithValue("@pos", row.Pos ?? string.Empty);

            var items = new ObservableCollection<StackPanel>();
            int idx = 0;
            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                idx++;
                var def = rdr.GetString(0);
                _currentDefinitions.Add(def);

                var tb = new TextBlock { TextWrapping = TextWrapping.Wrap, Foreground = (Brush)FindResource("TextPrimary") };
                BuildDefinitionInlines(tb, def);

                var numBlock = new TextBlock
                {
                    Text       = idx + ". ",
                    Width      = 30,
                    Foreground = (Brush)FindResource("TextAccent"),
                    FontWeight = FontWeights.SemiBold
                };
                var sp = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2, 0, 2) };
                sp.Children.Add(numBlock);
                sp.Children.Add(tb);
                items.Add(sp);
            }
            SensesPanel.ItemsSource = items;
        }

        private void ClearDetailPane()
        {
            SensesPanel.ItemsSource = null;
            DetailHeader.Visibility = Visibility.Collapsed;
            _currentHeadword = string.Empty;
            _currentPos      = string.Empty;
            _currentDefinitions.Clear();
        }

        private void BuildDefinitionInlines(TextBlock tb, string definition)
        {
            tb.Inlines.Clear();
            var matches = Utils.FindSeeAlso(definition).ToList();
            if (matches.Count == 0) { tb.Inlines.Add(new Run(definition)); return; }

            int pos = 0;
            foreach (var m in matches)
            {
                if (m.start > pos) tb.Inlines.Add(new Run(definition.Substring(pos, m.start - pos)));
                var term = definition.Substring(m.start, m.length);
                var link = new Hyperlink(new Run(term));
                link.Click += (_, __) => { SearchBox.Text = term; };
                tb.Inlines.Add(link);
                pos = m.start + m.length;
            }
            if (pos < definition.Length) tb.Inlines.Add(new Run(definition.Substring(pos)));
        }

        // ─── Action buttons ───────────────────────────────────────────────────

        private void BookmarkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentHeadword)) return;

            bool wasBookmarked = _settings.Bookmarks.Contains(_currentHeadword);
            if (wasBookmarked)
                _settings.Bookmarks.Remove(_currentHeadword);
            else
                _settings.Bookmarks.Add(_currentHeadword);

            AppSettings.Save(_settings, _settingsPath);

            // Update star in list
            if (ResultsList.SelectedItem is EntryRow row)
                row.IsBookmarked = !wasBookmarked;

            UpdateBookmarkButton(!wasBookmarked);
            StatusText.Text = wasBookmarked ? "Bookmark removed" : "Bookmarked!";
        }

        private void UpdateBookmarkButton(bool isBookmarked)
        {
            BookmarkBtn.Content  = isBookmarked ? "★" : "☆";
            BookmarkBtn.ToolTip  = isBookmarked ? "Remove bookmark" : "Bookmark this entry";
        }

        private void SpeakBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentHeadword)) return;
            try
            {
                _tts.SpeakAsyncCancelAll();
                _tts.SpeakAsync(_currentHeadword);
                StatusText.Text = $"Speaking \"{_currentHeadword}\"…";
            }
            catch (Exception ex)
            {
                StatusText.Text = "TTS unavailable";
                System.Diagnostics.Debug.WriteLine($"[TTS] {ex.Message}");
            }
        }

        private void CopyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentHeadword)) return;

            var sb = new StringBuilder();
            sb.AppendLine(_currentHeadword);
            if (!string.IsNullOrEmpty(_currentPos)) sb.AppendLine(_currentPos);
            sb.AppendLine();
            for (int i = 0; i < _currentDefinitions.Count; i++)
                sb.AppendLine($"{i + 1}. {_currentDefinitions[i]}");

            try
            {
                Clipboard.SetText(sb.ToString());
                StatusText.Text = "Copied to clipboard!";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Copy] {ex.Message}");
            }
        }

        private async void ShowBookmarks_Click(object sender, RoutedEventArgs e)
        {
            if (_settings.Bookmarks.Count == 0)
            {
                StatusText.Text = "No bookmarks yet — star an entry to bookmark it.";
                return;
            }

            _results.Clear();
            ClearDetailPane();
            FuzzyBorder.Visibility = Visibility.Collapsed;
            QueryLabelBorder.Visibility = Visibility.Visible;
            QueryLabel.Text = $"{_settings.Bookmarks.Count} bookmark{(_settings.Bookmarks.Count == 1 ? "" : "s")}";

            if (!File.Exists(_dbPath)) return;
            using var conn = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = _dbPath }.ToString());
            await conn.OpenAsync();

            foreach (var hw in _settings.Bookmarks)
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT display_headword, pos, COUNT(*) AS senses
                                    FROM entries WHERE display_headword = @hw
                                    GROUP BY display_headword, pos LIMIT 10";
                cmd.Parameters.AddWithValue("@hw", hw);
                using var rdr = await cmd.ExecuteReaderAsync();
                while (await rdr.ReadAsync())
                {
                    _results.Add(new EntryRow
                    {
                        DisplayHeadword = rdr.GetString(0),
                        Pos             = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1),
                        Senses          = rdr.GetInt32(2),
                        IsBookmarked    = true
                    });
                }
            }
            StatusText.Text = $"Showing {_results.Count} bookmarked entries";
        }

        // ─── Suggestion list interactions ─────────────────────────────────────

        private async void SuggestList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SuggestList.SelectedItem is not string s) return;
            SearchBox.Text = s;
            SuggestList.Visibility = Visibility.Collapsed;
            _settings.AddToHistory(s);
            AppSettings.Save(_settings, _settingsPath);
        }

        private async void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var q = SearchBox.Text?.Trim() ?? string.Empty;
                SuggestList.Visibility = Visibility.Collapsed;
                if (!string.IsNullOrWhiteSpace(q))
                {
                    _settings.AddToHistory(q);
                    AppSettings.Save(_settings, _settingsPath);
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (SuggestList.Visibility == Visibility.Visible)
                {
                    SuggestList.Visibility = Visibility.Collapsed;
                    e.Handled = true;
                }
                else if (!string.IsNullOrEmpty(SearchBox.Text))
                {
                    SearchBox.Clear();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Down && SuggestList.Visibility == Visibility.Visible)
            {
                SuggestList.Focus();
                if (SuggestList.Items.Count > 0)
                    SuggestList.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        // ─── Reverse search toggle ─────────────────────────────────────────────

        private async void ReverseSearchToggle_CheckedChanged(object sender, RoutedEventArgs e)
        {
            _reverseSearch = ReverseSearchToggle.IsChecked == true;
            ReverseSearchMenu.IsChecked = _reverseSearch;
            _searchCts.Cancel();
            _searchCts = new CancellationTokenSource();
            await LoadResultsAsync(SearchBox.Text?.Trim() ?? string.Empty, _searchCts.Token);
        }

        private async void ReverseSearchMenu_Checked(object sender, RoutedEventArgs e)
        {
            _reverseSearch = true;
            ReverseSearchToggle.IsChecked = true;
            _searchCts.Cancel();
            _searchCts = new CancellationTokenSource();
            await LoadResultsAsync(SearchBox.Text?.Trim() ?? string.Empty, _searchCts.Token);
        }

        private async void ReverseSearchMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            _reverseSearch = false;
            ReverseSearchToggle.IsChecked = false;
            _searchCts.Cancel();
            _searchCts = new CancellationTokenSource();
            await LoadResultsAsync(SearchBox.Text?.Trim() ?? string.Empty, _searchCts.Token);
        }

        // ─── Misc ─────────────────────────────────────────────────────────────

        private async void Clear_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            _searchCts.Cancel();
            _searchCts = new CancellationTokenSource();
            await LoadResultsAsync(string.Empty, _searchCts.Token);
            SuggestList.Visibility = Visibility.Collapsed;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var win = new AboutWindow { Owner = this };
            win.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();

        private async void RebuildFromExcel_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx|All files (*.*)|*.*" };
            if (dlg.ShowDialog() != true) return;
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                Utils.RebuildDatabaseFromExcel(dlg.FileName, _dbPath);
                await LoadResultsAsync(SearchBox.Text?.Trim() ?? string.Empty, CancellationToken.None);
                await LoadEntryCountAsync();
                MessageBox.Show("Database rebuilt successfully.", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Rebuild failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { Mouse.OverrideCursor = null; }
        }

        private async void ImportFromSqlite_Click(object sender, RoutedEventArgs e)
        {
            var win = new SqliteImportWindow { Owner = this };
            if (win.ShowDialog() != true || string.IsNullOrWhiteSpace(win.SourcePath)) return;
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                Utils.RebuildDatabaseFromSqlite(win.SourcePath!, win.TableName!, win.HeadCol!, win.DefCol!, win.PosCol, _dbPath);
                await LoadResultsAsync(SearchBox.Text?.Trim() ?? string.Empty, CancellationToken.None);
                await LoadEntryCountAsync();
                MessageBox.Show("Imported successfully from SQLite.", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Import failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { Mouse.OverrideCursor = null; }
        }
    }

    // ─── EntryRow with INotifyPropertyChanged ─────────────────────────────────

    public sealed class EntryRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public string DisplayHeadword { get; set; } = string.Empty;
        public string Pos             { get; set; } = string.Empty;
        public int    Senses          { get; set; }

        private bool _isBookmarked;
        public bool IsBookmarked
        {
            get => _isBookmarked;
            set { _isBookmarked = value; OnChanged(nameof(IsBookmarked)); OnChanged(nameof(BookmarkStar)); }
        }

        public string BookmarkStar => _isBookmarked ? "★" : string.Empty;
    }
}
