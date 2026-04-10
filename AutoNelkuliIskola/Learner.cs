namespace AutoNelkuliIskola;

public class Learner
{
    private string learnerName;
    private int age;
    private DateOnly bornDate;
    private string motherName;

    public Learner(string learnerName, DateOnly bornDate, string motherName)
    {
        this.learnerName = learnerName;
        this.age = Age;
        this.bornDate = bornDate;
        this.motherName = motherName;
    }

    public string LearnerName { get => learnerName; set => learnerName = value; }
    public int Age => GetAge(bornDate);
    public DateOnly BornDate { get => bornDate; set => bornDate = value; }
    public string MotherName { get => motherName; set => motherName = value; }


    private int GetAge(DateOnly bornDate)
    {
        return DateTime.Now.Year - bornDate.Year;
    }
}