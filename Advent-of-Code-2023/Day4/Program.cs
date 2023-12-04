using Advent_of_Code_2023;

namespace Day4
{
    internal class Program
    {
        public class Card
        {
            public int ID { get; set; }
            public List<int> WinningNumbers { get; set; }
            public List<int> MyNumbers { get; set; }
        }
        static void Main(string[] args)
        {
            string input = Input.Get();

            Console.WriteLine("--PART 1:--");
            int totalPoints = Part1(input);
            Console.WriteLine($"From scratch cards, the elf has won total points of: {totalPoints}\n");

            Console.WriteLine("--PART 2:--");
            int totalScratchCards = Part2(input);
            Console.WriteLine($"The total number of scratch cards is: {totalScratchCards}\n");
        }

        public static int Part1(string input)
        {
            string[] lines = input.Split("\r\n");
            List<Card> cards = new();

            processCards(lines, cards);

            int totalPoints = 0;
            foreach (Card card in cards)
            {
                int numberOfMatchingNumbers = 0;

                foreach (int myNumber in card.MyNumbers)
                {
                    if (card.WinningNumbers.Contains(myNumber))
                        numberOfMatchingNumbers++;
                }

                totalPoints += (int)Math.Pow(2, numberOfMatchingNumbers - 1);
            }

            return totalPoints;
        }

        public static int Part2(string input)
        {
            string[] lines = input.Split("\r\n");
            List<Card> cards = new();

            processCards(lines, cards);

            Dictionary<int, int> idToNumberOfMatches = new();

            // Set idToNumberOfMatches
            foreach (Card card in cards)
            {
                int numberOfMatchingNumbers = 0;

                foreach (int myNumber in card.MyNumbers)
                {
                    if (card.WinningNumbers.Contains(myNumber))
                        numberOfMatchingNumbers++;
                }

                idToNumberOfMatches.Add(card.ID, numberOfMatchingNumbers);
            }

            List<int> cardIds = new(idToNumberOfMatches.Keys);
            Dictionary<int, int> copiesAmount = new();

            // Initialize copiesAmount
            for (int i = 0; i < cardIds.Count; i++)
            {
                copiesAmount.Add(i, 1);
            }

            // Go through each card.
            // Add 'Amount to copy' times 'Amount of copies of this card' to copiesAmount
            for (int c = 0; c < cardIds.Count; c++)
            {
                int currentID = cardIds[c];
                int matchingAmount = idToNumberOfMatches[currentID];

                for (int i = 0; i < matchingAmount; i++)
                {
                    int j = i + currentID;
                    copiesAmount[j] += copiesAmount[c];
                }
            }

            //Sum all copies
            int totalNumberOfCards = 0;
            foreach (int copies in copiesAmount.Values)
            {
                totalNumberOfCards += copies;
            }

            return totalNumberOfCards;
        }

        private static void processCards(string[] lines, List<Card> cards)
        {
            foreach (string line in lines)
            {
                Card card = new();
                card.WinningNumbers = new List<int>();
                card.MyNumbers = new List<int>();

                string idString = line.Substring("Card ".Length, line.IndexOf(':') - "Card ".Length);
                card.ID = int.Parse(idString);

                string cardInfo = line.Substring(line.IndexOf(":") + 1,
                                                 line.Length - (line.IndexOf(":") + 1));

                string[] cardInfoSplit = cardInfo.Split('|');
                string[] winningNumbers = cardInfoSplit[0].Split(' ');
                string[] myNumbers = cardInfoSplit[1].Split(' ');

                foreach (string winningNumber in winningNumbers)
                {
                    if (string.IsNullOrEmpty(winningNumber))
                        continue;

                    card.WinningNumbers.Add(int.Parse(winningNumber));
                }

                foreach (string myNumber in myNumbers)
                {
                    if (string.IsNullOrEmpty(myNumber))
                        continue;

                    card.MyNumbers.Add(int.Parse(myNumber));
                }

                cards.Add(card);
            }
        }
    }
}
