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
        public PlayerControler controller { get; private set; }
        public CaughtDevil caughtDevil { get; private set; }

        public static Texture2D spriteSheet { get; private set; }
        protected Animator animator;

        private const string angelAnimationName = "angel";
        private const string devilAnimationName = "devil";

        private bool _isAngel;
        public bool isAngel 
        {
            get => _isAngel;
            set
            {
                _isAngel = value;
                animator.SetAnimation(_isAngel ? angelAnimationName : devilAnimationName);
            } 
        }

        public bool isFacingRight = true;

        //Testing Variables
        private const bool drawHitbox = true;
        private Texture2D hitboxTexture = null;

        public Player(float x, float y, bool isAngel = true) : base(x, y)
        {
            width = 32;
            height = width;

            animator = new Animator(spriteSheet, new Rectangle(32, 32, 2, 1));
            animator.AddAnimation(devilAnimationName, 0, 0);
            animator.AddAnimation(angelAnimationName, 3, 2);
            animator.SetAnimation(isAngel ? angelAnimationName : devilAnimationName);
            animator.isFacingRight = true;
            animator.scale = 6;

            this.isAngel = isAngel;


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

            controller = new PlayerControler(this);
            addTrait(controller);

            caughtDevil = new CaughtDevil(this);
            addTrait(caughtDevil);
        }

        public static void LoadContent(ContentManager Content)
        {
            spriteSheet = Content.Load<Texture2D>(@"Player");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            animator.isFacingRight = isFacingRight;
            animator.gameScale = Camera.gameScale;
            animator.Update(gameTime);
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
                    .8f);
            }

            animator.Draw(spriteBatch, x, y);
        }
    }
}
