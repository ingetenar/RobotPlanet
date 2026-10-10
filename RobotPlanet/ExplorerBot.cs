using System;
using System.Collections.Generic;
using System.Text;

namespace RobotPlanet.Core
{
    public class ExplorerBot : Robot, IChargeable, IScan
    {
        public ExplorerBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 15;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: uurimiseks pole piisavalt energiat.";
            }

            return $"{Name} uuris tundmatut kraatrit ja leidis kadunud laadimiskaabli. Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 25;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: tundmatu objekti uurimiseks pole piisavalt energiat.";
            }

            return $"{Name} skaneeris tundmatut eluvormi. Objekt osutus rösteriks! Energia: {Energy}/100.";
        }
        public string Scan()
        {
            const int energyCost = 5;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: skaneerimiseks pole piisavalt energiat.";
            }

            return $"{Name} skaneeris ümbrust ja leidis metallist banaani. Energia: {Energy}/100.";
        }
    }
}