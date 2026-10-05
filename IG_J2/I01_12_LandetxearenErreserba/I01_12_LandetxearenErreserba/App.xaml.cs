using Microsoft.Extensions.DependencyInjection;

namespace I01_12_LandetxearenErreserba
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