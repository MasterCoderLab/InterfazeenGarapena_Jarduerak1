using Microsoft.Extensions.DependencyInjection;

namespace I01_14_LurpekoParkingKudeaketa
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}