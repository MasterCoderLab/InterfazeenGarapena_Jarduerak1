
namespace I01_14_LurpekoParkingKudeaketa
{
    /// <summary>
    /// Aplikazioa hasieratzen du eta leiho nagusia sortzen du.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Aplikazioaren XAML osagaiak hasieratzen ditu.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// AppShell erabiliz aplikazioaren leiho nagusia sortzen du.
        /// </summary>
        /// <param name="activationState">
        /// MAUIk emandako aktibazio-egoera; aplikazio honetan ez da erabiltzen.
        /// </param>
        /// <returns>AppShell duen aplikazioaren leiho nagusia.</returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
