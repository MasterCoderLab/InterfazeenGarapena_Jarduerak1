using Microsoft.Extensions.DependencyInjection;

namespace I01_05_DeskontuakDituenFaktura
{
    /// <summary>
    /// Aplikazioaren konfigurazio nagusia kudeatzen duen klasea.
    /// Aplikazioaren leiho nagusia sortzen du eta haren propietateak ezartzen ditu.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Aplikazioa hasieratzen du eta XAML fitxategian definitutako baliabideak kargatzen ditu.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Aplikazioaren leiho nagusia sortzen du eta leihoaren izenburua ezartzen du.
        /// </summary>
        /// <param name="activationState">
        /// Aplikazioa aktibatzean erabilgarri dagoen egoeraren informazioa.
        /// </param>
        /// <returns>Aplikazioaren leiho nagusia.</returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            // AppShell kontrola aplikazioaren leiho nagusiaren edukia bezala erabiltzen da.
            Window window = new Window(new AppShell());

            // Leihoaren izenburua ezartzen da.
            window.Title = "Deskontuak";

            return window;
        }
    }
}