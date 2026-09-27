namespace I01_07_HiruMarran;

/// <summary>
/// Hiru marran jokoaren interfaze nagusia eta jokoaren logika kudeatzen dituen klasea.
/// Jokalariaren txanda, taularen egoera, irabazleak eta emaitzak kontrolatzen ditu.
/// </summary>
public partial class MainPage : ContentPage
{
    // Taularen egoera gordetzen du.
    // Posizio bakoitzean "X", "O" edo balio hutsa egon daiteke.
    private string[,] taula = new string[3, 3];

    // Une honetan jokatzen ari den jokalaria gordetzen du.
    private string jokalaria = "X";

    // Jokalari bakoitzak irabazitako partida kopurua gordetzen du.
    private int xIrabaziak = 0;
    private int oIrabaziak = 0;


    /// <summary>
    /// Orri nagusia hasieratzen du eta XAML interfazeko osagaiak kargatzen ditu.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
    }


    /// <summary>
    /// Taulako laukiren bat sakatzen denean jokaldia kudeatzen du.
    /// Laukia hutsik dagoen egiaztatzen du, jokalariaren ikurra jartzen du,
    /// irabazlerik dagoen begiratzen du eta hurrengo txanda prestatzen du.
    /// </summary>
    /// <param name="sender">Sakatu den ImageButton objektua.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private async void Casilla_Clicked(object? sender, EventArgs e)
    {
        ImageButton casilla = (ImageButton)sender;

        // Sakatu den laukitxoaren errenkada eta zutabea lortzen dira.
        string posizioa = casilla.CommandParameter.ToString();

        string[] datuak = posizioa.Split(',');

        int fila = int.Parse(datuak[0]);
        int columna = int.Parse(datuak[1]);


        // Laukia hutsik badago bakarrik egin daiteke jokaldia.
        if (string.IsNullOrEmpty(taula[fila, columna]))
        {
            taula[fila, columna] = jokalaria;


            // Uneko jokalariaren irudia erakusten da.
            if (jokalaria == "X")
            {
                casilla.Source = "x.png";
            }
            else
            {
                casilla.Source = "o.png";
            }


            // Jokaldia egin ondoren irabazlerik dagoen egiaztatzen da.
            if (IrabazleaDago(jokalaria))
            {
                if (jokalaria == "X")
                {
                    xIrabaziak++;
                    XIrabaziLabel.Text = xIrabaziak.ToString();
                }
                else
                {
                    oIrabaziak++;
                    OIrabaziLabel.Text = oIrabaziak.ToString();
                }


                // Erabiltzaileari beste partida bat jokatu nahi duen galdetzen zaio.
                bool berriro = await DisplayAlert(
                    "Partida amaitu da",
                    jokalaria + " jokalariak irabazi du partida.\n\nBeste partidarik jolastu nahi?",
                    "Bai",
                    "Ez");


                if (berriro)
                {
                    PartidaBerria();
                }
                else
                {
                    if (Window != null)
                    {
                        Application.Current?.CloseWindow(Window);
                    }
                }

                return;
            }


            // Irabazlerik ez badago, hurrengo jokalariaren txanda prestatzen da.
            if (jokalaria == "X")
            {
                jokalaria = "O";
                TxandaIkurraLabel.Text = "⭕";
            }
            else
            {
                jokalaria = "X";
                TxandaIkurraLabel.Text = "❌";
            }
        }
    }


    /// <summary>
    /// Adierazitako jokalariak partida irabazi duen egiaztatzen du.
    /// Hiru errenkadak, hiru zutabeak eta bi diagonalak aztertzen ditu.
    /// </summary>
    /// <param name="jokalaria">Egiaztatu behar den jokalaria: X edo O.</param>
    /// <returns>
    /// true jokalariak hiru ikur lerro berean baditu;
    /// bestela false.
    /// </returns>
    private bool IrabazleaDago(string jokalaria)
    {
        // Errenkadak egiaztatu.
        for (int fila = 0; fila < 3; fila++)
        {
            if (taula[fila, 0] == jokalaria &&
                taula[fila, 1] == jokalaria &&
                taula[fila, 2] == jokalaria)
            {
                return true;
            }
        }


        // Zutabeak egiaztatu.
        for (int columna = 0; columna < 3; columna++)
        {
            if (taula[0, columna] == jokalaria &&
                taula[1, columna] == jokalaria &&
                taula[2, columna] == jokalaria)
            {
                return true;
            }
        }


        // Ezkerretik eskuinera doan diagonala egiaztatu.
        if (taula[0, 0] == jokalaria &&
            taula[1, 1] == jokalaria &&
            taula[2, 2] == jokalaria)
        {
            return true;
        }


        // Eskuinetik ezkerrera doan diagonala egiaztatu.
        if (taula[0, 2] == jokalaria &&
            taula[1, 1] == jokalaria &&
            taula[2, 0] == jokalaria)
        {
            return true;
        }


        return false;
    }


    /// <summary>
    /// Partida berri bat prestatzen du.
    /// Taula garbitzen du eta lehenengo txanda X jokalariari ematen dio.
    /// Irabazitako partida kopuruak ez ditu ezabatzen.
    /// </summary>
    private void PartidaBerria()
    {
        taula = new string[3, 3];

        jokalaria = "X";

        TxandaIkurraLabel.Text = "❌";


        // Taulako irudi guztiak garbitu.
        Casilla00.Source = null;
        Casilla01.Source = null;
        Casilla02.Source = null;

        Casilla10.Source = null;
        Casilla11.Source = null;
        Casilla12.Source = null;

        Casilla20.Source = null;
        Casilla21.Source = null;
        Casilla22.Source = null;
    }


    /// <summary>
    /// "Berriro hasi" botoia sakatzean partida berrabiarazten du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void BerriroHasi_Clicked(object? sender, EventArgs e)
    {
        PartidaBerria();
    }
}