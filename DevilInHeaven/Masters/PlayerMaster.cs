using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DevilInHeaven.Entities
{
    public class PlayerMaster //TODO: Remove and replace with GameMaster
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
            ControllerConnect();

            for (int i = 0; i < playerCount; i++)
            {
                players[i] = new Player(300, 100);
                EntityHandler.entities.Add(players[i]);
            }

            //Temporary keyboard controled player
            if (playerCount == 0)
            {
                players[0] = new Player(300, 100);
                players[0].controller.isKeyboardControled = true;
                EntityHandler.entities.Add(players[0]);
            }
        }

        //To be called when connecting players to game
        public void ControllerConnect()
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

        //Creates players in requested locations
        //First player is devil
        public void CreatePlayers(Vector2[] playerPositions)
        {
            //TODO: Properly assign indecies to player controlers

            players[0] = new Player(playerPositions[0].X, playerPositions[0].Y, false);
            EntityHandler.entities.Add(players[0]);
            players[0].controller.isKeyboardControled = true;
            for (int i = 1; i < playerCount; i++)
            {
                players[i] = new Player(playerPositions[i].X, playerPositions[i].Y, true);
                EntityHandler.entities.Add(players[i]);
            }
        }

        //Note: Must update before gamePad does
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < playerCount; i++)
            {
                players[i].controller.gamePadState = GamePad.GetState(i);
            }
        }
    }
}
