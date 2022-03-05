using System;
using System.Collections.Generic;
using System.Text;
using Black_Magic;
using DevilInHeaven.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DevilInHeaven.Entities
{
    public class Map : Entity
    {
        public static int tileScale = 6;
        public static int tileSize = 32;

        private static int[,] grid;

        private static Texture2D tileSet;
        private static Texture2D cloudSprites;

        private RenderTarget2D renderTarget;
        private int textureWidth, textureHeight;

        public Map(MapData mapData) : base(0, 0)
        {

            //Create platforms
            tileScale = mapData.scale;
            tileSize = mapData.tileSize / tileScale;
            //TODO: Create matrix, not individual platforms
            grid = mapData.tiles.As2DArray();

            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] <= 0)
                    {
                        grid[r, c] = -1;
                        continue;
                    }

                    EntityHandler.entities.Add(new Platform(c * tileSize * tileScale, r * tileSize * tileScale, tileSize * tileScale, tileSize * tileScale));
                }
            }

            //Assign grid with adjacency values
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] <= 0)
                    {
                        grid[r, c] = -1;
                        continue;
                    }

                    int[,] sight = getSight(new Point(r, c), grid);
                    grid[r, c] = getTextureIndex(sight);
                }
            }

            //Create render target
            textureWidth = grid.GetLength(1) * tileSize;
            textureHeight = grid.GetLength(0) * tileSize;
            renderTarget = new RenderTarget2D(Game1.graphicsDevice, textureWidth, textureHeight);
            Game1.graphicsDevice.SetRenderTarget(renderTarget);
            Game1.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Game1.graphicsDevice.Clear(Color.Transparent);
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    if (grid[y, x] < 0) continue;
                    Game1.spriteBatch.Draw(tileSet,
                        new Vector2(x * tileSize, y * tileSize),
                        new Rectangle((grid[y, x]) % 5 * tileSize, (grid[y, x]) / 5 * tileSize, tileSize, tileSize),
                        Color.White);
                }
            }
            Game1.spriteBatch.End();
            Game1.graphicsDevice.SetRenderTarget(null);



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

        private static int[,] getSight(Point origin, int[,] grid)
        {
            int[,] sight = new int[3, 3];
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point toLook = new Point(x + origin.X, y + origin.Y);
                    if (toLook.X < 0 || toLook.X >= grid.GetLength(0) || toLook.Y < 0 || toLook.Y >= grid.GetLength(1))
                    {
                        sight[x + 1, y + 1] = 7; //middle sprite
                        continue;
                    }
                    sight[x + 1, y + 1] = grid[toLook.X, toLook.Y];
                }
            }
            return sight;
        }

        //Converts local position data into index of sprite to be drawn
        private static int getTextureIndex(int[,] sight)
        {
            //0 = Occupied, 1 = !Occupied
            int left = sight[1, 0] >= 0 ? 1 : 0;
            int right = sight[1, 2] >= 0 ? 1 : 0;
            int top = sight[0, 1] >= 0 ? 1 : 0;
            int bottom = sight[2, 1] >= 0 ? 1 : 0;

            int comp = (left << 3) + (right << 2) + (top << 1) + bottom;

            switch (comp)
            {
                case 0b0101:
                    return 0;
                case 0b1101:
                    return 1;
                case 0b1001:
                    return 2;
                case 0b0001:
                    return 3;
                case 0b0111:
                    return 5;
                case 0b1111:
                    return 6;
                case 0b1011:
                    return 7;
                case 0b0011:
                    return 8;
                case 0b0110:
                    return 10;
                case 0b1110:
                    return 11;
                case 0b1010:
                    return 12;
                case 0b0010:
                    return 13;
                case 0b0100:
                    return 15;
                case 0b1100:
                    return 16;
                case 0b1000:
                    return 17;
                case 0b0000:
                    return 18;
            }
            return 0;
        }

        public static void LoadContent(ContentManager Content)
        {
            tileSet = Content.Load<Texture2D>(@"HeavenTileSet16");
            cloudSprites = Content.Load<Texture2D>(@"Clouds");
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Draw(renderTarget,
                Vector2.Zero,
                new Rectangle(0, 0, textureWidth, textureHeight),
                Color.White,
                0,
                Vector2.Zero,
                tileScale * Camera.gameScale,
                SpriteEffects.None,
                0f);
        }

        ~Map()
        {
            renderTarget.Dispose();
        }
    }
}
