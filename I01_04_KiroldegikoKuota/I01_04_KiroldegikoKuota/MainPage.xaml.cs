namespace I01_04_KiroldegikoKuota;

/// <summary>
/// Kiroldegiko kuota kalkulatzeko aplikazioaren orri nagusia.
/// Erabiltzailearen kategoria, iraupena eta deskontuak kontuan hartuta
/// azken prezioa kalkulatzen eta erakusten du.
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Orri nagusia hasieratzen du, XAML interfazea kargatzen du
    /// eta aplikazioaren hasierako balioak ezartzen ditu.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        EzarriDefektuzkoBalioak();
        EguneratuPrezioa();
    }

    /// <summary>
    /// Interfazeko kontrolen hasierako balio lehenetsiak ezartzen ditu.
    /// Jubilatuen kategoria eta hilabeteko iraupena aukeratzen ditu,
    /// eta deskontuak desaktibatuta uzten ditu.
    /// </summary>
    private void EzarriDefektuzkoBalioak()
    {
        JubilatuakRadio.IsChecked = true;

        DesgaitasunaCheck.IsChecked = false;
        FamiliaUgariaCheck.IsChecked = false;

        // Hilabeteko iraupena hautatzen da.
        IraupenaPicker.SelectedIndex = 1;
    }

    /// <summary>
    /// Hautatutako kategoria, iraupena eta deskontuak kontuan hartuta
    /// kiroldegiko kuotaren azken prezioa kalkulatzen du.
    /// </summary>
    /// <returns>Kalkulatutako azken kuotaren prezioa.</returns>
    private decimal KalkulatuKuota()
    {
        decimal hilabetekoPrezioa;

        // Hautatutako kategoriaren arabera hileko oinarrizko prezioa ezartzen da.
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

        // Hautatutako iraupenaren arabera hasierako prezioa kalkulatzen da.
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

        // Hautatutako baldintzen araberako deskontuak kalkulatzen dira.
        decimal deskontua = 0m;

        if (DesgaitasunaCheck.IsChecked)
        {
            deskontua = deskontua + 0.50m;
        }

        if (FamiliaUgariaCheck.IsChecked)
        {
            deskontua = deskontua + 0.25m;
        }

        // Deskontu guztiak azken prezioari aplikatzen zaizkio.
        prezioa = prezioa * (1m - deskontua);

        return prezioa;
    }

    /// <summary>
    /// Uneko aukeren arabera prezioa berriz kalkulatzen du
    /// eta interfazeko prezioaren etiketa eguneratzen du.
    /// </summary>
    private void EguneratuPrezioa()
    {
        decimal prezioa = KalkulatuKuota();

        PrezioaLabel.Text = $"{prezioa:F2} €";
    }

    /// <summary>
    /// Kategoria aldatzen denean kuotaren prezioa automatikoki eguneratzen du.
    /// </summary>
    /// <param name="sender">Aldatu den RadioButton kontrola.</param>
    /// <param name="e">Egoera-aldaketaren datuak.</param>
    private void Kategoria_Changed(object? sender, CheckedChangedEventArgs e)
    {
        EguneratuPrezioa();
    }

    /// <summary>
    /// Deskontuetako CheckBox baten egoera aldatzen denean
    /// kuotaren prezioa automatikoki eguneratzen du.
    /// </summary>
    /// <param name="sender">Aldatu den CheckBox kontrola.</param>
    /// <param name="e">Egoera-aldaketaren datuak.</param>
    private void Deskontua_Changed(object? sender, CheckedChangedEventArgs e)
    {
        EguneratuPrezioa();
    }

    /// <summary>
    /// Iraupenaren aukera aldatzen denean kuotaren prezioa
    /// automatikoki berriz kalkulatzen du.
    /// </summary>
    /// <param name="sender">Iraupena aukeratzeko Picker kontrola.</param>
    /// <param name="e">Aldaketaren gertaera-datuak.</param>
    private void IraupenaPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        EguneratuPrezioa();
    }

    /// <summary>
    /// "Kalkulatu" botoia sakatzean kuotaren prezioa berriz kalkulatzen du.
    /// Prezioa aurrekoaren berdina bada, kalkulua egiten ari dela adierazten du
    /// denbora labur batez.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private async void KalkulatuButton_Clicked(object? sender, EventArgs e)
    {
        decimal prezioa = KalkulatuKuota();
        string emaitza = $"{prezioa:F2} €";

        // Prezioa aurrekoaren berdina bada, kalkulua berriro egiten ari dela erakusten da.
        if (PrezioaLabel.Text == emaitza)
        {
            PrezioaLabel.Text = "Kalkulatzen...";

            await Task.Delay(300);
        }

        PrezioaLabel.Text = emaitza;
    }

    /// <summary>
    /// "Garbitu" botoia sakatzean interfazea hasierako egoerara itzultzen du
    /// eta prezioa berriz kalkulatzen du.
    /// </summary>
    /// <param name="sender">Gertaera sortu duen botoia.</param>
    /// <param name="e">Klik gertaeraren datuak.</param>
    private void GarbituButton_Clicked(object? sender, EventArgs e)
    {
        EzarriDefektuzkoBalioak();
        EguneratuPrezioa();
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