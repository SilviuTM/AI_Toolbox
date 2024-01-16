using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AI_Toolbox.Content
{
    public static class GameContent
    {
        // Klotski
        public static Texture2D Square { get; set; }
        public static Texture2D HRectangle { get; set; }
        public static Texture2D VRectangle { get; set; }
        public static Texture2D BigSquare { get; set; }
        public static SoundEffect PieceMove { get; set; }
        
        
        public static Texture2D BFSActive { get; set; }
        public static Texture2D BFSInactive { get; set; }
        public static Texture2D DFSActive { get; set; }
        public static Texture2D DFSInactive { get; set; }

        public static Texture2D Reset { get; set; }
        public static Texture2D NoSolutions { get; set; }

        public static Texture2D SolveActive { get; set; }
        public static Texture2D SolveInactive { get; set; }
        public static Texture2D ReallySolveActive { get; set; }
        public static Texture2D ReallySolveInactive { get; set; }
        public static Texture2D SuperSolveActive { get; set; }
        public static Texture2D SuperSolveInactive { get; set; }


        // Maze
        public static Texture2D Player { get; set; }
        public static Texture2D PathActive { get; set; }
        public static Texture2D PathInactive { get; set; }
        public static Texture2D Goal { get; set; }
        public static Texture2D Wall { get; set; }
        public static Texture2D RegeneratePuzzle { get; set; }


        // Fifteen 
        public static Texture2D FifteenSquare { get; set; }


        // Menu
        public static Texture2D Klotski1 { get; set; }
        public static Texture2D Klotski2 { get; set; }
        public static Texture2D Klotski3 { get; set; }
        public static Texture2D Klotski4 { get; set; }
        public static Texture2D Klotski5 { get; set; }

        public static Texture2D Maze1 { get; set; }
        public static Texture2D Maze2 { get; set; }
        public static Texture2D Maze3 { get; set; }

        public static Texture2D Fifteen1 { get; set; }
        public static Texture2D Fifteen2 { get; set; }
        public static Texture2D Fifteen3 { get; set; }
        public static Texture2D Fifteen4 { get; set; }




        // Default
        public static SpriteFont SquareFont { get; set; }
        public static SpriteFont MenuFont { get; set; }
        public static Texture2D BG { get; set; }

        public static void LoadContent(ContentManager contentManager)
        {
            Square = contentManager.Load<Texture2D>("Klotski/Square");
            HRectangle = contentManager.Load<Texture2D>("Klotski/HRectangle");
            VRectangle = contentManager.Load<Texture2D>("Klotski/VRectangle");
            BigSquare = contentManager.Load<Texture2D>("Klotski/BigSquare");
            PieceMove = contentManager.Load<SoundEffect>("Klotski/PieceMove");

            BFSActive = contentManager.Load<Texture2D>("Klotski/BFSActive");
            DFSActive = contentManager.Load<Texture2D>("Klotski/DFSActive");
            BFSInactive = contentManager.Load<Texture2D>("Klotski/BFSInactive");
            DFSInactive = contentManager.Load<Texture2D>("Klotski/DFSInactive");

            Reset = contentManager.Load<Texture2D>("Klotski/Reset");
            NoSolutions = contentManager.Load<Texture2D>("Klotski/noSolutions");

            SolveActive = contentManager.Load<Texture2D>("Klotski/solveActive");
            SolveInactive = contentManager.Load<Texture2D>("Klotski/solveInactive");
            ReallySolveActive = contentManager.Load<Texture2D>("Klotski/reallysolveActive");
            ReallySolveInactive = contentManager.Load<Texture2D>("Klotski/reallysolveInactive");
            SuperSolveActive = contentManager.Load<Texture2D>("Klotski/supersolveActive");
            SuperSolveInactive = contentManager.Load<Texture2D>("Klotski/supersolveInactive");

            Player = contentManager.Load<Texture2D>("Maze/player");
            PathActive = contentManager.Load<Texture2D>("Maze/pathActive");
            PathInactive = contentManager.Load<Texture2D>("Maze/pathInactive");
            Goal = contentManager.Load<Texture2D>("Maze/goal");
            Wall = contentManager.Load<Texture2D>("Maze/wall");
            RegeneratePuzzle = contentManager.Load<Texture2D>("Maze/regen");

            FifteenSquare = contentManager.Load<Texture2D>("Fifteen/MYSquare");

            Klotski1 = contentManager.Load<Texture2D>("Menu/Klotski1");
            Klotski2 = contentManager.Load<Texture2D>("Menu/Klotski2");
            Klotski3 = contentManager.Load<Texture2D>("Menu/Klotski3");
            Klotski4 = contentManager.Load<Texture2D>("Menu/Klotski4");
            Klotski5 = contentManager.Load<Texture2D>("Menu/Klotski5");

            Maze1 = contentManager.Load<Texture2D>("Menu/Maze1");
            Maze2 = contentManager.Load<Texture2D>("Menu/Maze2");
            Maze3 = contentManager.Load<Texture2D>("Menu/Maze3");

            Fifteen1 = contentManager.Load<Texture2D>("Menu/Fifteen1");
            Fifteen2 = contentManager.Load<Texture2D>("Menu/Fifteen2");
            Fifteen3 = contentManager.Load<Texture2D>("Menu/Fifteen3");
            Fifteen4 = contentManager.Load<Texture2D>("Menu/Fifteen4");

            SquareFont = contentManager.Load<SpriteFont>("squaretext");
            MenuFont = contentManager.Load<SpriteFont>("menutext");
            BG = contentManager.Load<Texture2D>("Klotski/BG");
        }
    }
}
