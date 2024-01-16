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
    public class KlotskiMenu : CurrentMenu
    {
        public override void Update()
        {
            if (Game1.msNew.LeftButton == ButtonState.Pressed && Game1.msOld.LeftButton == ButtonState.Released)
            {
                if (new Rectangle(200 - GameContent.Klotski1.Width / 2,
                                  450 - GameContent.Klotski1.Height / 2,
                                  GameContent.Klotski1.Width, GameContent.Klotski1.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new Klotski(1);

                if (new Rectangle(500 - GameContent.Klotski2.Width / 2,
                                  450 - GameContent.Klotski2.Height / 2,
                                  GameContent.Klotski2.Width, GameContent.Klotski2.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new Klotski(2);

                if (new Rectangle(750 - GameContent.Klotski3.Width / 2,
                                  450 - GameContent.Klotski3.Height / 2,
                                  GameContent.Klotski3.Width, GameContent.Klotski3.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new Klotski(3);

                if (new Rectangle(1050 - GameContent.Klotski4.Width / 2,
                                  450 - GameContent.Klotski4.Height / 2,
                                  GameContent.Klotski4.Width, GameContent.Klotski4.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new Klotski(4);

                if (new Rectangle(1400 - GameContent.Klotski5.Width / 2,
                                  450 - GameContent.Klotski5.Height / 2,
                                  GameContent.Klotski5.Width, GameContent.Klotski5.Height).Contains(Game1.msNew.Position))
                    Game1._currentGame = new Klotski(5);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 sizes;
            sb.Draw(GameContent.Klotski1, new Vector2(200 - GameContent.Klotski1.Width / 2,
                                                      450 - GameContent.Klotski1.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 1");
            sb.DrawString(GameContent.MenuFont, "Level 1", new Vector2(200 - sizes.X / 2,
                                                                       450 + GameContent.Klotski1.Height / 2), Color.White);

            sb.Draw(GameContent.Klotski2, new Vector2(500 - GameContent.Klotski2.Width / 2,
                                                      450 - GameContent.Klotski2.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 2");
            sb.DrawString(GameContent.MenuFont, "Level 2", new Vector2(500 - sizes.X / 2,
                                                                       450 + GameContent.Klotski2.Height / 2), Color.White);

            sb.Draw(GameContent.Klotski3, new Vector2(750 - GameContent.Klotski3.Width / 2,
                                                      450 - GameContent.Klotski3.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 3");
            sb.DrawString(GameContent.MenuFont, "Level 3", new Vector2(750 - sizes.X / 2,
                                                                       450 + GameContent.Klotski3.Height / 2), Color.White);

            sb.Draw(GameContent.Klotski4, new Vector2(1050 - GameContent.Klotski4.Width / 2,
                                                      450 - GameContent.Klotski4.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 4");
            sb.DrawString(GameContent.MenuFont, "Level 4", new Vector2(1050 - sizes.X / 2,
                                                                       450 + GameContent.Klotski4.Height / 2), Color.White);

            sb.Draw(GameContent.Klotski5, new Vector2(1400 - GameContent.Klotski5.Width / 2,
                                                      450 - GameContent.Klotski5.Height / 2), Color.White);
            sizes = GameContent.MenuFont.MeasureString("Level 5");
            sb.DrawString(GameContent.MenuFont, "Level 5", new Vector2(1400 - sizes.X / 2,
                                                                       450 + GameContent.Klotski5.Height / 2), Color.White);
        }
    }
}
