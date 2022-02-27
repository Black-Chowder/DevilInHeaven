using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Black_Magic
{
    public abstract class Hitbox
    {
        public Entity parent { get; protected set; }

        public Entity left;
        public Entity right;
        public Entity top;
        public Entity bottom;

        public Hitbox(Entity parent)
        {
            this.parent = parent;
        }

        public abstract float getDistance(Vector2 point);

        public abstract bool isColliding(Hitbox other);

        //TODO: Create isColliding abstract method

        public void resetCollisionData()
        {
            left = null;
            right = null;
            top = null;
            bottom = null;
        }
    }

    //Rectangular Hitbox
    public class HitRect : Hitbox
    {
        private Rectangle hitbox;
        //Getters and setters:
        public int x
        {
            get => hitbox.X;
            set => hitbox.X = x;
        }
        public float absX { get => hitbox.X + parent.x; }

        public int y
        {
            get => hitbox.Y;
            set => hitbox.Y = y;
        }
        public float absY { get => hitbox.Y + parent.y; }

        public int width
        {
            get => hitbox.Width;
            set => hitbox.Width = width;
        }
        public int height
        {
            get => hitbox.Height;
            set => hitbox.Height = height;
        }

        public HitRect(Entity parent, Rectangle hitbox) : base(parent)
        {
            this.hitbox = hitbox;
        }
        public HitRect(Entity parent, int x, int y, int width, int height) : base(parent)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public override float getDistance(Vector2 point)
        {
            // Ripped from  https://www.youtube.com/watch?v=Cp5WWtMoeKg&t=45s
            // signed -> if point is inside the rect, then dist is negative
            float px = point.X;
            float py = point.Y;
            //These are myself I assume
            float rx = hitbox.X + parent.x;
            float ry = hitbox.Y + parent.y;
            float rw = hitbox.Width;
            float rh = hitbox.Height;

            float ox = rx + rw / 2;
            float oy = ry + rh / 2;
            float offsetX = Math.Abs(px - ox) - rw / 2;
            float offsetY = Math.Abs(py - oy) - rh / 2;

            float unsignedDist = General.getDistance(0, 0, Math.Max(offsetX, 0), Math.Max(offsetY, 0));
            float distInsideBox = Math.Max(Math.Min(offsetX, 0), Math.Min(offsetY, 0));
            return unsignedDist + distInsideBox;
        }

        public override bool isColliding(Hitbox other)
        {
            if (other is HitRect) return isColliding((HitRect)other);
            if (other is HitCirc) return isColliding((HitCirc)other);
            if (other is HitPoly) return isColliding((HitPoly)other);
            return false;
        }

        private bool isColliding(HitRect other)
        {
            return (
                y + height > other.y &&
                y < other.y + other.height &&
                x < other.x + other.width &&
                x + width > other.x
                );
        }

        //Handles collision with circles
        //Code gotten here: https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection/402010#402010
        private bool isColliding(HitCirc circle) //METHOD IS UNTESTED
        {
            Vector2 circleDistance = new Vector2();
            circleDistance.X = Math.Abs(circle.origin.X - x);
            circleDistance.Y = Math.Abs(circle.origin.Y - y);

            if (circleDistance.X > (width / 2 + circle.radius)) { return false; }
            if (circleDistance.Y > (height / 2 + circle.radius)) { return false; }

            if (circleDistance.X <= (width / 2)) { return true; }
            if (circleDistance.Y <= (height / 2)) { return true; }

            float cornerDistance_sq = MathF.Pow((circleDistance.X - width / 2), 2) +
                                        MathF.Pow((circleDistance.Y - height / 2), 2);

            return (cornerDistance_sq <= (circle.radius * circle.radius));
        }
        
        private bool isColliding(HitPoly other)
        {
            //TODO
            return false;
        }
    }

    //Circular hitbox
    public class HitCirc : Hitbox
    {
        public Vector2 origin { get; private set; }
        public float radius { get; private set; }

        public HitCirc(Entity parent, Vector2 origin, float radius) : base(parent)
        {
            this.origin = origin;
            this.radius = radius;
        }

        public override float getDistance(Vector2 point)
        {
            float cx = origin.X + parent.x;
            float cy = origin.Y + parent.y;
            float cr = radius / 2;

            return General.getDistance(point.X, point.Y, cx, cy) - cr;
        }

        public override bool isColliding(Hitbox other) //UNTESTED
        {
            return other.getDistance(origin) < radius;
        }
    }

    //Polygon Hitbox
    public class HitPoly : Hitbox
    {
        public Vector2[] points { get; private set; }

        public HitPoly(Entity parent, params Vector2[] points) : base(parent)
        {
            this.points = points;
        }

        public HitPoly(Entity parent, List<Vector2> points): base(parent)
        {
            this.points = points.ToArray();
        }

        public override float getDistance(Vector2 point)
        {
            float min = float.PositiveInfinity;
            for (int i = 0; i < points.Length; i++)
            {
                //Store the two points on the hitbox that make the edge
                Vector2 point1 = new Vector2(points[i].X + parent.x, points[i].Y + parent.y);

                Vector2 point2;
                if (i == points.Length - 1) point2 = new Vector2(points[0].X + parent.x, points[0].Y + parent.y);
                else point2 = new Vector2(points[i + 1].X + parent.x, points[i + 1].Y + parent.y);

                //Calculate distance to line
                float dist = (float)General.FindDistanceToSegment(point, point1, point2, out _);

                //Update min value
                if (dist < min) min = dist;
            }
            return min;
        }

        public override bool isColliding(Hitbox other) //UNTESTED
        {
            //Turn into line segments and then check for distance
            //TODO
            return false;
        }
    }

    //Trait to handle collision of hitboxes
    public class Rigidbody : Trait
    {
        public List<Type> ignoreTypes = new List<Type>();

        //Stores hitboxes
        public List<Hitbox> hitboxes = new List<Hitbox>();

        //isOverride means the entity cannot be pushed by other entities
        public Boolean isOverride = false;

        //isTotal means all rigidbody entities will collide with this entity
        public Boolean isTotal = false;  //<<== May Remove.  Not implemented Yet

        public Boolean isCircle = false; //Maybe change (probably change)

        //The number of rays the rigibody samples from 
        public int raysPerSide = 4;

        //Thickness from how far inside the hitbox the collision detection rays will be cast
        public float skinWidth = 5;

        // <Temporary Testing Variables>
        public List<Rectangle> testRects = new List<Rectangle>();
        public Boolean testBool = false;
        public List<Entity> doNotCollide = new List<Entity>();
        // </Temporary Testing Variables>

        //Constructor(s)
        const String traitName = "rigidbody";
        public Rigidbody(Entity parent, Boolean isOverride = false, Boolean isCircle = false) : base(traitName, parent)
        {
            base.priority = byte.MaxValue;
            this.isOverride = isOverride;

            //Create Hitbox
            if (isCircle) hitboxes.Add(new HitCirc(parent, new Vector2(0, 0), parent.width));
            else hitboxes.Add(new HitRect(parent, new Rectangle(0, 0, (int)parent.width, (int)parent.height)));//0, 0, parent.width, parent.height));
        }
        
        //Pass in collision method
        //TODO

        //Allows entity to create its own hitboxes
        public Rigidbody(Entity parent, List<Hitbox> hitboxes, Boolean isOverride = false) : base(traitName, parent)
        {
            base.priority = byte.MaxValue;
            this.isOverride = isOverride;
            this.hitboxes = hitboxes;
        }

        public Rigidbody(Entity parent, Hitbox hitbox, Boolean isOverride = false) : base(traitName, parent)
        {
            base.priority = byte.MaxValue;
            this.isOverride = isOverride;
            hitboxes = new List<Hitbox>();
            hitboxes.Add(hitbox);
        }



        //Gets the distance from the edge of the shape (only accepts circle and rectangle at the moment)
        //and a point given to it.
        public float getDistance(Vector2 pos, sbyte layer = 0)
        {
            float shortestDist = float.PositiveInfinity;
            foreach (Hitbox hitbox in hitboxes)
            {
                float dist = hitbox.getDistance(pos);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                }
            }
            return shortestDist;
        }

        //TODO: Don't create new rays every update.  Use rays from pool of already created rays.  Should improve efficiency
        public override void Update(GameTime gameTime)
        {
            //Clear Testing Variables
            testRects = new List<Rectangle>();
            testBool = false;

            if (isOverride) return;

            Ray ray;
            Vector2? rayData;

            Boolean hasGravity = parent.hasTrait<Gravity>();
            Gravity gravity = parent.getTrait<Gravity>();
            if (hasGravity) gravity.grounded = false;

            //TODO: if !isOverride, then change self variables and others to properly apply forces

            foreach (Hitbox raw in hitboxes)
            {
                if (!(raw is HitRect)) continue;
                HitRect hitbox = (HitRect)raw;

                hitbox.resetCollisionData();
                for (int i = 0; i < raysPerSide; i++)
                {
                    //Calculate Ray Casting Points
                    float raycastY = hitbox.absY + skinWidth + (hitbox.height - skinWidth * 2) * i / (raysPerSide - 1);
                    float raycastX = hitbox.absX + skinWidth + (hitbox.width - skinWidth * 2) * i / (raysPerSide - 1);

                    //Local Variable Used For Storing Entity Currently Colliding With:
                    Entity entity;
                    Rigidbody entityRigidbody;

                    //Top
                    ray = new Ray(raycastX, hitbox.absY + skinWidth, (float)(Math.PI * 3 / 2));
                    rayData = ray.cast(EntityHandler.entities, parent);
                    if (rayData.HasValue && rayData.Value.Y > hitbox.absY + parent.dy && parent.dy < 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.getTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dy = parent.dy;

                        hitbox.top = entity;

                        parent.dy = 0;
                        parent.y = rayData.Value.Y - hitbox.y;
                    }

                    //Bottom
                    ray = new Ray(raycastX, hitbox.absY + hitbox.height - skinWidth, (float)(Math.PI / 2));
                    rayData = ray.cast(EntityHandler.entities, parent);
                    if (rayData.HasValue && rayData.Value.Y < hitbox.absY + hitbox.height + parent.dy && parent.dy > 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.getTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dy = parent.dy;

                        hitbox.bottom = entity;
                        if (hasGravity) gravity.grounded = true;

                        parent.dy = 0;
                        parent.y = rayData.Value.Y - hitbox.height - hitbox.y;
                    }

                    //Right
                    ray = new Ray(hitbox.absX + hitbox.width - skinWidth, raycastY, 0);
                    rayData = ray.cast(EntityHandler.entities, parent);
                    if (rayData.HasValue && rayData.Value.X < hitbox.absX + hitbox.width + parent.dx && parent.dx > 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.getTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dx = parent.dx;

                        hitbox.left = entity;

                        parent.dx = 0;
                        parent.x = rayData.Value.X - hitbox.width - hitbox.x;
                    }

                    //Left
                    ray = new Ray(hitbox.absX + skinWidth, raycastY, (float)(Math.PI));
                    rayData = ray.cast(EntityHandler.entities, parent);
                    if (rayData.HasValue && rayData.Value.X > hitbox.absX + parent.dx && parent.dx < 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.getTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dx = parent.dx;

                        hitbox.right = entity;

                        parent.dx = 0;
                        parent.x = rayData.Value.X - hitbox.x;
                    }
                }
            }
        }
    }
}
