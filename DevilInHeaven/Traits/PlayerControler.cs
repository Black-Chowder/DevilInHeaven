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

        public bool isKeyboardControled { get; set; } = false;
        public int controlerNumber { get; set; } = 0;
        private const float deadZone = 0f;
        public bool startPressed { get; private set; } = false;

        public GamePadState gamePadState { get; set; }

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
            if (gamePadState == null || !gamePadState.IsConnected)
                return;

            Vector2 leftStick = gamePadState.ThumbSticks.Left;
            leftStick = new Vector2(leftStick.X, -leftStick.Y);
            float target = MathF.Atan2(leftStick.Y, leftStick.X);
            float determination = General.getDistance(leftStick, Vector2.Zero);

            parent.movement.Move(gameTime, target, determination);


            if (gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.B))
            {
                if (parent.wallSlider.isSliding)
                    parent.wallJumper.Jump();
                else
                    parent.movement.Jump(gameTime);
            }

            if (gamePadState.IsButtonDown(Buttons.RightShoulder))
            {
                parent.dasher.Dash(parent.isFacingRight ? 0 : MathF.PI);
            }

            startPressed = gamePadState.IsButtonDown(Buttons.Start);
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

            //Set isFacingRight
            if (target.X != 0)
                parent.isFacingRight = target.X > 0;


            parent.movement.Move(gameTime, target);


            if (keys.IsKeyDown(jump))
            {
                if (parent.wallSlider.isSliding)
                    parent.wallJumper.Jump();
                else
                    parent.movement.Jump(gameTime);
            }

            if (keys.IsKeyDown(dash))
                parent.dasher.Dash(parent.isFacingRight ? 0 : MathF.PI);
        }
    }
}
