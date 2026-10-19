using Microsoft.Extensions.DependencyInjection;

namespace I01_13_AhateariTiroEgitea
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