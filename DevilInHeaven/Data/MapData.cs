using System;
using System.Collections.Generic;
using System.Text;

namespace DevilInHeaven.Data
{
    public class MapData
    {
        public string name { get; set; }
        public int[][] tiles { get; set; }
        public Player[] players { get; set; }
        public MapData() { }
    }
    public class Player
    {
        public bool isAngel { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public Player() { }
    }
}
