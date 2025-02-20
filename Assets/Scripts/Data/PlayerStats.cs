using System;
using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;

namespace Data
{
    [Serializable]
    public class PlayerStats
    {
        public int Leadership;
        public int HandCapacity;
        public List<Guard> PlayerGuards;
        public List<DiceFaceData> FrontDiceFaces;
        public List<DiceFaceData> BackDiceFaces;
        public List<string> OpenPlayers;

        public PlayerStats()
        {
            Leadership = 0;
            HandCapacity = 0;
            PlayerGuards = new List<Guard>();
            OpenPlayers = new List<string>();
            FrontDiceFaces = new List<DiceFaceData>();
            BackDiceFaces = new List<DiceFaceData>();
        }

    }
}