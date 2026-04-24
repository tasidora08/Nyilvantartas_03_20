using System;
using System.Collections.Generic;
using System.Text;

namespace AutoNelkuliIskola;

public class DrivingSchool
{
    private string drivingSchoolName;

    private Dictionary<string, List<Learner>> instructors;
    private List<Learner> allLearners;
    private List<Learner> readyLearners;

    public DrivingSchool(string drivingSchoolName)
    {
        this.drivingSchoolName = drivingSchoolName;
        this.instructors = new Dictionary<string, List<Learner>>();
        this.allLearners = new List<Learner>();
        this.readyLearners = new List<Learner>();
    }

    public string DrivingSchoolName { get => drivingSchoolName; set => drivingSchoolName = value; }
    public int NumberOfInstructors => GetNumberOfInstructors(instructors);
    public int NumberOfLearners => GetNumberOfLearners(allLearners);


    //Data Handling

    public string IsReadyForTheExam(Learner learner)
    {
        if(learner.DrivedHours >= 50 && learner.HasTrafficPoliceExam && learner.HasMedicalExam)
        {
            readyLearners.Add(learner);
            return $"{learner.LearnerName} tanuló rendelkezik a vizsgához szükséges követleményekkel";
        }
        else
        {
            return $"{learner.LearnerName} tanuló nem elég felkészült a vizsgára!";
        }
    }

    public string ChangeDrivedHours(Learner learner, int newDrivedHours)
    {
        learner.DrivedHours = newDrivedHours;
        return $"{learner.LearnerName} tanuló vezetett órája {learner.DrivedHours} órára frissült";
    }

    public string SetMedicalExam(bool isMedicalExam, Learner learner)
    {
        if (isMedicalExam)
        {
            learner.HasMedicalExam = true;
            return $"{learner.LearnerName} tanuló mostantól rendelkezik egészségügyi vizsgával!";
        }
        else
        {
            learner.HasMedicalExam = false;
            return $"{learner.LearnerName} tanuló mostantól nem rendelkezik egészségügyi vizsgával!";
        }
    }

    public string SetTrafficPoliceEXam(bool isTrafficPoliceExam, Learner learner)
    {
        if (isTrafficPoliceExam)
        {
            learner.HasTrafficPoliceExam = true;
            return $"{learner.LearnerName} tanuló mostantól rendelkezik KRESZ vizsgával!";
        }
        else
        {
            learner.HasTrafficPoliceExam = false;
            return $"{learner.LearnerName} tanuló mostantól nem rendelkezik KRESZ vizsgával!";
        }
    }

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

    public List<Learner> GetAllLearners()
    {
        return allLearners;
    }

    public Learner GetLearnerByName(string learnerName)
    {
        return allLearners.Where(x => x.LearnerName == learnerName).First();
    }

    public List<string> GetAllInstructors()
    {
        return instructors.Keys.ToList();
    }
    public string ListLearnersOfInstructors(string instructorName)
    {
        if (!instructors.ContainsKey(instructorName))
        {
            return $"Nincs ilyen nevű oktató: {instructorName}";
        }

        if (instructors[instructorName].Count == 0)
        {
            return $"'{instructorName}' oktató nem rendelkezik tanulóval!";
        }

        var sb = new StringBuilder();

        sb.AppendLine(instructorName + " tanulói:");
        sb.AppendLine(new string('-', 30));

        instructors[instructorName].ForEach(learner =>
        {
            sb.AppendLine($"Név: {learner.LearnerName}");
            sb.AppendLine($"Életkor: {learner.Age}");
            sb.AppendLine($"Szül. Dátum: {learner.BornDate}");
            sb.AppendLine($"Anyja neve: {learner.MotherName}");
            sb.AppendLine(new string('-', 30));
        });

        return sb.ToString();
    }

    public Dictionary<string, List<Learner>> ListAllInstructorsWithLearners()
    {
        return instructors;
    }

    public int GetTheNumberOfLearnersWithMedicalExam()
    {
        return allLearners.Count(x => x.HasMedicalExam == true);
    }

    public int GetTheNumberOfLearnersWithTrafficPoliceExam()
    {
        return allLearners.Count(x => x.HasTrafficPoliceExam == true);
    }

    private int GetNumberOfLearners(List<Learner> learners)
    {
        return learners.Count;
    }

    private int GetNumberOfInstructors(Dictionary<string, List<Learner>> learners)
    {
        return learners.Count;
    }

    //Filtering

    public List<Learner> FilterByName(string name)
    {
        return allLearners.Where(x => x.LearnerName.Contains(name)).ToList();
    }
    public List<Learner> FilterByYear(int year)
    {
        return allLearners.Where(x => x.BornDate.Year == year).ToList();
    }
    public List<Learner> FilterByMonth(int month)
    {
        return allLearners.Where(x => x.BornDate.Month == month).ToList();
    }
    public List<Learner> FilterByDay(int day)
    {
        return allLearners.Where(x => x.BornDate.Day == day).ToList();
    }
    public List<Learner> FilterByAge(int age)
    {
        return allLearners.Where(x => x.Age == age).ToList();
    }
    public List<Learner> FilterByMotherName(string motherName)
    {
        return allLearners.Where(x => x.MotherName == motherName).ToList();
    }
    public List<Learner> FilterByMedicalExam(bool hasMedicalExam)
    {
        return allLearners.Where(x => x.HasMedicalExam == hasMedicalExam).ToList();
    }
    public List<Learner> FilterByTrafficPoliceExam(bool hasTrafficPoliceExam)
    {
        return allLearners.Where(x => x.HasTrafficPoliceExam == hasTrafficPoliceExam).ToList();
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
                    $"\nTanuló;{learner.LearnerName};{learner.Age};{learner.BornDate};{learner.MotherName};{learner.HasMedicalExam};{learner.HasTrafficPoliceExam}"));
            File.AppendAllText(fullFileName, "\n");
        }

        Console.WriteLine($"Fájl mentése '{fullFileName}' néven sikeres!");
    }

    public void SaveLearnersToFile(string fileName)
    {
        string fullFileName = fileName.EndsWith(".txt") ? fileName : fileName + ".txt";

        File.WriteAllText(fullFileName, "");

        allLearners.ForEach(learner =>
            File.AppendAllText(fullFileName,
                $";{learner.LearnerName};{learner.Age};{learner.BornDate};{learner.MotherName};{learner.HasMedicalExam};{learner.HasTrafficPoliceExam}"));
        File.AppendAllText(fullFileName, "\n");
        

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
                    motherName: parts[4],
                    hasMedicalExam: bool.Parse(parts[5]),
                    hasTrafficPoliceExam: bool.Parse(parts[6]),
                    drivedHours: int.Parse(parts[7])
                );

                CreateLearner(newLearner);
                AddNewLearner(currentInstructor, newLearner);
            }
        }

        Console.WriteLine($"Fájl betöltése '{fullFileName}' névről sikeres!");
    }
}
