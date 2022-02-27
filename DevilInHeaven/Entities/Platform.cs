using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DevilInHeaven.Entities
{
    public class Platform : Entity
    {
        public HitRect hitbox { get; private set; }
        public Rigidbody rigidbody { get; private set; }

        private const bool drawHitbox = true;
        private Texture2D hitboxTexture = null;

        public Platform(float x, float y, float width, float height) : base(x, y)
        {
            base.width = width;
            base.height = height;

            hitbox = new HitRect(this, new Rectangle((int)0, (int)0, (int)width, (int)height));
            rigidbody = new Rigidbody(this, hitbox, true);
            addTrait(rigidbody);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (drawHitbox)
            {
                hitboxTexture ??= General.createTexture(graphicsDevice);
                for (int i = 0; i < MathF.Floor(width / height); i++)
                {
                    spriteBatch.Draw(hitboxTexture,
                        new Vector2((x + hitbox.height * i - Camera.x) * Camera.gameScale, (y - Camera.y) * Camera.gameScale),
                        new Rectangle(0, 0, 1, 1),
                        Color.Black,
                        0,
                        new Vector2(0, 0),
                        hitbox.height * Camera.gameScale,
                        SpriteEffects.None,
                        0f);
                }

                spriteBatch.Draw(hitboxTexture,
                    new Vector2((x + hitbox.width - hitbox.height - Camera.x) * Camera.gameScale, (y - Camera.y) * Camera.gameScale),
                    new Rectangle(0, 0, 1, 1),
                    Color.Black,
                    0,
                    new Vector2(0, 0),
                    hitbox.height * Camera.gameScale,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
