namespace I01_03_OpariLista
{
    // Opari bakoitzaren datuak gordetzeko klasea
    public class Oparia
    {
        public string Izena { get; set; }
        public string Irudia { get; set; }

        public Oparia(string izena, string irudia)
        {
            Izena = izena;
            Irudia = irudia;
        }

        // ListView-ean opariaren izena erakusteko
        public override string ToString()
        {
            return Izena;
        }
    }

    public partial class MainPage : ContentPage
    {
        // Opari guztiak gordetzeko array-a
        private Oparia[] opariak;

        // Aukeratutako opariak
        private Oparia? aukeratutakoOparia;
        private Oparia? lehenOparia;
        private Oparia? bigarrenOparia;

        public MainPage()
        {
            InitializeComponent();

            // Opari objektuak sortu
            opariak = new Oparia[]
            {
                new Oparia("Erlojua", "erlojua.png"),
                new Oparia("Aurikularrak", "aurikularrak.png"),
                new Oparia("Bozgorailuak", "bozgorailuak.png"),
                new Oparia("Ermangarria", "eramangarria.png"),
                new Oparia("Bizikleta elektrikoa", "bizikleta.png")
            };

            // Opariak ListView-ean erakutsi
            OpariListView.ItemsSource = opariak;
        }

        // Listako opari bat aukeratzean
        private void OpariListView_ItemSelected(object? sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                aukeratutakoOparia = null;
                OpariImage.Source = null;
                AukeratutakoOpariaLabel.Text = "";
                return;
            }

            // Aukeratutako objektua Oparia motara bihurtu
            aukeratutakoOparia = (Oparia)e.SelectedItem;

            // Opariaren izena eta irudia erakutsi
            AukeratutakoOpariaLabel.Text = aukeratutakoOparia.Izena;
            OpariImage.Source = aukeratutakoOparia.Irudia;
        }

        // Aukeratu botoia sakatzean
        private void AukeratuButton_Clicked(object? sender, EventArgs e)
        {
            if (aukeratutakoOparia == null)
            {
                return;
            }

            // Lehen oparia
            if (lehenOparia == null)
            {
                lehenOparia = aukeratutakoOparia;
                LehenOpariaLabel.Text = lehenOparia.Izena;
            }

            // Bigarren oparia
            else if (bigarrenOparia == null)
            {
                bigarrenOparia = aukeratutakoOparia;
                BigarrenOpariaLabel.Text = bigarrenOparia.Izena;

                // Ezin dira bi opari baino gehiago aukeratu
                AukeratuButton.IsEnabled = false;
            }
        }

        // Dena garbitzeko
        private void EzabatuButton_Clicked(object? sender, EventArgs e)
        {
            aukeratutakoOparia = null;
            lehenOparia = null;
            bigarrenOparia = null;

            OpariListView.SelectedItem = null;

            OpariImage.Source = null;
            AukeratutakoOpariaLabel.Text = "";
            LehenOpariaLabel.Text = "";
            BigarrenOpariaLabel.Text = "";

            AukeratuButton.IsEnabled = true;
        }

        // Aplikaziotik irteteko
        private void IrtenButton_Clicked(object? sender, EventArgs e)
        {
            if (Window != null)
            {
                Application.Current?.CloseWindow(Window);
            }
        }
    }
}