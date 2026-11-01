using RobotPlanet.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RobotPlanet.WPF.ViewModels
{
    public class SimulationRobotOption : INotifyPropertyChanged
    {
        private bool isSelected;

        public SimulationRobotOption(Robot robot)
        {
            Robot = robot;
        }

        public Robot Robot { get; }

        public string DisplayName =>
            $"{Robot.Name} ({Robot.GetType().Name}, Energy: {Robot.Energy})";

        public bool IsSelected
        {
            get => isSelected;

            set
            {
                if (isSelected == value)
                {
                    return;
                }

                isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}