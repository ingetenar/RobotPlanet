namespace RobotPlanet.Core
{
    public class GuardBot : Robot, IChargeable, IScan
    {
        public GuardBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 14;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: ala valvamiseks pole piisavalt energiat.";
            }

            return $"{Name} valvas labori sissepääsu ja kontrollis kõiki külalisi. Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 22;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: häire käivitamiseks pole piisavalt energiat.";
            }

            return $"{Name} kuulutas prügikasti kahtlaseks ja käivitas kogu planeedi häire! Energia: {Energy}/100.";
        }

        public string Scan()
        {
            const int energyCost = 6;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: turvaskaneerimiseks pole piisavalt energiat.";
            }

            return $"{Name} skaneeris ümbrust ja avastas salaja liikunud kontoritooli. Energia: {Energy}/100.";
        }
    }
}
