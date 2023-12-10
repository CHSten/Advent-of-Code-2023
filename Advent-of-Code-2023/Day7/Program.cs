using Advent_of_Code_2023;
using static Day7.Program;

namespace Day7
{
    internal class Program
    {
        public enum HandType
        {
            HighCard, OnePair, TwoPair, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind,
        }

        public enum Card
        {
            two, three, four, five, six, seven, eight, nine, ten, jack, queen, king, ace
        }

        public enum Card2
        {
            joker, two, three, four, five, six, seven, eight, nine, ten, queen, king, ace
        }

        public static Dictionary<char, Card> characterToCard = new()
        {
            {'2', Card.two },
            {'3', Card.three },
            {'4', Card.four },
            {'5', Card.five },
            {'6', Card.six },
            {'7', Card.seven },
            {'8', Card.eight },
            {'9', Card.nine },
            {'T', Card.ten },
            {'J', Card.jack },
            {'Q', Card.queen },
            {'K', Card.king },
            {'A', Card.ace },
        };

        public static Dictionary<char, Card2> characterToCard2 = new()
        {
            {'J', Card2.joker},
            {'2', Card2.two },
            {'3', Card2.three },
            {'4', Card2.four },
            {'5', Card2.five },
            {'6', Card2.six },
            {'7', Card2.seven },
            {'8', Card2.eight },
            {'9', Card2.nine },
            {'T', Card2.ten },
            {'Q', Card2.queen },
            {'K', Card2.king },
            {'A', Card2.ace },
        };
        public class CardHand<T> : IComparable<CardHand<T>> where T : Enum
        {
            public int BidAmount { get; set; }
            public HandType Type { get; set; }
            public List<T> Cards { get; set; }

            public CardHand(List<T> cards, int bidAmount)
            {
                Cards = cards;
                BidAmount = bidAmount;
            }

            int IComparable<CardHand<T>>.CompareTo(CardHand<T>? other)
            {
                if (Type.CompareTo(other.Type) != 0)
                    return Type.CompareTo(other.Type);

                for (int i = 0; i < Cards.Count; i++)
                {
                    T card = Cards[i];
                    T otherCard = other.Cards[i];

                    if (card.CompareTo(otherCard) == 0)
                        continue;
                    else
                        return card.CompareTo(otherCard);
                }

                return 0;
            }
        }

        static void Main(string[] args)
        {
            string input = Input.Get();

            Console.WriteLine("--PART 1:--");
            int totalWinnings = Part1(input);
            Console.WriteLine($"The total winnings is: {totalWinnings}\n");

            Console.WriteLine("--PART 2:--");
            int newTotalWinnings = Part2(input);
            Console.WriteLine($"The new total winnings is: {newTotalWinnings}\n");
        }

        public static int Part1(string input)
        {
            string[] lines = input.Split("\r\n");
            List<CardHand<Card>> hands = new();

            processCardHands1(lines, hands);

            foreach (CardHand<Card> hand in hands)
                assignHandTypes1(hand);

            hands = hands.Order().ToList();

            int totalWinnings = 0;
            int rank = 0;
            foreach (CardHand<Card> hand in hands)
            {
                rank++;
                totalWinnings += hand.BidAmount * rank;
            }

            return totalWinnings;
        }

        public static int Part2(string input)
        {
            string[] lines = input.Split("\r\n");
            List<CardHand<Card2>> hands = new();

            processCardHands2(lines, hands);

            foreach (CardHand<Card2> hand in hands)
                assignHandTypes2(hand);

            hands = hands.Order().ToList();

            int totalWinnings = 0;
            int rank = 0;
            foreach (CardHand<Card2> hand in hands)
            {
                rank++;
                totalWinnings += hand.BidAmount * rank;
            }

            return totalWinnings;
        }

