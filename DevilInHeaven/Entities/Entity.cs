using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DevilInHeaven.Entities;

namespace Black_Magic
{
    public static class EntityHandler
    {
        public static List<Entity> entities;

        public static void Init()
        {
            entities = new List<Entity>();

            Platform platform = new Platform(100, 1000, 500, 200);
            entities.Add(platform);

            Platform platform2 = new Platform(600, 300, 100, 1000);
            entities.Add(platform2);

            //byte[] map = Spooky_Stealth.Properties.Resources.TestMap;
            //MapLoader.Import(map);            
        }

        public static void Update(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is null) continue;
                entities[i].Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is null) continue;
                entities[i].Draw(spriteBatch, graphicsDevice);
            }
        }

        //Set time modifier
        public static void setTimeMod(float set)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].timeMod = set;
            }
        }
    }

    public abstract class Entity
    {
        private List<Trait> traits;
        
        public string classId { get; protected set; }

        public float x;
        public float y;
        public float z;

        public Vector2 pos
        {
            get
            {
                return new Vector2(x, y);
            }
            set
            {
                x = value.X;
                y = value.Y;
            }
        }

        public float dx = 0;
        public float dy = 0;

        public float angle = 0f;

        public float width;
        public float height;

        public bool isVisable = true;

        public float timeMod = 1f;

        public Entity(string classId, float x, float y)
        {
            traits = new List<Trait>();
            this.classId = classId;
            this.x = x;
            this.y = y;
        }

        public Entity(float x, float y)
        {
            traits = new List<Trait>();
            this.classId = "Entity";
            this.x = x;
            this.y = y;
        }

        public void addTrait<T>(T t) where T : Trait
        {
            traits.Add(t);
            traits.Sort((a, b) => { return a.priority.CompareTo(b.priority); });
        }

        public T getTrait<T>() where T : Trait
        {
            foreach (Trait trait in traits)
                if (trait is T)
                    return (T)trait;
            return null;
        }

        public List<T> getTraits<T>() where T : Trait
        {
            List<T> toReturn = new List<T>();
            foreach (Trait trait in traits)
                if (trait is T)
                    toReturn.Add((T)trait);
            return toReturn;
        }

        public List<Trait> getTraits(params Type[] types)
        {
            List<Trait> toReturn = new List<Trait>();
            foreach (Trait trait in traits)
                foreach (Type T in types)
                    if (trait.GetType() == T)
                    {
                        toReturn.Add(trait);
                        break;
                    }
            return toReturn;
        }

        public Boolean hasTrait<T>() where T : Trait
        {
            foreach (Trait trait in traits)
                if (trait is T)
                    return true;
            return false;
        }

        //Legacy get type function
        public Trait getTrait(string name)
        {
            for (int i = 0; i < traits.Count; i++)
                if (traits[i].name == name)
                    return traits[i];
            return null;
        }

        //Legacy hasTrait function
        public Boolean hasTrait(string name)
        {
            for (int i = 0; i < traits.Count; i++)
                if (traits[i].name == name)
                    return true;
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            traitUpdate(gameTime);
        }

        protected void traitUpdate(GameTime gameTime)
        {
            for (int i = 0; i < traits.Count; i++)
                if (traits[i].isActive) traits[i].Update(gameTime);

            if (MathF.Abs(dx) < .01f) dx = 0;
            if (MathF.Abs(dy) < .01f) dy = 0;
            x += dx;
            y += dy;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) { }
    }
}
