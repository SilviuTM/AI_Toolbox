using AI_Toolbox.Content;
using AI_Toolbox.Games;
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
    public class MazeMenu : CurrentMenu
    {
        public override void Update()
        {
            if (Game1.msNew.LeftButton == ButtonState.Pressed && Game1.msOld.LeftButton == ButtonState.Released)
            {
                if (new Rectangle(300 - GameContent.Maze1.Width / 2,
                                  450 - GameContent.Maze1.Height / 2,
                                  GameContent.Maze1.Width, GameContent.Maze1.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new MazeGame(7, 7);

                if (new Rectangle(800 - GameContent.Maze2.Width / 2,
                                  450 - GameContent.Maze2.Height / 2,
                                  GameContent.Maze2.Width, GameContent.Maze2.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new MazeGame(25, 25);

                if (new Rectangle(1300 - GameContent.Maze3.Width / 2,
                                  450 - GameContent.Maze3.Height / 2,
                                  GameContent.Maze3.Width, GameContent.Maze3.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new MazeGame(55, 55);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 sizes;
            sb.Draw(GameContent.Maze1, new Vector2(300 - GameContent.Maze1.Width / 2,
                                                      450 - GameContent.Maze1.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 1");
            sb.DrawString(GameContent.MenuFont, "Level 1", new Vector2(300 - sizes.X / 2,
                                                                       450 + GameContent.Maze1.Height / 2), Color.White);

            sb.Draw(GameContent.Maze2, new Vector2(800 - GameContent.Maze2.Width / 2,
                                                      450 - GameContent.Maze2.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 2");
            sb.DrawString(GameContent.MenuFont, "Level 2", new Vector2(800 - sizes.X / 2,
                                                                       450 + GameContent.Maze2.Height / 2), Color.White);

            sb.Draw(GameContent.Maze3, new Vector2(1300 - GameContent.Maze3.Width / 2,
                                                      450 - GameContent.Maze3.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 3");
            sb.DrawString(GameContent.MenuFont, "Level 3", new Vector2(1300 - sizes.X / 2,
                                                                       450 + GameContent.Maze3.Height / 2), Color.White);
        }
    }
}
