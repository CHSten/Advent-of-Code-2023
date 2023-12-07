using Advent_of_Code_2023;

namespace Day6
{
    internal class Program
    {
        public class Race
        {
            public long Time { get; set; }
            public long RecordDistance { get; set; }
            public Race(long time, long recordDistance) 
            {
                Time = time;
                RecordDistance = recordDistance;
            }
        }

        public class RaceOption
        {
            public Race Race { get; set; }
            public long TraveledDistance
            {
                get { return ButtonHoldTime * (Race.Time - ButtonHoldTime); }
            }
            public long ButtonHoldTime { get; set; }

            public RaceOption(long buttonHoldTime, Race race) 
            { 
                ButtonHoldTime = buttonHoldTime;
                Race = race;
            }
        }
        static void Main(string[] args)
        {
            string input = Input.Get();

            Console.WriteLine("--PART 1:--");
            int multipliedWaysOfWinning = Part1(input);
            Console.WriteLine($"When multiplying each way to win, you get: {multipliedWaysOfWinning}\n");

            Console.WriteLine("--PART 2:--");
            int WaysOfWinning = Part2(input);
            Console.WriteLine($"The amount of ways to win: {WaysOfWinning}\n");
        }

        public static int Part1(string input)
        {
            string[] lines = input.Split("\r\n");

            List<Race> races = processRaces(lines);

            Dictionary<Race, List<RaceOption>> raceToOptions = new();
            List<int> allWaysToWin = new();

            foreach (Race race in races)
            {
                List<RaceOption> options = new();

                for (int i = 0; i <= race.Time; i++)
                {
                    RaceOption raceOption = new(i, race);
                    options.Add(raceOption);
                }

                raceToOptions.Add(race, options);
            }

            foreach (Race race in raceToOptions.Keys)
            {
                int waysToWin = 0;
                foreach (RaceOption option in raceToOptions[race])
                {
                    if (race.RecordDistance < option.TraveledDistance)
                        waysToWin++;
                }
                allWaysToWin.Add(waysToWin);
            }

            int multipliedWaysOfWinning = allWaysToWin[0];
            for (int i = 1; i < allWaysToWin.Count; i++)
            {
                multipliedWaysOfWinning *= allWaysToWin[i];
            }

            return multipliedWaysOfWinning;
        }

        public static int Part2(string input)
        {
            string[] lines = input.Split("\r\n");

            Race race = processRaces2(lines);

            Dictionary<Race, List<RaceOption>> raceToOptions = new();
            List<int> allWaysToWin = new();

            List<RaceOption> options = new();

            for (int i = 0; i <= race.Time; i++)
            {
                RaceOption raceOption = new(i, race);
                options.Add(raceOption);
            }

            raceToOptions.Add(race, options);

            int waysToWin = 0;
            foreach (RaceOption option in raceToOptions[race])
            {
                if (race.RecordDistance < option.TraveledDistance)
                    waysToWin++;
            }
            allWaysToWin.Add(waysToWin);

            int multipliedWaysOfWinning = allWaysToWin[0];
            for (int i = 1; i < allWaysToWin.Count; i++)
            {
                multipliedWaysOfWinning *= allWaysToWin[i];
            }

            return multipliedWaysOfWinning;
        }

        private static List<Race> processRaces(string[] lines)
        {
            List<Race> races = new();
            List<string> times = new();
            List<string> distances = new();
            foreach (string line in lines)
            {
                if (line.StartsWith("Time"))
                {
                    times = line.Split(" ").ToList();
                    times.RemoveAll(x => string.IsNullOrEmpty(x));
                }

                if (line.StartsWith("Distance"))
                {
                    distances = line.Split(" ").ToList();
                    distances.RemoveAll(x => string.IsNullOrEmpty(x));
                }
            }

            for (int i = 0; i < times.Count; i++)
            {
                int time = 0;
                int recordDistance = 0;

                if (int.TryParse(times[i], out time) &&
                    int.TryParse(distances[i], out recordDistance))
                {
                    Race race = new(time, recordDistance);
                    races.Add(race);
                }
            }
            return races;
        }

        private static Race processRaces2(string[] lines)
        {
            string timeString = string.Empty;
            string distanceString = string.Empty;
            foreach (string line in lines)
            {
                if (line.StartsWith("Time"))
                {
                    timeString = line.Substring("Time: ".Length);
                    timeString = timeString.Replace(" ", "");
                }

                if (line.StartsWith("Distance"))
                {
                    distanceString = line.Substring("Distances: ".Length);
                    distanceString = distanceString.Replace(" ", "");
                }
            }
            long time = long.Parse(timeString);
            long distance = long.Parse(distanceString);
            return new Race(time, distance);
        }
    }
}
