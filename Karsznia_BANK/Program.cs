using System;
using System.Linq;
using System.Collections.Generic;

namespace Karsznia_BANK

{
    class Program
    {
        public class Bank
        {
            public static List<Account> Lista_Kont = new List<Account>();
            public static Account Zalogowany_User { set; get; }
            public static int Utworzone_Konta { set; get; } = 0;
            public static int Aktywa_Banku { get; set; } = 1000;
            public static bool Czy_Zalogowany { get; set; } = false;
            public static string[,] Admin_Menu = { { "Dodaj Usera", "Dodaj_Usera" }, { "Zablokuj_Usera", "Zablokuj_Konto" }, { "Historia_Konta_Usera", "Historia_Innych_Kont" }, { "Lista_Wierzycieli", "Lista_Wierzycieli" }, { "Statystyki", "Podsumowanie_Bankowe" }, { "Historia_Konta", "Historia_Konta" }, { "Log_Out", "Log_Out" } };
            public static string[,] Menu_Usera = { { "Stan_konta", "Szczegoly_Konta" }, { "Wpłać_środki", "Wplata" }, { "Wypłać_środki", "Wyplata" }, { "Zaciagnij_Kredyt", "Kredyt" }, { "Splac_Kredyt", "Spłac_Kredyt" }, { "Wykonaj_Przelew", "Przelew" }, { "Historia_Konta", "Historia_Konta" }, { "Podsumowanie", "Podsumowanie" }, { "Log_Out", "Log_Out" } };
            public static string[,] Panel_do_Logowania = { { "Zaloguj sie", "null" }, { "Zamknij program", "null" } };
            public static bool Czy_User_Moze_Sie_Zalogowac(string LogIn, string Haslo)
            {
                bool Status_OK = false;
                foreach (Account User in Lista_Kont)
                {
                    if (LogIn == User.Nazwa_Usera)
                    {
                        if (Haslo == User.Haslo)
                        {
                            Console.Clear();
                            if (User.Czy_Konto_Jest_Aktywne)
                            {
                                Console.WriteLine($"Zalogowano do konta {User.Nazwa_Usera}.\n");
                                Status_OK = true;
                                Czy_Zalogowany = true;
                                Zalogowany_User = Lista_Kont[User.ID_Konta - 1];
                                User.Historia += $"\n[{User.Nazwa_Usera} {User.Nazwisko}] Poprawnie zalogowano do konta";
                            }
                            else
                            {
                                Console.WriteLine($"Dostęp do konta {User.Nazwa_Usera} jest zablokowany! Logowanie niemożliwe!\n");
                                User.Historia += $"\n[{User.Nazwa_Usera} {User.Nazwisko}] Próba zalogowania do zablokowanego konta.";
                                Status_OK = false;
                            }
                        }
                        else
                        {
                            User.Historia += $"\n[{User.Nazwa_Usera} {User.Nazwisko}] Próba zalogowania przy użyciu złego hasła.";
                        }
                    }
                }
                return Status_OK;
            }
            public static bool Czy_Jest_Takie_Konto(int Konto_Numer)
            {
                return (Lista_Kont.ElementAtOrDefault(Konto_Numer) != null ? true : false);
            }
        }
        public class Funkcje_Admina : Bank
        {
            public void Dodaj_Usera()
            {
                Console.Write("Podaj nazwę (imię) użytkownika: ");
                string LogIn = Console.ReadLine();
                Console.Write("Podaj nazwisko uzytkownika: ");
                string Nazwisko = Console.ReadLine();
                Console.Write("Podaj haslo uzytkownika: ");
                string Haslo = Console.ReadLine();
                Utworz_Usera(LogIn, Nazwisko, false, Haslo);
            }
            public void Log_Out()
            {
                Bank.Czy_Zalogowany = false;
                Uruchom_Bankowosc();
            }
            public static int Wez_Wszystkie_Aktywa()
            {
                return Aktywa_Banku;
            }
            public void Dezaktywuj_Konto(int ID_Usera)
            {
                Bank.Lista_Kont[ID_Usera].Czy_Konto_Jest_Aktywne = !Bank.Lista_Kont[ID_Usera].Czy_Konto_Jest_Aktywne;
                Bank.Lista_Kont[ID_Usera].Historia += $"\n[{Bank.Lista_Kont[ID_Usera].Nazwa_Usera} {Bank.Lista_Kont[ID_Usera].Nazwisko}] Status konta ustawiono na {Bank.Lista_Kont[ID_Usera].Czy_Konto_Jest_Aktywne}";
            }
            public void Lista_Wierzycieli()
            {
                Console.WriteLine("Lista wierzycieli: ");
                foreach (Account User in Bank.Lista_Kont)
                {
                    if (User.Czy_Ma_Kredyt)
                    {
                        Console.Write($"{User.Nazwa_Usera} {User.Nazwisko} - Kwota kredytu {User.Wysokosc_Kredytu}zl\n");
                    }
                }
            }
            public void Podsumowanie_Bankowe()
            {
                int Suma_Zadłużenia_Userow = 0;
                foreach (Account User in Bank.Lista_Kont)
                {
                    if (User.Czy_Ma_Kredyt)
                    {
                        Suma_Zadłużenia_Userow += User.Wysokosc_Kredytu;
                    }
                }
                Console.WriteLine($"\tŁącznie w banku jest {Wez_Wszystkie_Aktywa()}zl\n\tŁączna wartość kredytu wynosi {Suma_Zadłużenia_Userow}zl\n\tŁącznie mamy {Bank.Utworzone_Konta} użytkowników");
            }
            public void Zablokuj_Konto()
            {
                foreach (Account User in Bank.Lista_Kont)
                {
                    Console.WriteLine($"{User.ID_Konta - 1}. {User.Nazwa_Usera} [Konto {(User.Czy_Konto_Jest_Aktywne ? "aktywne" : "nieaktywne")}]");
                }
                Console.Write("Podaj numer konta do zablokowania/odblokowania: ");
                int ID_Usera = Convert.ToInt32(Console.ReadLine());
                if (Czy_Jest_Takie_Konto(ID_Usera))
                {
                    bool t = (Bank.Lista_Kont[ID_Usera].Czy_Konto_Jest_Aktywne = !Bank.Lista_Kont[ID_Usera].Czy_Konto_Jest_Aktywne);
                    Console.WriteLine($"Konto o ID {ID_Usera} zostało {(t ? "odblokowane" : "zablokowane")}");
                }
                else
                {
                    Console.WriteLine($"Konto o ID {ID_Usera} nie istnieje w naszej bazie!");
                }
            }
            public void Historia_Innych_Kont()
            {
                foreach (Account User in Bank.Lista_Kont)
                {
                    Console.WriteLine($"{User.ID_Konta - 1}. {User.Nazwa_Usera} [Konto {(User.Czy_Konto_Jest_Aktywne ? "aktywne" : "nieaktywne")}]");
                }
                Console.Write("Podaj numer konta do przejrzenia: ");
                int ID_Usera = Convert.ToInt32(Console.ReadLine());
                if (Czy_Jest_Takie_Konto(ID_Usera))
                {
                    Console.Write(Bank.Lista_Kont[ID_Usera].Historia + "\n");
                }
                else
                {
                    Console.WriteLine($"Konto o ID {ID_Usera} nie istnieje w naszej bazie!");
                }
            }
            public void Historia_Konta()
            {
                Console.WriteLine($"Historia konta {Bank.Zalogowany_User.Nazwa_Usera} o ID {Bank.Zalogowany_User.ID_Konta - 1}: ");
                Console.WriteLine(Bank.Zalogowany_User.Historia);
            }
        }
        public class Account
        {
            public int ID_Konta { get; set; }
            public string Nazwa_Usera { get; set; }
            public string Nazwisko { get; set; }
            public string Haslo { get; set; }
            public bool Czy_Konto_Jest_Aktywne { get; set; }
            public bool Czy_Ma_Uprawnienia_Admin { get; set; }
            public int Saldo { get; set; }
            public int Wszystkie_Dokonane_Wpłaty { get; set; }
            public int Wszystkie_Dokonane_Wypłaty { get; set; }
            public bool Czy_Ma_Kredyt { get; set; }
            public int Wysokosc_Kredytu { get; set; }
            public string Historia { get; set; }
            public Account(string Pierwsze_Imie, string Nazwisko, bool Czy_ma_uprawnienia_admina, string Haslo_Usera)
            {
                ID_Konta = Bank.Utworzone_Konta + 1;
                Bank.Utworzone_Konta++;
                Nazwa_Usera = Pierwsze_Imie;
                Nazwisko = Nazwisko;
                Haslo = Haslo_Usera;
                Czy_Konto_Jest_Aktywne = true;
                Czy_Ma_Uprawnienia_Admin = Czy_ma_uprawnienia_admina;
                Saldo = 0;
                Czy_Ma_Kredyt = false;
                Wysokosc_Kredytu = 0;
                Historia += $"\n[{Pierwsze_Imie} {Nazwisko}] Użytkownik został stworzony.";
            }
            public void Calkowita_Splata_Zadluzenia()
            {
                if (Saldo > Wysokosc_Kredytu)
                {
                    Saldo = Saldo - Wysokosc_Kredytu;
                    Czy_Ma_Kredyt = false;
                    Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Kredyt o wartości {Wysokosc_Kredytu}zl został spłacony.";
                }
                else
                {
                    Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Próba spłacenia kredytu o wartości {Wysokosc_Kredytu}zl zakończona niepowodzeniem.";
                    Console.WriteLine($"Splata kredytu niemozliwa!");
                }
            }
            public void Wplata()
            {
                Console.Write("Podaj kwotę do wpłacenia: ");
                int Kwota_Wplacona = Convert.ToInt32(Console.ReadLine());
                Saldo += Kwota_Wplacona;
                Wszystkie_Dokonane_Wpłaty += Kwota_Wplacona;
                Bank.Aktywa_Banku = Bank.Aktywa_Banku + Kwota_Wplacona;
                Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Na konto wpłacono {Kwota_Wplacona}zl";
            }
            public void Wyplata()
            {
                Console.Write("Podaj kwotę do wypłacenia: ");
                int Pieniadze_Wyplacone = Convert.ToInt32(Console.ReadLine());
                if (Saldo >= Pieniadze_Wyplacone)
                {
                    Saldo -= Pieniadze_Wyplacone;
                    Wszystkie_Dokonane_Wypłaty += Pieniadze_Wyplacone;
                    Bank.Aktywa_Banku = Bank.Aktywa_Banku - Pieniadze_Wyplacone;
                    Console.WriteLine($"Wypłacono {Pieniadze_Wyplacone}zl.");
                    Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Z konta wypłacono {Pieniadze_Wyplacone}zl";
                }
                else
                {
                    Console.WriteLine("Wypłata niemożliwa!");
                    Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Próba wypłacenia z konta kwoty {Pieniadze_Wyplacone}zl zakończona niepowodzeniem.";
                }
            }
            public void Log_Out()
            {
                Bank.Czy_Zalogowany = false;
                Uruchom_Bankowosc();
            }
            public void Historia_Konta()
            {
                Console.WriteLine($"Historia konta {Bank.Zalogowany_User.Nazwa_Usera} o ID {Bank.Zalogowany_User.ID_Konta - 1}: ");
                Console.WriteLine(Historia);
            }
            public bool Przelew()
            {
                Console.Write("Podaj numer konta użytkownika: ");
                int Numer_Konta_Usera = Convert.ToInt32(Console.ReadLine());
                if (Numer_Konta_Usera == Bank.Zalogowany_User.ID_Konta - 1)
                {
                    Console.WriteLine("\tNie możesz wykonać przelewu na swoje konto!");
                    return false;
                }
                if (Bank.Czy_Jest_Takie_Konto(Numer_Konta_Usera))
                {
                    Console.Write("Podaj kwotę do wpłacenia: ");
                    int Saldo = Convert.ToInt32(Console.ReadLine());
                    if (Saldo <= Bank.Zalogowany_User.Saldo)
                    {
                        Bank.Zalogowany_User.Saldo = Bank.Zalogowany_User.Saldo - Saldo;
                        Bank.Lista_Kont[Numer_Konta_Usera].Saldo += Saldo;
                        Wszystkie_Dokonane_Wypłaty += Saldo;
                        Console.WriteLine($"\tWykonano przelew {Saldo}zl na konto {Numer_Konta_Usera}. Po traksakcji stan konta wynosi {Bank.Zalogowany_User.Saldo}");
                        Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Wykonano przelew o wartości {Saldo}zl dla uzytkownika o numerze konta: {Numer_Konta_Usera}";
                        Bank.Lista_Kont[Numer_Konta_Usera].Historia += $"\n[{Bank.Lista_Kont[Numer_Konta_Usera].Nazwa_Usera} {Bank.Lista_Kont[Numer_Konta_Usera].Nazwisko}] Otrzymano przelew o wartości {Saldo}zl od uzytkownika o numerze konta: {Bank.Zalogowany_User.ID_Konta - 1}";
                    }
                    else
                    {
                        Console.WriteLine("\tNie możesz wykonać przelewu na kwotę wyższą niż aktualny stan twojego konta!");
                        Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Próba wykonania przelewu na kwotę wyższą niż aktualny stan konta (Podano: {Saldo})]";
                    }
                }
                return true;
            }
            public void Podsumowanie()
            {
                Console.WriteLine($"Podsumowanie konta {Bank.Zalogowany_User.Nazwa_Usera} o ID {Bank.Zalogowany_User.ID_Konta - 1}: ");
                Console.WriteLine($"\tŁącznie wpłacono : {Bank.Zalogowany_User.Wszystkie_Dokonane_Wpłaty}\n\tŁącznie wypłacono : {Bank.Zalogowany_User.Wszystkie_Dokonane_Wypłaty}");
            }
            public bool Kredyt()
            {
                if (Bank.Zalogowany_User.Czy_Ma_Kredyt)
                {
                    Console.WriteLine("\tAktualnie posiadasz aktywny kredyt. Bank nie jest w stanie przydzielić Ci kolejnego!\n");
                    return false;
                }
                Console.Write($"\n\tKwota kredytu nie może wynosić więcej niż {Funkcje_Admina.Wez_Wszystkie_Aktywa()}zl\nPodaj wartość kredytu: ");
                int Wysokosc_Kredytu = Convert.ToInt32(Console.ReadLine());
                if (Wysokosc_Kredytu <= Funkcje_Admina.Wez_Wszystkie_Aktywa())
                {
                    Bank.Zalogowany_User.Saldo += Wysokosc_Kredytu;
                    Bank.Zalogowany_User.Wysokosc_Kredytu = Wysokosc_Kredytu;
                    Bank.Zalogowany_User.Czy_Ma_Kredyt = true;
                    Console.WriteLine($"\tBank przydzielił kredyt na kwotę {Wysokosc_Kredytu}zl.");
                    Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Przyznano kredyt na kwote {Wysokosc_Kredytu}zl, stan konta po przyjeciu transakcji: {Saldo}";
                }
                return true;
            }
            public bool Spłac_Kredyt()
            {
                if (!Bank.Zalogowany_User.Czy_Ma_Kredyt)
                {
                    Console.WriteLine("\tNie posiadasz aktywnego kredytu. Opcja niemożliwa!\n");
                    return false;
                }
                Console.WriteLine($"Twój aktualny kredyt wynosi {Bank.Zalogowany_User.Wysokosc_Kredytu}zl.\nPodaj kwote ktora chcesz splacic: ");
                int Splata_Kredytu = Convert.ToInt32(Console.ReadLine());
                if (Splata_Kredytu <= Bank.Zalogowany_User.Saldo)
                {
                    Bank.Zalogowany_User.Saldo -= Splata_Kredytu;
                    Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Spłacono {Splata_Kredytu}zl z kredytu, stan konta po przyjeciu transakcji: {Saldo}";
                    if ((Bank.Zalogowany_User.Wysokosc_Kredytu - Splata_Kredytu) <= 0)
                    {
                        Bank.Zalogowany_User.Wysokosc_Kredytu = 0;
                        Bank.Zalogowany_User.Czy_Ma_Kredyt = false;
                        Console.WriteLine("\tCały kredyt został spłacony.\n");
                        Bank.Zalogowany_User.Historia += $"\n[{Nazwa_Usera} {Nazwisko}] Cały kredyt został spłacony.";
                    }
                    else
                    {
                        Bank.Zalogowany_User.Wysokosc_Kredytu -= Splata_Kredytu;
                        Console.WriteLine($"\tKwota {Splata_Kredytu}zl zostala rozliczona na poczet aktywnego kredytu.");
                    }
                }
                return true;
            }
            public void Szczegoly_Konta()
            {
                Console.WriteLine($"\nInformacje konta {Bank.Zalogowany_User.Nazwa_Usera} o ID {Bank.Zalogowany_User.ID_Konta - 1}: \n");
                Console.Write($"\tStan konta: {Bank.Zalogowany_User.Saldo}zl\n\tCzy ma kredyt?: {((Bank.Zalogowany_User.Czy_Ma_Kredyt) ? "Tak" : "Nie")}\n\n");

            }
        }
        public static void Menu(string[,] Liczba_Opcji)
        {
            int Liczba_Opcji_Count = Liczba_Opcji.Length / 2;
            int Wybrana_Opcja = 0;

            for (int i = 0; i < Liczba_Opcji_Count; i++)
            {
                Console.Write($"{i + 1}) ");
                Console.WriteLine(Liczba_Opcji[i, 0]);
            }
            Console.Write("\n\nTwój wybór: ");
            Wybrana_Opcja = Convert.ToInt32(Console.ReadLine());


            if (!Bank.Czy_Zalogowany)
            {
                if (Wybrana_Opcja == 1)
                {
                    Console.Write("Login: ");
                    var LogIn = Console.ReadLine();
                    Console.Write("Hasło: ");
                    var Haslo = Console.ReadLine();
                    if (Bank.Czy_User_Moze_Sie_Zalogowac(LogIn, Haslo));
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.Clear();
                if (!Bank.Lista_Kont[Bank.Zalogowany_User.ID_Konta - 1].Czy_Ma_Uprawnienia_Admin)
                {
                    Bank.Lista_Kont[Bank.Zalogowany_User.ID_Konta - 1].GetType().GetMethod(Liczba_Opcji[Wybrana_Opcja - 1, 1]).Invoke(Bank.Lista_Kont[Bank.Zalogowany_User.ID_Konta - 1], null);
                }
                else
                {
                    Funkcje_Admina Test = new Funkcje_Admina();
                    Test.GetType().GetMethod(Liczba_Opcji[Wybrana_Opcja - 1, 1]).Invoke(Test, null);
                }
                Glowne_Menu();
            }
        }
        public static void Panel_do_Logowania()
        {
            Console.Clear();
            while (!Bank.Czy_Zalogowany)
            {
                Menu(Bank.Panel_do_Logowania);
            }
        }
        public static void Glowne_Menu()
        {
            if (Bank.Lista_Kont[Bank.Zalogowany_User.ID_Konta - 1].Czy_Ma_Uprawnienia_Admin)
            {
                Menu(Bank.Admin_Menu);
            }
            else
            {
                Menu(Bank.Menu_Usera);
            }
            Console.ReadKey();
        }
        public static void Uruchom_Bankowosc()
        {
            Panel_do_Logowania();
            Glowne_Menu();
        }
        public static Account Utworz_Usera(string Pierwsze_Imie, string Nazwisko, bool Czy_Ma_Uprawnienia_Admin, string Haslo)
        {
            if ((Pierwsze_Imie == null || Pierwsze_Imie == "") || (Nazwisko == null || Nazwisko == "") || (Haslo == null || Haslo == ""))
            {
                Console.WriteLine("Użytkownik nie został dodany.");
                return null;
            }
            else
            {
                Account Konto = new Account(Pierwsze_Imie, Nazwisko, Czy_Ma_Uprawnienia_Admin, Haslo);
                Bank.Lista_Kont.Add(Konto);
                Console.WriteLine($"\nUżytkownik {Pierwsze_Imie} {Nazwisko} został stworzony.");
                return Konto;
            }
        }
        static void Main(string[] args)
        {
            Utworz_Usera("admin", "karsznia", true, "haslo");
            Utworz_Usera("urszula", "uzytkownik", false, "haslo2");
            Uruchom_Bankowosc();
        }
    }
}
