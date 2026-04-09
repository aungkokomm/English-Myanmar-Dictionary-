using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AkkDictionary.MAUI.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    public List<string> FontFamilies { get; } = new()
    {
        "Arial",
        "Segoe UI",
        "Helvetica",
        "Times New Roman"
    };

    [ObservableProperty]
    private string selectedFont = "Segoe UI";

    [ObservableProperty]
    private double fontScale = 1.0;

    [ObservableProperty]
    private bool defaultReverseSearch = false;

    [ObservableProperty]
    private bool enableSuggestions = true;

    public string FontScaleLabel => $"Font Size: {FontScale:F1}x";

    public SettingsViewModel()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        // Load from preferences
        SelectedFont = Preferences.Get("UiFontFamily", "Segoe UI");
        FontScale = Preferences.Get("FontScale", 1.0);
        DefaultReverseSearch = Preferences.Get("DefaultReverseSearch", false);
        EnableSuggestions = Preferences.Get("EnableSuggestions", true);
    }

    [RelayCommand]
    public async Task Save()
    {
        // Save to preferences
        Preferences.Set("UiFontFamily", SelectedFont);
        Preferences.Set("FontScale", FontScale);
        Preferences.Set("DefaultReverseSearch", DefaultReverseSearch);
        Preferences.Set("EnableSuggestions", EnableSuggestions);

        await Application.Current?.MainPage?.DisplayAlert("Success", "Settings saved successfully!", "OK")!;
    }

    [RelayCommand]
    public async Task Reset()
    {
        var result = await Application.Current?.MainPage?.DisplayAlert("Reset", "Reset all settings to defaults?", "Yes", "No")!;
        if (result)
        {
            SelectedFont = "Segoe UI";
            FontScale = 1.0;
            DefaultReverseSearch = false;
            EnableSuggestions = true;
            await Save();
        }
    }
}
