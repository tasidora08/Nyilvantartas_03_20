using AutoNelkuliIskola;
using System.Windows;

namespace AutoNelkuliIskolaWPF
{
    public partial class App : Application
    {
        public static DrivingSchool School { get; } = new DrivingSchool("AutoNélküli Autósiskola");
    }
}
