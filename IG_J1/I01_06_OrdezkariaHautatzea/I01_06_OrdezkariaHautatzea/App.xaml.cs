using Microsoft.Extensions.DependencyInjection;

namespace I01_06_OrdezkariaHautatzea
{
    /// <summary>
    /// Aplikazioaren konfigurazio nagusia kudeatzen duen klasea.
    /// Aplikazioaren baliabideak hasieratzen ditu eta leiho nagusia sortzen du.
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
        /// Aplikazioaren leiho nagusia sortzen du.
        /// Leihoaren edukia AppShell objektu baten bidez kargatzen da.
        /// </summary>
        /// <param name="activationState">
        /// Aplikazioa aktibatzean erabilgarri dagoen egoeraren informazioa.
        /// </param>
        /// <returns>Aplikazioaren leiho nagusia.</returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}