using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework.Graphics;

namespace DevilInHeaven.Entities
{
    public class Platform : Entity
    {
        public Platform(float x, float y, float width, float height) : base(x, y)
        {
            base.width = width;
            base.height = height;
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            base.Draw(spriteBatch, graphicsDevice);
        }
    }
}
