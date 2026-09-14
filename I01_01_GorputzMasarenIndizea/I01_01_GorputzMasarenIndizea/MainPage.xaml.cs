namespace I01_01_GorputzMasarenIndizea
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Kalkulatu botoia sakatzean exekutatzen da
        private void OnKalkulatuClicked(object? sender, EventArgs e)
        {
            // Erabiltzaileak sartutako datuak zenbaki bihurtzen saiatu
            bool altueraOndo = double.TryParse(AltueraEntry.Text, out double altueraCm);
            bool pisuaOndo = double.TryParse(PisuaEntry.Text, out double pisua);

            // Datuak zuzenak direla egiaztatu
            if (!altueraOndo || !pisuaOndo || altueraCm <= 0 || pisua <= 0)
            {
                GmkEntry.Text = "Datu baliogabeak";
                return;
            }

            // Altuera zentimetrotatik metrotara bihurtu
            double altueraM = altueraCm / 100;

            // GMK kalkulatu: pisua / altuera²
            double gmk = pisua / (altueraM * altueraM);

            // Emaitza bi dezimalekin erakutsi
            GmkEntry.Text = gmk.ToString("F2");
        }

        // Irten botoia sakatzean aplikazioa itxi
        private void OnIrtenClicked(object? sender, EventArgs e)
        {
            Application.Current?.Quit();
        }
    }
}