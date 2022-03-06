using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Black_Magic;
using DevilInHeaven.Entities;
using DevilInHeaven.Data;

namespace DevilInHeaven
{
    public static class MapLoader
    {
        public static Map LoadMap(byte[] mapJson)
        {
            EntityHandler.entities.Clear();

            MapData mapData = JsonSerializer.Deserialize<MapData>(mapJson);

            Map map = new Map(mapData);
            EntityHandler.entities.Add(map);
            return map;
        }
    }
}
