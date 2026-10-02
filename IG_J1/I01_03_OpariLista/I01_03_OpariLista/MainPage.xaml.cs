namespace I01_03_OpariLista
{
    /// <summary>
    /// Opari baten datuak gordetzeko erabiltzen den klasea.
    /// Opari bakoitzak izen bat eta irudi baten fitxategi-izena ditu.
    /// </summary>
    public class Oparia
    {
        // Opariaren izena gordetzen du.
        public string Izena { get; set; }

        // Opariarekin lotutako irudiaren fitxategi-izena gordetzen du.
        public string Irudia { get; set; }

        /// <summary>
        /// Oparia objektu berri bat sortzen du emandako izenarekin eta irudiarekin.
        /// </summary>
        /// <param name="izena">Opariaren izena.</param>
        /// <param name="irudia">Opariaren irudiaren fitxategi-izena.</param>
        public Oparia(string izena, string irudia)
        {
            Izena = izena;
            Irudia = irudia;
        }

        /// <summary>
        /// Opariaren izena testu moduan itzultzen du.
        /// Metodo hau ListView kontrolak elementuaren izena erakusteko erabiltzen du.
        /// </summary>
        /// <returns>Opariaren izena.</returns>
        public override string ToString()
        {
            return Izena;
        }
    }

    /// <summary>
    /// Opari-zerrendaren aplikazioaren orri nagusia.
    /// Opariak erakusten ditu eta gehienez bi opari aukeratzeko aukera ematen du.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        // Aplikazioan erabilgarri dauden opari guztiak gordetzen dituen array-a.
        private Oparia[] opariak;

        // Une honetan zerrendan hautatutako oparia gordetzen du.
        private Oparia? aukeratutakoOparia;

        // Erabiltzaileak aukeratutako lehenengo oparia gordetzen du.
        private Oparia? lehenOparia;

        // Erabiltzaileak aukeratutako bigarren oparia gordetzen du.
        private Oparia? bigarrenOparia;

        /// <summary>
        /// Orri nagusia hasieratzen du, opari objektuak sortzen ditu
        /// eta opari-zerrenda ListView kontrolean erakusten du.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Aplikazioan erabiliko diren opari objektuak sortzen dira.
            opariak = new Oparia[]
            {
                new Oparia("Erlojua", "erlojua.png"),
                new Oparia("Aurikularrak", "aurikularrak.png"),
                new Oparia("Bozgorailuak", "bozgorailuak.png"),
                new Oparia("Ermangarria", "eramangarria.png"),
                new Oparia("Bizikleta elektrikoa", "bizikleta.png")
            };

            // Sortutako opariak ListView kontrolean erakusten dira.
            OpariListView.ItemsSource = opariak;
        }

        /// <summary>
        /// ListView kontrolean opari bat hautatzen denean exekutatzen da.
        /// Hautatutako opariaren izena eta irudia pantailan erakusten ditu.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen ListView kontrola.</param>
        /// <param name="e">Hautatutako elementuari buruzko datuak.</param>
        private void OpariListView_ItemSelected(object? sender, SelectedItemChangedEventArgs e)
        {
            // Elementurik hautatuta ez badago, aurreko hautaketa garbitzen da.
            if (e.SelectedItem == null)
            {
                aukeratutakoOparia = null;
                OpariImage.Source = null;
                AukeratutakoOpariaLabel.Text = "";
                return;
            }

            // Hautatutako objektua Oparia motara bihurtzen da.
            aukeratutakoOparia = (Oparia)e.SelectedItem;

            // Hautatutako opariaren izena eta irudia erakusten dira.
            AukeratutakoOpariaLabel.Text = aukeratutakoOparia.Izena;
            OpariImage.Source = aukeratutakoOparia.Irudia;
        }

        /// <summary>
        /// "Aukeratu" botoia sakatzean hautatutako oparia gordetzen du.
        /// Gehienez bi opari aukeratzeko aukera ematen du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen botoia.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void AukeratuButton_Clicked(object? sender, EventArgs e)
        {
            // Oparirik hautatu ez bada, ez da ekintzarik egiten.
            if (aukeratutakoOparia == null)
            {
                return;
            }

            // Lehenengo oparirik oraindik aukeratu ez bada, hautatutakoa gordetzen da.
            if (lehenOparia == null)
            {
                lehenOparia = aukeratutakoOparia;
                LehenOpariaLabel.Text = lehenOparia.Izena;
            }

            // Lehenengo oparia badago baina bigarrena ez badago, bigarren oparia gordetzen da.
            else if (bigarrenOparia == null)
            {
                bigarrenOparia = aukeratutakoOparia;
                BigarrenOpariaLabel.Text = bigarrenOparia.Izena;

                // Bi opari aukeratu ondoren, ezin dira opari gehiago aukeratu.
                AukeratuButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// "Ezabatu" botoia sakatzean egindako hautaketa guztiak garbitzen ditu
        /// eta aplikazioa hasierako egoerara itzultzen du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen botoia.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void EzabatuButton_Clicked(object? sender, EventArgs e)
        {
            // Aukeratutako opari guztiak berrezartzen dira.
            aukeratutakoOparia = null;
            lehenOparia = null;
            bigarrenOparia = null;

            // ListView kontrolaren hautaketa garbitzen da.
            OpariListView.SelectedItem = null;

            // Interfazeko irudia eta testuak garbitzen dira.
            OpariImage.Source = null;
            AukeratutakoOpariaLabel.Text = "";
            LehenOpariaLabel.Text = "";
            BigarrenOpariaLabel.Text = "";

            // Opariak berriro aukeratzeko botoia gaitzen da.
            AukeratuButton.IsEnabled = true;
        }

        /// <summary>
        /// "Irten" botoia sakatzean aplikazioaren uneko leihoa ixten du.
        /// </summary>
        /// <param name="sender">Gertaera sortu duen botoia.</param>
        /// <param name="e">Klik gertaeraren datuak.</param>
        private void IrtenButton_Clicked(object? sender, EventArgs e)
        {
            if (Window != null)
            {
                Application.Current?.CloseWindow(Window);
            }
        }
    }
}