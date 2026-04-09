using AkkDictionary.MAUI.Views;
using AkkDictionary.MAUI.ViewModels;

namespace AkkDictionary.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            })
            // Register Pages
            .ConfigureMauiHandlers(handlers =>
            {
#if DEBUG
                handlers.AddHandler(typeof(Page), new DebugPageHandler());
#endif
            })
            // Register ViewModels
            .Services
            .AddSingleton<MainPage>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<SettingsPage>()
            .AddSingleton<SettingsViewModel>()
            .AddSingleton<AboutPage>()
            .AddSingleton<AboutViewModel>()
            .AddSingleton<AppShell>();

        return builder.Build();
    }
}

#if DEBUG
public class DebugPageHandler : IPropertyMapper<IPage, DebugPageHandler>, IElementHandler
{
    public static IPropertyMapper<IPage, DebugPageHandler> Mapper =>
        new PropertyMapper<IPage, DebugPageHandler>(ViewMapper);

    public static CommandMapper<IPage, DebugPageHandler> CommandMapper =>
        new(ViewCommandMapper);

    public object? NativeView => null;
    public MauiContext? MauiContext { get; set; }

    public void SetMauiContext(MauiContext mauiContext) => MauiContext = mauiContext;
    public void UpdateValue(string property) { }
    public void Connect(IPage handler) { }
    public void Disconnect() { }
}
#endif
