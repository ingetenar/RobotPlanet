using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RobotPlanet.Core
{
    public abstract class Robot : INotifyPropertyChanged
    {
        private int energy;

        public string Name { get; }

        public int Energy
        {
            get => energy;
            private set
            {
                if (energy == value)
                {
                    return;
                }

                energy = value;
                OnPropertyChanged();
            }
        }

        public List<string> Log { get; } = new();

        protected Robot(string name, int energy)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Roboti nimi ei tohi olla tühi.");
            }

            if (energy < 0 || energy > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(energy),
                    "Energia peab olema vahemikus 0 kuni 100.");
            }

            Name = name.Trim();
            this.energy = energy;
        }

        public string Charge(int amount)
        {
            if (amount <= 0)
            {
                return "Laadimise kogus peab olema suurem kui null.";
            }

            if (Energy + amount > 100)
            {
                return $"{Name}: energiat ei saa laadida üle 100.";
            }

            Energy += amount;

            return
                $"{Name} laadis {amount} energiapunkti. " +
                $"Energia: {Energy}/100.";
        }

        protected bool TryUseEnergy(int amount)
        {
            if (amount <= 0 || Energy < amount)
            {
                return false;
            }

            Energy -= amount;
            return true;
        }

        protected void IncreaseEnergy(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Energy = Math.Min(100, Energy + amount);
        }

        public virtual void Welcome(Robot robot)
        {
            Log.Add(
                $"{Name} tervitab uut robotit {robot.Name}.");
        }

        public virtual void Goodbye(Robot robot)
        {
            Log.Add(
                $"{Name} jätab robotiga {robot.Name} hüvasti.");
        }

        public virtual void ReactToCrazyAction(Robot robot)
        {
            IncreaseEnergy(5);

            Log.Add(
                $"{Name} reageeris roboti {robot.Name} " +
                $"Crazy Action tegevusele. Energia: {Energy}/100.");
        }

        public abstract string PerformAction();

        public abstract string CrazyAction();

        public override string ToString()
        {
            return
                $"{Name} ({GetType().Name}) – energia: {Energy}/100";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}