using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Black_Magic
{
    public class PFriction : Trait
    {
        public float coefficient;
        public float airCoefficient;
        public float frictionForce = 0;

        private const String traitName = "friction";
        public PFriction(Entity parent, float coefficient) : base(traitName, parent)
        {
            this.coefficient = coefficient;
            this.airCoefficient = coefficient;
        }
        public PFriction(Entity parent, float coefficient, float airCoefficient) : base(traitName, parent)
        {
            this.coefficient = coefficient;
            this.airCoefficient = airCoefficient;
        }


        public override void Update(GameTime gameTime)
        {
            //This doesn't properly apply friction while in the air to dy.
            if (parent.hasTrait("gravity") && ((Gravity)parent.getTrait("gravity")).grounded && coefficient != 0) 
                frictionForce = parent.dx - (parent.dx) / coefficient;

            else if (airCoefficient != 0) 
                frictionForce = parent.dx - (parent.dx) / airCoefficient;

            else 
                frictionForce = 0;

            parent.dx -= (frictionForce * parent.timeMod);
        }
    }
}
