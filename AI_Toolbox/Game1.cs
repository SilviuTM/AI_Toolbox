using AI_Toolbox.Content;
using AI_Toolbox.Games;
using AI_Toolbox.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AI_Toolbox
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static CurrentGame _currentGame;
        public static CurrentMenu _currentMenu;

        public static KeyboardState kbNew;
        public static KeyboardState kbOld;

        public static MouseState msNew;
        public static MouseState msOld;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 1600;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;

            _currentMenu = new MainMenu();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GameContent.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            kbOld = kbNew;
            kbNew = Keyboard.GetState();
            msOld = msNew;
            msNew = Mouse.GetState();

            // User Logic
            if (kbNew.IsKeyDown(Keys.Escape) && kbOld.IsKeyUp(Keys.Escape))
                if (_currentGame != null)
                    _currentGame = null;

                else if (_currentMenu is not MainMenu)
                    _currentMenu = new MainMenu();

                else
                    Exit();

            if (_currentGame == null)
            {
                // Menu Logic
                _currentMenu.Update();
            }
            else
            {
                // Game Logic
                _currentGame.UpdateUser();
                if (_currentGame != null && _currentGame.isAISolving)
                    _currentGame.LetAISolveIt();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MediumPurple);
            _spriteBatch.Begin(samplerState:SamplerState.PointClamp);

            _spriteBatch.Draw(GameContent.BG, new Rectangle(0, 0, 1600, 900), Color.White);

            if (_currentGame == null)
            {
                // Menu Logic
                _currentMenu.Draw(_spriteBatch);
            }
            else
            {
                // Game Logic
                _currentGame.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}