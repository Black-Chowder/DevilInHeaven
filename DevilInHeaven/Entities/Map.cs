using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using DevilInHeaven.Data;
using Microsoft.Xna.Framework;

namespace DevilInHeaven.Entities
{
    public class Map : Entity
    {
        public static int tileScale = 6;
        public static int tileSize = 32;

        public Map(MapData mapData) : base(0, 0)
        {
            //Create platforms
            //TODO

            //Create players
            Vector2[] playerPositions = new Vector2[4];
            int angelCount = 0;
            for (int i = 0; i < mapData.players.Length; i++) { 
                if (!mapData.players[i].isAngel)
                {
                    playerPositions[0] = new Vector2(mapData.players[i].x, mapData.players[i].y);
                    continue;
                }

                playerPositions[1 + angelCount] = new Vector2(mapData.players[i].x, mapData.players[i].y);
                angelCount++;
            }

            MasterHandler.playerMaster.CreatePlayers(playerPositions);
        }
    }
}
