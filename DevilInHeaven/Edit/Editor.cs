using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Black_Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using DevilInHeaven.Data;

namespace DevilInHeaven.Edit
{
    public static class Editor
    {
        private static Point gridLoc;
        public static int scale = 6;
        public static int tileSize = 16 * scale;

        private static int[,] grid;
        private const int rows = 12;
        private const int columns = 20;
        private static List<Player> players;

        private class Player
        {
            public int x;
            public int y;
            public bool isAngel;
            public static int size = 16 * scale;
            public Player(int _x, int _y, bool _isAngel)
            {
                x = _x;
                y = _y;
                isAngel = _isAngel;
            }
        }

        public static void Init()
        {
            Camera.SetDimensions(Game1.graphics, 1920 / 2, 1080 / 2, false);

            Game1.penumbra.AmbientColor = Color.White;

            grid = new int[rows, columns];
            players = new List<Player>();
        }

        public static void Update(GameTime gameTime)
        {
            ClickHandler.Update();

            //Handle creating tiles and whatnot
            KeyboardState keys = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            gridLoc = new Point((int)((mouse.X / Camera.gameScale) / tileSize), (int)((mouse.Y / Camera.gameScale) / tileSize));

            //Create platforms
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (gridLoc.Y >= rows || gridLoc.X >= columns) 
                {
                    Console.WriteLine("Warning: Requested to build tile outside bounds of map [" + gridLoc.Y + ", " + gridLoc.X + "]");
                }
                else
                    grid[gridLoc.Y, gridLoc.X] = 1;
            }

            else if (mouse.RightButton == ButtonState.Pressed)
            {
                if (gridLoc.Y >= rows || gridLoc.X >= columns)
                {
                    Console.WriteLine("Warning: Requested to remove tile outside bounds of map [" + gridLoc.Y + ", " + gridLoc.X + "]");
                }
                else
                    grid[gridLoc.Y, gridLoc.X] = 0;
            }


            //Create angels / devils
            if (keys.IsKeyDown(Keys.A))
            {
                bool didCollide = false;
                for (int i = 0; i < players.Count; i++)
                {
                    if (General.pointRectCollision(mouse.X, mouse.Y, players[i].x - Player.size / 2, players[i].y - Player.size / 2, Player.size, Player.size))
                    {
                        didCollide = true;
                        break;
                    }
                }

                if (!didCollide)
                {
                    Player player = new Player(mouse.X, mouse.Y, true);
                    players.Add(player);
                }
            }
            if (keys.IsKeyDown(Keys.D))
            {
                bool didCollide = false;
                for (int i = 0; i < players.Count; i++)
                {
                    if (General.pointRectCollision(mouse.X, mouse.Y, players[i].x - Player.size / 2, players[i].y - Player.size / 2, Player.size, Player.size))
                    {
                        didCollide = true;
                        break;
                    }
                }
                
                if (!didCollide)
                {
                    Player player = new Player(mouse.X, mouse.Y, false);
                    players.Add(player);
                }
            }

            //Delete player
            if (keys.IsKeyDown(Keys.S))
            {
                for (int i = 0; i < players.Count; i++)
                {
                    if (General.pointRectCollision(mouse.X, mouse.Y, players[i].x - Player.size / 2, players[i].y - Player.size / 2, Player.size, Player.size))
                    {
                        players.Remove(players[i]);
                    }
                }
            }
            
            //Export
            if (ClickHandler.IsClicked(Keys.E))
            {
                Export();
            }

            //Import
            if (ClickHandler.IsClicked(Keys.I))
            {
                Import();
            }
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //Draw Grid
            //TODO

            //Draw created platforms
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] <= 0) continue;
                    spriteBatch.Draw(General.createTexture(graphicsDevice),
                        new Vector2(c * tileSize * Camera.gameScale, r * tileSize * Camera.gameScale),
                        new Rectangle(0, 0, 1, 1),
                        Color.Black,
                        0,
                        Vector2.Zero,
                        tileSize * Camera.gameScale,
                        SpriteEffects.None,
                        0f);
                }
            }

            //Draw mouse loc
            Point mouseDisplayPos = new Point(gridLoc.X * tileSize, gridLoc.Y * tileSize); 
            spriteBatch.Draw(General.createTexture(graphicsDevice),
                new Vector2(mouseDisplayPos.X * Camera.gameScale, mouseDisplayPos.Y * Camera.gameScale),
                new Rectangle(0, 0, 1, 1),
                Color.DarkRed * .1f,
                0,
                Vector2.Zero,
                tileSize * Camera.gameScale,
                SpriteEffects.None,
                0f);


            //Draw player positions
            for (int i = 0; i < players.Count; i++)
            {
                spriteBatch.Draw(General.createTexture(graphicsDevice),
                    new Vector2(players[i].x - Player.size / 4, players[i].y - Player.size / 4),
                    new Rectangle(0, 0, 1, 1),
                    players[i].isAngel ? Color.Blue : Color.Red,
                    0,
                    Vector2.Zero,
                    Player.size * Camera.gameScale,
                    SpriteEffects.None,
                    0f);
            }

            spriteBatch.End();
        }

        public static void Export()
        {
            MapData mapData = new MapData();

            //Check legidemacy of map:
            if (players.Count != 4)
            {
                Console.WriteLine("Invalid number of players");
                return;
            }
            int angels = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].isAngel)
                    angels++;
            }
            if (angels != 3)
            {
                Console.WriteLine("Invalid number of angels (" + angels + ")");
                return;
            }

            Console.WriteLine("Map Name: ");
            mapData.name = Console.ReadLine();
            mapData.tiles = grid.ToJaggedArray();

            mapData.players = new PlayerData[4];
            for (int i = 0; i < 4; i++)
            {
                PlayerData playerData = new PlayerData();
                playerData.x = players[i].x;
                playerData.y = players[i].y;
                playerData.isAngel = players[i].isAngel;

                mapData.players[i] = playerData;
            }

            mapData.scale = scale;
            mapData.tileSize = tileSize;

            string serializedMapData;
            serializedMapData = JsonSerializer.Serialize(mapData);

            using (StreamWriter w = File.AppendText(@"ExportedMap.json"))
            {
                w.WriteLine(serializedMapData);
            }
            Console.WriteLine("-- Export Complete --");
        }

        public static void Import()
        {
            using (StreamReader r = File.OpenText(@"ExportedMap.json"))
            {
                string rawData = r.ReadLine();

                MapData mapData = JsonSerializer.Deserialize<MapData>(rawData);

                grid = new int[rows, columns];
                players.Clear();

                grid = mapData.tiles.As2DArray();

                for (int i = 0; i < 4; i++)
                {
                    PlayerData playerData = mapData.players[i];
                    players.Add(new Player(playerData.x, playerData.y, playerData.isAngel));
                }

                Console.WriteLine("-- Map " + mapData.name + " loaded --");
            }
        }
    }
}
