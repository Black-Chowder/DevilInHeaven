using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;

namespace DevilInHeaven.Traits
{
    public class Dasher : Trait
    {
        public float speed { get; set; }
        private const float defaultSpeed = 125f;
        public double cooldown { get; private set; } = 0d;
        public double cooldownTime { set; get; }
        private const double defaultCooldownTime = 1d;

        public Dasher(Entity parent, float speed = defaultSpeed, double cooldownTime = defaultCooldownTime) : base(parent)
        {
            this.speed = speed;
            this.cooldownTime = cooldownTime;
        }

        public override void Update(GameTime gameTime) 
        {
            if (cooldown < 0)
            {
                cooldown = 0;
                return;
            }
            if (cooldown > cooldownTime) cooldown = cooldownTime;

            cooldown -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void SetCooldown(double value)
        {
            cooldown = value <= cooldownTime ? value : cooldownTime;
        }

        public void Dash(float angle, float determination = 1f)
        {
            if (cooldown > 0) return;

            cooldown = cooldownTime;
            parent.dy += MathF.Sin(angle) * speed * determination;
            parent.dx += MathF.Cos(angle) * speed * determination;
        }
    }
}