        private static void assignHandTypes2(CardHand<Card2> hand)
        {
            Dictionary<Card2, int> matches = new();

            int jokerAmount = 0;

            foreach (Card2 cardEnum in Enum.GetValues(typeof(Card2)))
            {
                if (cardEnum == Card2.joker)
                    continue;

                matches.Add(cardEnum, 0);
            }

            foreach (Card2 card in hand.Cards)
            {
                if (card == Card2.joker)
                {
                    jokerAmount++;
                }
                else
                    matches[card]++;
            }

            int threeOfAKindAmount = 0;
            int pairsAmount = 0;

            matches = matches.OrderByDescending(x => x.Value).ToDictionary();

            foreach (Card2 card in matches.Keys)
            {
                if (matches[card] + jokerAmount == 5)
                {
                    hand.Type = HandType.FiveOfAKind;
                    return;
                }
                else if (matches[card] + jokerAmount == 4)
                {
                    hand.Type = HandType.FourOfAKind;
                    return;
                }
                else if (matches[card] + jokerAmount == 3)
                {
                    if (jokerAmount != 0)
                        jokerAmount = 0;

                    threeOfAKindAmount++;
                }
                else if (matches[card] + jokerAmount == 2)
                {
                    if (jokerAmount != 0)
                        jokerAmount = 0;

                    pairsAmount++;
                }
            }

            if (threeOfAKindAmount == 1 && pairsAmount == 1)
            {
                hand.Type = HandType.FullHouse;
                return;
            }
            else if (pairsAmount == 2)
            {
                hand.Type = HandType.TwoPair;
                return;
            }
            else if (threeOfAKindAmount == 1)
            {
                hand.Type = HandType.ThreeOfAKind;
                return;
            }
            else if (pairsAmount == 1)
            {
                hand.Type = HandType.OnePair;
                return;
            }
            else
            {
                hand.Type = HandType.HighCard;
                return;
            }
        }

        private static void assignHandTypes1(CardHand<Card> hand)
        {
            Dictionary<Card, int> matches = new();

            foreach (Card cardEnum in Enum.GetValues(typeof(Card)))
            {
                matches.Add(cardEnum, 0);
            }

            foreach (Card card in hand.Cards)
            {
                matches[card]++;
            }

            int threeOfAKindAmount = 0;
            int pairsAmount = 0;

            matches = matches.Where(x => x.Value != 0).ToDictionary();

            foreach (Card card in matches.Keys)
            {

                if (matches[card] == 5)
                {
                    hand.Type = HandType.FiveOfAKind;
                    return;
                }
                else if (matches[card] == 4)
                {
                    hand.Type = HandType.FourOfAKind;
                    return;
                }
                else if (matches[card] == 3)
                {
                    threeOfAKindAmount++;
                }
                else if (matches[card] == 2)
                {
                    pairsAmount++;
                }
            }

            if (threeOfAKindAmount == 1 && pairsAmount == 1)
            {
                hand.Type = HandType.FullHouse;
                return;
            }
            else if (pairsAmount == 2)
            {
                hand.Type = HandType.TwoPair;
                return;
            }
            else if (threeOfAKindAmount == 1)
            {
                hand.Type = HandType.ThreeOfAKind;
                return;
            }
            else if (pairsAmount == 1)
            {
                hand.Type = HandType.OnePair;
                return;
            }
            else
            {
                hand.Type = HandType.HighCard;
                return;
            }
        }

        private static void processCardHands1(string[] lines, List<CardHand<Card>> hands)
        {
            foreach (string line in lines)
            {
                string[] handInfo = line.Split(" ");
                List<Card> cards = new();
                int bidAmount = int.Parse(handInfo[1]);

                foreach (char character in handInfo[0])
                {
                    Card card = characterToCard[character];
                    cards.Add(card);
                }

                CardHand<Card> cardHand = new(cards, bidAmount);
                hands.Add(cardHand);
            }
        }

        private static void processCardHands2(string[] lines, List<CardHand<Card2>> hands)
        {
            foreach (string line in lines)
            {
                string[] handInfo = line.Split(" ");
                List<Card2> cards = new();
                int bidAmount = int.Parse(handInfo[1]);

                foreach (char character in handInfo[0])
                {
                    Card2 card = characterToCard2[character];
                    cards.Add(card);
                }

                CardHand<Card2> cardHand = new(cards, bidAmount);
                hands.Add(cardHand);
            }
        }
    }
}
