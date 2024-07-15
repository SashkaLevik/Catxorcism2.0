using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public int Coins;
        public int Crystals;
        public List<string> OpenPlayers;
        public List<string> OpenedGuards;
        public bool IsPlayerCreated;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            Coins = 10;
            Crystals = 0;
            OpenPlayers = new List<string>();
            OpenedGuards = new List<string>();
            IsPlayerCreated = false;
        }
    }
}

