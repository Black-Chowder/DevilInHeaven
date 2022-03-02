using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DevilInHeaven.Entities
{
    public class PlayerMaster
    {
        public const int maxPlayers = 4;
        private Player[] players = new Player[maxPlayers];
        public int playerCount { get; private set; } = 0;

        private GamePadCapabilities[] controlers = new GamePadCapabilities[maxPlayers];

        public PlayerMaster()
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                controlers[i] = GamePad.GetCapabilities(i);
            }
            ControlerConnect();

            for (int i = 0; i < playerCount; i++)
            {
                players[i] = new Player(300, 100);
                EntityHandler.entities.Add(players[i]);
            }

            //Temporary keyboard controled player
            if (playerCount == 0)
            {
                players[0] = new Player(300, 100);
                players[0].controler.isKeyboardControled = true;
                EntityHandler.entities.Add(players[0]);
            }
        }

        //To be called when connecting players to game
        public void ControlerConnect()
        {
            //TODO: handle players joining and leaving without destroying their entity
            //      every frame
            
            playerCount = 0;
            for (int i = 0; i < maxPlayers; i++)
            {
                if (!controlers[i].IsConnected)
                    break;

                playerCount++;
            }
        }


        //Note: Must update before gamePad does
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < playerCount; i++)
            {
                players[i].controler.gamePadState = GamePad.GetState(i);
            }
        }
    }
}
