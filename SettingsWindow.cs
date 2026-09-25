using System.Windows;

namespace AkkDictionaryApp
{
    public partial class SettingsWindow : Window
    {
        private AppSettings _settings;
        private string _settingsPath;

        public SettingsWindow()
        {
            InitializeComponent();
            _settingsPath = AppSettings.DefaultPath;
            _settings = AppSettings.Load(_settingsPath);
            LoadSettings();
        }

        private void LoadSettings()
        {
            EnableSuggestionsCheckBox.IsChecked = _settings.EnableSuggestions;
            DefaultReverseSearchCheckBox.IsChecked = _settings.DefaultReverseSearch;
            UIFontFamilyTextBox.Text = _settings.UiFontFamily;
            FontScaleTextBox.Text = _settings.FontScale.ToString();
            RememberWindowCheckBox.IsChecked = _settings.RememberWindow;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _settings.EnableSuggestions = EnableSuggestionsCheckBox.IsChecked ?? false;
            _settings.DefaultReverseSearch = DefaultReverseSearchCheckBox.IsChecked ?? false;
            _settings.UiFontFamily = UIFontFamilyTextBox.Text;
            if (double.TryParse(FontScaleTextBox.Text, out double scale))
                _settings.FontScale = scale;
            _settings.RememberWindow = RememberWindowCheckBox.IsChecked ?? false;

            AppSettings.Save(_settings, _settingsPath);
            MessageBox.Show("Settings saved successfully.", "Success");
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
