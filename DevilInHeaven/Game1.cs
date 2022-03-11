using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Black_Magic;
using DevilInHeaven.Edit;
using Penumbra;

namespace DevilInHeaven
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        public static PenumbraComponent penumbra;
        public static GraphicsDevice graphicsDevice;
        public static SpriteBatch spriteBatch;

        public const bool isEditing = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            IsFixedTimeStep = false;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            Window.Title = "A Devil In Heaven";

            //Add penumbra (lighting) component
            penumbra = new PenumbraComponent(this);
            Components.Add(penumbra);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            if (!isEditing) MasterHandler.Init();
            else Editor.Init();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            graphicsDevice ??= GraphicsDevice;
            spriteBatch ??= _spriteBatch;

            // TODO: use this.Content to load your game content here
            MasterHandler.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/ Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!isEditing) MasterHandler.Update(gameTime);
            else if (IsActive) Editor.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Game1.penumbra.BeginDraw();

            if (!isEditing) MasterHandler.Draw(_spriteBatch, GraphicsDevice);
            else Editor.Draw(_spriteBatch, GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
