using Advent_of_Code_2023;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using static Day5.Program;

namespace Day5
{
    internal class Program
    {
        public class Map
        {
            public long DestinationStart { get; set; }
            public long DestinationEnd
            {
                get { return DestinationStart + Range - 1; }
            }
            public long SourceStart { get; set; }
            public long SourceEnd
            {
                get { return SourceStart + Range - 1; }
            }

            public long Range {  get; set; }
            public long Offset
            {
                get { return DestinationStart - SourceStart; }
            }

            public Map(long destinationStart, long sourceStart, long range)
            {
                DestinationStart = destinationStart;
                SourceStart = sourceStart;
                Range = range;
            }
        }
        public class Interval
        {
            public long Start { get; set; }
            public long End
            {
                get { return Start + Range - 1; }
            }
            public long Range { get; set; }
            public Interval(long start, long range)
            {
                Start = start;
                Range = range;
            }
        }

        public static List<string> stringMapping = new()
        {
            "seed-to-soil map:",
            "soil-to-fertilizer map:",
            "fertilizer-to-water map:",
            "water-to-light map:",
            "light-to-temperature map:",
            "temperature-to-humidity map:",
            "humidity-to-location map:"
        };

        static void Main(string[] args)
        {
            string input = Input.Get();

            Console.WriteLine("--PART 1:--");
            long lowestLocationNumber = Part1(input);
            Console.WriteLine($"The lowest location number is: {lowestLocationNumber}\n");

            Console.WriteLine("--PART 2:--");
            long newLowestLocationNumber = Part2(input);
            Console.WriteLine($"The new lowest location number is: {newLowestLocationNumber}\n");
        }

        public static long Part1(string input)
        {
            string[] lines = input.Split("\r\n");

            string seedLine = lines[0];
            string[] seedsString = seedLine.Split(" ");

            List<Interval> seeds = new();

            for (int i = 0; i < seedsString.Length; i++)
            {
                long seedValue;

                if (long.TryParse(seedsString[i], out seedValue))
                {
                    Interval seedInterval = new(seedValue, 1);
                    seeds.Add(seedInterval);
                }
            }

            List<List<Map>> mappings = new();

            foreach (string stringMap in stringMapping)
            {
                List<Map> mapping = convertLinesToMaps(lines, stringMap);
                mappings.Add(mapping);
            }

            List<Interval> interval = new();
            List<Interval> previousInterval = seeds;
            foreach (List<Map> mapping in mappings)
            {
                interval = getPossibleValues(mapping, previousInterval);
                previousInterval = interval;
            }

            return interval.MinBy(x => x.Start).Start;
        }

        public static long Part2(string input)
        {
            string[] lines = input.Split("\r\n");

            string seedLine = lines[0];
            string[] seedsString = seedLine.Split(" ");

            List<Interval> seeds = new();

            long seedValue = 0;
            long range = 0;
            for (int i = 1; i < seedsString.Length; i++)
            {
                if (i % 2 == 0)
                {
                    range = long.Parse(seedsString[i]);

                    Interval seed = new(seedValue, range);
                    seeds.Add(seed);
                }
                else
                {
                    seedValue = long.Parse(seedsString[i]);
                }
            }

            List<List<Map>> mappings = new();

            foreach (string stringMap in stringMapping)
            {
                List<Map> mapping = convertLinesToMaps(lines, stringMap);
                mappings.Add(mapping);
            }

            List<Interval> interval = new();
            List<Interval> previousInterval = seeds;
            foreach (List<Map> mapping in mappings)
            {
                interval = getPossibleValues(mapping, previousInterval);
                previousInterval = interval;
            }

            return interval.MinBy(x => x.Start).Start;
        }

        private static List<Interval> getPossibleValues(List<Map> mapping, List<Interval> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                foreach (Map map in mapping)
                {
                    var temp = splitList(values[i], map);

                    if (!(temp.Count == 1 && temp[0] == values[i]))
                    {
                        values.Remove(values[i]);
                        values.AddRange(temp);
                    }
                }
            }

            List<Interval> intervals = new();
            foreach (var value in values)
            {
                Interval addedRange = value;

                foreach (Map map in mapping)
                {
                    if (map.SourceStart <= value.Start && value.Start <= map.SourceEnd)
                        addedRange = new(value.Start + map.Offset, value.Range);

                    //if (addedRange.start == 10516670)
                    //    throw new Exception();
                }

                intervals.Add(addedRange);
            }

            return intervals;
        }

        private static List<Interval> splitList(Interval range, Map map)
        {
            List<Interval> intervals = new()
            {
                range
            };

            bool doSpilt = true;

            while (doSpilt)
            {
                for (int i = 0; i < intervals.Count; i++)
                {
                    Interval interval = intervals[i];

                    if (interval.Start < map.SourceStart && map.SourceStart < interval.End)
                    {
                        long rangeToSourceStart = map.SourceStart + 1 - interval.Start;
                        intervals.Remove(interval);
                        intervals.Add(new Interval(interval.Start, rangeToSourceStart));
                        intervals.Add(new Interval(map.SourceStart + 1, interval.Range - rangeToSourceStart));
                    }
                    else if (interval.Start < map.SourceEnd && map.SourceEnd < interval.End)
                    {
                        long rangeToSourceEnd = map.SourceEnd + 1 - interval.Start;
                        intervals.Remove(interval);
                        intervals.Add(new Interval(interval.Start, rangeToSourceEnd));
                        intervals.Add(new Interval(map.SourceEnd + 1, interval.Range - rangeToSourceEnd));
                    }
                    else
                    {
                        doSpilt = false;
                    }
                }
            }
            return intervals;
        }

        private static List<Map> convertLinesToMaps(string[] lines, string mapping)
        {
            List<Map> result = new();
            List<string> list = lines.ToList();

            int destinationStartIndex = list.IndexOf(mapping) + 1;

            int currentIndex = destinationStartIndex;
            while (lines.Length > currentIndex && !string.IsNullOrEmpty(list[currentIndex]))
            {
                string currentLine = list[currentIndex];
                string[] values = currentLine.Split(" ");

                long destinationStart = long.Parse(values[0]);
                long sourceStart = long.Parse(values[1]);
                long range = long.Parse(values[2]);

                Map map = new(destinationStart, sourceStart, range);

                result.Add(map);

                currentIndex++;
            }

            return result;

        }
    }
}
