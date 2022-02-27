using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;

namespace Spooky_Stealth.Traits
{
    public class FloatAnimation : Trait
    {
        //Relative coordinates of where to draw the player's sprite
        public float dy { get; private set; } = 0;

        //Timer
        private byte timer = 0;
        private const byte clockCycle = 60 / 2;

        private const byte maxHeight = 10;
        private const float verticalSpeed = maxHeight / (clockCycle / 2f);

        private const string traitName = "floatAnimation";
        public FloatAnimation(Entity parent) : base(traitName, parent)
        {

        }

        public override void Update(GameTime gameTime)
        {
            timer++;
            if (timer >= clockCycle) timer = 0;

            bool cycleHalf = MathF.Floor(timer / (clockCycle / 2f)) == 0 ? true : false;
            if (cycleHalf)
            {
                dy += verticalSpeed;
                return;
            }
            dy -= verticalSpeed;
        }
    }
}
