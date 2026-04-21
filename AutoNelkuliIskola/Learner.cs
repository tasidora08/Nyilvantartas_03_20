namespace AutoNelkuliIskola;

public class Learner
{
    private string learnerName;
    private DateOnly bornDate;
    private string motherName;
    private bool hasMedicalExam;
    private bool hasTrafficPoliceExam;
    private int drivedHours;

    public Learner(string learnerName, DateOnly bornDate, string motherName, bool hasMedicalExam, bool hasTrafficPoliceExam, int drivedHours)
    {
        this.learnerName = learnerName;
        this.bornDate = bornDate;
        this.motherName = motherName;
        this.hasMedicalExam = hasMedicalExam;
        this.hasTrafficPoliceExam = hasTrafficPoliceExam;
        this.drivedHours = drivedHours;
    }

    public string LearnerName { get => learnerName; set => learnerName = value; }
    public int Age => GetAge(bornDate);
    public DateOnly BornDate { get => bornDate; set => bornDate = value; }
    public string MotherName { get => motherName; set => motherName = value; }
    public bool HasMedicalExam { get => hasMedicalExam; set => hasMedicalExam = value; }
    public bool HasTrafficPoliceExam { get => hasTrafficPoliceExam; set => hasTrafficPoliceExam = value; }
    public int DrivedHours { get => drivedHours; set => drivedHours = value; }


    private int GetAge(DateOnly bornDate)
    {
        return DateTime.Now.Year - bornDate.Year;
    }
}