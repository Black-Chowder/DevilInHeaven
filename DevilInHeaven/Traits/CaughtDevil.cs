using System;
using System.Collections.Generic;
using System.Text;
using DevilInHeaven.Entities;
using Black_Magic;
using Microsoft.Xna.Framework;
using DevilInHeaven;

namespace DevilInHeaven.Traits
{
    public class CaughtDevil : Trait<Player>
    {
        public bool isCaught = false;

        public CaughtDevil(Player parent) : base(parent) { }

        public override void Update(GameTime gameTime)
        {
            if (parent.isAngel)
                return;

            isCaught = false;
            Player[] players = MasterHandler.gameMaster.players;
            for (int i = 0; i < MasterHandler.gameMaster.playerCount; i++)
            {
                if (players[i] == parent)
                    continue;
                
                if (General.rectCollision(
                    players[i].hitbox.absX, 
                    players[i].hitbox.absY,
                    players[i].hitbox.width,
                    players[i].hitbox.height,
                    parent.hitbox.absX,
                    parent.hitbox.absY,
                    parent.hitbox.width,
                    parent.hitbox.height))
                {
                    isCaught = true;
                    return;
                }
            }
        }
    }
}
