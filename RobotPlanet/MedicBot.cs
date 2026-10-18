namespace RobotPlanet.Core
{
    public class MedicBot : Robot, IChargeable, IRepair
    {
        public MedicBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 10;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: ravimiseks pole piisavalt energiat.";
            }

            return
                $"{Name} parandas teise roboti juhtmed. " +
                $"Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 20;

            if (!TryUseEnergy(energyCost))
            {
                return
                    $"{Name}: hulluks ravimiseks pole piisavalt energiat.";
            }

            return
                $"{Name} ravis katkist rösterit ja kuulutas selle terveks! " +
                $"Energia: {Energy}/100.";
        }

        public string Repair()
        {
            const int energyCost = 8;

            if (!TryUseEnergy(energyCost))
            {
                return $"{Name}: parandamiseks pole piisavalt energiat.";
            }

            return
                $"{Name} parandas roboti energiaploki. " +
                $"Energia: {Energy}/100.";
        }
    }
}
