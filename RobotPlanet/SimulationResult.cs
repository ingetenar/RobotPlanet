using System;
using System.Collections.Generic;
using System.Text;

namespace RobotPlanet.Core
{
    public class SimulationResult
    {
        public string RobotName { get; set; } = string.Empty;

        public string FinalState { get; set; } = string.Empty;

        public string ResultMessage { get; set; } = string.Empty;

        public long DurationMilliseconds { get; set; }

        public bool IsSuccessful { get; set; }

        public bool IsCancelled { get; set; }

        public bool HasError { get; set; }
    }
}