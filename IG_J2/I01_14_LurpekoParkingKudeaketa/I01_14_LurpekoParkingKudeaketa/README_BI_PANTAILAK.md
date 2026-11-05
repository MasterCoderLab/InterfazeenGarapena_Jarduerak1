# I01_14 · Lurpeko parking baten kudeaketa

## Biharko erakustaldia (lehenengo bi pantailak)

- **Aparkalekua:** 12 plaza adibidezkoak, solairu/egoera/mota iragazkiak, plaza libreak soilik, hautatutako plazaren xehetasunak eta ibilgailura nabigazioa.
- **Ibilgailuak:** 4 ibilgailu adibidezkoak, matrikula/marka/jabe bilaketa, ibilgailuaren fitxa eta aparkatze-historia.
- **Preferences:** aparkalekuko iragazkien aukerak gailu berean gordetzen dira.
- **Lokalizazioa:** `Resources/Strings/AppResources.resx` (euskara) eta `AppResources.es.resx` (gaztelania).
- **DeviceInfo:** telefonoetan iragazkiak zutabe batean eta tabletetan hiru zutabetan antolatzen dira; leiho estuetan ere egokitzen da.

## Arkitektura

- `Ereduak/`: bezeroa, ibilgailua, plaza, aparkatzea eta enumerazioak.
- `Kudeaketa/`: `DatuBiltegia` (adibidezko datuak memorian) eta `ParkingKudeatzailea` (bilaketak eta iragazkiak).
- `Pantailak/`: XAML orriak eta haien aurkezpen-ekitaldiak.

## Oraindik egin gabe

`BezeroakPage`, `SarreraIrteeraPage` eta `AdministrazioaPage` ez daude amaituta. Ez dago SQLite datu-base iraunkorrik, ezta benetako sarrera/irteera edo ordainketa-eragiketarik ere.

Ez dugu sentsore edo baimen berezirik eskatzen, bi pantaila hauek ez baitituzte behar.

## Egiaztapena

.NET 10 + Android SDK + JDK 21 erabilita konpilatu eta `Pixel_Tablet`-en pantaila eta nabigazioa eskuz probatu.

> Fitxategi hau garapen-fasearen azalpena da; ez da proba automatikoen egiaztagiria.
