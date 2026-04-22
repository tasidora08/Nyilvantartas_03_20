using AutoNelkuliIskola;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoNelkuliIskolaWPF.UserControls
{
    public partial class EditInstructorView : UserControl
    {
        private string? _selectedInstructor;

        public EditInstructorView()
        {
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            var keys = App.School.ListAllInstructorsWithLearners().Keys.ToList();
            CmbInstructors.ItemsSource = keys;
            CmbInstructors.SelectedIndex = -1;
            FormPanel.Visibility = Visibility.Collapsed;
            StudentsPanel.Visibility = Visibility.Collapsed;
            MsgBorder.Visibility = Visibility.Collapsed;
            _selectedInstructor = null;
        }

        private void CmbInstructors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedInstructor = CmbInstructors.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(_selectedInstructor))
            {
                FormPanel.Visibility = Visibility.Collapsed;
                StudentsPanel.Visibility = Visibility.Collapsed;
                return;
            }

            TxtName.Text = _selectedInstructor;
            MsgBorder.Visibility = Visibility.Collapsed;
            FormPanel.Visibility = Visibility.Visible;

            RefreshStudentList();
        }

        private void RefreshStudentList()
        {
            if (_selectedInstructor == null) return;

            var dict = App.School.ListAllInstructorsWithLearners();
            if (!dict.ContainsKey(_selectedInstructor))
            {
                StudentsPanel.Visibility = Visibility.Collapsed;
                return;
            }

            var students = dict[_selectedInstructor];
            TxtStudentsHeader.Text = $"Tanulók ({students.Count} fő)";
            DgStudents.ItemsSource = students.ToList();

            // Move student combos
            CmbMoveStudent.ItemsSource = students.Select(l => l.LearnerName).ToList();
            CmbMoveStudent.SelectedIndex = -1;

            var otherInstructors = dict.Keys.Where(k => k != _selectedInstructor).ToList();
            CmbTargetInstructor.ItemsSource = otherInstructors;
            CmbTargetInstructor.SelectedIndex = -1;

            StudentsPanel.Visibility = Visibility.Visible;
        }

        private void BtnRename_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInstructor == null) return;

            string newName = TxtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(newName))
            {
                ShowMessage("Az oktató neve nem lehet üres!", false);
                return;
            }

            if (newName == _selectedInstructor)
            {
                ShowMessage("Az új név azonos a jelenlegivel!", false);
                return;
            }

            bool exists = App.School.ListAllInstructorsWithLearners().ContainsKey(newName);
            if (exists)
            {
                ShowMessage($"'{newName}' névvel már létezik oktató!", false);
                return;
            }

            // Rename = add new, move all students, delete old
            var dict = App.School.ListAllInstructorsWithLearners();
            var students = dict[_selectedInstructor].ToList();

            App.School.AddNewInstructor(newName);
            foreach (var s in students)
                App.School.UpdateLearner(s, newName);

            App.School.DeleteInstructor(_selectedInstructor);

            string oldName = _selectedInstructor;
            _selectedInstructor = newName;
            Refresh();
            ShowMessage($"'{oldName}' → '{newName}' átnevezve!", true);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInstructor == null) return;

            var result = MessageBox.Show(
                $"Biztosan törlöd '{_selectedInstructor}' oktatót? A tanulói oktató nélkül maradnak.",
                "Törlés megerősítése",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string deleted = _selectedInstructor;
                App.School.DeleteInstructor(_selectedInstructor);
                Refresh();
                ShowMessage($"'{deleted}' oktató törölve!", true);
            }
        }

        private void BtnMove_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedInstructor == null) return;

            string? studentName = CmbMoveStudent.SelectedItem?.ToString();
            string? targetInstructor = CmbTargetInstructor.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(studentName) || string.IsNullOrEmpty(targetInstructor))
            {
                ShowMessage("Válassz tanulót és cél oktatót!", false);
                return;
            }

            var learner = App.School.ListAllInstructorsWithLearners()[_selectedInstructor]
                .FirstOrDefault(l => l.LearnerName == studentName);

            if (learner == null)
            {
                ShowMessage("Tanuló nem található!", false);
                return;
            }

            App.School.UpdateLearner(learner, targetInstructor);
            RefreshStudentList();
            ShowMessage($"'{studentName}' áthelyezve '{targetInstructor}' oktatóhoz!", true);
        }

        private void ShowMessage(string msg, bool success)
        {
            TxtMessage.Text = msg;
            MsgBorder.Background = success
                ? new SolidColorBrush(Color.FromRgb(198, 246, 213))
                : new SolidColorBrush(Color.FromRgb(254, 215, 215));
            TxtMessage.Foreground = success
                ? new SolidColorBrush(Color.FromRgb(34, 84, 61))
                : new SolidColorBrush(Color.FromRgb(130, 40, 40));
            MsgBorder.Visibility = Visibility.Visible;
        }
    }
}
