namespace AutoNelkuliIskola;

class Program
{
    static void Main(string[] args)
    {

        //DrivingSchool iskolaa = new DrivingSchool("Autóóóó");

        //iskolaa.LoadFromFile("mentés");


        DrivingSchool iskola = new DrivingSchool("Autósikola 2000");
        //Console.WriteLine(iskola.DrivingSchoolName);

        Learner tanulo1 = new Learner("Ábel", new DateOnly(2000, 02, 19), "anya");
        Learner tanulo2 = new Learner("Pista", new DateOnly(2010, 02, 09), "Mutter");
        Learner tanulo3 = new Learner("Lali", new DateOnly(2000, 02, 19), "Mom");
        Learner tanulo4 = new Learner("Géza", new DateOnly(1900, 02, 11), "Gizi");

        iskola.AddNewInstructor("Kis Béla");
        iskola.AddNewInstructor("Nagy Laci");
        iskola.AddNewInstructor("Kis Béla");
        iskola.AddNewInstructor("Gazsi");

        iskola.CreateLearner(tanulo1);
        iskola.CreateLearner(tanulo2);
        iskola.CreateLearner(tanulo3);
        iskola.CreateLearner(tanulo4);

        iskola.AddNewLearner("Tök Ödön", tanulo1);
        iskola.AddNewLearner("Kis Béla", tanulo2);
        iskola.AddNewLearner("Nagy Laci", tanulo3);
        iskola.AddNewLearner("Gazsi", tanulo4);
        iskola.AddNewLearner("Gazsi", tanulo3);

        iskola.DeleteLearnerData(tanulo2);

        iskola.UpdateLearnerData(tanulo1, "Ábelke");
        iskola.AddNewLearner("Gazsi", tanulo1);



        //foreach (KeyValuePair<string, List<Learner>> item in iskola.ListAllInstructorsWithLearners())
        //{
        //    Console.WriteLine($"Oktató: {item.Key}");
        //    item.Value.ForEach(x => Console.WriteLine($"  - {x.LearnerName} - {x.Age} - {x.BornDate} - {x.MotherName}"));
        //}



        //iskola.SaveToFile("mentés");

        UI.MainMenu();




    }
}