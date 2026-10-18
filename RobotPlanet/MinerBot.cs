namespace RobotPlanet.Core
{
    public class MinerBot : Robot, IChargeable, IScan
    {
        public MinerBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 12;

            if (!TryUseEnergy(energyCost))
            {
                return
                    $"{Name}: kaevandamiseks pole piisavalt energiat.";
            }

            return
                $"{Name} kaevandas haruldast kosmosemetalli. " +
                $"Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 22;

            if (!TryUseEnergy(energyCost))
            {
                return
                    $"{Name}: hulluks kaevandamiseks pole piisavalt energiat.";
            }

            return
                $"{Name} kaevas tunneli otse roboti garaaži! " +
                $"Energia: {Energy}/100.";
        }

        public string Scan()
        {
            const int energyCost = 6;

            if (!TryUseEnergy(energyCost))
            {
                return
                    $"{Name}: skaneerimiseks pole piisavalt energiat.";
            }

            return
                $"{Name} leidis maa alt energiapatarei. " +
                $"Energia: {Energy}/100.";
        }
    }
}
