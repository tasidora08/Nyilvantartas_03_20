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
    public string CreateLearner(Learner learner)
    {
        if (allLearners.Contains(learner))
        {
            return "Ez a tanuló már létezik!";
        }
        else
        {
            allLearners.Add(learner);
            return  $"'{learner.LearnerName}' tanuló hozzáadva a rendszerhez!";
        }
    }

    public string DeleteLearnerData(Learner learner)
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
                    break;
                }
            }

            return $"'{learner.LearnerName}' nevű tanuló törölve a rendszerből!";
        }
        else
        {
            return $"'{learner.LearnerName}' nevű tanuló nem található!";
        }
    }

    public string UpdateLearnerData(Learner learner, string? newName = null, DateOnly? newBornDate = null, string? newMotherName = null)
    {
        var foundInAll = allLearners.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
        if (foundInAll == null)
        {
            return $"'{learner.LearnerName}' nevű tanuló nem található!";
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

        return $"'{foundInAll.LearnerName}' tanuló adatai sikeresen frissítve!";
    }

    public string AddNewLearner(string instructorName, Learner learner)
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
                    return $"'{learner.LearnerName}' nevű tanuló már szerepel egy másik oktatónál!";
                }
                else
                {
                    instructors[instructorName].Add(learner);
                    return $"Tanuló '{learner.LearnerName}' felvéve oktatóhoz '{instructorName}'.";
                }
            }
            else
            {
                return $"Nem található oktató '{instructorName}' névvel!";
            }
        }
        else
        {
            return $"'{learner.LearnerName}' tanuló nem található a rendszerben!";
        }
    }

    public string DeleteLearner(Learner learner)
    {
        foreach (var instructor in instructors)
        {
            var found = instructor.Value.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
            if (found != null)
            {
                instructor.Value.Remove(found);
                return $"'{learner.LearnerName}' nevű tanuló törölve!";
            }
        }
        return $"'{learner.LearnerName}' nevű tanuló nem található!";
    }

    public string UpdateLearner(Learner learner, string newInstructorName)
    {
        if (!instructors.ContainsKey(newInstructorName))
        {
            return $"Nem található oktató '{newInstructorName}' névvel!";
        }

        foreach (var instructor in instructors)
        {
            var found = instructor.Value.FirstOrDefault(x => x.LearnerName == learner.LearnerName);
            if (found != null)
            {
                if (instructor.Key == newInstructorName)
                {
                    return $"'{learner.LearnerName}' már '{newInstructorName}' oktatónál van!";
                }

                instructor.Value.Remove(found);
                instructors[newInstructorName].Add(found);
                return $"'{learner.LearnerName}' áthelyezve '{instructor.Key}' -> '{newInstructorName}'";
            }
        }

        return $"'{learner.LearnerName}' nevű tanuló nem található!";
    }


    public string AddNewInstructor(string instructorName)
    {
        if (!instructors.ContainsKey(instructorName))
        {
            List<Learner> learners = new List<Learner>();
            instructors.Add(instructorName, learners);
            return $"{instructorName} oktató felvéve";
        }
        else
        {
            return "Ez az oktató már létezik!";
        }
    }

    
    public string DeleteInstructor(string instructorName)
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
            return $"'{instructorName}' oktató törölve! A tanulói visszakerültek a rendszerbe oktató nélkül.";
        }
        else
        {
            return $"Nem található oktató '{instructorName}' névvel";
        }
    }


    //List Methods
    public string ListLearnersOfInstructors(string instructorName)
    {
        if (!instructors.ContainsKey(instructorName))
            return $"'{instructorName}' oktató nem található!";

        var learners = instructors[instructorName];

        if (learners.Count == 0)
            return $"'{instructorName}' oktató nem rendelkezik tanulóval!";

        var header = $"{instructorName} tanulói:\n{new string('-', 30)}\n";

        var body = string.Join("\n", learners.Select(learner =>
            $"Név: {learner.LearnerName}\n" +
            $"Életkor: {learner.Age}\n" +
            $"Szül. Dátum: {learner.BornDate}\n" +
            $"Anyja neve: {learner.MotherName}\n" +
            $"{new string('-', 30)}"
        ));

        return header + body;
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
    public string SaveToFile(string fileName)
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

        return $"Fájl mentése '{fullFileName}' néven sikeres!";
    }

    public string LoadFromFile(string fileName)
    {
        string fullFileName = fileName.EndsWith(".txt") ? fileName : fileName + ".txt";

        if (!File.Exists(fullFileName))
        {
            return $"Nem található fájl '{fullFileName}' néven!";
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

        return $"Fájl betöltése '{fullFileName}' névről sikeres!";
    }

    // Tanuló keresése név alapján
    public Learner GetLearnerByName(string name)
    {
        return allLearners.FirstOrDefault(x => x.LearnerName == name);
    }
}
