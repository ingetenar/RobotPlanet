using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotPlanet.Core
{
    public static class RobotQueries
    {
        public static IEnumerable<Robot> SearchByName(
            IEnumerable<Robot> robots,
            string? text)
        {
            string searchText = text?.Trim() ?? string.Empty;

            return robots.Where(robot =>
                robot.Name.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Robot> SortByName(
            IEnumerable<Robot> robots)
        {
            return robots.OrderBy(robot => robot.Name);
        }

        public static IEnumerable<string> GroupByType(
            IEnumerable<Robot> robots)
        {
            return robots
                .GroupBy(robot => robot.GetType().Name)
                .OrderBy(group => group.Key)
                .Select(group =>
                    $"{group.Key}: {group.Count()} robotit");
        }
    }
}