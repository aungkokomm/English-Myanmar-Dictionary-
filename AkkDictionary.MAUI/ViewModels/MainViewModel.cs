using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace AkkDictionary.MAUI.ViewModels;

public class EntryRow
{
    public string DisplayHeadword { get; set; } = string.Empty;
    public string Pos { get; set; } = string.Empty;
    public int Senses { get; set; }
}

public partial class MainViewModel : ObservableObject
{
    private readonly string _dbPath;
    
    [ObservableProperty]
    private string searchQuery = string.Empty;

    [ObservableProperty]
    private bool isReverseSearch = false;

    [ObservableProperty]
    private string selectedDefinition = string.Empty;

    public ObservableCollection<EntryRow> Results { get; } = new();

    public MainViewModel()
    {
        _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionary.db");
    }

    [RelayCommand]
    public async Task Search()
    {
        if (string.IsNullOrWhiteSpace(SearchQuery))
        {
            Results.Clear();
            return;
        }

        await SearchDictionary(SearchQuery);
    }

    [RelayCommand]
    public async Task SelectResult(EntryRow? result)
    {
        if (result == null)
            return;

        await LoadDefinition(result);
    }

    [RelayCommand]
    public void Clear()
    {
        SearchQuery = string.Empty;
        Results.Clear();
        SelectedDefinition = string.Empty;
    }

    [RelayCommand]
    public async Task Load()
    {
        // Initialize database on app load
        await Task.CompletedTask;
    }

    private async Task SearchDictionary(string query)
    {
        Results.Clear();

        if (!File.Exists(_dbPath))
            return;

        try
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            
            if (!IsReverseSearch)
            {
                cmd.CommandText = @"
SELECT display_headword, pos, COUNT(*) AS senses 
FROM entries 
WHERE search_key LIKE @query 
GROUP BY display_headword, pos 
ORDER BY display_headword 
LIMIT 100";
                cmd.Parameters.AddWithValue("@query", $"{query.ToLower()}%");
            }
            else
            {
                cmd.CommandText = @"
SELECT display_headword, pos, COUNT(*) AS senses 
FROM entries 
WHERE definition LIKE @query 
GROUP BY display_headword, pos 
ORDER BY display_headword 
LIMIT 100";
                cmd.Parameters.AddWithValue("@query", $"%{query}%");
            }

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Results.Add(new EntryRow
                {
                    DisplayHeadword = reader.GetString(0),
                    Pos = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                    Senses = reader.GetInt32(2)
                });
            }
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current?.MainPage?.DisplayAlert("Error", $"Search failed: {ex.Message}", "OK");
            });
        }
    }

    private async Task LoadDefinition(EntryRow result)
    {
        if (!File.Exists(_dbPath))
            return;

        try
        {
            using var conn = new SqliteConnection($"Data Source={_dbPath}");
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
SELECT definition 
FROM entries 
WHERE display_headword = @hw 
AND (pos = @pos OR (@pos = '' AND (pos IS NULL OR pos = ''))) 
ORDER BY definition 
LIMIT 1";
            cmd.Parameters.AddWithValue("@hw", result.DisplayHeadword);
            cmd.Parameters.AddWithValue("@pos", result.Pos ?? string.Empty);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                SelectedDefinition = reader.GetString(0);
            }
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current?.MainPage?.DisplayAlert("Error", $"Failed to load definition: {ex.Message}", "OK");
            });
        }
    }
}
