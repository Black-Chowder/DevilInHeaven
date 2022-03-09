using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using DevilInHeaven.Data;
using DevilInHeaven.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DevilInHeaven
{
    //Handles game stuff like initial connection, player stats and whatnot
    public class GameMaster
    {
        public int[] scores = new int[PlayerMaster.maxPlayers];

        public PlayerMaster playerMaster = new PlayerMaster();

        private Map map;
        private const int totalMaps = 5;
        public bool gameStart { get; private set; }

        public Player[] players = new Player[4];
        public int playerCount = 0;
        private int deltaPlayerCount = 0;
        private int devilIndex = 0;

        public int phase { get; private set; } = CONNECTING; //Indicates how to control players
        private const int CONNECTING = 0; //In waiting area waiting for controllers to connect
        private const int WAITING = 1; //In waiting area (for character assignment)
        private const int PREGAME = 2; //In map but can't move
        private const int INGAME = 3;  //In map and playing
        private const int POSTGAME = 4;//Devil is dead but still in map
        private const int WIN = 5; //Win screen

        private double timer = 0d;
        private const double waitingTimer = 5d * 1000d;

        public int roundsPlayed { get; private set; } = 0;
        public int roundsPerPlayer = 5;
        private List<int> devilTurns;

        public bool GameOver { get; private set; } = false;

        private Random rand = new Random();

        public GameMaster()
        {
            ConnectPlayers();
        }

        //Puts players in waiting area and waits for players to connect
        public void ConnectPlayers() //TODO: Handle if controllers are already connected on initialization
        {
            //Load waiting room
            map ??= MapLoader.LoadMap(Properties.Resources.WaitingRoom);
            phase = CONNECTING;

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

            //Start game
            if (playerCount > 1 && players[0].controller.startPressed)
            {
                Console.WriteLine("\n == Game Has Begun == ");

                for (int i = 0; i < playerCount; i++)
                    players[i].score = 0;

                //Initialize times devil gets to play
                devilTurns = new List<int>(playerCount * roundsPerPlayer);
                for (int i = 0; i < playerCount; i++)
                    for (int j = 0; j < roundsPerPlayer; j++)
                        devilTurns.Add(i);

                NewRound();
            }
        }

        public void Update(GameTime gt)
        {
            //Feed inputs into players
            for (int i = 0; i < playerCount; i++)
            {
                players[i].controller.gamePadState = GamePad.GetState(i);
            }

            Player[] angels;
            int angelCount;

            switch (phase)
            {
                //Waiting Room
                case CONNECTING:
                    ConnectPlayers();
                    break;

                //Round behavior
                case WAITING:
                    //Increment Timer
                    timer -= gt.ElapsedGameTime.TotalMilliseconds;

                    //After alloted time, load new map
                    if (timer > 0) break;
                    timer = waitingTimer;
                    //TODO: Add smooth transition

                    map = MapLoader.LoadMap(getMap(rand.Next(1, totalMaps + 1)));

                    //Spawn devil
                    players[devilIndex].x = map.playerPositions[0].X;
                    players[devilIndex].y = map.playerPositions[0].Y;
                    players[devilIndex].isAngel = false;

                    //Spawn angels
                    angels = new Player[playerCount - 1];
                    angelCount = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (players[i] is null || !players[i].isAngel)
                            continue;
                        angels[angelCount] = players[i];
                        angelCount++;
                    }
                    rand.Shuffle(angels);
                    for (int i = 0; i < playerCount - 1; i++)
                    {
                        if (angels[i] is null)
                            continue;
                        angels[i].x = map.playerPositions[i + 1].X;
                        angels[i].y = map.playerPositions[i + 1].Y;
                        angels[i].isAngel = true;
                    }

                    for (int i = 0; i < playerCount; i++)
                        EntityHandler.entities.Add(players[i]);


                    //Deactivate player movement
                    for (int i = 0; i < playerCount; i++)
                    {
                        players[i].controller.isActive = false;
                        players[i].gravity.isActive = false;
                        players[i].dx = 0;
                        players[i].dy = 0;
                    }

                    phase = PREGAME;
                    break;

                case PREGAME:
                    //Increment Timer
                    timer -= gt.ElapsedGameTime.TotalMilliseconds;

                    //After alloted time, load new map
                    if (timer > 0) break;
                    timer = waitingTimer;

                    //Reactivate player movement
                    for (int i = 0; i < playerCount; i++)
                    {
                        players[i].controller.isActive = true;
                        players[i].gravity.isActive = true;

                        players[i].caughtDevil.isCaught = false;
                    }

                    phase = INGAME;

                    break;
                case INGAME:
                    players[devilIndex].score += gt.ElapsedGameTime.TotalSeconds;
                    if (players[devilIndex].caughtDevil.isCaught)
                    {
                        phase = POSTGAME;
                        Console.WriteLine("Devil Caught");
                    }
                        
                    break;
                case POSTGAME:
                    //Increment Timer
                    timer -= gt.ElapsedGameTime.TotalMilliseconds;

                    //After alloted time, load new map
                    if (timer > 0) break;
                    timer = waitingTimer * 5;

                    if (roundsPlayed >= roundsPerPlayer * playerCount)
                    {
                        map = MapLoader.LoadMap(Properties.Resources.Win);

                        Console.WriteLine("Final Scores:");
                        for (int i = 0; i < playerCount; i++)
                            Console.WriteLine("\tPlayer[" + i + "] score = " + players[i].score);

                        List<Player> pList = new List<Player>(playerCount);
                        for (int i = 0; i < playerCount; i++)
                            pList.Add(players[i]);
                        pList.Sort((p1, p2) => p2.score.CompareTo(p1.score));
                        for (int i = 0; i < playerCount; i++)
                        {
                            pList[i].x = map.mapData.players[i].x;
                            pList[i].y = map.mapData.players[i].y;
                            pList[i].isAngel = i != 0;
                            pList[i].dx = 0;
                            pList[i].dy = 0;
                            players[i].caughtDevil.isCaught = false;
                            players[i].caughtDevil.canBeCaught = false;
                            EntityHandler.entities.Add(players[i]);
                        }
                        Console.WriteLine(" == Game Over == \n");

                        phase = WIN;
                    }
                    else
                        NewRound();
                    break;
                case WIN:
                    timer -= gt.ElapsedGameTime.TotalMilliseconds;

                    if (timer > 0) break;
                    timer = waitingTimer;

                    GameOver = true;
                    break;
            }
        }

        public void NewRound()
        {
            gameStart = true;
            phase = WAITING;
            timer = waitingTimer;
            roundsPlayed++;
            EntityHandler.entities.Clear();
            map = MapLoader.LoadMap(Properties.Resources.WaitingRoom);

            //Assign players angles / devils (can definitely be made more efficient, but I can't be bothered right now)
            //TODO: Change devilIndex to not be truely random but randomly select index from set of indecies so that everyone
            //      gets to play devil same # of times
            int devilTurnsIndex = rand.Next(devilTurns.Count);
            devilIndex = devilTurns[devilTurnsIndex];
            devilTurns.RemoveAt(devilTurnsIndex);
            Console.WriteLine("Round " + roundsPlayed + " started.  Player [" + devilIndex + "] is devil");
            for (int i = 0; i < playerCount; i++)
                Console.WriteLine("\tPlayer[" + i + "] score = " + players[i].score);

            for (int i = 0; i < playerCount; i++)
            {
                players[i].isAngel = true;
            }
            //Spawn devil
            players[devilIndex].x = map.playerPositions[0].X;
            players[devilIndex].y = map.playerPositions[0].Y;
            players[devilIndex].isAngel = false;

            //Spawn angels
            Player[] angels;
            int angelCount;
            angels = new Player[playerCount - 1];
            angelCount = 0;
            for (int i = 0; i < 4; i++)
            {
                if (players[i] is null || !players[i].isAngel)
                    continue;
                angels[angelCount] = players[i];
                angelCount++;
            }
            rand.Shuffle(angels);
            for (int i = 0; i < playerCount - 1; i++)
            {
                if (angels[i] is null)
                    continue;
                angels[i].x = map.playerPositions[i + 1].X;
                angels[i].y = map.playerPositions[i + 1].Y;
                angels[i].isAngel = true;
            }

            //Place players in correct position
            for (int i = 0; i < playerCount; i++)
            {
                if (players[i] is null)
                    continue;
                players[i].x = map.playerPositions[i].X;
                players[i].y = map.playerPositions[i].Y;
                players[i].playerId = i;
                players[i].caughtDevil.isCaught = false;
            }

            for (int i = 0; i < playerCount; i++)
                EntityHandler.entities.Add(players[i]);
        }

        private byte[] getMap(int index)
        {
            switch (index)
            {
                case 1: 
                    return Properties.Resources.Map1;
                case 2:
                    return Properties.Resources.Map2;
                case 3:
                    return Properties.Resources.Map3;
                case 4:
                    return Properties.Resources.Map4;
                case 5:
                    return Properties.Resources.Map5;
            }
            return null;
        }
    }
}
