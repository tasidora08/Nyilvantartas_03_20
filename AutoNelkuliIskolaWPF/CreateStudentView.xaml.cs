using AutoNelkuliIskola;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoNelkuliIskolaWPF.UserControls
{
    public partial class CreateStudentView : UserControl
    {
        public CreateStudentView()
        {
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            var instructors = App.School.ListAllInstructorsWithLearners().Keys.ToList();
            instructors.Insert(0, "— Nincs oktató —");
            CmbInstructor.ItemsSource = instructors;
            CmbInstructor.SelectedIndex = 0;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();
            string motherName = TxtMotherName.Text.Trim();
            string hoursText = TxtHours.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(motherName))
            {
                ShowMessage("Kérjük töltsd ki a kötelező mezőket (*)!", false);
                return;
            }

            if (DpBornDate.SelectedDate == null)
            {
                ShowMessage("Kérjük add meg a születési dátumot!", false);
                return;
            }

            if (!int.TryParse(hoursText, out int hours) || hours < 0)
            {
                ShowMessage("A vezetett órák száma nem érvényes!", false);
                return;
            }

            var bornDate = DateOnly.FromDateTime(DpBornDate.SelectedDate.Value);
            var learner = new Learner(
                learnerName: name,
                bornDate: bornDate,
                motherName: motherName,
                hasMedicalExam: ChkMedical.IsChecked == true,
                hasTrafficPoliceExam: ChkKresz.IsChecked == true,
                drivedHours: hours
            );

            App.School.CreateLearner(learner);

            string selectedInstructor = CmbInstructor.SelectedItem?.ToString() ?? "";
            if (selectedInstructor != "— Nincs oktató —" && !string.IsNullOrEmpty(selectedInstructor))
            {
                App.School.AddNewLearner(selectedInstructor, learner);
            }

            // Reset form
            TxtName.Clear();
            TxtMotherName.Clear();
            TxtHours.Text = "0";
            DpBornDate.SelectedDate = null;
            ChkMedical.IsChecked = false;
            ChkKresz.IsChecked = false;
            CmbInstructor.SelectedIndex = 0;

            ShowMessage($"'{name}' tanuló sikeresen hozzáadva!", true);
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
