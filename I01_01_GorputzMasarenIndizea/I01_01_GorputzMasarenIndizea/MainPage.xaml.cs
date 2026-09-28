namespace I01_01_GorputzMasarenIndizea
{
    /// <summary>
    /// Gorputz Masaren Indizea kalkulatzeko erabiltzen den orri nagusia.
    /// Erabiltzaileak altuera eta pisua sartzen ditu, eta aplikazioak GMK balioa kalkulatzen du.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Orri nagusia hasieratzen du eta XAML fitxategian definitutako interfazea kargatzen du.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Kalkulatu" botoia sakatzean erabiltzaileak sartutako datuak baliozkotzen ditu
        /// eta Gorputz Masaren Indizea kalkulatzen du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen kontrola.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void OnKalkulatuClicked(object? sender, EventArgs e)
        {
            // Erabiltzaileak sartutako altuera eta pisua zenbaki bihurtzen saiatzen da.
            bool altueraOndo = double.TryParse(AltueraEntry.Text, out double altueraCm);
            bool pisuaOndo = double.TryParse(PisuaEntry.Text, out double pisua);

            // Sartutako datuak zuzenak eta zero baino handiagoak direla egiaztatzen da.
            if (!altueraOndo || !pisuaOndo || altueraCm <= 0 || pisua <= 0)
            {
                GmkEntry.Text = "Datu baliogabeak";
                return;
            }

            // Altuera zentimetrotatik metrotara bihurtzen da.
            double altueraM = altueraCm / 100;

            // Gorputz Masaren Indizea kalkulatzen da:
            // pisua / (altuera * altuera).
            double gmk = pisua / (altueraM * altueraM);

            // Kalkulatutako emaitza bi dezimalekin erakusten da.
            GmkEntry.Text = gmk.ToString("F2");
        }

        /// <summary>
        /// "Irten" botoia sakatzean aplikazioa ixten du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen kontrola.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void OnIrtenClicked(object? sender, EventArgs e)
        {
            Application.Current?.Quit();
        }
    }
}