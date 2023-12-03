using Advent_of_Code_2023;
using System.Drawing;

namespace Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = Input.Get();
            Console.WriteLine("--PART 1:--");
            int sumOfPartNumbers = Part1(input);
            Console.WriteLine($"The sum of all the part numbers is: {sumOfPartNumbers}\n");

            Console.WriteLine("--PART 2:--");
            int sumOfGearRatios = Part2(input);
            Console.WriteLine($"The sum of all the gear ratios is: {sumOfGearRatios}\n");
        }

        public static int Part1(string input)
        {
            Dictionary<Point, List<Point>> symbols = new();
            Dictionary<Point, int> numbers = new();
            int maxXValue, maxYValue;

            string[] lines = input.Split("\r\n");
            maxYValue = lines.Length;
            maxXValue = lines.First().Length;

            processParts(symbols, numbers, lines);

            Dictionary<Point, List<Point>> partNumbers = new();

            assignPartNumbers(symbols, numbers, maxXValue, maxYValue, partNumbers);

            int sumOfPartNumbers = 0;
            
            List<Point> checkedNumbers = new();
            foreach (Point point in partNumbers.Keys)
            {
                if (checkedNumbers.Contains(point))
                    continue;

                sumOfPartNumbers += numbers[point];

                checkedNumbers.AddRange(partNumbers[point]);
            }

            return sumOfPartNumbers;
        }
        public static int Part2(string input)
        {
            Dictionary<Point, List<Point>> symbols = new();
            Dictionary<Point, int> numbers = new();
            int maxXValue, maxYValue;

            string[] lines = input.Split("\r\n");
            maxYValue = lines.Length;
            maxXValue = lines.First().Length;

            processParts(symbols, numbers, lines);

            Dictionary<Point, List<Point>> partNumbers = new();

            assignPartNumbers(symbols, numbers, maxXValue, maxYValue, partNumbers);

            List<int> gearRatios = new();

            foreach (Point point in symbols.Keys)
            {
                List<Point> parts = symbols[point];

                if (parts.Count == 2)
                {
                    int gear0 = numbers[parts[0]];
                    int gear1 = numbers[parts[1]];
                    gearRatios.Add(gear0 * gear1);
                }
            }

            int sumOfGearRatios = 0;
            foreach (int gearRatio in gearRatios)
            {
                sumOfGearRatios += gearRatio;
            }

            return sumOfGearRatios;
        }

        private static void assignPartNumbers(Dictionary<Point, List<Point>> symbols, Dictionary<Point, int> numbers, int maxXValue, int maxYValue, Dictionary<Point, List<Point>> partNumbers)
        {
            foreach (Point point in numbers.Keys)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        if (partNumbers.ContainsKey(point))
                            continue;

                        Point neighbor = new Point(point.X + x, point.Y + y);

                        if (0 > neighbor.X || neighbor.X >= maxXValue)
                            continue;

                        if (0 > neighbor.Y || neighbor.Y >= maxYValue)
                            continue;

                        if (symbols.ContainsKey(neighbor))
                        {
                            symbols[neighbor].Add(point);
                            List<Point> partNumber = new();
                            Point nextPoint = point;
                            while (numbers.ContainsKey(nextPoint))
                            {
                                partNumber.Add(nextPoint);
                                nextPoint.X++;
                            }

                            foreach (Point newPoint in partNumber)
                            {
                                partNumbers.Add(newPoint, partNumber);
                            }
                        }

                    }
                }
            }
        }


        private static void processParts(Dictionary<Point, List<Point>> symbols, Dictionary<Point, int> numbers, string[] lines)
        {
            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];
                for (int x = 0; x < lines[y].Length; x++)
                {
                    char character = line[x];
                    Point point = new Point(x, y);

                    if (character is '.')
                        continue;

                    if (numbers.ContainsKey(point))
                        continue;

                    if (char.IsDigit(character))
                    {
                        string partString = string.Empty;
                        int nextXIndex = x;
                        while (nextXIndex < line.Length && char.IsDigit(line[nextXIndex]))
                        {
                            partString += line[nextXIndex];
                            nextXIndex++;
                        }

                        int partValue = int.Parse(partString);

                        for (int i = x; i < nextXIndex; i++)
                        {
                            Point newPoint = new Point(i, y);
                            numbers.Add(newPoint, partValue);
                        }
                    }
                    else
                    {
                        symbols.Add(point, new List<Point>());
                    }
                }
            }
        }
    }
}
