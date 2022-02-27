using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Black_Magic
{
    public class Gravity : Trait
    {
        public float weight;
        public Boolean grounded = false;

        private const String traitName = "gravity";
        public Gravity(Entity parent, float weight) : base(traitName, parent)
        {
            this.weight = weight;
        }

        public override void Update(GameTime gameTime)
        {
            parent.dy += weight;// * parent.tm;
        }
    }
}
