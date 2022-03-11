using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DevilInHeaven.Entities;

namespace Black_Magic
{
    public class PMovement : Trait
    {
        public float speed { get; set; }
        private const float defaultSpeed = 5f;

        private double soundTimer = 0f;

        public float verticalSpeed 
        { 
            get => MathF.Sqrt(upSpeed * upSpeed + downSpeed * downSpeed); 
            set { upSpeed = value; downSpeed = value; } 
        }
        private const float defaultVerticalSpeed = 0f;
        public float upSpeed { get; set; }
        public float downSpeed { get; set; }

        public float jumpHeight { get; set; }
        private const float defaultJumpHeight = 35f;

        private Gravity gravity;

        public PMovement(Entity parent, Gravity gravity, float speed = defaultSpeed, float jumpHeight = defaultJumpHeight, float verticalSpeed = defaultVerticalSpeed) : base(parent)
        {
            this.gravity = gravity;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
            this.verticalSpeed = verticalSpeed;
        }

        public void Move(GameTime gameTime, Vector2 target, float determination = float.NaN)
        {
            Move(gameTime, MathF.Atan2(target.Y, target.X), float.IsNaN(determination) ? target.Length() : determination);
        }

        public void Move(GameTime gameTime, float angle, float determination = 1)
        {
            parent.dx += MathF.Cos(angle) * determination * speed * (float)gameTime.ElapsedGameTime.TotalSeconds * 60f;
            parent.dy += MathF.Sin(angle) * determination * (angle < 0 ? upSpeed : downSpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds * 60f;

            if (!parent.hasTrait<Gravity>())
                return;
            soundTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (determination > .5f && soundTimer <= 0 && parent.getTrait<Gravity>().grounded)
            {
                soundTimer = Player.footstepSound.Duration.TotalSeconds;
                Player.footstepSound.Play();
            }
        }

        public void Jump(GameTime gameTime, float determination = 1f)
        {
            //TODO: Add comfort timers so that rules around when can jump aren't so strict
            if (!gravity.grounded) return;

            parent.dy -= determination * jumpHeight;

            Player.jumpSound.Play(); //<= This should be parameterized, not hard coded
        }

        public override void Update(GameTime gameTime) { }
    }
}
