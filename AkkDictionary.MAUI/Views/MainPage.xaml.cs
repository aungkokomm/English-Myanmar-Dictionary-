namespace AkkDictionary.MAUI.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ViewModels.MainViewModel vm)
        {
            vm.LoadCommand.Execute(null);
        }
    }
}
