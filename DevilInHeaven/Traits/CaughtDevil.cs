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
        private bool _isCaught = false;
        public bool isCaught 
        { 
            get => _isCaught;
            set 
            {
                _isCaught = value;
                parent.controller.isActive = true;
                if (!parent.isAngel)
                    parent.controller.isActive = !value;
            } 
        }

        public CaughtDevil(Player parent) : base(parent) { }

        public override void Update(GameTime gameTime)
        {
            Player[] players = MasterHandler.gameMaster.players;
            for (int i = 0; i < MasterHandler.gameMaster.playerCount; i++)
            {
                if (players[i] == parent)
                    continue;

                
                if (parent.rigidbody.hitboxes[0].GetEntitiesTouching().Contains(players[i]))
                {
                    isCaught = true;
                    players[i].caughtDevil.isCaught = true;
                    return;
                }
            }

            if (parent.isAngel)
            {
                parent.animator.SetAnimation("angel");
            }
            else if (!parent.isAngel && !isCaught)
            {
                parent.animator.SetAnimation("devil");
            }
            else if (!parent.isAngel && isCaught)
            {
                parent.animator.SetAnimation("death");
            }
        }
    }
}
