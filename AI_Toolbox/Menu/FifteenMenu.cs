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
    public class FifteenMenu : CurrentMenu
    {
        public override void Update()
        {
            if (Game1.msNew.LeftButton == ButtonState.Pressed && Game1.msOld.LeftButton == ButtonState.Released)
            {
                if (new Rectangle(200 - GameContent.Fifteen1.Width / 2,
                                  450 - GameContent.Fifteen1.Height / 2,
                                  GameContent.Fifteen1.Width, GameContent.Fifteen1.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new FifteenPuzzle(2);

                if (new Rectangle(600 - GameContent.Fifteen2.Width / 2,
                                  450 - GameContent.Fifteen2.Height / 2,
                                  GameContent.Fifteen2.Width, GameContent.Fifteen2.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new FifteenPuzzle(3);

                if (new Rectangle(1000 - GameContent.Fifteen3.Width / 2,
                                  450 - GameContent.Fifteen3.Height / 2,
                                  GameContent.Fifteen3.Width, GameContent.Fifteen3.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new FifteenPuzzle(4);

                if (new Rectangle(1400 - GameContent.Fifteen4.Width / 2,
                                  450 - GameContent.Fifteen4.Height / 2,
                                  GameContent.Fifteen4.Width, GameContent.Fifteen4.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new FifteenPuzzle(5);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 sizes;
            sb.Draw(GameContent.Fifteen1, new Vector2(200 - GameContent.Fifteen1.Width / 2,
                                                      450 - GameContent.Fifteen1.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 1");
            sb.DrawString(GameContent.MenuFont, "Level 1", new Vector2(200 - sizes.X / 2,
                                                                       450 + GameContent.Fifteen1.Height / 2), Color.White);

            sb.Draw(GameContent.Fifteen2, new Vector2(600 - GameContent.Fifteen2.Width / 2,
                                                      450 - GameContent.Fifteen2.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 2");
            sb.DrawString(GameContent.MenuFont, "Level 2", new Vector2(600 - sizes.X / 2,
                                                                       450 + GameContent.Fifteen2.Height / 2), Color.White);

            sb.Draw(GameContent.Fifteen3, new Vector2(1000 - GameContent.Fifteen3.Width / 2,
                                                      450 - GameContent.Fifteen3.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 3");
            sb.DrawString(GameContent.MenuFont, "Level 3", new Vector2(1000 - sizes.X / 2,
                                                                       450 + GameContent.Fifteen3.Height / 2), Color.White);

            sb.Draw(GameContent.Fifteen4, new Vector2(1400 - GameContent.Fifteen4.Width / 2,
                                                      450 - GameContent.Fifteen4.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 4");
            sb.DrawString(GameContent.MenuFont, "Level 4", new Vector2(1400 - sizes.X / 2,
                                                                       450 + GameContent.Fifteen4.Height / 2), Color.White);
        }
    }
}
