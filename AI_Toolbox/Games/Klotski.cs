using AI_Toolbox.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AI_Toolbox.Games
{
    struct Move
    {
        public int piece;
        public string direction;

        public Move(int piece, string direction)
        {
            this.piece = piece;
            this.direction = direction;
        }
    }

    public class Klotski : CurrentGame
    {
        int[,] initial;
        int[,] puzzle;
        int width, height;
        int pieceCount;
        List<Move> moves;
        bool isBFS = true;

        public Klotski(int level)
        {
            if (level == 4)
            {
                puzzle = new int[,] {
                    {1,  0,  0, 2},
                    {1,  0,  0, 2},
                    {3,  5,  5, 4},
                    {3,  6,  7, 4},
                    {8, -1, -1, 9}
                };
                pieceCount = 10;
            }

            else if (level == 1)
            {
                puzzle = new int[,]
                {
                {1,  0, 0,  2 },
                {3,  0, 0,  4 },
                {-1, 5, 6, -1 }
                };
                pieceCount = 7;
            }

            else if (level == 5)
            {
                puzzle = new int[,] {
                    {17, 18, 1,  0,  0,  7, 15, 16},
                    {19, 20, 2,  0,  0,  8, 21, 22},
                    {23, 24, 3,  5,  6,  9, 25, 26},
                    {27, 28, 11, 12, 13, 14, 29, 30},
                    {35, 36, 37, 38, 39, 40, 41, 42},
                    {31, 32, 4, -1, -1, 10, 33, 34}
                };
                pieceCount = 43;
            }

            else if (level == 3)
            {
                puzzle = new int[,] {
                    {1,  0,  0, 7},
                    {1,  0,  0, 7},
                    {3,  5,  6, 8},
                    {9, 10, 11, 12},
                    {4, 4, -1, 2}
                };
                pieceCount = 13;
            }

            else if (level == 2)
            {
                puzzle = new int[,] {
                    {1,  0,  0,  7},
                    {2,  0,  0,  8},
                    {3,  5,  6,  9},
                    {11, 12, 13, 14},
                    {4, -1, -1, 10}
                };
                pieceCount = 15;
            }

            height = puzzle.GetLength(0); 
            width = puzzle.GetLength(1);
            initial = (int[,])puzzle.Clone();
        }

        public override void ResetButtonClicked()
        {
            puzzle = (int[,])initial.Clone();
            moves = null;
            isAISolving = isAIReallySolving = ISAIREALLYSOLVING = false;
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
            if (isBFS)
                moves = SolveKlotskiBFS(puzzle);
            else moves = SolveKlotskiDFS(puzzle);
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
                for (int i = 0; i < config.GetLength(0); i++)
                    for (int j = 0; j < config.GetLength(1); j++)
                        if (config[i, j] == piece)
                        {
                            config[i, j - 1] = config[i, j];
                            config[i, j] = -1;
                        }
            }

            else if (direction == "right")
            {
                for (int i = config.GetLength(0) - 1; i >= 0; i--)
                    for (int j = config.GetLength(1) - 1; j >= 0; j--)
                        if (config[i, j] == piece)
                        {
                            config[i, j + 1] = config[i, j];
                            config[i, j] = -1;
                        }
            }

            else if (direction == "up")
            {
                for (int i = 0; i < config.GetLength(0); i++)
                    for (int j = 0; j < config.GetLength(1); j++)
                        if (config[i, j] == piece)
                        {
                            config[i - 1, j] = config[i, j];
                            config[i, j] = -1;
                        }
            }

            else if (direction == "down")
            {
                for (int i = config.GetLength(0) - 1; i >= 0; i--)
                    for (int j = config.GetLength(1) - 1; j >= 0; j--)
                        if (config[i, j] == piece)
                        {
                            config[i + 1, j] = config[i, j];
                            config[i, j] = -1;
                        }
            }

            return config;
        }

        bool CanMove(int piece, string direction, int[,] config)
        {
            if (direction == "left")
            {
                for (int i = 0; i < config.GetLength(0); i++)
                    for (int j = 0; j < config.GetLength(1); j++)
                        if (config[i, j] == piece)
                        {
                            if (j == 0) return false;
                            if (config[i, j - 1] != -1 && config[i, j - 1] != piece) return false;
                        }
            }

            else if (direction == "right")
            {
                for (int i = config.GetLength(0) - 1; i >= 0; i--)
                    for (int j = config.GetLength(1) - 1; j >= 0; j--)
                        if (config[i, j] == piece)
                        {
                            if (j == config.GetLength(1) - 1) return false;
                            if (config[i, j + 1] != -1 && config[i, j + 1] != piece) return false;
                        }
            }

            else if (direction == "up")
            {
                for (int i = 0; i < config.GetLength(0); i++)
                    for (int j = 0; j < config.GetLength(1); j++)
                        if (config[i, j] == piece)
                        {
                            if (i == 0) return false;
                            if (config[i - 1, j] != -1 && config[i - 1, j] != piece) return false;
                        }
            }

            else if (direction == "down")
            {
                for (int i = config.GetLength(0) - 1; i >= 0; i--)
                    for (int j = config.GetLength(1) - 1; j >= 0; j--)
                        if (config[i, j] == piece)
                        {
                            if (i == config.GetLength(0) - 1) return false;
                            if (config[i + 1, j] != -1 && config[i + 1, j] != piece) return false;
                        }
            }

            return true;
        }
        #endregion

        #region AI-Man
        List<Move> SolveKlotskiBFS(int[,] initialConfiguration)
        {
            // configurarea initiala ca string
            string initialConfigString = GetStringRepresentation(initialConfiguration);

            // lista pentru algoritmul BFS (stocheaza starile posibile curent, dupa verificarea mutarilor)
            Queue<int[,]> queue = new Queue<int[,]>();
            queue.Enqueue(initialConfiguration);

            // tinem minte starile vizitate (evitam bucla infinita + tinem minte solutia cea mai rapida)
            Dictionary<string, Move> visitedStates = new();
            visitedStates.Add(initialConfigString, new());

            while (queue.Count > 0)
            {
                // starea pe care o verificam curent
                int[,] currentConfig = queue.Dequeue();

                // daca e cea finala, computam lista de mutari pe care o va simula AI-ul
                if (IsSolved(currentConfig))
                {
                    List<Move> solution = new();

                    string startingConfig = GetStringRepresentation(puzzle);
                    string endConfig = GetStringRepresentation(currentConfig);

                    while (startingConfig != endConfig)
                    {
                        Move curMove = visitedStates[endConfig];

                        string backwardsMove = "";
                        if (curMove.direction == "up") backwardsMove = "down";
                        if (curMove.direction == "down") backwardsMove = "up";
                        if (curMove.direction == "left") backwardsMove = "right";
                        if (curMove.direction == "right") backwardsMove = "left";
                        
                        solution.Insert(0, new(curMove.piece, curMove.direction));

                        // go backwards
                        currentConfig = Move(curMove.piece, backwardsMove, currentConfig);
                        endConfig = GetStringRepresentation(currentConfig);
                    }

                    return solution;
                }

                // verificam mutarile posibile
                List<string> dir = new() { "up", "down", "left", "right" };

                foreach (var direction in dir) // pentru fiecare directie
                    for (int piece = 0; piece < pieceCount; piece++) // pentru fiecare piesa
                        if (CanMove(piece, direction, currentConfig)) // daca mutarea e posibila
                        {
                            // atunci o computam
                            int[,] newConfig = Move(piece, direction, currentConfig);
                            string newConfigString = GetStringRepresentation(newConfig);

                            // si daca nu a fost vizitata
                            if (!visitedStates.ContainsKey(newConfigString))
                            {
                                // o adaugam
                                visitedStates.Add(newConfigString, new(piece, direction));
                                queue.Enqueue(newConfig);
                            }
                        }
            }

            // altfel, am ajuns aici pentru ca nu mai avem mutari, si nu am gasit solutia
            noSolution = true;
            return null;
        }
        List<Move> SolveKlotskiDFS(int[,] initialConfiguration)
        {
            // configurarea initiala ca string
            string initialConfigString = GetStringRepresentation(initialConfiguration);

            // lista pentru algoritmul BFS (stocheaza starile posibile curent, dupa verificarea mutarilor)
            Stack<int[,]> queue = new Stack<int[,]>();
            queue.Push(initialConfiguration);

            // tinem minte starile vizitate (evitam bucla infinita + tinem minte solutia cea mai rapida)
            Dictionary<string, Move> visitedStates = new();
            visitedStates.Add(initialConfigString, new());

            while (queue.Count > 0)
            {
                // starea pe care o verificam curent
                int[,] currentConfig = queue.Pop();

                // daca e cea finala, computam lista de mutari pe care o va simula AI-ul
                if (IsSolved(currentConfig))
                {
                    List<Move> solution = new();

                    string startingConfig = GetStringRepresentation(puzzle);
                    string endConfig = GetStringRepresentation(currentConfig);

                    while (startingConfig != endConfig)
                    {
                        Move curMove = visitedStates[endConfig];

                        string backwardsMove = "";
                        if (curMove.direction == "up") backwardsMove = "down";
                        if (curMove.direction == "down") backwardsMove = "up";
                        if (curMove.direction == "left") backwardsMove = "right";
                        if (curMove.direction == "right") backwardsMove = "left";

                        solution.Insert(0, new(curMove.piece, curMove.direction));

                        // go backwards
                        currentConfig = Move(curMove.piece, backwardsMove, currentConfig);
                        endConfig = GetStringRepresentation(currentConfig);
                    }

                    return solution;
                }

                // verificam mutarile posibile
                List<string> dir = new() { "up", "down", "left", "right" };

                foreach (var direction in dir) // pentru fiecare directie
                    for (int piece = 0; piece < pieceCount; piece++) // pentru fiecare piesa
                        if (CanMove(piece, direction, currentConfig)) // daca mutarea e posibila
                        {
                            // atunci o computam
                            int[,] newConfig = Move(piece, direction, currentConfig);
                            string newConfigString = GetStringRepresentation(newConfig);

                            // si daca nu a fost vizitata
                            if (!visitedStates.ContainsKey(newConfigString))
                            {
                                // o adaugam
                                visitedStates.Add(newConfigString, new(piece, direction));
                                queue.Push(newConfig);
                            }
                        }
            }

            // altfel, am ajuns aici pentru ca nu mai avem mutari, si nu am gasit solutia
            noSolution = true;
            return null;
        }
        bool IsSolved(int[,] configuration)
        {
            bool okay = true;
            if (configuration[height - 2, width / 2 - 1] != 0) okay = false;
            if (configuration[height - 1, width / 2 - 1] != 0) okay = false;
            if (configuration[height - 2, width / 2] != 0) okay = false;
            if (configuration[height - 1, width / 2] != 0) okay = false;

            return okay;
        }

        string GetStringRepresentation(int[,] configuration)
        {
            string configString = "";
            bool[] visited = new bool[pieceCount];
            for (int i = 0; i < configuration.GetLength(0); i++)
            {
                for (int j = 0; j < configuration.GetLength(1); j++)
                {
                    int piece = configuration[i, j], type;
                    if (!(piece == -1 || visited[piece] == true))
                    {
                        visited[piece] = true;

                        if (i < height - 1 && j < width - 1) // if not on any edge
                        {
                            if (configuration[i + 1, j + 1] == piece)
                            {
                                type = 3; // big square
                            }
                            else if (configuration[i, j + 1] == piece)
                            {
                                type = 2; // HRect
                            }
                            else if (configuration[i + 1, j] == piece)
                            {
                                type = 1; // VRect
                            }
                            else type = 0; // small square
                        }

                        else if (i < height - 1) // if only on right edge
                        {
                            if (configuration[i + 1, j] == piece)
                            {
                                type = 1; // VRect
                            }
                            else type = 0; // small square
                        }

                        else if (j < width - 1) // if only on down edge
                        {
                            if (configuration[i, j + 1] == piece)
                            {
                                type = 2; // HRect
                            }
                            else type = 0; // small square
                        }

                        else type = 0; // then it's in the corner, so it's definitely square

                        configString += "." + type;
                    }
                    else configString += "." + "-1";
                    //configString += "." + configuration[i, j].ToString();

                }
            }

            return configString;
        }
        #endregion

        #region Update User Input
        public override void UpdateUser()
        {
            MouseState ms = Mouse.GetState();
            Vector2 pos = ms.Position.ToVector2();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (new Rectangle(1150, 50, GameContent.BFSActive.Width, GameContent.BFSActive.Height).Contains(pos))
                {
                    isBFS = true;
                    moves = null;
                }
                if (new Rectangle(1350, 50, GameContent.BFSActive.Width, GameContent.BFSActive.Height).Contains(pos))
                {
                    isBFS = false;
                    moves = null;
                }

                if (new Rectangle(1250, 750, GameContent.Reset.Width, GameContent.Reset.Height).Contains(pos))
                {
                    ResetButtonClicked();
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
            int startY = 900 / 2 - ((height * 128) + ((height - 1) * 8)) / 2;
            int startX = 1150 / 2 - ((width * 128) + ((width - 1) * 8)) / 2;

            for (int piece = 0; piece < pieceCount; piece++)
            {
                int type = -1;
                int posX = 0, posY = 0;

                for (int i = 0; i < height && type == -1; i++)
                    for (int j = 0; j < width && type == -1; j++)
                        if (puzzle[i, j] == piece)
                        {
                            posX = j; posY = i;

                            if (i < height - 1 && j < width - 1) // if not on any edge
                            {
                                if (puzzle[i + 1, j + 1] == piece)
                                {
                                    type = 3; // big square
                                }
                                else if (puzzle[i, j + 1] == piece)
                                {
                                    type = 2; // HRect
                                }
                                else if (puzzle[i + 1, j] == piece)
                                {
                                    type = 1; // VRect
                                }
                                else type = 0; // small square
                            }

                            else if (i < height - 1) // if only on right edge
                            {
                                if (puzzle[i + 1, j] == piece)
                                {
                                    type = 1; // VRect
                                }
                                else type = 0; // small square
                            }

                            else if (j < width - 1) // if only on down edge
                            {
                                if (puzzle[i, j + 1] == piece)
                                {
                                    type = 2; // HRect
                                }
                                else type = 0; // small square
                            }

                            else type = 0; // then it's in the corner, so it's definitely square
                        }

                Texture2D shape = GameContent.Square;
                if (type == 3) shape = GameContent.BigSquare;
                if (type == 2) shape = GameContent.HRectangle;
                if (type == 1) shape = GameContent.VRectangle;

                sb.Draw(shape, new Rectangle(startX + posX * 128 + posX * 8,
                                           startY + posY * 128 + posY * 8,
                                           shape.Width,
                                           shape.Height), Color.White);
            }
            #endregion

            #region ui
            if (isBFS)
            {
                sb.Draw(GameContent.BFSActive, new Vector2(1150, 50), Color.White);
                sb.Draw(GameContent.DFSInactive, new Vector2(1350, 50), Color.White);
            }
            else
            {
                sb.Draw(GameContent.BFSInactive, new Vector2(1150, 50), Color.White);
                sb.Draw(GameContent.DFSActive, new Vector2(1350, 50), Color.White);
            }

            if (noSolution)
                sb.Draw(GameContent.NoSolutions, new Vector2(1600 / 2 - GameContent.NoSolutions.Width / 2,
                                                             900 / 2), Color.White);

            sb.Draw(GameContent.Reset, new Vector2(1250, 750), Color.White);

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
    }
}