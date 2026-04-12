using System;
using System.Collections.Generic;

namespace AutoNelkuliIskola;

public class DrivingSchool
{
    private string drivingSchoolName;

    private Dictionary<string, List<Learner>> instructors;
    private List<Learner> allLearners;

    public DrivingSchool(string drivingSchoolName)
    {
        this.drivingSchoolName = drivingSchoolName;
        this.instructors = new Dictionary<string, List<Learner>>();
        this.allLearners = new List<Learner>();
    }

    public string DrivingSchoolName { get => drivingSchoolName; set => drivingSchoolName = value; }
    public int NumberOfInstructors => GetNumberOfInstructors(instructors);
    public int NumberOfLearners => GetNumberOfLearners(allLearners);


    //Data Handling
    public void CreateLearner(Learner learner)
    {
        if (allLearners.Contains(learner))
        {
            Console.WriteLine("Ez a tanuló már létezik!");
        }
        else
        {
            allLearners.Add(learner);
            Console.WriteLine($"'{learner.LearnerName}' tanuló hozzáadva a rendszerhez!");
        }
    }

    public void DeleteLearnerData(Learner learner)
    {
        var found = allLearners.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
        if (found != null)
        {
            allLearners.Remove(found);

            foreach (var instructor in instructors)
            {
                var inDict = instructor.Value.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
                if (inDict != null)
                {
                    instructor.Value.Remove(inDict);
                    Console.WriteLine($"'{learner.LearnerName}' törölve '{instructor.Key}' oktatótól!");
                    break;
                }
            }

            Console.WriteLine($"'{learner.LearnerName}' nevű tanuló törölve a rendszerből!");
        }
        else
        {
            Console.WriteLine($"'{learner.LearnerName}' nevű tanuló nem található!");
        }
    }

    public void UpdateLearnerData(Learner learner, string? newName = null, DateOnly? newBornDate = null, string? newMotherName = null)
    {
        var foundInAll = allLearners.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
        if (foundInAll == null)
        {
            Console.WriteLine($"'{learner.LearnerName}' nevű tanuló nem található!");
            return;
        }

        if (newName != null) foundInAll.LearnerName = newName;
        if (newBornDate != null) foundInAll.BornDate = newBornDate.Value;
        if (newMotherName != null) foundInAll.MotherName = newMotherName;

        foreach (var instructor in instructors)
        {
            var inDict = instructor.Value.FirstOrDefault(x => x == foundInAll || x.LearnerName == learner.LearnerName);
            if (inDict != null && !ReferenceEquals(inDict, foundInAll))
            {
                if (newName != null) inDict.LearnerName = newName;
                if (newBornDate != null) inDict.BornDate = newBornDate.Value;
                if (newMotherName != null) inDict.MotherName = newMotherName;
            }
        }

        Console.WriteLine($"'{foundInAll.LearnerName}' tanuló adatai sikeresen frissítve!");
    }

