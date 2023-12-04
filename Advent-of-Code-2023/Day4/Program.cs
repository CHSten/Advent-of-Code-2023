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
            //input = "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";
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

            //List<int> cardIds = new(idToNumberOfMatches.Keys);
            //for (int c = 0; c < cardIds.Count; c++)
            //{
            //    int cardID = cardIds[c];
            //    int numberOfMatches = idToNumberOfMatches[cardID];
            //    for (int i = 0; i < numberOfMatches; i++)
            //    {
            //        int index = cardIds[c] + i;
            //        cardIds.Add(cardIds[index]);
            //    }

            //    cardIds = cardIds.Order().ToList();
            //}

            //return cardIds.Count;

            List<int> cardIds = new(idToNumberOfMatches.Keys);
            Dictionary<int, int> copiesAmount = new();

            for (int i = 0; i < cardIds.Count; i++)
            {
                copiesAmount.Add(i, 1);
            }

            for (int c = 0; c < cardIds.Count; c++)
            {
                int currentID = cardIds[c];
                int matchingAmount = idToNumberOfMatches[currentID];

                for (int i = 0; i < matchingAmount; i++)
                {
                    int j = i + currentID;
                    copiesAmount[j]++;
                }
            }

            int totalNumberOfCards = 0;
            for (int i = 0; i < copiesAmount.Count; i++)
            {
                for (int j = 0; j < copiesAmount[i]; j++)
                {
                    totalNumberOfCards += j * j;
                }
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
