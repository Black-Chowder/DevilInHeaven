using System;
using System.Collections.Generic;
using System.Text;
using DevilInHeaven.Entities;
using Black_Magic;
using Microsoft.Xna.Framework;

namespace DevilInHeaven.Traits
{
    public class StageLooper : Trait
    {
        public StageLooper(Entity parent) : base(parent) { }

        public override void Update(GameTime gameTime)
        {
            if (parent.x - parent.width / 2 >= Camera.width)
            {
                parent.x = - parent.width;
            }
            else if (parent.x + parent.width / 2 <= 0)
            {
                parent.x = Camera.width - parent.width;
            }


            if (parent.y - parent.height / 2 >= Camera.height)
            {
                parent.y = - parent.height;
            }
            else if (parent.y + parent.height / 2 <= 0)
            {
                parent.y = Camera.height - parent.height;
            }
        }
    }
}
