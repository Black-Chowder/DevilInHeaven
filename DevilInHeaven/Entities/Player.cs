using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DevilInHeaven.Entities
{
    public class Player : Entity
    {
        //Traits
        public Rigidbody rigidbody { get; private set; }
        public HitRect hitbox { get; private set; }
        public Gravity gravity { get; private set; }
        public PMovement movement { get; private set; }

        //Testing Variables
        private const bool drawHitbox = true;
        private Texture2D hitboxTexture = null;

        public Player(float x, float y) : base(x, y)
        {
            width = 16;
            height = width;

            hitbox = new HitRect(this, new Rectangle((int)-width / 2, (int)0, (int)width, (int)height));
            rigidbody = new Rigidbody(this, hitbox);
            addTrait(rigidbody);

            gravity = new Gravity(this, 0);
            addTrait(gravity);

            movement = new PMovement(this, gravity);
            movement.directControl = true;
            addTrait(movement);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            if (drawHitbox)
            {
                hitboxTexture ??= General.createTexture(graphicsDevice);
                spriteBatch.Draw(hitboxTexture,
                    new Vector2((x + hitbox.x - Camera.x) * Camera.gameScale, (y + hitbox.y - Camera.y) * Camera.gameScale),
                    new Rectangle(0, 0, 1, 1),
                    Color.Red,
                    0,
                    new Vector2(0, 0),
                    hitbox.width * Camera.gameScale,
                    SpriteEffects.None,
                    0f);
            }
        }
    }
}
