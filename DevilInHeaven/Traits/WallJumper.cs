using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using DevilInHeaven.Entities;
using Microsoft.Xna.Framework;

namespace DevilInHeaven.Traits
{
    public class WallJumper : Trait<Player>
    {
        private WallSlider wallSlider;
        public float jumpDist { get; set; }
        public float jumpHeight { get; set; }
        public bool isDynamic { get; private set; }

        public WallJumper(Player parent, float jumpDist = float.NaN, float jumpHeight = float.NaN, bool isDynamic = false) : base(parent)
        {
            wallSlider = parent.wallSlider;

            this.jumpDist = jumpDist;
            if (float.IsNaN(jumpDist))
                this.jumpDist = parent.movement.jumpHeight;

            this.jumpHeight = jumpHeight;
            if (float.IsNaN(jumpHeight))
                this.jumpHeight = parent.movement.jumpHeight;

            this.isDynamic = isDynamic;
        }

        public override void Update(GameTime gameTime) { }

        public void Jump()
        {
            if (!wallSlider.isSliding) return;

            if (isDynamic)
                parent.dy -= jumpHeight;
            else
                parent.dy = -jumpHeight;

            parent.dx += jumpDist * (wallSlider.isSlidingRight ? -1 : 1);
        }
    }
}
