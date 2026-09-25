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
            Window window = new Window(new AppShell());

            window.Title = "Deskontuak";

            return window;
        }
    }
}