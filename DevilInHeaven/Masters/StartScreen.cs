using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Black_Magic;

namespace DevilInHeaven
{
    public class StartScreen
    {
        public bool onStartScreen = true;

        private static Texture2D background;
        private static Texture2D startBtn;

        public StartScreen()
        {

        }

        public void Update()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!GamePad.GetState(i).IsConnected)
                    break;
                if (!GamePad.GetState(i).IsButtonDown(Buttons.A))
                    continue;

                MasterHandler.gameMaster = new GameMaster();
                onStartScreen = false;
            }
        }

        public static void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>(@"StartScreen");
            startBtn = Content.Load<Texture2D>(@"StartButton");
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(background,
                new Vector2(0, 0),
                new Rectangle(0, 0, 192, 120),
                Color.White,
                0,
                new Vector2(0, 0),
                Camera.gameScale * 10,
                SpriteEffects.None,
                0f);

            spriteBatch.Draw(startBtn,
                new Vector2(
                    (Camera.width / 2 - 320) * Camera.gameScale, 
                    (Camera.height / 2 + 200) * Camera.gameScale),
                new Rectangle(0, 0, 64, 32),
                Color.White,
                0,
                new Vector2(0, 0),
                Camera.gameScale * 10,
                SpriteEffects.None,
                0f);
        }
    }
}
