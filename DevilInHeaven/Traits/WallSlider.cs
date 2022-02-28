using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using DevilInHeaven.Entities;

namespace DevilInHeaven.Traits
{
    public class WallSlider : Trait<Player>
    {
        private Rigidbody rigidbody;
        private HitRect hitbox;
        private Gravity gravity;
        public float slidingFriction { get; private set; }
        public bool isSliding { get; private set; } = false;
        public bool isSlidingRight { get; private set; } = false;

        public WallSlider(Player parent, float friction = float.NaN) : base(parent)
        {
            rigidbody = parent.rigidbody;
            hitbox = parent.hitbox;
            gravity = parent.gravity;

            slidingFriction = float.IsNaN(friction) ? parent.friction.coefficient : friction;
        }
        
        public override void Update(GameTime gameTime)
        {
            isSliding = hitbox.right is Platform || hitbox.left is Platform;
            isSlidingRight = hitbox.right is Platform;

            if (!isSliding) return;

            parent.dy -= slidingFriction == 0 ? 0 : (parent.dy - (parent.dy) / slidingFriction);
        }
    }
}
