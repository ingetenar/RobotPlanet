using System;
using System.Collections.Generic;
using System.Text;

namespace RobotPlanet.Core
{
    public class CleanerBot : Robot, IChargeable
    {
        public CleanerBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 10;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: koristamiseks pole piisavalt energiat.";
            }

            return $"{Name} koristas labori põranda. Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 20;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: koristustornaado käivitamiseks pole piisavalt energiat.";
            }

            return $"{Name} käivitas koristustornaado ja puhastas kogemata ka seintelt värvi! Energia: {Energy}/100.";
        }
    }
}