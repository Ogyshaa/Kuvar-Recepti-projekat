# Kuvar-Recepti-projekat

Kuvar je ASP.NET MVC 5 web aplikacija za cuvanje i pregled recepata. Projekat je napravljen sa MVC arhitekturom, XML cuvanjem podataka i servisnim slojem.

## Opis aplikacije

Aplikacija predstavlja mali portal za recepte gde korisnici mogu da naprave nalog, prijave se i vode svoju zbirku recepata. Podaci se ne cuvaju u bazi, vec u XML fajlovima unutar `App_Data` foldera. Na taj nacin je ispunjen zahtev za rad sa `System.Xml.Linq` i XML servisima.

## Glavne funkcionalnosti

- registracija i prijava korisnika
- korisnicki profil i izmena sopstvenih podataka
- CRUD operacije nad receptima
- upload slike sa racunara prilikom dodavanja ili izmene recepta
- pretraga recepata po nazivu i sastojcima 
- filtriranje po kategoriji
- sortiranje liste recepata
- omiljeni recepti
- admin panel dostupan samo admin korisniku
- stampanje admin izvestaja
- eksport podataka u XML formatu
- validacija preko Data Annotations i jQuery Unobtrusive Validation

## Napredne funkcionalnosti

U projektu su uradjene sledece dodatne funkcionalnosti:

- korisnicki profil
- stampanje izvestaja za admin korisnika
- upload slike sa racunara

## Tehnologije

- ASP.NET MVC 5
- C#
- Razor Views
- XML (`System.Xml.Linq`)
- jQuery Validation
- Bootstrap

## Struktura projekta

- `Controllers` - obrada korisnickih zahteva
- `Models` - modeli i view modeli
- `Service` - rad sa XML podacima
- `Views` - prikaz stranica
- `App_Data` - XML fajlovi sa korisnicima i receptima
- `Content/Uploads` - slike recepata koje korisnici dodaju

## Glavni entitet

Glavni entitet projekta je `Recept`. Nad njim su omogucene sve CRUD operacije:

- dodavanje recepta
- prikaz detalja recepta
- izmena recepta
- brisanje recepta

## Korisnicke uloge

### Obican korisnik

Moze da:

- registruje nalog
- prijavi se
- dodaje recepte
- menja svoje recepte
- brise svoje recepte
- oznacava recepte kao omiljene
- menja podatke na profilu

### Admin korisnik

Admin korisnik ima dodatni pristup admin panelu. Moze da:

- vidi sve korisnike
- vidi sve recepte
- obrise bilo kog korisnika osim glavnog admina
- obrise bilo koji recept
- preuzme XML podatke
- otvori izvestaj za stampu

## Pokretanje projekta

1. Otvoriti `Kuvar.sln` u Visual Studio.
2. Proveriti da su NuGet paketi restore-ovani.
3. Pokrenuti projekat preko IIS Express.
4. Po otvaranju aplikacije korisnik dolazi na stranicu za prijavu.

## Demo nalozi

- korisnik: `milan` / `123`
- korisnik: `Branka` / `fejsbukovci`
- korisnik: `Vesna_CipelajojTesna` / `vesna123`
- korisnik: `vov` / `123456789`
- admin: `admin` / `admin`


## XML podaci

Podaci se cuvaju u sledecim fajlovima:

- `App_Data/korisnici.xml`
- `App_Data/recepti.xml`

Servisne klase citaju podatke iz XML fajlova, mapiraju ih u C# objekte i zatim vracaju izmene nazad u XML.

## Autor

Ognjen Radojković IV-1.

