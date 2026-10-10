using RobotPlanet.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace RobotPlanet.WPF
{
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Robot> robots = new();

        public MainWindow()
        {
            InitializeComponent();

            RobotsListBox.ItemsSource = robots;

            AddLogMessage("Rakendus käivitati.");
        }

        private Robot? GetSelectedRobot()
        {
            return RobotsListBox.SelectedItem as Robot;
        }

        private void AddRobotButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                AddLogMessage("Viga: roboti nimi ei tohi olla tühi.");
                return;
            }

            if (!int.TryParse(EnergyTextBox.Text, out int energy))
            {
                AddLogMessage("Viga: energia peab olema täisarv.");
                return;
            }

            if (energy < 0 || energy > 100)
            {
                AddLogMessage("Viga: energia peab olema vahemikus 0 kuni 100.");
                return;
            }

            if (RobotTypeComboBox.SelectedItem is not ComboBoxItem selectedItem)
            {
                AddLogMessage("Viga: vali roboti tüüp.");
                return;
            }

            string? robotType = selectedItem.Content?.ToString();

            try
            {
                Robot robot;

                switch (robotType)
                {
                    case "CleanerBot":
                        robot = new CleanerBot(name, energy);
                        break;

                    case "ExplorerBot":
                        robot = new ExplorerBot(name, energy);
                        break;

                    case "RepairBot":
                        robot = new RepairBot(name, energy);
                        break;

                    default:
                        AddLogMessage("Viga: tundmatu roboti tüüp.");
                        return;
                }

                robots.Add(robot);
                RobotsListBox.SelectedItem = robot;

                AddLogMessage(
                    $"Lisati {robot.GetType().Name} nimega {robot.Name}. Energia: {robot.Energy}/100.");

                NameTextBox.Clear();
                EnergyTextBox.Text = "100";
                NameTextBox.Focus();

                UpdateSelectedRobotDetails();
            }
            catch (ArgumentException exception)
            {
                AddLogMessage($"Viga: {exception.Message}");
            }
            catch (Exception)
            {
                AddLogMessage("Viga: robotit ei õnnestunud lisada.");
            }
        }

        private void RemoveRobotButton_Click(object sender, RoutedEventArgs e)
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                AddLogMessage("Viga: eemaldamiseks vali kõigepealt robot.");
                return;
            }

            string robotName = selectedRobot.Name;
            string robotType = selectedRobot.GetType().Name;

            robots.Remove(selectedRobot);

            AddLogMessage($"Eemaldati {robotType} nimega {robotName}.");

            UpdateSelectedRobotDetails();
        }

        private void RobotsListBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            UpdateSelectedRobotDetails();
        }

        private void PerformActionButton_Click(object sender, RoutedEventArgs e)
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                AddLogMessage("Viga: vali kõigepealt robot.");
                return;
            }

            string message = selectedRobot.PerformAction();

            AddLogMessage(message);
            RefreshRobotInformation(selectedRobot);
        }

        private void CrazyActionButton_Click(object sender, RoutedEventArgs e)
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                AddLogMessage("Viga: vali kõigepealt robot.");
                return;
            }

            string message = selectedRobot.CrazyAction();

            AddLogMessage(message);
            RefreshRobotInformation(selectedRobot);
        }

        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                AddLogMessage("Viga: vali kõigepealt robot.");
                return;
            }

            if (selectedRobot is IChargeable chargeable)
            {
                string message = chargeable.Charge(10);

                AddLogMessage(message);
                RefreshRobotInformation(selectedRobot);
            }
            else
            {
                AddLogMessage(
                    $"{selectedRobot.Name} ei toeta laadimist.");
            }
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                AddLogMessage("Viga: vali kõigepealt robot.");
                return;
            }

            if (selectedRobot is IScan scannable)
            {
                string message = scannable.Scan();

                AddLogMessage(message);
                RefreshRobotInformation(selectedRobot);
            }
            else
            {
                AddLogMessage(
                    $"{selectedRobot.Name} ei oska skaneerida.");
            }
        }

        private void RepairButton_Click(object sender, RoutedEventArgs e)
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                AddLogMessage("Viga: vali kõigepealt robot.");
                return;
            }

            if (selectedRobot is IRepair repairable)
            {
                string message = repairable.Repair();

                AddLogMessage(message);
                RefreshRobotInformation(selectedRobot);
            }
            else
            {
                AddLogMessage(
                    $"{selectedRobot.Name} ei oska parandada.");
            }
        }

        private void UpdateSelectedRobotDetails()
        {
            Robot? selectedRobot = GetSelectedRobot();

            if (selectedRobot is null)
            {
                SelectedRobotTextBlock.Text = "Robot pole valitud.";
                EnergyProgressBar.Value = 0;
                EnergyTextBlock.Text = "0/100";
                return;
            }

            SelectedRobotTextBlock.Text =
                $"Nimi: {selectedRobot.Name}\n" +
                $"Tüüp: {selectedRobot.GetType().Name}\n" +
                $"Võimed: {GetRobotAbilities(selectedRobot)}";

            EnergyProgressBar.Value = selectedRobot.Energy;
            EnergyTextBlock.Text = $"{selectedRobot.Energy}/100";
        }

        private string GetRobotAbilities(Robot robot)
        {
            string abilities = "Tavaline tegevus, Crazy Action";

            if (robot is IChargeable)
            {
                abilities += ", laadimine";
            }

            if (robot is IScan)
            {
                abilities += ", skaneerimine";
            }

            if (robot is IRepair)
            {
                abilities += ", parandamine";
            }

            return abilities;
        }

        private void RefreshRobotInformation(Robot selectedRobot)
        {
            RobotsListBox.Items.Refresh();
            RobotsListBox.SelectedItem = selectedRobot;

            UpdateSelectedRobotDetails();
        }

        private void AddLogMessage(string message)
        {
            string logEntry = $"{DateTime.Now:HH:mm:ss} – {message}";

            LogListBox.Items.Insert(0, logEntry);
        }
    }
}