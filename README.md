# Robot Planet

## Projekti eesmärk

Robot Planet on humoorikas WPF-rakendus, mis demonstreerib pärilust, liideseid, polümorfismi, kapseldamist, valideerimist ja objektide kollektsiooni kasutamist.

Kasutaja saab roboteid lisada, valida ja eemaldada ning käivitada robotite tavalisi ja humoorikaid tegevusi.

## Lahenduse struktuur

Lahendus koosneb kahest projektist:

- `RobotPlanet.Core` – klassid, liidesed, valideerimine ja robotite tegevused.
- `RobotPlanet.WPF` – kasutajaliides, robotite nimekiri, tegevusnupud ja logi.

WPF-projekt kasutab Core-projekti. Core-projekt ei sõltu WPF-projektist.

## Käivitamine

1. Klooni repositoorium arvutisse.
2. Ava `RobotPlanet.slnx` Visual Studios.
3. Määra `RobotPlanet.WPF` käivitusprojektiks.
4. Koosta lahendus klahvikombinatsiooniga `Ctrl + Shift + B`.
5. Käivita rakendus klahviga `F5`.

## Klassihierarhia

```text
Robot
├── CleanerBot
├── ExplorerBot
└── RepairBot
```

`Robot` on abstraktne baasklass, mis sisaldab kõikide robotite ühiseid omadusi ja meetodeid.

Roboti omadused on:

- `Name` – roboti nimi ja identiteet;
- `Energy` – roboti hetkeenergia.

Kõik alamklassid kirjutavad `override` abil üle järgmised meetodid:

- `PerformAction()`;
- `CrazyAction()`.

## Liidesed

Projektis kasutatakse kolme liidest:

- `IChargeable` – robotit saab laadida;
- `IScan` – robot oskab ümbrust skaneerida;
- `IRepair` – robot oskab seadmeid parandada.

Liideste jaotus klasside vahel:

```text
CleanerBot  → IChargeable
ExplorerBot → IChargeable, IScan
RepairBot   → IChargeable, IRepair
```

`ExplorerBot` ja `RepairBot` rakendavad korraga kahte liidest.

Robotite võimeid kontrollitakse operaatoriga `is`.

## Robotite tegevused

### CleanerBot

`CleanerBot` on koristusrobot.

- `PerformAction()` koristab labori põrandat ja kasutab 10 energiapunkti.
- `CrazyAction()` käivitab koristustornaado ja kasutab 20 energiapunkti.
- Robotit saab laadida liidese `IChargeable` kaudu.

### ExplorerBot

`ExplorerBot` on uurimisrobot.

- `PerformAction()` uurib tundmatut kraatrit ja kasutab 15 energiapunkti.
- `CrazyAction()` skaneerib tundmatut eluvormi, mis osutub rösteriks, ja kasutab 25 energiapunkti.
- `Scan()` skaneerib ümbrust ja kasutab 5 energiapunkti.
- Robotit saab laadida liidese `IChargeable` kaudu.
- Robot oskab skaneerida liidese `IScan` kaudu.

### RepairBot

`RepairBot` on remondirobot.

- `PerformAction()` parandab laboriantenni ja kasutab 12 energiapunkti.
- `CrazyAction()` parandab töökorras kohvimasina, mis hakkab valmistama suppi, ja kasutab 18 energiapunkti.
- `Repair()` parandab laadimisjaama ja kasutab 8 energiapunkti.
- Robotit saab laadida liidese `IChargeable` kaudu.
- Robot oskab parandada liidese `IRepair` kaudu.

## Olek ja valideerimine

Igal robotil on nimi ja energia.

Rakenduses kehtivad järgmised reeglid:

- roboti nimi määratakse konstruktoris;
- roboti nimi ei tohi olla tühi;
- roboti nimi ei tohi koosneda ainult tühikutest;
- pärast objekti loomist ei saa nime muuta;
- energia peab olema vahemikus 0 kuni 100;
- energiat ei saa väljastpoolt otse muuta;
- tegevus käivitub ainult piisava energia korral;
- kui energiat ei ole piisavalt, jääb roboti olek muutumatuks;
- laadimise kogus peab olema suurem kui null;
- energiat ei saa laadida üle 100.

