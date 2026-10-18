using RobotPlanet.WPF.ViewModels;
using System.Windows;

namespace RobotPlanet.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}