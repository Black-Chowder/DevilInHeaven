using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using DevilInHeaven.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DevilInHeaven.Traits
{
    public class PlayerControler : Trait<Player>
    {
        public Keys right { get; set; } = Keys.D;
        public Keys left { get; set; } = Keys.A;
        public Keys down { get; set; } = Keys.S;
        public Keys up { get; set; } = Keys.W;

        public Keys jump { get; set; } = Keys.Space;
        public Keys dash { get; set; } = Keys.OemPeriod;

        public bool isKeyboardControled { get; set; } = true;
        public int controlerNumber { get; set; } = 0;

        public PlayerControler(Player parent) : base(parent)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (isKeyboardControled) keyboardControls(gameTime);
            else gamepadControls(gameTime);
        }

        private void gamepadControls(GameTime gameTime)
        {
            //TODO
        }

        private void keyboardControls(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();

            Vector2 target = Vector2.Zero;
            if (keys.IsKeyDown(right))
                target = new Vector2(1, target.Y);

            if (keys.IsKeyDown(left))
                target = new Vector2(-1, target.Y);

            if (keys.IsKeyDown(up))
                target = new Vector2(target.X, -1);

            if (keys.IsKeyDown(down))
                target = new Vector2(target.X, 1);

            parent.movement.Move(gameTime, target);


            if (keys.IsKeyDown(jump))
            {
                if (parent.wallSlider.isSliding)
                    parent.wallJumper.Jump();
                else
                    parent.movement.Jump(gameTime);
            }
        }
    }
}
