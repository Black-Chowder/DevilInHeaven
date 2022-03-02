using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DevilInHeaven.Edit
{
    public static class Editor
    {
        public static void Init()
        {
            Camera.SetDimensions(Game1.graphics, 1920 / 2, 1080 / 2, false);

            Game1.penumbra.AmbientColor = Color.White;
        }

        public static void Update(GameTime gameTime)
        {
            ClickHandler.Update();

            //Handle creating tiles and whatnot
            //TODO
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Draw Grid
            //TODO

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
