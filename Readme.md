
WebApi mo¿na wykorzystywaæ za pomoc¹ m.in. Swaggerem, Postmana. W celu zalogowania w us³udze Swagger, nale¿y uzyæ przycisku Authorize w prawym górnym rogu i wpisaæ wartoœæ z tokenu z logowania w postaci "bearer wartoœæTokeny"

Ad.1. Utowrzy³em w³asn¹ klasê User, aby Identity/IdentityDbContext. Co do ograniczeñ co do pól/tabel dodatkowych poza email, password, creditCardNumber to jednak aby "sensownie" wykorzystywaæ np. JWT, cookies to moim zdaniem nale¿y rozszerzyæ tabelê User o dodatkwoe pola dotycz¹ce tokena, albo dobr¹ praktyk¹ dodaæ dodatkow¹ tabelê dla tokenów.
Ad.2. Doda³em akcje rejestracja, logowanie i wylogowanie.
 Zadania/Zmiany:

1. Przechowywanie u¿ytkowników we "w³asnej tabeli" (bez wykorzystania IdentityDbContext). Czyli tabela users z polami: Id, email, password, creditCardNumber (to pole szyfrowane) i bez ¿adnych dodatkowych kolumn oraz tabel.
2. Rejestracja to dodanie u¿ytkownika do tabeli users, Logowanie to sprawdzenie czy u¿ytkownik jest w tabeli i jeœli tak to wystawienie mu Cookie. Wystawianie oraz usuwanie cookie (akcja wyloguj) z poziomu kontrolera "Users".
3. Odszyfrowanie numeru karty kredytowej i wyœwietlenie jej np. obok loginu po poprawnym zalogowaniu.

