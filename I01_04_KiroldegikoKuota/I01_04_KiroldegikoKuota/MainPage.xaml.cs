namespace I01_04_KiroldegikoKuota;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        EzarriDefektuzkoBalioak();
        EguneratuPrezioa();
    }


    // Hasierako balioak ezartzen ditu
    private void EzarriDefektuzkoBalioak()
    {
        JubilatuakRadio.IsChecked = true;

        DesgaitasunaCheck.IsChecked = false;
        FamiliaUgariaCheck.IsChecked = false;

        // Hilabetekoa
        IraupenaPicker.SelectedIndex = 1;
    }


    // Kiroldegiko kuota kalkulatzen du
    private decimal KalkulatuKuota()
    {
        decimal hilabetekoPrezioa;

        // KATEGORIA
        if (UmeakRadio.IsChecked)
        {
            hilabetekoPrezioa = 120m;
        }
        else if (FamiliaRadio.IsChecked)
        {
            hilabetekoPrezioa = 250m;
        }
        else
        {
            hilabetekoPrezioa = 198m;
        }


        // IRAUPENA
        decimal prezioa = 0m;

        switch (IraupenaPicker.SelectedIndex)
        {
            case 0:
                prezioa = hilabetekoPrezioa * 10m;
                break;

            case 1:
                prezioa = hilabetekoPrezioa;
                break;

            case 2:
                prezioa = hilabetekoPrezioa * 0.60m;
                break;

            case 3:
                prezioa = hilabetekoPrezioa * 0.35m;
                break;

            case 4:
                prezioa = hilabetekoPrezioa * 0.08m;
                break;
        }


        // DESKONTUAK
        decimal deskontua = 0m;

        if (DesgaitasunaCheck.IsChecked)
        {
            deskontua = deskontua + 0.50m;
        }

        if (FamiliaUgariaCheck.IsChecked)
        {
            deskontua = deskontua + 0.25m;
        }

        prezioa = prezioa * (1m - deskontua);

        return prezioa;
    }


    // Pantailako prezioa eguneratzen du
    private void EguneratuPrezioa()
    {
        decimal prezioa = KalkulatuKuota();

        PrezioaLabel.Text = $"{prezioa:F2} €";
    }


    // RadioButton bat aldatzean
    private void Kategoria_Changed(object? sender, CheckedChangedEventArgs e)
    {
        EguneratuPrezioa();
    }


    // CheckBox bat aldatzean
    private void Deskontua_Changed(object? sender, CheckedChangedEventArgs e)
    {
        EguneratuPrezioa();
    }


    // Iraupena aldatzean
    private void IraupenaPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        EguneratuPrezioa();
    }


    // Kalkulatu botoia sakatzean
    private async void KalkulatuButton_Clicked(object? sender, EventArgs e)
    {
        decimal prezioa = KalkulatuKuota();
        string emaitza = $"{prezioa:F2} €";

        // Prezioa bera bada, kalkulua berriro egiten dela erakusten du
        if (PrezioaLabel.Text == emaitza)
        {
            PrezioaLabel.Text = "Kalkulatzen...";

            await Task.Delay(300);
        }

        PrezioaLabel.Text = emaitza;
    }


    // Garbitu botoia sakatzean
    private void GarbituButton_Clicked(object? sender, EventArgs e)
    {
        EzarriDefektuzkoBalioak();
        EguneratuPrezioa();
    }


    // Irten botoia sakatzean
    private void IrtenButton_Clicked(object? sender, EventArgs e)
    {
        if (Window != null)
        {
            Application.Current?.CloseWindow(Window);
        }
    }
}