using RobotPlanet.Core;
using RobotPlanet.WPF.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RobotPlanet.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Repository<CleanerBot> cleanerRepository = new();
        private readonly Repository<ExplorerBot> explorerRepository = new();
        private readonly Repository<RepairBot> repairRepository = new();
        private readonly Repository<GuardBot> guardRepository = new();

        private readonly Dictionary<Robot, int> readLogCounts = new();

        private Robot? selectedRobot;
        private string selectedRepositoryName = "CleanerBot";
        private string newRobotName = string.Empty;
        private string newRobotEnergy = "100";
        private string searchText = string.Empty;

        public MainViewModel()
        {
            AddCommand = new RelayCommand(
                AddRobot,
                CanAddRobot);

            RemoveCommand = new RelayCommand(
                RemoveRobot,
                () => SelectedRobot != null);

            PerformActionCommand = new RelayCommand(
                PerformAction,
                () => SelectedRobot != null);

            CrazyActionCommand = new RelayCommand(
                CrazyAction,
                () => SelectedRobot != null);

            ChargeCommand = new RelayCommand(
                Charge,
                () => SelectedRobot is IChargeable);

            ScanCommand = new RelayCommand(
                Scan,
                () => SelectedRobot is IScan);

            RepairCommand = new RelayCommand(
                Repair,
                () => SelectedRobot is IRepair);

            ApplyLinqCommand = new RelayCommand(
                RefreshDisplayedRobots);

            LogMessage("Rakendus käivitati.");
            RefreshDisplayedRobots();
        }

        public ObservableCollection<string> RepositoryNames { get; } = new()
        {
            "CleanerBot",
            "ExplorerBot",
            "RepairBot",
            "GuardBot"
        };

        public ObservableCollection<Robot> Robots { get; } = new();

        public ObservableCollection<string> SummaryResults { get; } = new();

        public ObservableCollection<string> LogMessages { get; } = new();

        public string SelectedRepositoryName
        {
            get => selectedRepositoryName;

            set
            {
                if (selectedRepositoryName == value)
                {
                    return;
                }

                selectedRepositoryName = value;
                OnPropertyChanged();

                SelectedRobot = null;
                RefreshDisplayedRobots();
            }
        }

        public string NewRobotName
        {
            get => newRobotName;

            set
            {
                if (newRobotName == value)
                {
                    return;
                }

                newRobotName = value;
                OnPropertyChanged();

                RaiseAddCanExecuteChanged();
            }
        }

        public string NewRobotEnergy
        {
            get => newRobotEnergy;

            set
            {
                if (newRobotEnergy == value)
                {
                    return;
                }

                newRobotEnergy = value;
                OnPropertyChanged();

                RaiseAddCanExecuteChanged();
            }
        }

        public string SearchText
        {
            get => searchText;

            set
            {
                if (searchText == value)
                {
                    return;
                }

                searchText = value;
                OnPropertyChanged();

                RefreshDisplayedRobots();
            }
        }

        public Robot? SelectedRobot
        {
            get => selectedRobot;

            set
            {
                if (ReferenceEquals(selectedRobot, value))
                {
                    return;
                }

                selectedRobot = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedRobotDetails));
                OnPropertyChanged(nameof(SelectedRobotEnergyText));

                RaiseRobotCommandCanExecuteChanged();
            }
        }

        public string SelectedRobotDetails
        {
            get
            {
                if (SelectedRobot == null)
                {
                    return "Robot pole valitud.";
                }

                return
                    $"Nimi: {SelectedRobot.Name}\n" +
                    $"Tüüp: {SelectedRobot.GetType().Name}\n" +
                    $"Võimed: {GetRobotAbilities(SelectedRobot)}";
            }
        }

        public string SelectedRobotEnergyText
        {
            get
            {
                if (SelectedRobot == null)
                {
                    return "0/100";
                }

                return $"{SelectedRobot.Energy}/100";
            }
        }

        public ICommand AddCommand { get; }

        public ICommand RemoveCommand { get; }

        public ICommand PerformActionCommand { get; }

        public ICommand CrazyActionCommand { get; }

        public ICommand ChargeCommand { get; }

        public ICommand ScanCommand { get; }

        public ICommand RepairCommand { get; }

        public ICommand ApplyLinqCommand { get; }

        private bool CanAddRobot()
        {
            if (string.IsNullOrWhiteSpace(NewRobotName))
            {
                return false;
            }

            if (!int.TryParse(NewRobotEnergy, out int energy))
            {
                return false;
            }

            return energy >= 0 && energy <= 100;
        }

        private void AddRobot()
        {
            if (!int.TryParse(NewRobotEnergy, out int energy))
            {
                LogMessage("Viga: energia peab olema täisarv.");
                return;
            }

            if (energy < 0 || energy > 100)
            {
                LogMessage(
                    "Viga: energia peab olema vahemikus 0 kuni 100.");
                return;
            }

            try
            {
                Robot robot = CreateAndAddRobot(energy);

                RefreshDisplayedRobots();
                SelectedRobot = robot;

                SyncRobotLogs();

                LogMessage(
                    $"Lisati {robot.GetType().Name} nimega " +
                    $"{robot.Name}. Energia: {robot.Energy}/100.");

                NewRobotName = string.Empty;
                NewRobotEnergy = "100";
            }
            catch (ArgumentException exception)
            {
                LogMessage($"Viga: {exception.Message}");
            }
            catch (Exception exception)
            {
                LogMessage(
                    $"Viga: {exception.GetType().Name}: " +
                    $"{exception.Message}");
            }
        }

        private Robot CreateAndAddRobot(int energy)
        {
            switch (SelectedRepositoryName)
            {
                case "CleanerBot":
                    {
                        CleanerBot robot =
                            new CleanerBot(NewRobotName, energy);

                        cleanerRepository.Add(robot);
                        return robot;
                    }

                case "ExplorerBot":
                    {
                        ExplorerBot robot =
                            new ExplorerBot(NewRobotName, energy);

                        explorerRepository.Add(robot);
                        return robot;
                    }

                case "RepairBot":
                    {
                        RepairBot robot =
                            new RepairBot(NewRobotName, energy);

                        repairRepository.Add(robot);
                        return robot;
                    }

                case "GuardBot":
                    {
                        GuardBot robot =
                            new GuardBot(NewRobotName, energy);

                        guardRepository.Add(robot);
                        return robot;
                    }

                default:
                    throw new InvalidOperationException(
                        "Tundmatu roboti tüüp.");
            }
        }

        private void RemoveRobot()
        {
            if (SelectedRobot == null)
            {
                return;
            }

            Robot removedRobot = SelectedRobot;

            switch (removedRobot)
            {
                case CleanerBot cleanerBot:
                    cleanerRepository.Remove(cleanerBot);
                    break;

                case ExplorerBot explorerBot:
                    explorerRepository.Remove(explorerBot);
                    break;

                case RepairBot repairBot:
                    repairRepository.Remove(repairBot);
                    break;

                case GuardBot guardBot:
                    guardRepository.Remove(guardBot);
                    break;

                default:
                    LogMessage(
                        "Viga: valitud roboti hoidlat ei leitud.");
                    return;
            }

            SelectedRobot = null;

            RefreshDisplayedRobots();
            SyncRobotLogs();

            readLogCounts.Remove(removedRobot);

            LogMessage(
                $"Eemaldati {removedRobot.GetType().Name} nimega " +
                $"{removedRobot.Name}.");
        }

        private void PerformAction()
        {
            if (SelectedRobot == null)
            {
                return;
            }

            string result = SelectedRobot.PerformAction();

            LogMessage(result);
            RefreshRobotState();
        }

        private void CrazyAction()
        {
            if (SelectedRobot == null)
            {
                return;
            }

            string result;

            switch (SelectedRobot)
            {
                case CleanerBot cleanerBot:
                    result = cleanerRepository
                        .TriggerCrazyAction(cleanerBot);
                    break;

                case ExplorerBot explorerBot:
                    result = explorerRepository
                        .TriggerCrazyAction(explorerBot);
                    break;

                case RepairBot repairBot:
                    result = repairRepository
                        .TriggerCrazyAction(repairBot);
                    break;

                case GuardBot guardBot:
                    result = guardRepository
                        .TriggerCrazyAction(guardBot);
                    break;

                default:
                    LogMessage(
                        "Viga: valitud roboti hoidlat ei leitud.");
                    return;
            }

            LogMessage(result);

            SyncRobotLogs();
            RefreshRobotState();
        }

        private void Charge()
        {
            if (SelectedRobot is not IChargeable chargeable)
            {
                return;
            }

            string result = chargeable.Charge(10);

            LogMessage(result);
            RefreshRobotState();
        }

        private void Scan()
        {
            if (SelectedRobot is not IScan scanner)
            {
                return;
            }

            string result = scanner.Scan();

            LogMessage(result);
            RefreshRobotState();
        }

        private void Repair()
        {
            if (SelectedRobot is not IRepair repairable)
            {
                return;
            }

            string result = repairable.Repair();

            LogMessage(result);
            RefreshRobotState();
        }

        private void RefreshDisplayedRobots()
        {
            Robot? previousSelection = SelectedRobot;

            IEnumerable<Robot> filteredRobots =
                RobotQueries.SearchByName(
                    GetSelectedRepositoryRobots(),
                    SearchText);

            IEnumerable<Robot> sortedRobots =
                RobotQueries.SortByName(filteredRobots);

            Robots.Clear();

            foreach (Robot robot in sortedRobots)
            {
                Robots.Add(robot);
            }

            RefreshSummary();

            if (previousSelection != null &&
                Robots.Contains(previousSelection))
            {
                SelectedRobot = previousSelection;
            }
            else if (previousSelection != null)
            {
                SelectedRobot = null;
            }
        }

        private void RefreshSummary()
        {
            SummaryResults.Clear();

            IEnumerable<string> summary =
                RobotQueries.GroupByType(GetAllRobots());

            foreach (string line in summary)
            {
                SummaryResults.Add(line);
            }

            if (SummaryResults.Count == 0)
            {
                SummaryResults.Add("Roboteid pole lisatud.");
            }
        }

        private IEnumerable<Robot> GetSelectedRepositoryRobots()
        {
            return SelectedRepositoryName switch
            {
                "CleanerBot" =>
                    cleanerRepository.GetAll().Cast<Robot>(),

                "ExplorerBot" =>
                    explorerRepository.GetAll().Cast<Robot>(),

                "RepairBot" =>
                    repairRepository.GetAll().Cast<Robot>(),

                "GuardBot" =>
                    guardRepository.GetAll().Cast<Robot>(),

                _ => Enumerable.Empty<Robot>()
            };
        }

        private IEnumerable<Robot> GetAllRobots()
        {
            IEnumerable<Robot> cleaners =
                cleanerRepository.GetAll().Cast<Robot>();

            IEnumerable<Robot> explorers =
                explorerRepository.GetAll().Cast<Robot>();

            IEnumerable<Robot> repairBots =
                repairRepository.GetAll().Cast<Robot>();

            IEnumerable<Robot> guardBots =
                guardRepository.GetAll().Cast<Robot>();

            return cleaners
                .Concat(explorers)
                .Concat(repairBots)
                .Concat(guardBots);
        }

        private void SyncRobotLogs()
        {
            foreach (Robot robot in GetAllRobots())
            {
                int startIndex = readLogCounts.TryGetValue(
                    robot,
                    out int savedCount)
                    ? savedCount
                    : 0;

                for (int index = startIndex;
                     index < robot.Log.Count;
                     index++)
                {
                    LogMessage(robot.Log[index]);
                }

                readLogCounts[robot] = robot.Log.Count;
            }
        }

        private void RefreshRobotState()
        {
            OnPropertyChanged(nameof(SelectedRobot));
            OnPropertyChanged(nameof(SelectedRobotDetails));
            OnPropertyChanged(nameof(SelectedRobotEnergyText));

            RefreshDisplayedRobots();
        }

        private void LogMessage(string message)
        {
            LogMessages.Insert(
                0,
                $"{DateTime.Now:HH:mm:ss} – {message}");
        }

        private static string GetRobotAbilities(Robot robot)
        {
            List<string> abilities = new()
            {
                "tavaline tegevus",
                "Crazy Action"
            };

            if (robot is IChargeable)
            {
                abilities.Add("laadimine");
            }

            if (robot is IScan)
            {
                abilities.Add("skaneerimine");
            }

            if (robot is IRepair)
            {
                abilities.Add("parandamine");
            }

            return string.Join(", ", abilities);
        }

        private void RaiseAddCanExecuteChanged()
        {
            if (AddCommand is RelayCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
        }

        private void RaiseRobotCommandCanExecuteChanged()
        {
            RaiseCanExecuteChanged(RemoveCommand);
            RaiseCanExecuteChanged(PerformActionCommand);
            RaiseCanExecuteChanged(CrazyActionCommand);
            RaiseCanExecuteChanged(ChargeCommand);
            RaiseCanExecuteChanged(ScanCommand);
            RaiseCanExecuteChanged(RepairCommand);
        }

        private static void RaiseCanExecuteChanged(ICommand command)
        {
            if (command is RelayCommand relayCommand)
            {
                relayCommand.RaiseCanExecuteChanged();
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