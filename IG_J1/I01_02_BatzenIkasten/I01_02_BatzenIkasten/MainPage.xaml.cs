namespace I01_02_BatzenIkasten
{
    /// <summary>
    /// Batuketak praktikatzeko aplikazioaren orri nagusia.
    /// Ausazko bi batugai sortzen ditu eta erabiltzailearen erantzuna egiaztatzen du.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        // Ausazko zenbakiak sortzeko erabiltzen den objektua.
        private Random rnd = new Random();

        // Uneko batuketaren bi batugaiak gordetzen dituzten aldagaiak.
        private int batugaia1;
        private int batugaia2;

        /// <summary>
        /// Orri nagusia hasieratzen du eta XAML fitxategian definitutako interfazea kargatzen du.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Balio berriak sortu" botoia sakatzean bi ausazko zenbaki sortzen ditu,
        /// pantailan erakusten ditu eta aurreko erantzuna eta emaitza garbitzen ditu.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen kontrola.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void OnBalioBerriakClicked(object? sender, EventArgs e)
        {
            // 1 eta 10 arteko ausazko zenbakiak sortzen dira.
            batugaia1 = (int)(rnd.NextDouble() * 10) + 1;
            batugaia2 = (int)(rnd.NextDouble() * 10) + 1;

            // Sortutako zenbakiak interfazeko eremuetan erakusten dira.
            LehenBatugaiaEntry.Text = batugaia1.ToString();
            BigarrenBatugaiaEntry.Text = batugaia2.ToString();

            // Aurreko erantzuna eta balioztapenaren emaitza garbitzen dira.
            ErantzunaEntry.Text = "";
            EmaitzaEntry.Text = "";
        }

        /// <summary>
        /// "Balioztatu emaitza" botoia sakatzean erabiltzailearen erantzuna egiaztatzen du.
        /// Erantzuna zenbakia den aztertzen du eta batuketaren emaitza zuzenarekin konparatzen du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen kontrola.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void OnBalioztatuClicked(object? sender, EventArgs e)
        {
            // Erabiltzaileak sartutako erantzuna zenbaki oso bihurtzen saiatzen da.
            bool zenbakiaDa = int.TryParse(ErantzunaEntry.Text, out int erantzuna);

            // Erantzuna zenbakia ez bada, errore mezua erakusten da.
            if (!zenbakiaDa)
            {
                EmaitzaEntry.Text = "Sartu zenbaki bat";
                return;
            }

            // Uneko batuketaren emaitza zuzena kalkulatzen da.
            int emaitzaZuzena = batugaia1 + batugaia2;

            // Erabiltzailearen erantzuna emaitza zuzenarekin konparatzen da.
            if (erantzuna == emaitzaZuzena)
            {
                EmaitzaEntry.Text = "Zuzena!";
            }
            else
            {
                EmaitzaEntry.Text = "Okerra";
            }
        }
    }
}