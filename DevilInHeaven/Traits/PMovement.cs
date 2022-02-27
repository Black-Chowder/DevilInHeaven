using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Black_Magic
{
    public class PMovement : Trait
    {
        public float speed { get; set; }
        public const float defaultSpeed = 5f;

        public float jumpHeight { get; set; }
        public const float defaultJumpHeight = 30f;

        public bool directControl { private get; set; }

        private Gravity gravity;

        public Keys rightKey { get; set; } = Keys.D;
        public Keys leftKey { get; set; } = Keys.A;
        public Keys jumpKey { get; set; } = Keys.Space;

        public PMovement(Entity parent, Gravity gravity, float speed = defaultSpeed, float jumpHeight = defaultJumpHeight, bool directControl = false) : base(parent)
        {
            this.gravity = gravity;
            this.speed = speed;
            this.jumpHeight = jumpHeight;
            this.directControl = directControl;
        }

        //direction 1 = right, -1 = left
        public void Move(GameTime gameTime, float direction)
        {
            if (direction > 1) direction = 1f;
            else if (direction < -1) direction = -1;

            parent.dx += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds * 60f;
        }

        public void Jump(GameTime gameTime, float determination = 1f)
        {
            //TODO: Add comfort timers so that rules around when can jump aren't so strict
            if (!gravity.grounded) return;

            parent.dy -= determination * jumpHeight;
        }

        public override void Update(GameTime gameTime)
        {
            if (directControl) directControls(gameTime);
        }

        private void directControls(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();

            float target = 0;
            if (keys.IsKeyDown(rightKey))
            {
                target += 1;
            }
            if (keys.IsKeyDown(leftKey))
            {
                target -= 1;
            }
            Move(gameTime, target);

            if (keys.IsKeyDown(jumpKey)) Jump(gameTime);
        }
    }
}
