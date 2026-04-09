using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AkkDictionary.MAUI.ViewModels;

public partial class AboutViewModel : ObservableObject
{
    [RelayCommand]
    public async Task OpenGitHub()
    {
        try
        {
            await Launcher.OpenAsync(new Uri("https://github.com/aungkokomm/English-Myanmar-Dictionary-"));
        }
        catch (Exception ex)
        {
            await Application.Current?.MainPage?.DisplayAlert("Error", $"Could not open GitHub: {ex.Message}", "OK")!;
        }
    }
}
