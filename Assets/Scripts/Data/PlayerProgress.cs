using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public PlayerStats PlayerStats;
        public MapData MapData;
        public int Coins;
        public int Materials;
        public bool IsPlayerCreated;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            PlayerStats = new PlayerStats();
            MapData = new MapData();
            Coins = 10;
            Materials = 30;
            IsPlayerCreated = false;
        }
    }
}

