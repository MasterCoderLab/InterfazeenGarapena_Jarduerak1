using System;
using System.Collections.Generic;
using System.Text;

namespace I01_06_OrdezkariaHautatzea;

/// <summary>
/// Ikasle baten oinarrizko datuak gordetzen dituen klasea.
/// Ikaslearen izena, abizena eta interfazean erakutsiko den izen osoa kudeatzen ditu.
/// </summary>
class Ikaslea
{
    // Ikaslearen izena gordetzen duen eremu pribatua.
    private string _izena = "";

    // Ikaslearen abizena gordetzen duen eremu pribatua.
    private string _abizena = "";

    // Ikaslearen testua interfazeko zerrendetan erakusteko erabil daitekeen kolorea.
    public Color testuKolore = Colors.White;

    /// <summary>
    /// Ikaslearen izena lortzen edo ezartzen du.
    /// Izena aldatzen denean, erakusteko izen osoa ere eguneratzen da.
    /// </summary>
    public string Izena
    {
        get
        {
            return _izena;
        }
        set
        {
            _izena = value;

            // Izena eta abizena elkartuta erakusteko testua eguneratzen da.
            ErakustekoIzena = _izena + " " + _abizena;
        }
    }

    /// <summary>
    /// Ikaslearen abizena lortzen edo ezartzen du.
    /// Abizena aldatzen denean, erakusteko izen osoa ere eguneratzen da.
    /// </summary>
    public string Abizena
    {
        get
        {
            return _abizena;
        }
        set
        {
            _abizena = value;

            // Izena eta abizena elkartuta erakusteko testua eguneratzen da.
            ErakustekoIzena = _izena + " " + _abizena;
        }
    }

    /// <summary>
    /// Interfazean erakutsiko den ikaslearen izen osoa gordetzen du.
    /// </summary>
    public string? ErakustekoIzena { get; set; }
}