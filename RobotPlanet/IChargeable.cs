using System;
using System.Collections.Generic;
using System.Text;

namespace RobotPlanet.Core
{
    public interface IChargeable
    {
        string Charge(int amount);
    }
}