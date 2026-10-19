using Microsoft.Extensions.Logging;

namespace I01_13_AhateariTiroEgitea
{
    /// <summary>
    /// .NET MAUI aplikazioaren hasierako konfigurazioa kudeatzen duen klasea.
    /// Aplikazioa, letra-tipoak eta garapenerako erregistro-sistema prestatzen ditu.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// MAUI aplikazioa sortu eta beharrezko zerbitzuak konfiguratzen ditu.
        /// </summary>
        /// <returns>
        /// Konfiguratutako eta erabiltzeko prest dagoen MauiApp objektua.
        /// </returns>
        public static MauiApp CreateMauiApp()
        {
            // Aplikazioa konfiguratzeko eraikitzailea sortzen da.
            var builder = MauiApp.CreateBuilder();

            builder
                // App klasea aplikazioaren abiapuntu gisa ezartzen da.
                .UseMauiApp<App>()

                // Aplikazioan erabiliko diren letra-tipoak erregistratzen dira.
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            // Arazketa moduan erroreak eta diagnostiko-mezuak erregistratzen dira.
            builder.Logging.AddDebug();
#endif

            // Konfigurazioa amaitu eta MAUI aplikazioa sortzen da.
            return builder.Build();
        }
    }
}