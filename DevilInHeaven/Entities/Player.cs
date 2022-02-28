using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using DevilInHeaven.Traits;

namespace DevilInHeaven.Entities
{
    public class Player : Entity
    {
        //Traits
        public Rigidbody rigidbody { get; private set; }
        public HitRect hitbox { get; private set; }
        public Gravity gravity { get; private set; }
        public PMovement movement { get; private set; }
        public PFriction friction { get; private set; }
        public WallSlider wallSlider { get; private set; }
        public WallJumper wallJumper { get; private set; }
        public Dasher dasher { get; private set; }
        public PlayerControler controler { get; private set; }

        public static Texture2D spriteSheet { get; private set; }
        protected Animator animator;

        public bool isAngel;

        //Testing Variables
        private const bool drawHitbox = true;
        private Texture2D hitboxTexture = null;

        public Player(float x, float y, bool isAngel = true) : base(x, y)
        {
            width = 32;
            height = width;
            this.isAngel = isAngel;

            animator = new Animator(spriteSheet, new Rectangle(32, 32, 2, 1));
            animator.AddAnimation("devil", 0, 0);
            animator.AddAnimation("angel", 1, 1);
            animator.SetAnimation("devil");
            animator.isFacingRight = true;
            animator.scale = 6;


            hitbox = new HitRect(this, new Rectangle((int)(-width / 2f), (int)height, (int)width, (int)height));
            rigidbody = new Rigidbody(this, hitbox);
            addTrait(rigidbody);

            gravity = new Gravity(this, 1.5f);
            addTrait(gravity);

            movement = new PMovement(this, gravity);
            movement.upSpeed = .5f;
            movement.downSpeed = 2f;
            addTrait(movement);

            friction = new PFriction(this, 1.5f);
            addTrait(friction);

            wallSlider = new WallSlider(this, 1.15f);
            addTrait(wallSlider);

            wallJumper = new WallJumper(this);
            addTrait(wallJumper);

            dasher = new Dasher(this);
            addTrait(dasher);

            controler = new PlayerControler(this);
            addTrait(controler);
        }

        public static void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>(@"Player");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.OemPeriod))
                dasher.Dash(keys.IsKeyDown(Keys.D) ? 0 : MathF.PI);

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

            animator.Draw(spriteBatch, x, y);
        }
    }
}
