using AI_Toolbox.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Toolbox.Menu
{
    public class MainMenu : CurrentMenu
    {
        public override void Update()
        {
            if (Game1.msNew.LeftButton == ButtonState.Pressed && Game1.msOld.LeftButton == ButtonState.Released)
            {
                if (new Rectangle(300 - GameContent.Klotski4.Width / 2,
                                  500 - GameContent.Klotski4.Height / 2,
                                  GameContent.Klotski4.Width, GameContent.Klotski4.Height).Contains(Game1.msNew.Position))
                    Game1._currentMenu = new KlotskiMenu();

                if (new Rectangle(800 - GameContent.Maze2.Width / 2,
                                  500 - GameContent.Maze2.Height / 2,
                                  GameContent.Maze2.Width, GameContent.Maze2.Height).Contains(Game1.msNew.Position))
                    Game1._currentMenu = new MazeMenu();

                if (new Rectangle(1300 - GameContent.Fifteen2.Width / 2,
                                  500 - GameContent.Fifteen2.Height / 2,
                                  GameContent.Fifteen2.Width, GameContent.Fifteen2.Height).Contains(Game1.msNew.Position))
                    Game1._currentMenu = new FifteenMenu();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            // draw title card
            Vector2 sizes = GameContent.MenuFont.MeasureString("AI Toybox");
            sb.DrawString(GameContent.MenuFont, "AI Toybox", new Vector2(800 - sizes.X / 2, 100), Color.White);

            // draw each game with image and text
            sb.Draw(GameContent.Klotski4, new Vector2(300 - GameContent.Klotski4.Width / 2,
                                                      500 - GameContent.Klotski4.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Klotski");
            sb.DrawString(GameContent.MenuFont, "Klotski", new Vector2(300 - sizes.X / 2, 
                                                                       500 + GameContent.Klotski4.Height / 2), Color.White);


            sb.Draw(GameContent.Maze2, new Vector2(800 - GameContent.Maze2.Width / 2,
                                                      500 - GameContent.Maze2.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Maze");
            sb.DrawString(GameContent.MenuFont, "Maze", new Vector2(800 - sizes.X / 2,
                                                                    500 + GameContent.Maze2.Height / 2), Color.White);


            sb.Draw(GameContent.Fifteen2, new Vector2(1300 - GameContent.Fifteen2.Width / 2,
                                                      500 - GameContent.Fifteen2.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("N-Puzzle");
            sb.DrawString(GameContent.MenuFont, "N-Puzzle", new Vector2(1300 - sizes.X / 2, 
                                                                        500 + GameContent.Fifteen2.Height / 2), Color.White);
        }
    }
}
