using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Black_Magic
{
    public class PMovement : Trait
    {
        public float speed { get; set; }
        private const float defaultSpeed = 5f;

        public float verticalSpeed { get; set; }
        private const float verticalSpeedDefault = 1f;

        public float jumpHeight { get; set; }
        private const float defaultJumpHeight = 35f;

        private Gravity gravity;

        public PMovement(Entity parent, Gravity gravity, float speed = defaultSpeed, float jumpHeight = defaultJumpHeight) : base(parent)
        {
            this.gravity = gravity;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
        }

        //direction 1 = right, -1 = left
        public void Move(GameTime gameTime, float direction)
        {
            if (direction > 1) direction = 1f;
            else if (direction < -1) direction = -1;

            parent.dx += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds * 60f;
        }

        //Movement with vertical suggestion
        public void Move(GameTime gameTime, Vector2 direction)
        {
            //TODO
        }

        public void Jump(GameTime gameTime, float determination = 1f)
        {
            //TODO: Add comfort timers so that rules around when can jump aren't so strict
            if (!gravity.grounded) return;

            parent.dy -= determination * jumpHeight;
        }

        public override void Update(GameTime gameTime) { }
    }
}
