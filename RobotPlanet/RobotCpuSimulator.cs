using System;
using System.Diagnostics;
using System.Threading;

namespace RobotPlanet.Core
{
    public class RobotCpuSimulator
    {
        public SimulationResult RunSimulation(
            string robotName,
            int initialEnergy,
            int workload,
            CancellationToken cancellationToken,
            bool forceError = false)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                if (string.IsNullOrWhiteSpace(robotName))
                {
                    throw new ArgumentException("Robot name cannot be empty.");
                }

                if (workload <= 0)
                {
                    throw new ArgumentException("Workload must be greater than zero.");
                }

                if (forceError)
                {
                    throw new InvalidOperationException(
                        $"Simulation error for {robotName}.");
                }

                int simulatedEnergy = initialEnergy;
                double calculationResult = 0;

                for (int i = 0; i < workload; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    calculationResult += Math.Sqrt(i + 1) * Math.Sin(i);

                    if (i % 1000 == 0)
                    {
                        simulatedEnergy--;

                        if (simulatedEnergy < 0)
                        {
                            simulatedEnergy = initialEnergy;
                        }
                    }
                }

                stopwatch.Stop();

                return new SimulationResult
                {
                    RobotName = robotName,
                    FinalState = $"Energy: {simulatedEnergy}",
                    ResultMessage =
                        $"Completed. Calculation result: {calculationResult:F2}",
                    DurationMilliseconds = stopwatch.ElapsedMilliseconds,
                    IsSuccessful = true,
                    IsCancelled = false,
                    HasError = false
                };
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();

                return new SimulationResult
                {
                    RobotName = robotName,
                    FinalState = $"Initial energy: {initialEnergy}",
                    ResultMessage = "Simulation was cancelled.",
                    DurationMilliseconds = stopwatch.ElapsedMilliseconds,
                    IsSuccessful = false,
                    IsCancelled = true,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                return new SimulationResult
                {
                    RobotName = robotName,
                    FinalState = $"Initial energy: {initialEnergy}",
                    ResultMessage = ex.Message,
                    DurationMilliseconds = stopwatch.ElapsedMilliseconds,
                    IsSuccessful = false,
                    IsCancelled = false,
                    HasError = true
                };
            }
        }
    }
}