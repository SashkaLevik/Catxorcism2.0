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
        //public PlayerStats PlayerStats;
        //public PlayerParts PlayerParts;
        public List<CardData> OpenPlayers;
        public List<CardData> ClosePlayers;
        public List<CardData> OpenedGuards;
        public List<CardData> CloseGuards;
        public bool IsPlayerCreated;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            Coins = 0;
            Crystals = 0;
            //PlayerStats = new PlayerStats();
            //PlayerParts = new PlayerParts();
            OpenPlayers = new List<CardData>();
            ClosePlayers = new List<CardData>();
            OpenedGuards = new List<CardData>();
            CloseGuards = new List<CardData>();
            IsPlayerCreated = false;
        }
    }
}

