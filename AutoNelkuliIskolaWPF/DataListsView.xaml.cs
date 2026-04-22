using AutoNelkuliIskola;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AutoNelkuliIskolaWPF.UserControls
{
    public partial class DataListsView : UserControl
    {
        public DataListsView()
        {
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            var school = App.School;
            var instructorsDict = school.ListAllInstructorsWithLearners();

            // Stat cards
            TxtInstructorCount.Text = school.NumberOfInstructors.ToString();
            TxtLearnerCount.Text = school.NumberOfLearners.ToString();
            TxtMedicalCount.Text = school.GetTheNumberOfLearnersWithMedicalExam().ToString();
            TxtKreszCount.Text = school.GetTheNumberOfLearnersWithTrafficPoliceExam().ToString();

            // Instructors table
            DgInstructors.ItemsSource = instructorsDict
                .Select(kv => new { Name = kv.Key, LearnerCount = kv.Value.Count })
                .ToList();

            // Filter combobox
            var filterOptions = new List<string> { "Összes" };
            filterOptions.AddRange(instructorsDict.Keys);
            CmbInstructorFilter.ItemsSource = filterOptions;
            if (CmbInstructorFilter.SelectedIndex < 0)
                CmbInstructorFilter.SelectedIndex = 0;

            RefreshStudentGrid(instructorsDict);
        }

        private void RefreshStudentGrid(Dictionary<string, List<Learner>>? instructorsDict = null)
        {
            instructorsDict ??= App.School.ListAllInstructorsWithLearners();
            string filter = CmbInstructorFilter.SelectedItem?.ToString() ?? "Összes";

            var rows = new List<StudentRow>();

            foreach (var kv in instructorsDict)
            {
                if (filter != "Összes" && kv.Key != filter) continue;
                foreach (var learner in kv.Value)
                {
                    rows.Add(new StudentRow(learner, kv.Key));
                }
            }

            // Add learners without instructor (not in any instructor's list)
            if (filter == "Összes")
            {
                var allLearners = App.School.FilterByName("");
                var assignedNames = instructorsDict.Values
                    .SelectMany(v => v)
                    .Select(l => l.LearnerName)
                    .ToHashSet();

                foreach (var learner in allLearners)
                {
                    if (!assignedNames.Contains(learner.LearnerName))
                        rows.Add(new StudentRow(learner, "— nincs oktató —"));
                }
            }

            DgStudents.ItemsSource = rows;
        }

        private void CmbInstructorFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshStudentGrid();
        }

        // Helper display class
        private class StudentRow
        {
            public string LearnerName { get; }
            public string BornDate { get; }
            public int Age { get; }
            public string MotherName { get; }
            public int DrivedHours { get; }
            public bool HasMedicalExam { get; }
            public bool HasTrafficPoliceExam { get; }
            public string InstructorName { get; }

            public StudentRow(Learner l, string instructor)
            {
                LearnerName = l.LearnerName;
                BornDate = l.BornDate.ToString("yyyy.MM.dd");
                Age = l.Age;
                MotherName = l.MotherName;
                DrivedHours = l.DrivedHours;
                HasMedicalExam = l.HasMedicalExam;
                HasTrafficPoliceExam = l.HasTrafficPoliceExam;
                InstructorName = instructor;
            }
        }
    }
}
