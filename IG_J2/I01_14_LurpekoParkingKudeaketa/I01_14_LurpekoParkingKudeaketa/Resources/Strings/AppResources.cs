using System.Globalization;
using System.Resources;

namespace I01_14_LurpekoParkingKudeaketa.Resources.Strings;

/// <summary>
/// Interfazeko testuak erabiltzailearen hizkuntzaren arabera berreskuratzen ditu.
/// Euskara erabiltzen da beste itzulpenik ez dagoenean.
/// </summary>
public static class AppResources
{
    private static readonly ResourceManager Baliabideak = new(
        "I01_14_LurpekoParkingKudeaketa.Resources.Strings.AppResources",
        typeof(AppResources).Assembly);

    /// <summary>
    /// Gako baten itzulpena ematen du, uneko interfaze-hizkuntza erabilita.
    /// </summary>
    /// <param name="gakoa">.resx fitxategiko baliabide-gakoa.</param>
    /// <returns>Itzulitako testua edo gakoa, baliabidea falta bada.</returns>
    private static string Lortu(string gakoa) =>
        Baliabideak.GetString(gakoa, CultureInfo.CurrentUICulture) ?? gakoa;

    /// <summary>Lurpeko Parkinga</summary>
    public static string AppTitle => Lortu(nameof(AppTitle));

    /// <summary>Aparkalekua</summary>
    public static string ParkingTab => Lortu(nameof(ParkingTab));

    /// <summary>Ibilgailuak</summary>
    public static string VehiclesTab => Lortu(nameof(VehiclesTab));

    /// <summary>Aparkalekuaren egoera</summary>
    public static string ParkingStatusTitle => Lortu(nameof(ParkingStatusTitle));

    /// <summary>Plazak kargatzen...</summary>
    public static string LoadingSpaces => Lortu(nameof(LoadingSpaces));

    /// <summary>Guztiak</summary>
    public static string All => Lortu(nameof(All));

    /// <summary>Solairua</summary>
    public static string Floor => Lortu(nameof(Floor));

    /// <summary>Egoera</summary>
    public static string Status => Lortu(nameof(Status));

    /// <summary>Mota</summary>
    public static string Type => Lortu(nameof(Type));

    /// <summary>Libre dauden plazak soilik</summary>
    public static string OnlyFree => Lortu(nameof(OnlyFree));

    /// <summary>Ez dago iragazki horiekin bat datorren plazarik.</summary>
    public static string NoSpaces => Lortu(nameof(NoSpaces));

    /// <summary>Plazaren xehetasunak</summary>
    public static string SpaceDetailsTitle => Lortu(nameof(SpaceDetailsTitle));

    /// <summary>Hautatu plaza bat.</summary>
    public static string SelectSpace => Lortu(nameof(SelectSpace));

    /// <summary>Ibilgailua ikusi</summary>
    public static string ShowVehicle => Lortu(nameof(ShowVehicle));

    /// <summary>Ibilgailuak</summary>
    public static string VehicleListTitle => Lortu(nameof(VehicleListTitle));

    /// <summary>Matrikula, jabea edo marka...</summary>
    public static string SearchPlaceholder => Lortu(nameof(SearchPlaceholder));

    /// <summary>Ez da ibilgailurik aurkitu.</summary>
    public static string NoVehicles => Lortu(nameof(NoVehicles));

    /// <summary>Ibilgailuaren fitxa</summary>
    public static string VehicleCardTitle => Lortu(nameof(VehicleCardTitle));

    /// <summary>Hautatu ibilgailu bat.</summary>
    public static string SelectVehicle => Lortu(nameof(SelectVehicle));

    /// <summary>Aparkatzeen historikoa</summary>
    public static string HistoryTitle => Lortu(nameof(HistoryTitle));

    /// <summary>Ez dago aparkatze-erregistrorik.</summary>
    public static string NoHistory => Lortu(nameof(NoHistory));

    /// <summary>{0} plaza erakusten</summary>
    public static string SpaceCountFormat => Lortu(nameof(SpaceCountFormat));

    /// <summary>Solairua: {0}</summary>
    public static string FloorFormat => Lortu(nameof(FloorFormat));

    /// <summary>{0} · {1}. solairua · {2} · {3}</summary>
    public static string SpaceDetailsFormat => Lortu(nameof(SpaceDetailsFormat));

    /// <summary>Ez dago ibilgailurik plaza honetan.</summary>
    public static string NoVehicleInSpace => Lortu(nameof(NoVehicleInSpace));

    /// <summary>{0} · {1} {2} · {3}</summary>
    public static string VehicleInSpaceFormat => Lortu(nameof(VehicleInSpaceFormat));

    /// <summary>Ibilgailuaren datuak erakusteko formatu-testua.</summary>
    public static string VehicleDetailsFormat => Lortu(nameof(VehicleDetailsFormat));

    /// <summary>Aparkatze-historia erakusteko formatu-testua.</summary>
    public static string HistoryEntryFormat => Lortu(nameof(HistoryEntryFormat));

    /// <summary>Aktibo</summary>
    public static string Active => Lortu(nameof(Active));

    /// <summary>Libre</summary>
    public static string Available => Lortu(nameof(Available));

    /// <summary>Okupatuta</summary>
    public static string Occupied => Lortu(nameof(Occupied));

    /// <summary>Erreserbatuta</summary>
    public static string Reserved => Lortu(nameof(Reserved));

    /// <summary>Mantentze-lanetan</summary>
    public static string Maintenance => Lortu(nameof(Maintenance));

    /// <summary>Autoa</summary>
    public static string Car => Lortu(nameof(Car));

    /// <summary>Motorra</summary>
    public static string Motorcycle => Lortu(nameof(Motorcycle));

    /// <summary>Mugikortasun urrikoa</summary>
    public static string Accessible => Lortu(nameof(Accessible));

    /// <summary>Elektrikoa</summary>
    public static string Electric => Lortu(nameof(Electric));

}
