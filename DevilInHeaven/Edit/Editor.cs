using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DevilInHeaven.Edit
{
    public static class Editor
    {
        private static Point gridLoc;
        public static int tileSize = 32 * 6;

        private static int[,] grid;

        public static void Init()
        {
            Camera.SetDimensions(Game1.graphics, 1920 / 2, 1080 / 2, false);

            Game1.penumbra.AmbientColor = Color.White;

            grid = new int[6, 10];
        }

        public static void Update(GameTime gameTime)
        {
            ClickHandler.Update();

            //Handle creating tiles and whatnot
            KeyboardState keys = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            gridLoc = new Point((int)((mouse.X / Camera.gameScale) / tileSize), (int)((mouse.Y / Camera.gameScale) / tileSize));

            //Create platforms
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                grid[gridLoc.Y, gridLoc.X] = 1;
            }

            else if (mouse.RightButton == ButtonState.Pressed)
            {
                grid[gridLoc.Y, gridLoc.X] = 0;
            }


            //Create angels / devils
            if (keys.IsKeyDown(Keys.A))
            {
                //TODO
            }
            if (keys.IsKeyDown(Keys.D))
            {
                //TODO
            }

            //Delete player
            if (keys.IsKeyDown(Keys.S))
            {
                //TODO
            }
            
            //Export
            if (keys.IsKeyDown(Keys.E))
            {
                //TODO
            }

            //Import
            if (keys.IsKeyDown(Keys.I))
            {
                //TODO
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Draw Grid
            //TODO

            //Draw created platforms
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] <= 0) continue;
                    spriteBatch.Draw(General.createTexture(graphicsDevice),
                        new Vector2(c * tileSize * Camera.gameScale, r * tileSize * Camera.gameScale),
                        new Rectangle(0, 0, 1, 1),
                        Color.Black,
                        0,
                        Vector2.Zero,
                        tileSize * Camera.gameScale,
                        SpriteEffects.None,
                        0f);
                }
            }

            //Draw mouse loc
            Point mouseDisplayPos = new Point(gridLoc.X * tileSize, gridLoc.Y * tileSize); 
            spriteBatch.Draw(General.createTexture(graphicsDevice),
                new Vector2(mouseDisplayPos.X * Camera.gameScale, mouseDisplayPos.Y * Camera.gameScale),
                new Rectangle(0, 0, 1, 1),
                Color.Black,
                0,
                Vector2.Zero,
                tileSize * Camera.gameScale,
                SpriteEffects.None,
                0f);

            spriteBatch.End();
        }

        public static void Export()
        {
            //TODO
        }

        public static void Import()
        {
            //TODO
        }
    }
}
