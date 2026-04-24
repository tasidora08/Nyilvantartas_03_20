namespace AutoNelkuliIskola;

public class UI
{ 
    public static void MainMenu()
    {
        bool fut = true;
        Console.WriteLine("Üdvözöljük az AutoNélküli Autósiskola nyilvántartásában!\n");
        DrivingSchool iskola;
        Console.WriteLine("Rendszerbe belépés...");
        Console.Write("Autósiskola neve: ");
        iskola = new DrivingSchool(Console.ReadLine());
        while (fut)
        {
            Console.Clear();
            Console.WriteLine("[1] Tanuló hozzáadása");
            Console.WriteLine("[2] Tanuló törlése");
            Console.WriteLine("[3] Tanuló adatainak módosítása");
            Console.WriteLine("[4] Tanuló hozzáadása egy oktatóhoz");
            Console.WriteLine("[5] Tanuló áthelyezése másik oktatóhoz");
            Console.WriteLine("[6] Oktató felvétele");
            Console.WriteLine("[7] Oktató törlése");
            Console.WriteLine("[8] Oktató tanulóinak listája");
            Console.WriteLine("[9] Tanulók kilistázása");
            Console.WriteLine("[F1] Adatok betöltése fájlból");
            Console.WriteLine("[F2] Adatok mentése fájlba");
            Console.WriteLine("[Esc] Kilépés");
            Console.Write("Válasszon! ");
            ConsoleKeyInfo valasztas = Console.ReadKey();

            switch (valasztas.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    Console.WriteLine("\nTanuló hozzáadása");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.Write("Tanuló neve: ");
                    string nev = Console.ReadLine();
                    Console.Write("Tanuló születési dátuma [2000.01.01 formátumban]: ");
                    DateOnly szuletes;
                    try
                    {
                        szuletes = DateOnly.Parse(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Hibás dátumformátum! Kérem adja meg újra a tanuló születési dátumát [2000.01.01 formátumban]: ");
                        szuletes = DateOnly.Parse(Console.ReadLine());
                    }
                    Console.Write("Tanuló anyja neve: ");
                    string anyjaNeve = Console.ReadLine();
                    Console.Write("Egészségügyi vizsgával rendelkezik? (I/N) ");
                    ConsoleKeyInfo valasztasE = Console.ReadKey();
                    bool egeszsegugyi = false;
                    if (valasztasE.Key == ConsoleKey.I)
                    {
                        egeszsegugyi = true;
                    }
                    Console.Write("KRESZ vizsgával rendelkezik? (I/N) ");
                    ConsoleKeyInfo valasztasK = Console.ReadKey();
                    bool kresz = false;
                    if (valasztasK.Key == ConsoleKey.I)
                    {
                        kresz = true;
                    }
                    Console.Write("Eddig vezetett órák száma: ");
                    int vezetettOrak = Convert.ToInt32(Console.ReadLine());
                    Learner tanulo = new Learner(nev, szuletes, anyjaNeve, egeszsegugyi, kresz, vezetettOrak);
                    iskola.CreateLearner(tanulo);
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.WriteLine("\nTanuló törlése");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.WriteLine("Tanulók:");
                    iskola.GetAllLearners().ForEach(t => Console.WriteLine($"\t{t.LearnerName}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.Write("Törölni kívánt tanuló neve: ");
                    string tanuloNeveT = Console.ReadLine();
                    Learner tanuloTorol = iskola.GetLearnerByName(tanuloNeveT);
                    iskola.DeleteLearner(tanuloTorol);
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.WriteLine("\nTanuló adatainak módosítása");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.WriteLine("Tanulók:");
                    iskola.GetAllLearners().ForEach(t => Console.WriteLine($"\t{t.LearnerName}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.Write("Módosítani kívánt tanuló neve: ");
                    string nevModosit = Console.ReadLine();
                    Console.Write("Új név (Enter ha nem változik): ");
                    string ujNev = Console.ReadLine();
                    Console.Write("Új születési dátum (Enter ha nem változik): ");
                    string ujDatumStr = Console.ReadLine();
                    Console.Write("Új anyja neve (Enter ha nem változik): ");
                    string ujAnyja = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ujNev)) ujNev = null;
                    if (string.IsNullOrWhiteSpace(ujAnyja)) ujAnyja = null;
                    DateOnly? ujDatum = null;
                    if (!string.IsNullOrWhiteSpace(ujDatumStr))
                    {
                        try
                        {
                            ujDatum = DateOnly.Parse(ujDatumStr);
                        }
                        catch
                        {
                            Console.WriteLine("Hibás dátum, nem lesz módosítva!");
                        }
                    }
                    Learner tanuloModosit = iskola.GetLearnerByName(nevModosit);
                    iskola.UpdateLearnerData(tanuloModosit, ujNev, ujDatum, ujAnyja);
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    Console.WriteLine("\nTanuló hozzáadása egy oktatóhoz");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.WriteLine("Tanulók:");
                    iskola.GetAllLearners().ForEach(t => Console.WriteLine($"\t{t.LearnerName}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.WriteLine("Oktatók:");
                    iskola.GetAllInstructors().ForEach(o => Console.WriteLine($"\t{o}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.Write("Tanuló neve: ");
                    string tanuloNeve = Console.ReadLine();
                    Console.Write("Oktató neve: ");
                    string oktatoHozzaad = Console.ReadLine();
                    Learner tanuloHozzaad = iskola.GetLearnerByName(tanuloNeve);
                    iskola.AddNewLearner(oktatoHozzaad, tanuloHozzaad);
                    break;

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    Console.WriteLine("\nTanuló áthelyezése másik oktatóhoz");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    iskola.GetAllLearners().ForEach(t => Console.WriteLine($"\t{t.LearnerName}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.WriteLine("Oktatók:");
                    iskola.GetAllInstructors().ForEach(o => Console.WriteLine($"\t{o}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.Write("Tanuló neve: ");
                    string tanuloAthelyezese = Console.ReadLine();
                    Learner tanuloAthelyez = iskola.GetLearnerByName(tanuloAthelyezese);
                    Console.Write("Új oktató neve: ");
                    string ujOktatoNeve = Console.ReadLine();
                    iskola.UpdateLearner(tanuloAthelyez, ujOktatoNeve);
                    break;

                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    Console.WriteLine("\nOktató felvétele");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.Write("Oktató neve: ");
                    string oktatoFelvetel = Console.ReadLine();
                    iskola.AddNewInstructor(oktatoFelvetel);
                    break;

                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    Console.WriteLine("\nOktató törlése");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.WriteLine("Oktatók:");
                    iskola.GetAllInstructors().ForEach(o => Console.WriteLine($"\t{o}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.Write("Oktató neve: ");
                    string oktatoTorles = Console.ReadLine();
                    iskola.DeleteInstructor(oktatoTorles);
                    break;

                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    Console.WriteLine("\nOktató tanulóinak listája");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.WriteLine("Oktatók:");
                    iskola.GetAllInstructors().ForEach(o => Console.WriteLine($"\t{o}"));
                    Console.WriteLine("-".PadLeft(29, '-'));
                    Console.Write("Oktató neve: ");
                    string OktatoLista = Console.ReadLine();
                    Console.WriteLine(iskola.ListLearnersOfInstructors(OktatoLista));
                    Console.Write("Visszalépés - F4");
                    ConsoleKeyInfo vissza = Console.ReadKey();
                    if (vissza.Key == ConsoleKey.F4)
                        break;
                    break;

                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    Console.WriteLine("\nTanulók kilistázása");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    iskola.GetAllLearners().ForEach(t => Console.WriteLine(t.LearnerName));
                    Console.Write("Visszalépés - F4");
                    vissza = Console.ReadKey();
                    if (vissza.Key == ConsoleKey.F4)
                        break;
                    break;

                case ConsoleKey.F1:
                    Console.WriteLine("\nAdatok betöltése fájlból");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.Write("Fájl neve: ");
                    string fajlNev = Console.ReadLine();
                    iskola.LoadFromFile(fajlNev);
                    break;

                case ConsoleKey.F2:
                    Console.WriteLine("\nAdatok mentése fájlba");
                    Console.WriteLine("-".PadLeft(49, '-'));
                    Console.Write("Fájl neve: ");
                    string mentesFajlNev = Console.ReadLine();
                    iskola.SaveToFile(mentesFajlNev);
                    break;

                case ConsoleKey.Escape:
                    fut = false;
                    Console.Clear();
                    Console.WriteLine("\nKöszönjük, hogy használta rendszerünket!\nVárjuk vissza!");
                    break;
                default:
                    break;
            } 
        }
    }
}