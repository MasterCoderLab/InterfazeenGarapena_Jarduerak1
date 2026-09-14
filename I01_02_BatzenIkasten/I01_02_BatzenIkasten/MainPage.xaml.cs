namespace I01_02_BatzenIkasten
{
    public partial class MainPage : ContentPage
    {
        // Ausazko zenbakiak sortzeko objektua
        Random rnd = new Random();

        // Bi batugaiak gordetzeko aldagaiak
        int batugaia1;
        int batugaia2;

        public MainPage()
        {
            InitializeComponent();
        }

        // "Balio berriak sortu" botoia sakatzean exekutatzen da
        private void OnBalioBerriakClicked(object? sender, EventArgs e)
        {
            // 1 eta 10 arteko ausazko zenbakiak sortu
            batugaia1 = (int)(rnd.NextDouble() * 10) + 1;
            batugaia2 = (int)(rnd.NextDouble() * 10) + 1;

            // Zenbakiak pantailan erakutsi
            LehenBatugaiaEntry.Text = batugaia1.ToString();
            BigarrenBatugaiaEntry.Text = batugaia2.ToString();

            // Aurreko erantzuna eta emaitza garbitu
            ErantzunaEntry.Text = "";
            EmaitzaEntry.Text = "";
        }

        // "Balioztatu emaitza" botoia sakatzean exekutatzen da
        private void OnBalioztatuClicked(object? sender, EventArgs e)
        {
            // Erabiltzailearen erantzuna zenbaki bihurtzen saiatu
            bool zenbakiaDa = int.TryParse(ErantzunaEntry.Text, out int erantzuna);

            // Erantzuna zenbakia ez bada
            if (!zenbakiaDa)
            {
                EmaitzaEntry.Text = "Sartu zenbaki bat";
                return;
            }

            // Batuketaren emaitza kalkulatu
            int emaitzaZuzena = batugaia1 + batugaia2;

            // Erantzuna zuzena den egiaztatu
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