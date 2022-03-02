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

        public static PlayerMaster playerMaster;

        private const bool drawGrid = false;

        public static void Init()
        {
            //Set Camera Screen Dimensions
            Camera.SetDimensions(Game1.graphics, 1920, 1080, false);

            //Initialize Lighting Settings
            Game1.penumbra.AmbientColor = Color.White; //TODO: Set to black once basic components and lighting is added
            
            EntityHandler.Init();

            playerMaster = new PlayerMaster();
        }

        public static void Update(GameTime gameTime)
        {
            ClickHandler.Update();

            playerMaster.Update(gameTime);

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
            Player.LoadContent(Content);

            //GhostPlayer.LoadContent(Content);
            //Wall.LoadContent(Content);
            //TestCreature.LoadContent(Content);
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            //Clear Background
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            /*

            Possessor.Draw(spriteBatch);//<==TEMP

            //Draw Grid: (TEMPORARY)
            if (drawGrid)
            {
                int columns = (int)(Camera.width / (Wall.tileSize * Camera.gameScale * Camera.zoom));
                int rows = (int)(Camera.height / (Wall.tileSize * Camera.gameScale * Camera.zoom));

                //Vertical Lines
                for (int x = 0; x < columns + 1; x++)
                {
                    General.DrawLine(spriteBatch,
                        new Vector2((x * Wall.tileSize * 2 - betweenTiles(Camera.x)) * Camera.gameScale * Camera.zoom, 0),
                        new Vector2((x * Wall.tileSize * 2 - betweenTiles(Camera.x)) * Camera.gameScale * Camera.zoom, Camera.height),
                        Color.Black);
                }

                //Horizontal lines
                for (int y = 0; y < rows; y++)
                {
                    General.DrawLine(spriteBatch,
                        new Vector2(0, (y * Wall.tileSize * 2 - betweenTiles(Camera.y)) * Camera.gameScale * Camera.zoom),
                        new Vector2(Camera.width, (y * Wall.tileSize * 2 - betweenTiles(Camera.y)) * Camera.gameScale * Camera.zoom),
                        Color.Black);
                }
            }
            */


            EntityHandler.Draw(spriteBatch, graphicsDevice);

            spriteBatch.End();
        }

        //WARNING: This method can sometimes cause a stack overflow.  Change to use Modulo
        private static float betweenTiles(float v)
        {
            /*
            if (v > Wall.tileSize) return betweenTiles(v - betweenTiles(v - Wall.tileSize * 2));
            else if (v < -Wall.tileSize) return betweenTiles(v + Wall.tileSize * 2);
            return v;
            */
            return 10;
        }
    }
}
