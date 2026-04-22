using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutoNelkuliIskolaWPF.UserControls
{
    public partial class CreateInstructorView : UserControl
    {
        public CreateInstructorView()
        {
            InitializeComponent();
            Refresh();
        }

        public void Refresh()
        {
            LstInstructors.ItemsSource = App.School
                .ListAllInstructorsWithLearners()
                .Select(kv => $"👨‍🏫 {kv.Key}  ({kv.Value.Count} tanuló)")
                .ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                ShowMessage("Kérjük add meg az oktató nevét!", false);
                return;
            }

            bool exists = App.School.ListAllInstructorsWithLearners().ContainsKey(name);
            if (exists)
            {
                ShowMessage($"'{name}' névvel már létezik oktató!", false);
                return;
            }

            App.School.AddNewInstructor(name);
            TxtName.Clear();
            Refresh();
            ShowMessage($"'{name}' oktató sikeresen hozzáadva!", true);
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