    public void AddNewLearner(string instructorName, Learner learner)
    {
        if (allLearners.Contains(learner))
        {
            if (instructors.ContainsKey(instructorName))
            {
                bool found = instructors.Values
                    .SelectMany(x => x)
                    .Any(x => x.LearnerName == learner.LearnerName);

                if (found)
                {
                    Console.WriteLine($"'{learner.LearnerName}' nevű tanuló már szerepel egy másik oktatónál!");
                    return;
                }
                else
                {
                    instructors[instructorName].Add(learner);
                    Console.WriteLine($"Tanuló '{learner.LearnerName}' felvéve oktatóhoz '{instructorName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Nem található oktató '{instructorName}' névvel!");
            }
        }
        else
        {
            Console.WriteLine($"'{learner.LearnerName}' tanuló nem található a rendszerben!");
        }
    }

    public void DeleteLearner(Learner learner)
    {
        foreach (var instructor in instructors)
        {
            var found = instructor.Value.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
            if (found != null)
            {
                instructor.Value.Remove(found);
                Console.WriteLine($"'{learner.LearnerName}' nevű tanuló törölve!");
                return;
            }
        }
        Console.WriteLine($"'{learner.LearnerName}' nevű tanuló nem található!");
    }

    public void UpdateLearner(Learner learner, string newInstructorName)
    {
        if (!instructors.ContainsKey(newInstructorName))
        {
            Console.WriteLine($"Nem található oktató '{newInstructorName}' névvel!");
            return;
        }

        foreach (var instructor in instructors)
        {
            var found = instructor.Value.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
            if (found != null)
            {
                if (instructor.Key == newInstructorName)
                {
                    Console.WriteLine($"'{learner.LearnerName}' már '{newInstructorName}' oktatónál van!");
                    return;
                }

                instructor.Value.Remove(found);
                instructors[newInstructorName].Add(found);
                Console.WriteLine($"'{learner.LearnerName}' áthelyezve '{instructor.Key}' -> '{newInstructorName}'");
                return;
            }
        }

        Console.WriteLine($"'{learner.LearnerName}' nevű tanuló nem található!");
    }


    public void AddNewInstructor(string instructorName)
    {
        if (!instructors.ContainsKey(instructorName))
        {
            List<Learner> learners = new List<Learner>();
            instructors.Add(instructorName, learners);
            Console.WriteLine($"{instructorName} oktató felvéve");
        }
        else
        {
            Console.WriteLine("Ez az oktató már létezik!");
        }
    }

    
    public void DeleteInstructor(string instructorName)
    {
        if (instructors.ContainsKey(instructorName))
        {
            foreach (var learner in instructors[instructorName])
            {
                if (!allLearners.Contains(learner))
                {
                    allLearners.Add(learner);
                }
            }

            instructors.Remove(instructorName);
            Console.WriteLine($"'{instructorName}' oktató törölve! A tanulói visszakerültek a rendszerbe oktató nélkül.");
        }
        else
        {
            Console.WriteLine($"Nem található oktató '{instructorName}' névvel");
        }
    }


    //List Methods
    public void ListLearnersOfInstructors(string instructorName)
    {
        if (instructors.ContainsKey(instructorName))
        {
            if (instructors[instructorName].Count == 0)
            {
                Console.WriteLine($"'{instructorName}' oktató nem rendelkezik tanulóval!");
            }
            else
            {
                Console.WriteLine(instructorName + " tanulói: ");
                Console.WriteLine(new string('-', 30));
                instructors[instructorName].ForEach(learner =>
                    Console.WriteLine($"Név: {learner.LearnerName} \nÉletkor: {learner.Age} \nSzül. Dátum: {learner.BornDate} \nAnyja neve: {learner.MotherName}\n{new string('-', 30)}"));
            }
        }
    }

    public Dictionary<string, List<Learner>> ListAllInstructorsWithLearners()
    {
        return instructors;
    }

    private int GetNumberOfLearners(List<Learner> learners)
    {
        return learners.Count;
    }

    private int GetNumberOfInstructors(Dictionary<string, List<Learner>> learners)
    {
        return learners.Count;
    }

    public List<Learner> GetAllLearners()
    {
        return allLearners;
    }

    public List<string> GetAllInstructors()
    {
        return instructors.Keys.ToList();
    }

    //File Handling
    public void SaveToFile(string fileName)
    {
        string fullFileName = fileName.EndsWith(".txt") ? fileName : fileName + ".txt";

        File.WriteAllText(fullFileName, "");

        foreach (KeyValuePair<string, List<Learner>> item in instructors)
        {
            File.AppendAllText(fullFileName, $"Oktató;{item.Key}");
            item.Value.ForEach(learner =>
                File.AppendAllText(fullFileName,
                    $"\nTanuló;{learner.LearnerName};{learner.Age};{learner.BornDate};{learner.MotherName}"));
            File.AppendAllText(fullFileName, "\n");
        }

        Console.WriteLine($"Fájl mentése '{fullFileName}' néven sikeres!");
    }

    public void LoadFromFile(string fileName)
    {
        string fullFileName = fileName.EndsWith(".txt") ? fileName : fileName + ".txt";

        if (!File.Exists(fullFileName))
        {
            Console.WriteLine($"Nem található fájl '{fullFileName}' néven!");
            return;
        }

        string currentInstructor = null;

        foreach (string line in File.ReadAllLines(fullFileName))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(';');

            if (parts[0] == "Oktató")
            {
                currentInstructor = parts[1];
                AddNewInstructor(currentInstructor);
            }
            else if (parts[0] == "Tanuló" && currentInstructor != null && parts.Length >= 5)
            {
                Learner newLearner = new Learner(
                    learnerName: parts[1],
                    bornDate: DateOnly.Parse(parts[3]),
                    motherName: parts[4]
                );

                CreateLearner(newLearner);
                AddNewLearner(currentInstructor, newLearner);
            }
        }

        Console.WriteLine($"Fájl betöltése '{fullFileName}' névről sikeres!");
    }

    // Tanuló keresése név alapján
    public Learner GetLearnerByName(string name)
        {
        return allLearners.FirstOrDefault(x => x.LearnerName == name);
        }
}
