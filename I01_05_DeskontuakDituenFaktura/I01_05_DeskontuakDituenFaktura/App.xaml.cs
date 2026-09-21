using Microsoft.Extensions.DependencyInjection;

namespace I01_05_DeskontuakDituenFaktura
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