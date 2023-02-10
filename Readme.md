#Rozwi�zanie
W celu szybkiego rozwiazania zadnaia, utowrzy�em nowy projekt dla potrzeb klasy User, w�asnego prostego mechanizmu logowania z JWT, oraz szyfrowania/deszyforawnia jednego pola danych.
Wzorce projektowe, testy, ��dniejszy kod pokaza�em na szybko w projekcie SuperDevices.
WebApi mo�na wykorzystywa� za pomoc� m.in. Swaggerem, Postmana. W celu zalogowania w us�udze Swagger, nale�y uzy� przycisku Authorize w prawym g�rnym rogu i wpisa� warto�� z tokenu z logowania w postaci "bearer warto��Tokeny"

Ad.1. Utowrzy�em w�asn� klas� User, aby nie u�ywa� klas Identity/IdentityDbContext. Co do ogranicze� do p�l/tabel dodatkowych poza email, password, creditCardNumber to jednak aby "sensownie" wykorzystywa� np. JWT/cookies to moim zdaniem nale�y rozszerzy� tabel� User o dodatkwoe pola dotycz�ce tokena(RefreshToken, TokenCreated, TokenExpires), albo dobr� praktyk� doda� dodatkow� tabel� dla token�w.

Ad.2. Doda�em akcje rejestracja, logowanie i wylogowanie w kotrolerze AuthController w projekcie SuperUsers.WebApi

Ad.3. Doda�em akcje update i get w kotrolerze CreditCardController w projekcie SuperUsers.WebApi, wykorzystuje szyfrowanie AES z projektu SuperUsers.Encryption


#Tre��
 Zadania/Zmiany:

1. Przechowywanie u�ytkownik�w we "w�asnej tabeli" (bez wykorzystania IdentityDbContext). Czyli tabela users z polami: Id, email, password, creditCardNumber (to pole szyfrowane) i bez �adnych dodatkowych kolumn oraz tabel.
2. Rejestracja to dodanie u�ytkownika do tabeli users, Logowanie to sprawdzenie czy u�ytkownik jest w tabeli i je�li tak to wystawienie mu Cookie. Wystawianie oraz usuwanie cookie (akcja wyloguj) z poziomu kontrolera "Users".
3. Odszyfrowanie numeru karty kredytowej i wy�wietlenie jej np. obok loginu po poprawnym zalogowaniu.

