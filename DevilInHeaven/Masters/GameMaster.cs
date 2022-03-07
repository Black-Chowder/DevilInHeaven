using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using DevilInHeaven.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DevilInHeaven
{
    //Handles game stuff like initial connection, player stats and whatnot
    public class GameMaster : Entity
    {
        public int[] scores = new int[PlayerMaster.maxPlayers];

        public PlayerMaster playerMaster = new PlayerMaster();

        private Map map;
        private bool inWaitingRoom { get => map.name == "Waiting Room"; }

        public Player[] players = new Player[4];
        public int playerCount = 0;
        private int deltaPlayerCount = 0;

        public GameMaster() : base(0, 0)
        {
            ConnectPlayers();
        }

        //Puts players in waiting area and waits for players to connect
        public void ConnectPlayers() //TODO: Handle if controllers are already connected on initialization
        {
            //Load waiting room
            map ??= MapLoader.LoadMap(Properties.Resources.WaitingRoom);

            //Enable players to connect
            deltaPlayerCount = playerCount;
            playerCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (!GamePad.GetCapabilities(i).IsConnected)
                    break;
                playerCount++;
            }

            //New player
            for (int i = deltaPlayerCount; i < playerCount; i++)
            {
                Console.WriteLine("Player [" + i + "] connected");

                players[i] = new Player(
                    map.mapData.players[i].x,
                    map.mapData.players[i].y);

                EntityHandler.entities.Add(players[i]);
            }

            //Player disconnected
            //TODO: Handle multiple controllers disconnecting at the same time
            if (deltaPlayerCount > playerCount)
            {
                Console.WriteLine("Player [" + playerCount + "] disconnected");
                EntityHandler.entities.Remove(players[playerCount]);
                players[playerCount] = null;
            }
        }

        //Starts game (duh)
        //  May not need this.  Maybe just construct new GameMaster for every game
        public void StartGame()
        {
            //Send to waiting area
        }

        public override void Update(GameTime gameTime)
        {
            //Feed inputs into players
            for (int i = 0; i < playerCount; i++)
            {
                players[i].controller.gamePadState = GamePad.GetState(i);
            }

            //Handle waiting room
            if (inWaitingRoom)
            {
                ConnectPlayers();
                return;
            }

            //If in game, if devil is caught,
            //  send to waiting area for next round
        }

        public void NewRound()
        {
            //Send to waiting area for players to re-establish themselves
            
            //After alloted time, load new map

            //Be sure to have the players not able to move for the first
            //  few seconds
        }
    }
}
