using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public PlayerStats PlayerStats;
        public int Coins;
        public int Crystals;
        public bool IsPlayerCreated;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            PlayerStats = new PlayerStats();
            Coins = 10;
            Crystals = 0;
            IsPlayerCreated = false;
        }
    }
}

