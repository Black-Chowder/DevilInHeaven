using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Black_Magic
{
    //Handles playing requested animations and storing animation sequences
    public class Animator
    {
        //Stores and handles rectangles that represent locations on the sprite sheet for each frame of animation
        private class Animation
        {
            Animator parent;

            public bool looping = false;

            List<Rectangle> animationRects;

            //Constructor
            //NOTE: Just realized inReverse is never actually implemented
            public Animation(Animator parent, int startSprite, int endSprite, bool inReverse = false, bool looping = false)
            {
                this.parent = parent;

                this.looping = looping;

                animationRects = new List<Rectangle>();
                AppendAnimation(startSprite, endSprite, inReverse);
            }


            public void AppendAnimation(int startSprite, int endSprite, bool inReverse = false)
            {
                List<Rectangle> newRects = new List<Rectangle>();

                //Itteration Variables
                ushort spriteCounter = 0;
                Boolean wantToBreak = false;

                //TODO: Simplify loops to start at startSprite
                for (int y = 0; y < parent.rows; y++)
                {
                    for (int x = 0; x < parent.columns; x++)
                    {
                        spriteCounter++;

                        //Don't add frame if before starting sprite
                        if (spriteCounter < startSprite - 1) continue;

                        //Add rectangle to animation
                        newRects.Add(new Rectangle(
                            x * parent.width,
                            y * parent.height,
                            parent.width,
                            parent.height));

                        //End if spriteCounter is past endSprite
                        if (spriteCounter >= endSprite) { wantToBreak = true; break; }
                    }
                    if (wantToBreak) break;
                }

                //Reverses animation (if applicable) (only newly appended rectangles)
                if (inReverse) newRects = Reverse(newRects);

                //Add new rectangles to animation rectangles
                animationRects.AddRange(newRects);
            }


            private List<Rectangle> Reverse(List<Rectangle> originalRects, int startSprite = 0, int endSprite = -1)
            {
                //Sets endSprite to whole animation if left to default value
                if (endSprite == -1) { endSprite = originalRects.Count(); }

                List<Rectangle> reverseReturn = new List<Rectangle>();

                for (int i = startSprite; i < endSprite; i++)
                    reverseReturn.Add(originalRects[originalRects.Count() - i - 1]);

                return reverseReturn;
            }

            //Returns number of animation rectangles
            public int Count()
            {
                return animationRects.Count();
            }

            //Return animation rectangle pertaining to index
            public Rectangle get(int index)
            {
                return animationRects[index];
            }
        }
        private Animation animation;
        private Dictionary<string, Animation> animations;

        //Controls which sprite to display in animation
        private int animator;

        //Tracks time between keyframes
        private float aniMod = 10;
        public float KeyframeTime 
        { 
            get { return aniMod; } 
            set { aniMod = value; }
        }


        //Tracks if animation is over
        private bool animationOver = false;
        public bool isAnimationOver() { return animationOver; }

        public bool isFacingRight { get; set; }

        public float scale { get; set; } = 1;

        public float gameScale { private get; set; } = 1;

        public float angle { get; set; } = 0;

        public int width { get; private set; }
        public int height { get; private set; }
        public int columns { get; private set; }
        public int rows { get; private set; }

        public float layer { get; set; } = 0;

        public static class layers
        {
            public const float background = .1f;
            public const float walls = .2f;
            public const float objects = .3f;
            public const float effects = .4f;
            public const float objects2 = .5f;
            public const float people = .6f;
            public const float effects2 = .7f;
            public const float nextFloorBackground = .8f;
            public const float nextFloorWalls = .9f;
            public const float newxtFloorObjects = .99f;
        }
        /* Layer Designations
         * background => .1
         * walls => .2
         * objects => .3
         * effects => .4
         * objects 2 => .5
         * people => .6
         * effects 2 => .7
         * next floor background (faded) => .8
         * next floor walls (faded) => .9
         * next floor objects (faded) => .99
         */

        //TODO: Allow for storage of multiple sprite sheets
        public Texture2D spriteSheet { get; private set; }

        //Constructor(s)
        //TODO: Add constructor to read info / data from json file
        public Animator(Texture2D spriteSheet, int width, int height, int columns, int rows)
        {
            this.spriteSheet = spriteSheet;
            Init(width, height, columns, rows);
        }
        public Animator(Texture2D spriteSheet, Rectangle data)
        {
            this.spriteSheet = spriteSheet;
            Init(data.X, data.Y, data.Width, data.Height);
        }

        private void Init(int width, int height, int columns, int rows)
        {
            this.width = width;
            this.height = height;
            this.columns = columns;
            this.rows = rows;

            //Initialize animations dicrectory
            animations = new Dictionary<string, Animation>();
            animation = animations.FirstOrDefault().Value;
        }


        public void AddAnimation(string name, int startSprite, int endSprite, bool inReverse = false, bool looping = false)
        {
            animations.Add(name, new Animation(this, startSprite, endSprite, inReverse, looping));
        }


        public void AppendAnimation(string name, int startSprite, int endSprite, bool inReverse = false)
        {
            Animation animation = animations[name];
            animation.AppendAnimation(startSprite, endSprite, inReverse);
        }
        
        public void SetAnimation(string name)
        {
            animation = animations[name];
        }

        //Returns name of animation currently playing
        public string GetAnimation()
        {
            foreach (KeyValuePair<string, Animation> ani in animations)
            {
                if (ani.Value == animation)
                {
                    return ani.Key;
                }
            }
            return "";
        }

        //To be run by every frame by entity
        //TODO: Add animation timing handling (take gametime)
        public void Update(GameTime gt)
        {
            animator++;
            animationOver = false;

            //If animation isn't set, still try to run
            if (animation == null)
            {
                if (animations.ContainsKey("neutral"))
                    animation = animations["neutral"];

                else
                    animation = animations.FirstOrDefault().Value;

                animator = 0;
            }
            
            if (animator / aniMod >= animation.Count())
            {
                animator = animation.looping ? animator = 0 : (animation.Count() - 1);

                //Animation over is only true when on final keyframe of animation
                animationOver = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float x, float y)
        {
            animation ??= animations.FirstOrDefault().Value;

            spriteBatch.Draw(spriteSheet, //Texture
                new Vector2(x * gameScale, y * gameScale), //Position
                animation.get((int)(animator / aniMod)), //Source Rectangle
                Color.White, // Color Tint
                angle, //Rotation Angle
                new Vector2(width / 2, height / 2), //Origin Of Sprite (where to rotate around)
                scale * gameScale, //Scale
                isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, //Sprite Effects
                layer); //Layer
        }
    }
}
