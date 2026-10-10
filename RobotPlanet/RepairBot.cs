using System;
using System.Collections.Generic;
using System.Text;

namespace RobotPlanet.Core
{
    public class RepairBot : Robot, IChargeable, IRepair
    {
        public RepairBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 12;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: seadme parandamiseks pole piisavalt energiat.";
            }

            return $"{Name} parandas katkise laboriantenni. Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 18;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: hulluks paranduseks pole piisavalt energiat.";
            }

            return $"{Name} parandas täiesti töökorras kohvimasina. Nüüd valmistab see ainult suppi! Energia: {Energy}/100.";
        }

        public string Repair()
        {
            const int energyCost = 8;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: parandamiseks pole piisavalt energiat.";
            }

            return $"{Name} parandas katkise laadimisjaama. Energia: {Energy}/100.";
        }
    }
}
