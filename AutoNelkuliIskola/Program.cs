namespace AutoNelkuliIskola;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        DrivingSchool iskola = new DrivingSchool("Autósikola 2000");
        Console.WriteLine(iskola.DrivingSchoolName);


        iskola.AddNewInstructor("Kis Béla");
        iskola.AddNewInstructor("Nagy Laci");
        iskola.AddNewInstructor("Kis Béla");
        iskola.AddNewInstructor("Tök Ödön");

        Console.WriteLine("ALMA");

        iskola.DeleteInstructor("Kis Béla");
        iskola.DeleteInstructor("Gazsi");

        Console.WriteLine("VÉGE");
        Console.WriteLine();

        Learner tanulo1 = new Learner("Ábel",new DateOnly(2000,02,19), "anya");
        Learner tanulo2 = new Learner("Pista",new DateOnly(2000,02,19), "anya");
        Learner tanulo3 = new Learner("Lali",new DateOnly(2000,02,19), "anya");
        Learner tanulo4 = new Learner("Géza",new DateOnly(2000,02,19), "anya");

        Console.WriteLine($"Név: {tanulo1.LearnerName} \nÉletkor: {tanulo1.Age} \nSzül. Dátum: {tanulo1.BornDate} \nAnyja neve: {tanulo1.MotherName}");


        iskola.AddNewLearner("Tök Ödön", tanulo1);
        iskola.AddNewLearner("Tök Ödön", tanulo1);
        iskola.AddNewLearner("Nagy Laci", tanulo1);
        iskola.AddNewLearner("Tök Ödön", tanulo1);

        iskola.DeleteLearner(tanulo1);

        iskola.AddNewLearner("Nagy Laci", tanulo1 );

        iskola.UpdateLearner(tanulo1, "Tök Ödön");
        iskola.AddNewLearner("Tök Ödön", tanulo2);


        iskola.ListLearnersOfInstructors("Nagy Laci");
        iskola.ListLearnersOfInstructors("Tök Ödön");

        Console.WriteLine(iskola.NumberOfInstructors);
        Console.WriteLine(iskola.NumberOfLearners);


        foreach (KeyValuePair<string, List<Learner>> item in iskola.ListAllInstructorsWithLearners())
        {
            Console.WriteLine($"Oktató: {item.Key}");
            item.Value.ForEach(x => Console.WriteLine($"  - {x.LearnerName} - {x.Age} - {x.BornDate} - {x.MotherName}"));
        }

        iskola.AddNewLearner("Nagy Laci", tanulo3);
        iskola.AddNewLearner("Nagy Laci", tanulo4);

        iskola.SaveToFile("mentés");

    }
}