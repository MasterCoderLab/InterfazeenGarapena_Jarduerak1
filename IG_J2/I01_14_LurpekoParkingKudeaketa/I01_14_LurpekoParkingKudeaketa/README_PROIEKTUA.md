# I01_14 — Lurpeko parking baten kudeaketa

## Egitura

- `Ereduak/`: bezeroak, ibilgailuak, plazak eta aparkatzeak irudikatzen dituzten klaseak.
- `Kudeaketa/ParkingDatuBasea.cs`: lau SQLite taulak eta datuen irakurketa/idazketa.
- `Kudeaketa/ParkingKudeatzailea.cs`: iragazkiak eta aparkalekuaren negozio-arauak.
- `Kudeaketa/DatuBiltegia.cs`: datu-base hutsa lehen aldiz betetzeko adibideak.
- `Pantailak/`: XAML bistak eta C# code-behind gertaerak. Ez da MVVM erabiltzen.

## Funtzionalitateak

1. Plazak ikusi, iragazi eta okupatutako plazako ibilgailua kontsultatu.
2. Ibilgailuak matrikulaz, markaz edo jabearen izenaz bilatu; historikoa taula trinkoan ikusi.
3. Bezeroak hautatu, datuak eta ibilgailuak ikusi; ibilgailura bi aldiz sakatuta joan.
4. Matrikularen arabera sarrera proposatu eta baieztatu; ibilgailu berari bigarren aparkatze aktiborik ez esleitu.
5. Irteeraren iraupena eta zenbatekoa kalkulatu, gorde eta plaza askatu.
6. Plazak gehitu, aldatu, mantentze-lanetan jarri eta ezabatu (ez okupatuak).

## Datu-basea

`FileSystem.AppDataDirectory/parking.db3` bidean sortzen da. Lehen abioan bakarrik txertatzen dira hasierako adibideak. Datuak aplikazioa ixtean mantentzen dira; Androiden aplikazioaren datuak ezabatuz gero, berriro hasieratzen dira. Plazak ezabatzean ezabapen logikoa erabiltzen da, historikoak beren erreferentziak gorde ditzan.

## Erabakiak

- Tarifa adibidezkoa: **1,80 €/ordu**, minutuaren proportzioan eta bi hamartarrekin. Enuntziatuak ez du tarifa zehazten; `ParkingKudeatzailea.OrdukoTarifa` aldagai konstantean alda daiteke.
- Auto elektrikoentzat plaza elektrikoak lehenesten dira; auto arruntentzat auto-plazak, eta motorrentzat motor-plazak.
- Mugikortasun urriko plaza eskatzeko aukera dago sarreran. Enuntziatuak ez du egiaztapen administratiborik zehazten.
- Ez da `ViewModel` edo MVVM gehitu: gertaerak `.xaml.cs` fitxategietan kudeatzen dira.

## Probatu beharrekoak

- Sarrera bat baieztatu, `Aparkalekua` pantailan okupatuta agertzen dela egiaztatu eta bigarren sarrera baztertu.
- Irteera baieztatu, plaza aske eta historikoa eguneratuta daudela egiaztatu.
- Datuak aplikazioa ixteko eta berriro irekitzeko proba egin.
- Plaza okupatua ezabatzeko saiakera errefusatzen dela egiaztatu.
- Bezero baten ibilgailuan bi aldiz sakatu eta Ibilgailuak fitxan azaltzen dela egiaztatu.
- Android telefonoan eta tabletan bisualki probatu.

> README honek ez du emuladoreko proba funtzionalen gainditzea ziurtatzen. Konpilazioaren emaitza scriptak erakutsiko du; Androiden probak eskuz egiaztatu behar dira.
