using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutoNelkuliIskola;
using AutoNelkuliIskolaWPF.UserControls;

namespace AutoNelkuliIskolaWPF.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static DrivingSchool drivingSchool = new DrivingSchool("AutonelkuliIskola");

        public MainPage()
        {
            InitializeComponent();
            CreateDrivingSchool();
        }

        private void CreateStudent_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CreateStudentView();
        }

        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EditStudentView();
            MainContentViewer.Content = new DataListsView();
        }


        private void CreateDrivingSchool()
        {
        }
    }
}
