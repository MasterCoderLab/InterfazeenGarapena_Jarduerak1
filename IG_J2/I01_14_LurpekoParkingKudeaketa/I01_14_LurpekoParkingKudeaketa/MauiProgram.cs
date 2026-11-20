using Microsoft.Extensions.Logging;

namespace I01_14_LurpekoParkingKudeaketa
{
    /// <summary>
    /// .NET MAUI aplikazioa eta haren hasierako konfigurazioa prestatzen ditu.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// MAUI aplikazioa sortzen du, letra-tipoak eta
        /// garapeneko erregistro-sistema konfiguratuta.
        /// </summary>
        /// <returns>
        /// Konfiguratutako MAUI aplikazioaren instantzia.
        /// </returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
