using Advent_of_Code_2023;
using System.Linq;

namespace Day8
{
    internal class Program
    {
        public class Node
        {
            public string Current { get; set; }
            public string Left { get; set; }
            public string Right { get; set; }
        }

        public struct State
        {
            public string Current { get; set; }
            public int InstructionIndex { get; set; }
        }

        public class Loop
        {
            public State State { get; set; }
            public long Start { get; set; }
            public long Length { get; set; }
            public List<long> StepsToEndStates { get; set; } = new();
        }

        static void Main(string[] args)
        {
            string input = Input.Get();

            Console.WriteLine("--PART 1:--");
            long numberOfSteps = Part1(input);
            Console.WriteLine($"The total number of steps is: {numberOfSteps}\n");

            Console.WriteLine("--PART 2:--");
            // = "LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)";
            long newNumberOfSteps = Part2(input);
            Console.WriteLine($"The total number of steps is: {newNumberOfSteps}\n");
        }

        public static long Part1(string input)
        {
            string[] lines = input.Split("\r\n");
            string instructions = string.Empty;
            List<Node> nodes = new();
            Dictionary<string, Node> allNodes = new();

            processDirections(lines, nodes, out instructions);

            foreach (Node node in nodes)
            {
                allNodes.Add(node.Current, node);
            }

            Node currentNode = allNodes["AAA"];
            long numberOfSteps = 0;
            bool hasBeenFound = false;

            while (!hasBeenFound)
            {
                foreach (char instruction in instructions)
                {
                    if (instruction == 'L')
                        currentNode = allNodes[currentNode.Left];
                    else if (instruction == 'R')
                        currentNode = allNodes[currentNode.Right];

                    numberOfSteps++;

                    if (currentNode.Current == "ZZZ")
                    {
                        hasBeenFound = true;
                        break;
                    }
                }
            }

            return numberOfSteps;
        }

        public static long Part2(string input)
        {
            string[] lines = input.Split("\r\n");
            string instructions = string.Empty;
            List<Node> nodesList = new();
            Dictionary<string, Node> allNodes = new();

            processDirections(lines, nodesList, out instructions);

            foreach (Node node in nodesList)
            {
                allNodes.Add(node.Current, node);
            }

            List<Node> nodes = allNodes.Values.Where(x => x.Current.EndsWith("A")).ToList();
            Dictionary<int, List<State>> savedStates = new();
            Dictionary<int, Loop> loops = new();
            List<Loop> tempLoops = new();

            for (int i = 0; i < nodes.Count; i++)
            {
                State state = new() { Current = nodes[i].Current, InstructionIndex = 0 };
                savedStates.Add(i, new List<State>());
                tempLoops.Add(new Loop());
            }

            long numberOfSteps = 0;
            bool hasBeenFound = false;

            while (!hasBeenFound)
            {
                for (int idx = 0; idx < instructions.Length; idx++)
                {
                    char instruction = instructions[idx];

                    // PROCESS EACH NODE ACCORDING TO STATE & INSTRUCTION
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (loops.ContainsKey(i))
                            continue;

                        State state = new() { Current = nodes[i].Current, InstructionIndex = idx };

                        if (savedStates[i].Contains(state))
                        {
                            // LOOP FOUND
                            loops.Add(i, tempLoops[i]);
                            loops[i].State = state;
                            continue;
                        }

                        savedStates[i].Add(state);

                        nodes[i] = applyNextInstruction(allNodes, nodes[i], instruction);

                        tempLoops[i].Length++;

                        if (nodes[i].Current.EndsWith("Z"))
                        {
                            // ENDSTATE FOUND
                            tempLoops[i].StepsToEndStates.Add(tempLoops[i].Length);
                        }

                    }

                    if (loops.Count == nodes.Count)
                    {
                        hasBeenFound = true;
                        break;
                    }
                }
            }

            // PROCESS LOOPS
            for (int i = 0; i < loops.Count; i++)
            {
                State state = loops[i].State;
                loops[i].Start = savedStates[i].IndexOf(state);
                loops[i].Length -= loops[i].Start;

                for (int j = 0; j < loops[i].StepsToEndStates.Count; j++)
                {
                    loops[i].StepsToEndStates[j] -= loops[i].Start;
                }
            }

            // ORDER LOOPS BY SMALLEST LENGTH UNTIL END OF LOOP
            loops.OrderBy(x => x.Value.Length);


            long maxStart = loops.Values.Select(x => x.Start).Max();
            long maxLoopLength = loops.Last().Value.Length;
            List<long> loopIndexes = new(loops.Count);
            List<long> jumpBetweenEndstates = new();

            // SET ALL LOOPS SO THEY START IN THE LOOP
            for (int i = 0; i < loops.Count; i++)
            {
                loopIndexes.Add(maxStart);
                loopIndexes[i] -= loops[i].Start;
                loopIndexes[i] %= loops[i].Length;
            }
            numberOfSteps += maxStart;

            // FIND JUMPS BETWEEN ENDSTATES OF THE BIGGEST LOOP

            Loop biggestLoop = loops.Last().Value;
            for (int i = 0; i < biggestLoop.StepsToEndStates.Count; i++)
            {
                List<long> stepsToEndstates = biggestLoop.StepsToEndStates;

                if (i == 0)
                    jumpBetweenEndstates.Add(stepsToEndstates[i]);
                else
                    jumpBetweenEndstates.Add(stepsToEndstates[i] - stepsToEndstates[i - 1]);
            }

            jumpBetweenEndstates.Add(biggestLoop.Length - biggestLoop.StepsToEndStates.Last() + biggestLoop.StepsToEndStates.First());

            // CALCULATE STEPS SO ALL END AT AN ENDSTATE
            bool isDone = false;
            while (!isDone)
            {
                for (int i = 0; i < jumpBetweenEndstates.Count; i++)
                {
                    isDone = true;

                    numberOfSteps += jumpBetweenEndstates[i];
                    for (int j = 0; j < loops.Count; j++)
                    {
                        loopIndexes[j] += jumpBetweenEndstates[i];
                        loopIndexes[j] %= loops[j].Length;

                        if (!loops[j].StepsToEndStates.Contains(loopIndexes[j]))
                            isDone = false;
                    }

                    if (isDone)
                        break;
                }
            }

            return numberOfSteps;
        }

        private static Node applyNextInstruction(Dictionary<string, Node> allNodes, Node node, char instruction)
        {
            if (instruction == 'L')
                return allNodes[node.Left];
            else
                return allNodes[node.Right];
        }

        private static void processDirections(string[] lines, List<Node> nodes, out string instructions)
        {
            instructions = string.Empty;
            int index = 0;
            while (lines[index] != string.Empty)
            {
                instructions += lines[index];
                index++;
            }
            index++;

            List<string> directions = new();
            for (int i = index; i < lines.Length; i++)
            {
                directions.Add(lines[i]);
            }

            foreach (string direction in directions)
            {
                Node node = new();

                string current = direction.Substring(0, "AAA".Length);
                string left = direction.Substring("AAA = (".Length, "AAA".Length);
                string right = direction.Substring("AAA = (AAA, ".Length, "AAA".Length);

                node.Current = current;
                node.Left = left;
                node.Right = right;

                nodes.Add(node);
            }
        }
    }
}
