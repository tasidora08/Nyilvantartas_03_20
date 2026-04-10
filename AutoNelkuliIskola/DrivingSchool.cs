using System;
using System.Collections.Generic;

namespace AutoNelkuliIskola;

public class DrivingSchool
{
    private string drivingSchoolName;

    private Dictionary<string, List<Learner>> instructors;

    public DrivingSchool(string drivingSchoolName)
    {
        this.drivingSchoolName = drivingSchoolName;
        this.instructors = new Dictionary<string, List<Learner>>();
    }

    public string DrivingSchoolName { get => drivingSchoolName; set => drivingSchoolName = value; }
    public int NumberOfInstructors => GetNumberOfInstructors(instructors);
    public int NumberOfLearners => GetNumberOfLearners(instructors);



    public void AddNewLearner(string instructorName, Learner learner)
    {
        if (instructors.ContainsKey(instructorName))
        {
            bool found = instructors.Values
            .SelectMany(x => x)
            .Any(x => x.LearnerName == learner.LearnerName);

            if (found)
            {
                Console.WriteLine($" '{learner.LearnerName}' nevű tanuló már szerepel egy másik oktatónál!");
                return;
            }
            else
            {
                if (instructors[instructorName].Any(x => x.LearnerName == learner.LearnerName))
                {
                    Console.WriteLine($"A tanuló már szerepel ennél az oktatónál '{instructorName}'!");
                }
                else
                {
                    instructors[instructorName].Add(learner);
                    Console.WriteLine($"Tanuló felvéve oktatóhoz '{instructorName}'.");
                }
            }
           
        }
        return;
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
                Console.WriteLine($"'{learner.LearnerName}' áthelyezve '{instructor.Key}' ->'{newInstructorName}'");
                return;
            }
        }

        Console.WriteLine($"'{learner.LearnerName}' nevű tanuló nem található!");
    }


    public void AddNewInstructor(string instructorName)
    { 
        if(!instructors.ContainsKey(instructorName))
        {
            List<Learner> learners = new List<Learner>();
            instructors.Add(instructorName, learners);
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
            instructors.Remove(instructorName);
            Console.WriteLine("Oktató törölve!");
        }
        else
        {
            Console.WriteLine($"Nem található oktató '{instructorName}' névvel");
        }
    }



    public void ListLearnersOfInstructors(string instructorName)
    {
        if (instructors[instructorName].Count == 0)
        {
            Console.WriteLine($"'{instructorName}' oktató nem rendelkezik tanulóval!");
        }
        else
        {
            Console.WriteLine(instructorName + " tanulói: ");
            Console.WriteLine(new string('-', 30));

            instructors[instructorName].ForEach(learner => Console.WriteLine($"Név: {learner.LearnerName} \nÉletkor: {learner.Age} \nSzül. Dátum: {learner.BornDate} \nAnyja neve: {learner.MotherName}\n{new string('-', 30)}"));
        }
    }
    public Dictionary<string, List<Learner>> ListAllInstructorsWithLearners()
    {
        return instructors;
    }

    private int GetNumberOfLearners(Dictionary<string, List<Learner>> learners)
    {
        int numberOfLearners = 0;
        foreach (KeyValuePair<string, List<Learner>> item in learners)
        {
            numberOfLearners += item.Value.Count;
        }
        return numberOfLearners;
    }
    private int GetNumberOfInstructors(Dictionary<string, List<Learner>> learners)
    {
        return learners.Count;
    }


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
            File.AppendAllText(fullFileName,"\n");
        }

        Console.WriteLine($"Fájl mentése '{fullFileName}' néven sikeres!");
    }
    public void LoadFromFile(string fileName)
    {

    }

}