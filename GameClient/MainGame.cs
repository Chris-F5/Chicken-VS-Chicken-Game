using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharedClassLibrary.GameLogic;

namespace GameClient
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteRendererSystem spriteRenderer;
        private SpriteManager spriteManager;
        private Camera camera;
        private bool readyToDraw = false;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Chicken VS Chicken Game Client";
            //TargetElapsedTime = TimeSpan.FromSeconds(SharedClassLibrary.Constants.SECONDS_PER_TICK);
            IsFixedTimeStep = false;

            camera = new Camera(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            camera.position = new SharedClassLibrary.Utilities.Vector2(10, 10);

            base.Initialize();

            GameLogic.Instance.StartGameLogicThread(AfterTickUpdate, BeforeTickUpdate, InitWorld);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteManager = new SpriteManager(Content);
            spriteManager.LoadContent();
        }

        private void InitWorld()
        {
            spriteRenderer = new SpriteRendererSystem(GameLogic.Instance.world, spriteBatch, spriteManager, camera);
        }

        private void BeforeTickUpdate() 
        {
        }

        private void AfterTickUpdate()
        {
            readyToDraw = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!readyToDraw)
            {
                SuppressDraw();
            }
            else
                readyToDraw = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.End();

            spriteRenderer.Update();

            base.Draw(gameTime);
        }
    }
}
