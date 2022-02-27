using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Black_Magic
{
    public abstract class Trait<T> : Trait where T : Entity
    {
        public new T parent;
        public Trait(string name, T parent, byte priority = defaultPriority) : base(name, parent, priority) { this.parent = parent; }
        public Trait(T parent, byte priority = defaultPriority) : base(parent, priority) { this.parent = parent; }
    }

    public abstract class Trait
    {
        public string name;
        public Entity parent;
        public bool isActive = true;
        public const byte defaultPriority = 100;
        public byte priority { get; protected set; } = defaultPriority;

        public Trait(string name, Entity parent, byte priority = defaultPriority)
        {
            this.name = name;
            this.parent = parent;
            this.priority = priority;
        }

        public Trait(Entity parent, byte priority = defaultPriority)
        {
            this.name = GetType().ToString();
            this.parent = parent;
            this.priority = priority;
        }

        public abstract void Update(GameTime gameTime);
    }
}
