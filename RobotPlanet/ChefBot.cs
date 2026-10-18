namespace RobotPlanet.Core
{
    public class ChefBot : Robot, IChargeable
    {
        public ChefBot(string name, int energy)
            : base(name, energy)
        {
        }

        public override string PerformAction()
        {
            const int energyCost = 9;

            if (!TryUseEnergy(energyCost))
            {
                return
                    $"{Name}: toidu valmistamiseks pole piisavalt energiat.";
            }

            return
                $"{Name} valmistas robotitele metallipitsa. " +
                $"Energia: {Energy}/100.";
        }

        public override string CrazyAction()
        {
            const int energyCost = 19;

            if (!TryUseEnergy(energyCost))
            {
                return
                    $"{Name}: Crazy Action jaoks pole piisavalt energiat.";
            }

            return
                $"{Name} küpsetas mutritest sünnipäevatordi! " +
                $"Energia: {Energy}/100.";
        }
    }
}
