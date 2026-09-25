namespace I01_07_HiruMarran;

public partial class MainPage : ContentPage
{
    // Taularen egoera gordetzeko
    private string[,] taula = new string[3, 3];

    // Uneko jokalaria
    private string jokalaria = "X";


    public MainPage()
    {
        InitializeComponent();
    }


    // Taulako laukiren bat sakatzen denean
    private void Casilla_Clicked(object? sender, EventArgs e)
    {
        ImageButton casilla = (ImageButton)sender;

        string posizioa = casilla.CommandParameter.ToString();

        string[] datuak = posizioa.Split(',');

        int fila = int.Parse(datuak[0]);
        int columna = int.Parse(datuak[1]);


        // Laukia hutsik badago bakarrik jolastu
        if (string.IsNullOrEmpty(taula[fila, columna]))
        {
            taula[fila, columna] = jokalaria;

            if (jokalaria == "X")
            {
                casilla.Source = "x.png";

                jokalaria = "O";
                TxandaIkurraLabel.Text = "⭕";
            }
            else
            {
                casilla.Source = "o.png";

                jokalaria = "X";
                TxandaIkurraLabel.Text = "❌";
            }
        }
    }


    // Partida berriro hasteko
    private void BerriroHasi_Clicked(object? sender, EventArgs e)
    {
        taula = new string[3, 3];

        jokalaria = "X";

        TxandaIkurraLabel.Text = "❌";

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
}