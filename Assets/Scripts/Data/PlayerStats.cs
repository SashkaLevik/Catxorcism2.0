using System;
using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;

namespace Data
{
    [Serializable]
    public class PlayerStats
    {
        public int Level;
        public int Leadership;
        public int HandCapacity;
        public int DeckCapacity;
        public int DefaultDeckCapacity;
        public int DefaultLeadership;
        public int DefaultHandCapacity;
        public DiceData DiceData;
        public List<string> PlayerDeck;

        public PlayerStats()
        {
            Level = 1;
            Leadership = 3;
            HandCapacity = 3;
            DeckCapacity = 4;
            DefaultLeadership = 3;
            DefaultHandCapacity = 3;
            DefaultDeckCapacity = 4;
            DiceData = new DiceData();
            PlayerDeck = new List<string>();
        }

    }
}