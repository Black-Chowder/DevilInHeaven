using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using DevilInHeaven;
using DevilInHeaven.Entities;

namespace Black_Magic
{
    public static class MasterHandler
    {

        public static SpriteFont font;

        public static GameMaster gameMaster;

        public static StartScreen startScreen;

        private const bool drawGrid = false;

        public static void Init()
        {
            //Set Camera Screen Dimensions
            Camera.SetDimensions(Game1.graphics, 1920, 1080, false);

            //Initialize Lighting Settings
            Game1.penumbra.AmbientColor = Color.White; //TODO: Set to black once basic components and lighting is added
            
            EntityHandler.Init();

            startScreen = new StartScreen();
        }

        public static void Update(GameTime gameTime)
        {
            ClickHandler.Update();

            if (startScreen.onStartScreen)
                startScreen.Update();
            else
                gameMaster.Update(gameTime);

            if (!(gameMaster is null) && gameMaster.GameOver)
            {
                gameMaster = new GameMaster();
            }

            //Buffer Time
            //if (gameTime.TotalGameTime.TotalSeconds > 2)
            //{
                EntityHandler.Update(gameTime);
            //}
        
            Camera.Update(gameTime);
        }

        public static void LoadContent(ContentManager Content)
        {
            //Must individually load content from each entity

            //Load Font
            //font = Content.Load<SpriteFont>("DefaultFont");
            StartScreen.LoadContent(Content);
            Player.LoadContent(Content);
            Map.LoadContent(Content);
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            //Clear Background
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            if (startScreen.onStartScreen)
                startScreen.Draw(spriteBatch, graphicsDevice);

            EntityHandler.Draw(spriteBatch, graphicsDevice);

            spriteBatch.End();
        }
    }
}
