using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Black_Magic
{

    public class Camera
    {
        public static Random rand = new Random();

        //Position
        public static float x { get; private set; } = 0;
        public static float y { get; private set; } = 0;
        public static float z { get; set; } = 0;

        //Size
        public const int defaultWidth = 1920;
        public const int defaultHeight = 1080;
        public static int width { get; private set; } = defaultWidth;
        public static int height { get; private set; } = defaultHeight;

        //Game scaler
        public static float gameScale { get; private set; } = 1f;
        public static float scale { get; private set; } = 2f;

        //Zoom
        public static float zoom { get; set; } = 1f;

        //Target position.  Where the camera wants to go
        private static float targetX = 0;
        private static float targetY = 0;

        //Speed
        public static float speed { get; set; } = 15f;

        //Camera Shake Variables
        private static float timer = 0;
        private static float intensity = 0;

        //Manual Mouse Control (Mostly To Be Used For Testing)
        private static bool mouseControl = true;
        private static Vector2 deltaMouse = Vector2.Zero;

        //To run every frame
        public static void Update(GameTime gameTime)//TODO: Account for gametime
        {
            camShakeHandler(gameTime);

            //Mouse control stuff
            MouseState mouse = Mouse.GetState();
            if (mouseControl) MouseControlHandler(mouse);
            deltaMouse = mouse.Position.ToVector2();

            //Move camera to requested location based on speed
            x += (targetX - x) / speed;
            y += (targetY - y) / speed;
        }

        //Update gamescale according to screen dimensions
        private static void updateGameScale()
        {
            gameScale = width / (float)defaultWidth;
            Console.WriteLine("Game Scale Updated.  New Game Scale = " + gameScale);
        }

        //Sets window dimensions to fullscreen
        public static void SetDimensions(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            SetDimensions(graphics, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, true);
        }

        //Sets window dimensions to requested width and height
        public static void SetDimensions(GraphicsDeviceManager graphics, int Width = defaultWidth, int Height = defaultHeight, Boolean isFullScreen = true)
        {
            //Actually change window dimensions
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();

            //Stores new changes
            Camera.width = Width;
            Camera.height = Height;

            updateGameScale();
        }

        //Force the camera to go to a specific location
        public static void SudoGoTo(float x, float y)
        {
            //Sets both camera and target to same location
            Camera.x = x;
            Camera.y = y;
            targetX = x;
            targetY = y;
        }

        //Travel to target location according to speed
        public static void GoTo(float x, float y)
        {
            targetX = x;
            targetY = y;
        }

        //Public method to make camera shake
        public static void Shake(float intensity, float duration)
        {
            timer = duration;
            Camera.intensity = intensity;

        }

        //Handle camera shaking
        private static void camShakeHandler(GameTime gameTime)
        {
            //Only run if timer is more than 0
            if (timer <= 0) return;

            //Update timer
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Pick close positions to go to
            //TODO: Adjust so that these are picked in a circle, not a square (using sin and cos)
            float dx = ((float)rand.NextDouble() * 2 - 1) * intensity;
            float dy = ((float)rand.NextDouble() * 2 - 1) * intensity;
            

            x += dx;
            y += dy;


            Console.WriteLine("Shaking!\nTimer = " + timer + " | Elapsed Game Time = " + gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        private static void MouseControlHandler(MouseState mouse)
        {
            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                Camera.SudoGoTo(Camera.x + (deltaMouse.X - mouse.X) / Camera.gameScale, Camera.y + (deltaMouse.Y - mouse.Y) / Camera.gameScale);
            }
        }
    }

    /*public IEnumerator Shake(float duration, float intensity)
    {
        Vector3 originalPos = transform.localPosition;

        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }
    */
}