Energia ohutuks vähendamiseks kasutatakse baasklassi kaitstud meetodit `TryUseEnergy()`.

Meetod kontrollib kõigepealt, kas robotil on piisavalt energiat. Energia väheneb ainult siis, kui tegevuse käivitamine on võimalik.

## Kollektsioon ja polümorfism

Roboteid hoitakse ühises kollektsioonis:

```csharp
ObservableCollection<Robot>
```

Kuigi kollektsioon kasutab baastüüpi `Robot`, võib see sisaldada erinevaid alamklasse:

- `CleanerBot`;
- `ExplorerBot`;
- `RepairBot`.

Meetodeid kutsutakse välja baastüübi kaudu:

```csharp
selectedRobot.PerformAction();
selectedRobot.CrazyAction();
```

Tegelikult käivitub valitud roboti alamklassis `override` abil kirjutatud meetod.

WPF ei pea tegevuse käivitamiseks kontrollima, kas valitud objekt on `CleanerBot`, `ExplorerBot` või `RepairBot`. See on polümorfismi kasutamise näide.

## Liideste kontrollimine

Robotite lisavõimeid kontrollitakse operaatoriga `is`.

Näiteks skaneerimise kontroll:

```csharp
if (selectedRobot is IScan scannable)
{
    string message = scannable.Scan();
}
```

Kui robot ei rakenda vajalikku liidest, kuvatakse kasutajale arusaadav teade ja rakendus jätkab töötamist.

Sarnaselt kontrollitakse:

- `IChargeable` – laadimine;
- `IScan` – skaneerimine;
- `IRepair` – parandamine.

## WPF-kasutajaliides

Rakenduse kasutajaliides võimaldab:

- sisestada roboti nime;
- valida roboti tüübi;
- määrata roboti algenergia;
- lisada uue roboti;
- kuvada kõik robotid;
- valida roboti;
- kuvada valitud roboti andmed;
- eemaldada valitud roboti;
- käivitada tavalise tegevuse;
- käivitada `CrazyAction()` tegevuse;
- laadida robotit;
- skaneerida ümbrust;
- parandada seadet;
- vaadata roboti energiataset;
- vaadata tegevuste ja vigade logi.

Core-projekti meetodid tagastavad kasutajale arusaadavad sõnumid. WPF-projekt lisab saadud sõnumid tegevuste logisse.

## Iseseisvalt õpitud WPF-element

Projektis kasutatakse iseseisvalt õpitud WPF-elementi `ProgressBar`.

`ProgressBar` näitab valitud roboti energiataset visuaalselt vahemikus 0 kuni 100.

See element valiti, sest kasutaja näeb kiiresti, kui palju energiat robotil alles on ja kas robotil võib olla piisavalt energiat järgmise tegevuse käivitamiseks.

Pärast tegevuse käivitamist, laadimist või teise roboti valimist uuendatakse nii `ProgressBar` elementi kui ka energia tekstilist väärtust.

## Kontrollitud kasutusjuhud

### 1. Kehtiva roboti lisamine

Sisestati roboti nimi, valiti roboti tüüp ja määrati energia vahemikus 0 kuni 100.

Tulemus: robot lisati nimekirja, valiti automaatselt ja roboti andmed kuvati kasutajaliideses.

### 2. Tühja nimega roboti lisamine

Nimeväli jäeti tühjaks ja vajutati nuppu `Lisa robot`.

Tulemus: robotit ei loodud ning logis kuvati veateade.

### 3. Vigase energiaga roboti lisamine

Energiaks sisestati väärtus, mis oli väiksem kui 0 või suurem kui 100.

Tulemus: robotit ei loodud ja olemasolevate objektide olek ei muutunud.

### 4. CleanerBot tavaline tegevus

Valiti `CleanerBot` ja vajutati nuppu `Tavaline tegevus`.

Tulemus: robot koristas labori põrandat, logisse lisati tegevuse sõnum ja energia vähenes 10 punkti võrra.

