using Advent_of_Code_2023;
using System;

namespace Day1
{
    internal class Program
    {
        public const int LOWEST_LETTER_AMOUNT_FOR_NUMBER = 3;

        public static Dictionary<string, int> WordToInteger = new()
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
        };

        static void Main(string[] args)
        {
            string input = Input.Get();

            Console.WriteLine("--PART 1:--");
            int totalCalibrationValuePart1 = Part1(input);
            Console.WriteLine($"The total calibration value is: {totalCalibrationValuePart1}\n");

            Console.WriteLine("--PART 2:--");
            int totalCalibrationValuePart2 = Part2(input);
            Console.WriteLine($"The total calibration value is: {totalCalibrationValuePart2}\n");

        }

        public static int Part1(string input)
        {
            string[] lines = input.Split("\r\n");
            List<int> calibrationValues = new List<int>();
            int totalCalibrationValue = 0;

            foreach (string line in lines)
            {
                int firstDigit = 0;
                int secondDigit = 0;

                for (int i = 0; i < line.Length; i++)
                {
                    if (int.TryParse($"{line[i]}", out firstDigit))
                        break;
                }

                for (int i = line.Length - 1; i >= 0; i--)
                {
                    if (int.TryParse($"{line[i]}", out secondDigit))
                        break;
                }

                int calibrationValue = firstDigit * 10 + secondDigit;
                calibrationValues.Add(calibrationValue);
            }

            foreach (var calibrationValue in calibrationValues)
            {
                totalCalibrationValue += calibrationValue;
            }

            return totalCalibrationValue;
        }

        public static int Part2(string input)
        {
            string[] lines = input.Split("\r\n");
            List<int> calibrationValues = new();
            int totalCalibrationValue = 0;

            foreach (string line in lines)
            {
                int firstDigit = 0;
                int secondDigit = 0;

                for (int i = 0; i < line.Length; i++)
                {
                    if (isIndexANumber(line, i, out firstDigit))
                        break;
                }

                for (int i = line.Length - 1; i >= 0; i--)
                {
                    if (isIndexANumber(line, i, out secondDigit))
                        break;

                }

                int calibrationValue = firstDigit * 10 + secondDigit;
                calibrationValues.Add(calibrationValue);
            }

            foreach (var calibrationValue in calibrationValues)
            {
                totalCalibrationValue += calibrationValue;
            }

            return totalCalibrationValue;
        }

        private static bool isIndexANumber(string line, int index, out int digit)
        {
            char character = line[index];
            if (int.TryParse($"{character}", out digit))
            {
                return true;
            }

            int remainingIndexes = line.Length - index;

            if (remainingIndexes < LOWEST_LETTER_AMOUNT_FOR_NUMBER)
                return false;

            foreach (string word in WordToInteger.Keys)
            {
                if (remainingIndexes < word.Length)
                    continue;

                string substring = line.Substring(index, word.Length);

                if (word == substring)
                {
                    digit = WordToInteger[substring];
                    return true;
                }
            }
            return false;
        }
    }
}