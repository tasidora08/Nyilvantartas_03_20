using AutoNelkuliIskola;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoNelkuliIskolaWPF.UserControls
{
    public partial class EditStudentView : UserControl
    {
        private Learner? _selectedLearner;

        public EditStudentView()
        {
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            var allLearners = App.School.FilterByName("");
            CmbStudents.ItemsSource = allLearners.Select(l => l.LearnerName).ToList();
            CmbStudents.SelectedIndex = -1;
            FormPanel.Visibility = Visibility.Collapsed;
            MsgBorder.Visibility = Visibility.Collapsed;
            _selectedLearner = null;

            // Refresh instructor combo
            var instructors = App.School.ListAllInstructorsWithLearners().Keys.ToList();
            instructors.Insert(0, "— Nincs oktató —");
            CmbInstructor.ItemsSource = instructors;
        }

        private void CmbStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selected = CmbStudents.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selected))
            {
                FormPanel.Visibility = Visibility.Collapsed;
                return;
            }

            _selectedLearner = App.School.FilterByName(selected)
                .FirstOrDefault(l => l.LearnerName == selected);

            if (_selectedLearner == null)
            {
                FormPanel.Visibility = Visibility.Collapsed;
                return;
            }

            // Fill form
            TxtName.Text = _selectedLearner.LearnerName;
            DpBornDate.SelectedDate = _selectedLearner.BornDate.ToDateTime(TimeOnly.MinValue);
            TxtMotherName.Text = _selectedLearner.MotherName;
            TxtHours.Text = _selectedLearner.DrivedHours.ToString();
            ChkMedical.IsChecked = _selectedLearner.HasMedicalExam;
            ChkKresz.IsChecked = _selectedLearner.HasTrafficPoliceExam;

            // Set current instructor in combobox
            var dict = App.School.ListAllInstructorsWithLearners();
            string currentInstructor = dict
                .FirstOrDefault(kv => kv.Value.Any(l => l.LearnerName == _selectedLearner.LearnerName))
                .Key ?? "— Nincs oktató —";
            CmbInstructor.SelectedItem = currentInstructor;

            MsgBorder.Visibility = Visibility.Collapsed;
            FormPanel.Visibility = Visibility.Visible;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLearner == null) return;

            string newName = TxtName.Text.Trim();
            string newMotherName = TxtMotherName.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newMotherName))
            {
                ShowMessage("A név és az anyja neve nem lehet üres!", false);
                return;
            }

            if (!int.TryParse(TxtHours.Text.Trim(), out int hours) || hours < 0)
            {
                ShowMessage("A vezetett órák száma nem érvényes!", false);
                return;
            }

            if (DpBornDate.SelectedDate == null)
            {
                ShowMessage("Kérjük add meg a születési dátumot!", false);
                return;
            }

            // Update basic data
            string oldName = _selectedLearner.LearnerName;
            DateOnly newBorn = DateOnly.FromDateTime(DpBornDate.SelectedDate.Value);
            App.School.UpdateLearnerData(_selectedLearner, newName, newBorn, newMotherName);
            App.School.ChangeDrivedHours(_selectedLearner, hours);
            App.School.SetMedicalExam(ChkMedical.IsChecked == true, _selectedLearner);
            App.School.SetTrafficPoliceEXam(ChkKresz.IsChecked == true, _selectedLearner);

            // Update instructor assignment
            string newInstructor = CmbInstructor.SelectedItem?.ToString() ?? "— Nincs oktató —";
            var dict = App.School.ListAllInstructorsWithLearners();
            string? currentInstructor = dict
                .FirstOrDefault(kv => kv.Value.Any(l => l.LearnerName == _selectedLearner.LearnerName))
                .Key;

            if (newInstructor != "— Nincs oktató —" && newInstructor != currentInstructor)
            {
                if (currentInstructor != null)
                    App.School.UpdateLearner(_selectedLearner, newInstructor);
                else
                    App.School.AddNewLearner(newInstructor, _selectedLearner);
            }

            Refresh();
            ShowMessage($"'{newName}' tanuló adatai sikeresen frissítve!", true);
            FormPanel.Visibility = Visibility.Collapsed;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLearner == null) return;

            var result = MessageBox.Show(
                $"Biztosan törlöd '{_selectedLearner.LearnerName}' tanulót?",
                "Törlés megerősítése",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                string deletedName = _selectedLearner.LearnerName;
                App.School.DeleteLearnerData(_selectedLearner);
                Refresh();
                ShowMessage($"'{deletedName}' tanuló törölve!", true);
            }
        }

        private void BtnReady_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedLearner == null) return;
            string result = App.School.IsReadyForTheExam(_selectedLearner);
            bool isReady = result.Contains("rendelkezik a vizsgához");
            ShowMessage(result, isReady);
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
