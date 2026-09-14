namespace I01_03_OpariLista;

public partial class MainPage : ContentPage
{
    private string? aukeratutakoOparia;
    private string? lehenOparia;
    private string? bigarrenOparia;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OpariListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            aukeratutakoOparia = null;
            OpariImage.Source = null;
            AukeratutakoOpariaLabel.Text = "";
            return;
        }

        aukeratutakoOparia = e.SelectedItem.ToString();
        AukeratutakoOpariaLabel.Text = aukeratutakoOparia;

        switch (aukeratutakoOparia)
        {
            case "Erlojua":
                OpariImage.Source = "erlojua.png";
                break;

            case "Aurikularrak":
                OpariImage.Source = "aurikularrak.png";
                break;

            case "Bozgorailuak":
                OpariImage.Source = "bozgorailuak.png";
                break;

            case "Eramangarria":
                OpariImage.Source = "eramangarria.png";
                break;

            case "Bizikleta elektrikoa":
                OpariImage.Source = "bizikleta.png";
                break;
        }
    }

    private void AukeratuButton_Clicked(object sender, EventArgs e)
    {
        if (aukeratutakoOparia == null)
        {
            return;
        }

        if (lehenOparia == null)
        {
            lehenOparia = aukeratutakoOparia;
            LehenOpariaLabel.Text = lehenOparia;
        }
        else if (bigarrenOparia == null)
        {
            bigarrenOparia = aukeratutakoOparia;
            BigarrenOpariaLabel.Text = bigarrenOparia;

            AukeratuButton.IsEnabled = false;
        }
    }

    private void EzabatuButton_Clicked(object sender, EventArgs e)
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

    private void IrtenButton_Clicked(object sender, EventArgs e)
    {
        if (Window != null)
        {
            Application.Current?.CloseWindow(Window);
        }
    }
}