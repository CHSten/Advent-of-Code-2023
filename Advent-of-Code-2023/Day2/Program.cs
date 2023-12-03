using Advent_of_Code_2023;

namespace Day2
{
    internal class Program
    {
        public class Bag
        {
            public int RedCubes { get; set; }
            public int GreenCubes { get; set; }
            public int BlueCubes { get; set; }
        }

        public class Game
        {
            public int ID { get; set; }
            public List<Bag> Bags { get; set;}
        }

        static void Main(string[] args)
        {
            string input = Input.Get();
            Console.WriteLine("--PART 1:--");
            int totalIDs = Part1(input);
            Console.WriteLine($"The combined IDs from all possible games is: {totalIDs}\n");

            Console.WriteLine("--PART 2:--");
            int totalPower = Part2(input);
            Console.WriteLine($"The total power of minimum red, green and blue from all possible games is: {totalPower}\n");

        }

        public static int Part1(string input)
        {
            List<Game> games = new();
            int maxRedCubes = 12;
            int maxGreenCubes = 13;
            int maxBlueCubes = 14;

            string[] lines = input.Split("\r\n");

            processGames(games, lines);

            int totalIDs = 0;
            foreach (Game game in games)
            {
                bool addGameID = true;

                foreach (Bag bag in game.Bags)
                {
                    if ((bag.RedCubes > maxRedCubes) ||
                        (bag.GreenCubes > maxGreenCubes) ||
                        (bag.BlueCubes > maxBlueCubes))
                    {
                        addGameID = false;
                    }
                }

                if (addGameID)
                {
                    totalIDs += game.ID;
                }
            }

            return totalIDs;
        }

        public static int Part2(string input)
        {
            List<Game> games = new();

            string[] lines = input.Split("\r\n");

            processGames(games, lines);

            int totalPower = 0;
            foreach (Game game in games)
            {
                int minimumRed = 0;
                int minimumGreen = 0;
                int minimumBlue = 0;

                foreach (Bag bag in game.Bags)
                {
                    if (bag.RedCubes > minimumRed)
                        minimumRed = bag.RedCubes;

                    if (bag.GreenCubes > minimumGreen)
                        minimumGreen = bag.GreenCubes;

                    if (bag.BlueCubes > minimumBlue)
                        minimumBlue = bag.BlueCubes;
                }

                totalPower += minimumRed * minimumGreen * minimumBlue;
            }

            return totalPower;
        }

        private static void processGames(List<Game> games, string[] lines)
        {
            foreach (string line in lines)
            {
                Game game = new();
                game.Bags = new List<Bag>();

                string idString = line.Substring("Game ".Length, line.IndexOf(':') - "Game ".Length);
                game.ID = int.Parse(idString);

                string recordedGames = line.Substring($"Game {idString}: ".Length);

                string[] bagsString = recordedGames.Split(";");

                foreach (string bagString in bagsString)
                {
                    Bag bag = new();

                    string[] cubesString = bagString.Split(", ");

                    foreach (string cubeString in cubesString)
                    {
                        if (cubeString.EndsWith("red"))
                        {
                            int substringLength = cubeString.Length - " red".Length;
                            string redCubesString = cubeString.Substring(0, substringLength);
                            bag.RedCubes = int.Parse(redCubesString);
                        }
                        else if (cubeString.EndsWith("green"))
                        {
                            int substringLength = cubeString.Length - " green".Length;
                            string greenCubesString = cubeString.Substring(0, substringLength);
                            bag.GreenCubes = int.Parse(greenCubesString);
                        }
                        else if (cubeString.EndsWith("blue"))
                        {
                            int substringLength = cubeString.Length - " blue".Length;
                            string blueCubesString = cubeString.Substring(0, substringLength);
                            bag.BlueCubes = int.Parse(blueCubesString);
                        }
                    }

                    game.Bags.Add(bag);
                }

                games.Add(game);
            }
        }
    }
}
