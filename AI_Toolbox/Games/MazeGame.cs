using AI_Toolbox.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace AI_Toolbox.Games
{
    class Node
    {
        public Vector2 Position;
        public bool isWall = false;
        public double HeuristicCost = 0;
        public double PathCost = 0;

        public double TotalCost => HeuristicCost + PathCost;

        public Node Parent;

        public Node() { }
        public Node(Vector2 position, double pathCost, double heuristicCost, Node parent)
        {
            Position = position;
            PathCost = pathCost;
            HeuristicCost = heuristicCost;
            Parent = parent;
        }

        public Node LazyClone()
        {
            return new Node
            {
                Parent = Parent,
                Position = Position
            };
        }
    }

    public class MazeGame : CurrentGame
    {
        int mazeWidth, mazeHeight;
        Maze puzzle;
        Maze initial;
        AStar aStar;
        List<Vector2> shownResult;
        Vector2 curPos;

        public MazeGame(int mazeW, int mazeH) 
        {
            mazeWidth = mazeW;
            mazeHeight = mazeH;
            MazeGenerator mg = new(mazeH, mazeW);
            mg.Generate();

            puzzle = new Maze(mg.GetMazeList(), new(1, 1), new(mazeW - 2, mazeH - 2));
            initial = puzzle.Clone();
            curPos = new(1, 1);
        }

        public override void ResetButtonClicked()
        {
            puzzle = initial.Clone();
            aStar = null;
            shownResult = null;
            curPos = new(1, 1);
            isAISolving = isAIReallySolving = ISAIREALLYSOLVING = false;
        }

        public override void SolutionButtonClicked()
        {
            isAISolving = true;
            isAIReallySolving = false;
            ISAIREALLYSOLVING = false;
        }

        public override void ReallySolutionButtonClicked()
        {
            isAIReallySolving = true;
            isAISolving = isAIReallySolving;
            ISAIREALLYSOLVING = false;
        }

        public override void REALLYSOLUTIONButtonClicked()
        {
            ISAIREALLYSOLVING = true;
            isAIReallySolving = ISAIREALLYSOLVING;
            isAISolving = ISAIREALLYSOLVING;
        }

        public override void RegenButtonClicked()
        {
            ResetButtonClicked();

            MazeGenerator mg = new(mazeHeight, mazeWidth);
            mg.Generate();

            puzzle = new Maze(mg.GetMazeList(), new(1, 1), new(mazeWidth - 2, mazeHeight - 2));
            initial = puzzle.Clone();
            curPos = new(1, 1);
        }

        public override void ComputeSolution()
        {
           aStar = new AStar(mazeWidth, mazeHeight);
           aStar.Solve(puzzle);
            shownResult = new();
            curPos = new(1, 1);
        }

        public override void LetAISolveIt()
        {
            if (noSolution)
            {
                aStar = null;
                shownResult = null;
                curPos = new(1, 1);
                isAISolving = false;
                isAIReallySolving = false;
                ISAIREALLYSOLVING = false;
            }
            else if (aStar == null)
                ComputeSolution();
            else
            {
                while (aStar.GetResult().Count > 0)
                {
                    waitFrame++;
                    if (waitFrame >= MAX_wait / 2 * waitSlider / ((isAIReallySolving == true) ? 10f : 1))
                    {
                        waitFrame = 0;

                        // then we can simulate next move
                        shownResult.Add(curPos);
                        curPos = aStar.GetResult()[0];
                        aStar.GetResult().RemoveAt(0);
                    }

                    if (!ISAIREALLYSOLVING) // if not instant solve, then allow async solve
                        break;
                }
            }
        }

        #region Update User Input
        public override void UpdateUser()
        {
            Vector2 pos = Game1.msNew.Position.ToVector2();
            if (Game1.msNew.LeftButton == ButtonState.Pressed && Game1.msOld.LeftButton == ButtonState.Released)
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
            #region maze
            // maze
            int startY = 900 / 2 - mazeHeight * 16 / 2;
            int startX = 1150 / 2 - mazeWidth * 16 / 2;

            for (int i = 0; i < puzzle.MazePuzzle.Count; i++)
            {
                for (int j = 0; j < puzzle.MazePuzzle[i].Count; j++)
                {
                    if (puzzle.MazePuzzle[i][j] == true)
                    {
                        sb.Draw(GameContent.Wall, new Vector2(startX + j * 16, startY + i * 16), Color.White);
                    }
                    else
                        sb.Draw(GameContent.PathInactive, new Vector2(startX + j * 16, startY + i * 16), Color.White);
                }
            }

            if (shownResult != null)
                foreach (var p in shownResult)
                    sb.Draw(GameContent.PathActive, new Vector2(startX + p.Y * 16, startY + p.X * 16), Color.White);
            
            sb.Draw(GameContent.Goal, new Vector2(startX + (mazeWidth - 2) * 16, startY + (mazeHeight - 2) * 16), Color.White);
            sb.Draw(GameContent.Player, new Vector2(startX + curPos.Y * 16, startY + curPos.X * 16), Color.White);
            #endregion

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
        }
        #endregion
    }

    #region AI Man
    public class Maze
    {
        public List<List<bool>> MazePuzzle;
        public Vector2 InitialPosition;
        public Vector2 GoalPosition;

        public Maze(List<List<bool>> maze, Vector2 initialPosition, Vector2 goalPosition)
        {
            MazePuzzle = maze;
            InitialPosition = initialPosition;
            GoalPosition = goalPosition;
        }

        public double[,] GetHeuristic(int mazeWidth, int mazeHeight)
        {
            double[,] HeuristicMap = new double[mazeWidth, mazeHeight];

            for (int x = 0; x < mazeWidth; x++)
            {
                for (int y = 0; y < mazeHeight; y++)
                {
                    HeuristicMap[x, y] = Math.Sqrt(((x - GoalPosition.X) * (x - GoalPosition.X)) + ((y - GoalPosition.Y) * (y - GoalPosition.Y)));
                }
            }

            return HeuristicMap;
        }

        public Maze Clone()
        {
            return new Maze(new(MazePuzzle), new(InitialPosition.X, InitialPosition.Y), new(GoalPosition.X, GoalPosition.Y));
        }
    }

    public class MazeGenerator
    {
        public class Cell
        {
            public bool visited;
            public bool blocked;
            public int row;
            public int col;
        }

        private Random random = new();
        private Cell[,] cells;
        private int numRows;
        private int numCols;

        public List<Cell> mapGeneration = new();

        public MazeGenerator(int numRows, int numCols)
        {
            cells = new Cell[numRows, numCols];
            this.numRows = numRows;
            this.numCols = numCols;

            // init toate celulele ca pereti
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    cells[r, c] = new();
                    cells[r, c].visited = false;
                    cells[r, c].blocked = true;
                    cells[r, c].row = r;
                    cells[r, c].col = c;
                }
            }
        }

        public void Generate()
        {
            var stack = new Stack<Cell>();

            // adauga prima celula in stiva
            stack.Push(cells[1, 1]);
            mapGeneration.Add(cells[1, 1]);
            stack.Peek().visited = true;
            stack.Peek().blocked = false;

            while (stack.Any())
            {
                // preia celula din stiva
                Cell thisCell = stack.Peek();

                // preia vecinii nevizitati ai celulei curenta
                List<Cell> neighbourCells = GetNeighbourCells(thisCell);

                if (neighbourCells.Any())
                {
                    // ia un vecin aleator
                    Cell nextCell = neighbourCells[random.Next(neighbourCells.Count)];

                    // computeaza celula din mijloc (intre curent si vecin)
                    int midRow = thisCell.row + (nextCell.row - thisCell.row) / 2;
                    int midCol = thisCell.col + (nextCell.col - thisCell.col) / 2;
                    Cell wallCell = cells[midRow, midCol];

                    // marcheaza vecinul si celula din mijloc ca spatiu liber
                    nextCell.blocked = false;
                    wallCell.blocked = false;
                    mapGeneration.Add(wallCell);
                    mapGeneration.Add(nextCell);

                    //adauga vecinul in stiva si marcheaza ca vizitat
                    stack.Push(nextCell);
                    nextCell.visited = true;
                }
                else
                {
                    // celula nu are vecini, deci o stergem
                    stack.Pop();
                }
            }
        }

        private List<Cell> GetNeighbourCells(Cell cell)
        {
            List<Cell> neighbourCells = new();

            int row = cell.row;
            int col = cell.col;

            if (row > 2 && !cells[row - 2, col].visited)
            {
                neighbourCells.Add(cells[row - 2, col]);
            }
            if (col > 2 && !cells[row, col - 2].visited)
            {
                neighbourCells.Add(cells[row, col - 2]);
            }
            if (row < numRows - 3 && !cells[row + 2, col].visited)
            {
                neighbourCells.Add(cells[row + 2, col]);
            }
            if (col < numCols - 3 && !cells[row, col + 2].visited)
            {
                neighbourCells.Add(cells[row, col + 2]);
            }

            return neighbourCells;
        }

        public bool[,] GetMaze()
        {
            var maze = new bool[numRows, numCols];
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    maze[r, c] = cells[r, c].blocked;
                }
            }

            return maze;
        }

        public List<List<bool>> GetMazeList()
        {
            var maze = new List<List<bool>>();
            for (int r = 0; r < numRows; r++)
            {
                maze.Add(new List<bool>());
                for (int c = 0; c < numCols; c++)
                {
                    maze[r].Add(cells[r, c].blocked);
                }
            }

            return maze;
        }
    }

    class AStar
    {
        private Node currentPosition;
        private Vector2 goalPosition;

        private List<Node> frontier;
        private List<Vector2> resultPath;
        private List<Vector2> exploredPaths;

        private bool[,] closedNodes;
        private bool[,] frontierMap;
        private readonly int MazeWidth;
        private readonly int MazeHeight;

        public AStar(int mazeWidth, int mazeHeight)
        {
            MazeWidth = mazeWidth;
            MazeHeight = mazeHeight;

            closedNodes = new bool[MazeWidth, MazeHeight];
            frontierMap = new bool[MazeWidth, MazeHeight];
            frontier = new List<Node>();
            resultPath = new List<Vector2>();
            exploredPaths = new List<Vector2>();
        }

        public List<Vector2> GetResult()
        {
            return resultPath;
        }

        public List<Vector2> GetExploredPaths()
        {
            return exploredPaths;
        }

        public void Solve(Maze maze)
        {
            currentPosition = new Node(new Vector2(maze.InitialPosition.X, maze.InitialPosition.Y), 0, GetDistance(new Vector2(maze.InitialPosition.X, maze.InitialPosition.Y), maze.GoalPosition), null);
            goalPosition = new Vector2(maze.GoalPosition.X, maze.GoalPosition.Y);
            FillClosedNodes(maze);

            // cat timp nu am ajuns la sfarsit
            while (currentPosition.Position.X != goalPosition.X || currentPosition.Position.Y != goalPosition.Y)
            {
                // inchidem nodul curent (nu il mai bagam in seama)
                closedNodes[(int)currentPosition.Position.X, (int)currentPosition.Position.Y] = true;

                // adaugam vecinii nodului curent
                AddNeighbourNodes();

                // Check frontier for cheapest node
                currentPosition = GetBestFrontierNode();

                exploredPaths.Add(new Vector2(currentPosition.Position.X, currentPosition.Position.Y));
            }

            // am ajuns la sfarsit, acum doar mergem backwards ca sa vedem nodurile parcurse
            while (currentPosition != null)
            {
                resultPath.Insert(0, new Vector2(currentPosition.Position.X, currentPosition.Position.Y));
                currentPosition = currentPosition.Parent;
            }
        }

        private void FillClosedNodes(Maze maze)
        {
            for (int x = 0; x < MazeWidth; x++)
                for (int y = 0; y < MazeHeight; y++)
                    closedNodes[x, y] = maze.MazePuzzle[x][y];
        }

        private Node GetBestFrontierNode()
        {
            Node node = frontier[0];
            foreach (Node n in frontier)
                if (n.TotalCost < node.TotalCost)
                    node = n;

            frontier.Remove(node);
            return node;
        }

        private void AddNeighbourNodes()
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // verificam doar directiile cardinale
                    if (x * y != 0)
                    {
                        continue;
                    }

                    if (CheckBounds(new Vector2(currentPosition.Position.X + x, currentPosition.Position.Y + y)))
                    {
                        if (!closedNodes[(int)currentPosition.Position.X + x, (int)currentPosition.Position.Y + y]) // n a fost deja vizitat
                        {
                            if (!frontierMap[(int)currentPosition.Position.X + x, (int)currentPosition.Position.Y + y]) // nu urmeaza sa fie vizitat
                            {
                                var newPosition = new Vector2(currentPosition.Position.X + x, currentPosition.Position.Y + y);
                                var newNode = new Node(
                                    newPosition,
                                    currentPosition.PathCost + GetDistance(newPosition, currentPosition.Position),
                                    GetDistance(newPosition, goalPosition), currentPosition.LazyClone());
                                frontier.Add(newNode);
                                frontierMap[(int)currentPosition.Position.X + x, (int)currentPosition.Position.Y + y] = true;
                            }
                        }
                    }
                }
            }
        }

        private bool CheckBounds(Vector2 point)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= MazeWidth || point.Y >= MazeHeight)
            {
                return false;
            }
            return true;
        }

        private double GetDistance(Vector2 a, Vector2 b)
        {
            return Math.Sqrt(((a.X - b.X) * (a.X - b.X)) + ((a.Y - b.Y) * (a.Y - b.Y)));
        }
    }
    #endregion
}
