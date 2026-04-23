using AutoNelkuliIskolaWPF.UserControls;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace AutoNelkuliIskolaWPF.Pages
{
    public partial class MainPage : System.Windows.Controls.Page
    {
        private DataListsView _dataListsView;
        private CreateInstructorView _createInstructorView;
        private CreateStudentView _createStudentView;
        private EditStudentView _editStudentView;
        private EditInstructorView _editInstructorView;

        private Button? _activeButton;

        public AutoSaver Saver { get; } = new AutoSaver();

        public MainPage()
        {
            InitializeComponent();

            DataContext = this;

            Saver.StartAutoSave();

            _dataListsView = new DataListsView();
            _createInstructorView = new CreateInstructorView();
            _createStudentView = new CreateStudentView();
            _editStudentView = new EditStudentView();
            _editInstructorView = new EditInstructorView();

            MainContent.Content = _dataListsView;
            _activeButton = BtnDataLists;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Saver.StartAutoSave();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Saver.StopAutoSave();
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;

            // Style swap
            if (_activeButton != null)
                _activeButton.Style = (Style)Resources["NavButton"];
            btn.Style = (Style)Resources["NavButtonActive"];
            _activeButton = btn;

            string tag = btn.Tag?.ToString() ?? "";
            switch (tag)
            {
                case "DataLists":
                    _dataListsView.Refresh();
                    MainContent.Content = _dataListsView;
                    break;
                case "CreateInstructor":
                    _createInstructorView.Refresh();
                    MainContent.Content = _createInstructorView;
                    break;
                case "CreateStudent":
                    _createStudentView.Refresh();
                    MainContent.Content = _createStudentView;
                    break;
                case "EditStudent":
                    _editStudentView.Refresh();
                    MainContent.Content = _editStudentView;
                    break;
                case "EditInstructor":
                    _editInstructorView.Refresh();
                    MainContent.Content = _editInstructorView;
                    break;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = ".txt",
                FileName = "autoskola_adatok"
            };
            if (dlg.ShowDialog() == true)
            {
                App.School.SaveToFile(dlg.FileName);
                MessageBox.Show("Fájl sikeresen mentve!", "Mentés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = ".txt"
            };
            if (dlg.ShowDialog() == true)
            {
                App.School.LoadFromFile(dlg.FileName);
                _dataListsView.Refresh();
                _createStudentView.Refresh();
                _editStudentView.Refresh();
                _editInstructorView.Refresh();
                MessageBox.Show("Fájl sikeresen betöltve!", "Betöltés", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
