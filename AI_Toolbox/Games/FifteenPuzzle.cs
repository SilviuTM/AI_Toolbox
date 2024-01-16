using AI_Toolbox.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_Toolbox.Games
{
    public class FifteenPuzzle : CurrentGame
    {
        int size;
        int[,] puzzle;
        int[,] initial;
        List<Move> moves;
        Dictionary<int, List<Node>> frontier;
        Dictionary<string, Move> visitedStates;

        public FifteenPuzzle(int pSize)
        {
            size = pSize;
            GeneratePuzzle();
            initial = (int[,])puzzle.Clone();
        }

        void GeneratePuzzle()
        {
            do
            {
                int[] newpuzzle = new int[size * size];
                Random random = new();

                // initializeaza puzzle ul ca rezolvat
                for (int i = 0; i < size * size - 1; i++)
                {
                    newpuzzle[i] = i + 1;
                }

                // mutari aleatorii pentru shuffle
                int n = size * size;
                while (n > 1)
                {
                    n--;
                    int k = random.Next(n + 1);
                    int temp = newpuzzle[k];
                    newpuzzle[k] = newpuzzle[n];
                    newpuzzle[n] = temp;
                }

                // din 1d in 2d
                puzzle = new int[size, size];
                for (int i = 0; i < size * size; i++)
                    puzzle[i / size, i % size] = newpuzzle[i];

            }
            while (!Helper.isSolvable(puzzle, size));
        }

        public override void ResetButtonClicked()
        {
            puzzle = (int[,])initial.Clone();
            moves = null;
            isAISolving = isAIReallySolving = ISAIREALLYSOLVING = false;
        }

        public override void RegenButtonClicked()
        {
            ResetButtonClicked();
            noSolution = false;

            GeneratePuzzle();
            initial = (int[,])puzzle.Clone();
        }

        public override void SolutionButtonClicked()
        {
            moves = null;
            isAISolving = true;
            isAIReallySolving = false;
            ISAIREALLYSOLVING = false;
        }

        public override void ReallySolutionButtonClicked()
        {
            moves = null;
            isAIReallySolving = true;
            isAISolving = isAIReallySolving;
            ISAIREALLYSOLVING = false;
        }

        public override void REALLYSOLUTIONButtonClicked()
        {
            moves = null;
            ISAIREALLYSOLVING = true;
            isAIReallySolving = ISAIREALLYSOLVING;
            isAISolving = ISAIREALLYSOLVING;
        }

        public override void ComputeSolution()
        {
            moves = Solve(puzzle);
        }

        public override void LetAISolveIt()
        {
            if (noSolution)
            {
                moves = null;
                isAISolving = false;
                isAIReallySolving = false;
                ISAIREALLYSOLVING = false;
            }
            else if (moves == null)
                ComputeSolution();
            else
            {
                while (moves.Count > 0)
                {
                    waitFrame++;
                    if (waitFrame >= MAX_wait * waitSlider / ((isAIReallySolving == true) ? 10f : 1))
                    {
                        waitFrame = 0;

                        // then we can simulate next move
                        puzzle = Move(moves[0].piece, moves[0].direction, puzzle);
                        moves.RemoveAt(0);

                        // play sound effect
                        if (!ISAIREALLYSOLVING)
                            GameContent.PieceMove.CreateInstance().Play(); // if not instant solve, then allow sfx
                    }

                    if (!ISAIREALLYSOLVING) // if not instant solve, then allow async solve
                        break;
                }
            }
        }

        #region Piece Movement
        int[,] Move(int piece, string direction, int[,] configuration)
        {
            int[,] config = (int[,])configuration.Clone();

            if (direction == "left")
            {
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        if (config[i, j] == piece)
                        {
                            var aux = config[i, j - 1];
                            config[i, j - 1] = config[i, j];
                            config[i, j] = aux;
                        }
            }

            else if (direction == "right")
            {
                for (int i = size - 1; i >= 0; i--)
                    for (int j = size - 1; j >= 0; j--)
                        if (config[i, j] == piece)
                        {
                            var aux = config[i, j + 1];
                            config[i, j + 1] = config[i, j];
                            config[i, j] = aux;
                        }
            }

            else if (direction == "up")
            {
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        if (config[i, j] == piece)
                        {
                            var aux = config[i - 1, j];
                            config[i - 1, j] = config[i, j];
                            config[i, j] = aux;
                        }
            }

            else if (direction == "down")
            {
                for (int i = size - 1; i >= 0; i--)
                    for (int j = size - 1; j >= 0; j--)
                        if (config[i, j] == piece)
                        {
                            var aux = config[i + 1, j];
                            config[i + 1, j] = config[i, j];
                            config[i, j] = aux;
                        }
            }

            return config;
        }
        #endregion

        #region AI-Man
        List<Move> Solve(int[,] initialConfiguration)
        {
            // configurarea initiala ca nod
            Node InitialNode = new()
            {
                ParentNode = null,
                GValue = 0,
                PuzzleState = (int[,])initialConfiguration.Clone(),
                Heuristic = ManhattanDistance(initialConfiguration)
            };

            // lista pentru algoritmul A* (stocheaza starile posibile curent, dupa verificarea mutarilor)
            frontier = new();
            frontier.Add(InitialNode.Value, new() { InitialNode });

            // tinem minte starile vizitate (evitam bucla infinita + tinem minte solutia cea mai rapida)
            visitedStates = new();
            visitedStates.Add(stringRep(InitialNode.PuzzleState), new());

            while (true)
            {
                // starea pe care o verificam curent (cu valoare minima)
                int costMinim = frontier.Keys.Min();
                Node CurrentNode = frontier[costMinim][0];
                frontier[costMinim].Remove(CurrentNode);

                if (frontier[costMinim].Count == 0)
                    frontier.Remove(costMinim);

                // daca e cea finala, computam lista de mutari pe care o va simula AI-ul
                if (IsSolved(CurrentNode.PuzzleState))
                {
                    List<Move> solution = new();

                    while (CurrentNode.ParentNode != null)
                    {
                        Move curMove = visitedStates[stringRep(CurrentNode.PuzzleState)];

                        solution.Insert(0, curMove);

                        // go backwards
                        CurrentNode = CurrentNode.ParentNode;
                    }

                    return solution;
                }

                // verificam mutarile posibile
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        if (CurrentNode.PuzzleState[i, j] == 0)
                        {
                            if (i > 0) AddNewNode(CurrentNode, "up");
                            if (i < size - 1) AddNewNode(CurrentNode, "down");
                            if (j > 0) AddNewNode(CurrentNode, "left");
                            if (j < size - 1) AddNewNode(CurrentNode, "right");
                        }
            }
        }

        void AddNewNode(Node CurrentNode, string direction)
        {
            int[,] newConfig = Move(0, direction, CurrentNode.PuzzleState);
            Node NewNode = new()
            {
                ParentNode = CurrentNode,
                GValue = CurrentNode.GValue + 1,
                PuzzleState = newConfig,
                Heuristic = ManhattanDistance(newConfig)
            };

            // si daca nu a fost vizitata
            if (!visitedStates.ContainsKey(stringRep(NewNode.PuzzleState)))
            {
                // o adaugam
                visitedStates.Add(stringRep(NewNode.PuzzleState), new(0, direction));

                if (!frontier.ContainsKey(NewNode.Value))
                    frontier.Add(NewNode.Value, new());
                frontier[NewNode.Value].Add(NewNode);
            }
        }

        bool IsSolved(int[,] configuration)
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (configuration[i, j] != i * size + j + 1 && (i != size - 1 || j != size - 1))
                        return false;

            return true;
        }

        int ManhattanDistance(int[,] configuration)
        {
            int distance = 0;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (configuration[i, j] != i * size + j + 1)
                        distance += Math.Abs(i - configuration[i, j] / size) + Math.Abs(j - configuration[i, j] % size) + 1;

            for (int i = 0; i < size - 3; i++)
                for (int j = 0; j < size; j++)
                    if (configuration[i, j] == i * size + j + 1)
                        distance -= size;
                    else { i = j = int.MaxValue - 1; }

            for (int i = 0; i < size - 3; i++)
                for (int j = size - 1; j >= 0; j--)
                    if (configuration[i, j] == i * size + j + 1)
                        distance -= size;
                    else { i = int.MaxValue - 1; j = -2; }

            for (int j = 0; j < size - 3; j++)
                for (int i = 0; i < size; i++)
                    if (configuration[i, j] == i * size + j + 1)
                        distance -= size;
                    else { i = j = int.MaxValue - 1; }

            for (int j = 0; j < size - 3; j++)
                for (int i = size - 1; i >= 0; i--)
                    if (configuration[i, j] == i * size + j + 1)
                        distance -= size;
                    else { j = int.MaxValue - 1; i = -2; }

            return distance;
        }

        string stringRep(int[,] config)
        {
            string s = "";
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    s += config[i, j] + ".";

            return s;
        }
        #endregion

        #region Update User Input
        public override void UpdateUser()
        {
            MouseState ms = Mouse.GetState();
            Vector2 pos = ms.Position.ToVector2();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (new Rectangle(1250, 750, GameContent.Reset.Width, GameContent.Reset.Height).Contains(pos))
                {
                    ResetButtonClicked();
                }

                if (new Rectangle(1150, 650, GameContent.RegeneratePuzzle.Width, GameContent.RegeneratePuzzle.Height).Contains(pos))
                {
                    RegenButtonClicked();
                }

                if (new Rectangle(1150, 200, GameContent.SolveInactive.Width, GameContent.SolveInactive.Height).Contains(pos))
                {
                    SolutionButtonClicked();
                }

                if (new Rectangle(1150, 300, GameContent.ReallySolveInactive.Width, GameContent.ReallySolveInactive.Height).Contains(pos))
                {
                    ReallySolutionButtonClicked();
                }

                if (new Rectangle(1150, 400, GameContent.SuperSolveActive.Width, GameContent.SuperSolveActive.Height).Contains(pos))
                {
                    REALLYSOLUTIONButtonClicked();
                }
            }
        }
        #endregion

        #region Draw Code
        public override void Draw(SpriteBatch sb)
        {
            #region pieces
            int startY = 900 / 2 - ((size * 96) + ((size - 1) * 6)) / 2;
            int startX = 1150 / 2 - ((size * 96) + ((size - 1) * 6)) / 2;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    if (puzzle[i, j] != 0)
                    {
                        Texture2D shape = GameContent.FifteenSquare;
                        sb.Draw(shape, new Rectangle(startX + j * 96 + j * 6,
                                                     startY + i * 96 + i * 6, shape.Width, shape.Height), Color.White);


                        Vector2 sizes = GameContent.SquareFont.MeasureString(puzzle[i, j].ToString());
                        sb.DrawString(GameContent.SquareFont, puzzle[i, j].ToString(),
                                        new(startX + j * 96 + j * 6 + shape.Width / 2 - sizes.X / 2,
                                            startY + i * 96 + i * 6 + shape.Height / 2 - sizes.Y / 2), Color.White);
                    }
                }
            #endregion

            #region ui
            if (noSolution)
                sb.Draw(GameContent.NoSolutions, new Vector2(1600 / 2 - GameContent.NoSolutions.Width / 2,
                                                             900 / 2), Color.White);

            sb.Draw(GameContent.Reset, new Vector2(1250, 750), Color.White);
            sb.Draw(GameContent.RegeneratePuzzle, new Vector2(1150, 650), Color.White);

            if (ISAIREALLYSOLVING)
            {
                sb.Draw(GameContent.SolveInactive, new Vector2(1150, 200), Color.White);
                sb.Draw(GameContent.ReallySolveInactive, new Vector2(1150, 300), Color.White);
                sb.Draw(GameContent.SuperSolveActive, new Vector2(1150, 400), Color.White);
            }
            else if (isAIReallySolving)
            {
                sb.Draw(GameContent.SolveInactive, new Vector2(1150, 200), Color.White);
                sb.Draw(GameContent.ReallySolveActive, new Vector2(1150, 300), Color.White);
                sb.Draw(GameContent.SuperSolveInactive, new Vector2(1150, 400), Color.White);
            }
            else if (isAISolving)
            {
                sb.Draw(GameContent.SolveActive, new Vector2(1150, 200), Color.White);
                sb.Draw(GameContent.ReallySolveInactive, new Vector2(1150, 300), Color.White);
                sb.Draw(GameContent.SuperSolveInactive, new Vector2(1150, 400), Color.White);
            }
            else
            {
                sb.Draw(GameContent.SolveInactive, new Vector2(1150, 200), Color.White);
                sb.Draw(GameContent.ReallySolveInactive, new Vector2(1150, 300), Color.White);
                sb.Draw(GameContent.SuperSolveInactive, new Vector2(1150, 400), Color.White);
            }
            #endregion
        }
        #endregion

        #region A* stuff
        public class Node
        {
            public int[,] PuzzleState { get; set; } // configurare curenta
            public int GValue { get; set; } // costul de la start pana aici
            public int Heuristic { get; set; } // costul de aici pana la end
            public int Value { get { return GValue + Heuristic; } } // Fvalue, valoarea nodului (g + h)
            public Node ParentNode { get; set; } // NOD PARINTE!!!
        }
        #endregion
    }

    static class Helper
    {
        static int getInvCount(int[] arr, int size)
        {
            int inv_count = 0;
            for (int i = 0; i < size * size - 1; i++)
            {
                for (int j = i + 1; j < size * size; j++)
                {
                    // count pairs(arr[i], arr[j]) such that
                    // i < j but arr[i] > arr[j] (inversions)
                    if (arr[j] != 0 && arr[i] != 0
                        && arr[i] > arr[j])
                        inv_count++;
                }
            }
            return inv_count;
        }

        // find Position of blank from bottom
        static int findXPosition(int[,] puzzle, int size)
        {
            // start from bottom-right corner of matrix
            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = size - 1; j >= 0; j--)
                {
                    if (puzzle[i, j] == 0)
                        return size - i;
                }
            }
            return -1;
        }

        // This function returns true if given
        // instance of N*N - 1 puzzle is solvable
        public static bool isSolvable(int[,] puzzle, int size)
        {
            int[] arr = new int[size * size];
            int k = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    arr[k++] = puzzle[i, j];
                }
            }

            // Count inversions in given puzzle
            int invCount = getInvCount(arr, size);

            // If grid is odd, return true if inversion
            // count is even.
            if (size % 2 == 1)
                return invCount % 2 == 0;
            else // grid is even
            {
                int pos = findXPosition(puzzle, size);
                if (pos % 2 == 1)
                    return invCount % 2 == 0;
                else
                    return invCount % 2 == 1;
            }
        }
    }
}