### 5. CrazyAction käivitamine

Valitud robotil käivitati `CrazyAction()`.

Tulemus: käivitati konkreetse robotitüübi humoorikas tegevus, logisse lisati sõnum ja roboti energia vähenes.

### 6. CleanerBot skaneerimise katse

Valiti `CleanerBot` ja vajutati nuppu `Skaneeri`.

Tulemus: rakendus ei sulgunud ning logis kuvati teade, et valitud robot ei oska skaneerida.

### 7. ExplorerBot skaneerimine

Valiti `ExplorerBot` ja vajutati nuppu `Skaneeri`.

Tulemus: skaneerimine käivitati, logisse lisati tegevuse sõnum ja energia vähenes 5 punkti võrra.

### 8. RepairBot parandamine

Valiti `RepairBot` ja vajutati nuppu `Paranda`.

Tulemus: parandamine käivitati, logisse lisati tegevuse sõnum ja energia vähenes 8 punkti võrra.

### 9. Tegevus ebapiisava energiaga

Roboti energia vähendati tasemeni, millest ei piisanud järgmise tegevuse käivitamiseks.

Tulemus: tegevust ei käivitatud, logis kuvati arusaadav teade ja roboti energia jäi muutumatuks.

### 10. Laadimine üle maksimumi

Robotil, mille energia oli väärtuse 100 lähedal, vajutati nuppu `Laadi +10`.

Tulemus: laadimist ei tehtud, energia jäi muutumatuks ja logis kuvati teade.

### 11. Roboti eemaldamine

Valiti nimekirjast robot ja vajutati nuppu `Eemalda valitud robot`.

Tulemus: robot eemaldati kollektsioonist ning valitud roboti andmed tühjendati.

## GitHubi koostöö

Uue `GuardBot` alamklassi lisamiseks loodi GitHub Issue:

- Issue: https://github.com/ingetenar/RobotPlanet/issues/1
- Kaasüliõpilane: Oleksandr Bosiuk
- Pull request: https://github.com/ingetenar/RobotPlanet/pull/2

Kaasüliõpilase ülesanne on lisada `GuardBot` eraldi harus:

```text
feature/add-guard-bot
```

`GuardBot` peab pärima klassist `Robot`, rakendama liideseid ning kirjutama üle meetodid `PerformAction()` ja `CrazyAction()`.

Kaasüliõpilane peab tegema väikesed ja arusaadavad commit'id ning avama pull request'i põhiharusse `main`.

Pärast pull request'i avamist kontrollitakse:

- päriluse õiget kasutamist;
- liideste rakendamist;
- oleku valideerimist;
- `PerformAction()` meetodit;
- `CrazyAction()` meetodit;
- energia muutmist;
- lahenduse kompileerimist;
- rakenduse käivitamist.

Kaasüliõpilase nimi ja pull request'i link lisatakse pärast koostööülesande lõpetamist.

## AI kasutamine

Projekti tegemisel kasutati Microsoft Copilotit.

AI-d kasutati:

- ülesande nõuete analüüsimiseks;
- projekti struktuuri planeerimiseks;
- klasside ja liideste kavandamiseks;
- koodinäidete koostamiseks;
- WPF-kasutajaliidese kavandamiseks;
- valideerimisreeglite kontrollimiseks;
- koodi selgitamiseks;
- GitHubi töövoo planeerimiseks;
- README koostamiseks.

Kõik koodiosad vaadati üle ja sisestati projekti kontrollitult.

Lahendus kompileeriti Visual Studios ning rakenduse funktsioone kontrolliti käsitsi.

Kontrolliti:

- robotite lisamist;
- robotite valimist;
- robotite eemaldamist;
- tavalisi tegevusi;
- `CrazyAction()` tegevusi;
- laadimist;
- skaneerimist;
- parandamist;
- energia muutumist;
- vigast sisendit;
- tegevuste logi;
- `ProgressBar` uuendamist.

Projekti autor kontrollis loodud koodi, parandas tekkinud vead ning vastutab lõpliku lahenduse korrektsuse eest.
`
