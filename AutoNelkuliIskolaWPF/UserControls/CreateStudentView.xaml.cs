using AutoNelkuliIskola;
using AutoNelkuliIskolaWPF.Pages;
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

namespace AutoNelkuliIskolaWPF.UserControls
{
    /// <summary>
    /// Interaction logic for CreateStudentView.xaml
    /// </summary>
    public partial class CreateStudentView : UserControl
    {
        public CreateStudentView()
        {
            InitializeComponent();
        }

        private void CreateStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameTextBox.Text != "" && bornDate.Text != "" && motherNameTextBox.Text != "")
            {
                feedbackText.Content = "Tanuló sikeresen létrehozva!";
                Learner learner = new Learner(nameTextBox.Text,DateOnly.Parse(bornDate.Text), motherNameTextBox.Text);
                MainPage.drivingSchool.CreateLearner(learner);
            }
            else
            {
                feedbackText.Content = "Hiányzó adatok!";
            }
        }
    }
}
