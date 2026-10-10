using System;
using System.Collections.Generic;
using System.Text;

namespace RobotPlanet.Core
{
    public abstract class Robot
    {
        public string Name { get; }
        public int Energy { get; private set; }

        protected Robot(string name, int energy)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Roboti nimi ei tohi olla tühi.");
            }

            if (energy < 0 || energy > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(energy),
                    "Energia peab olema vahemikus 0 kuni 100.");
            }

            Name = name.Trim();
            Energy = energy;
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

            return $"{Name} laadis {amount} energiapunkti. Energia: {Energy}/100.";
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

        public abstract string PerformAction();

        public abstract string CrazyAction();

        public override string ToString()
        {
            return $"{Name} ({GetType().Name}) – energia: {Energy}/100";
        }
    }
}