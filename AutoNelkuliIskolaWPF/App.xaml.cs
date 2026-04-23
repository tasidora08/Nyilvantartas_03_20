using AutoNelkuliIskola;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace AutoNelkuliIskolaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DrivingSchool School { get; } = new DrivingSchool("AutoNélküli Autósiskola");
        
    }

}
