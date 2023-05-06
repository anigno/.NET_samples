using log4net;
using LoggingProvider;
namespace TestMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {// Must run once at app start
        new LoggingProvider.LoggingInitiator(logsPath: "d:/dev/logs", isUseConsole: true);
        ILog logger = LogManager.GetLogger("main_logger");
        logger.Info("Init app");
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}
