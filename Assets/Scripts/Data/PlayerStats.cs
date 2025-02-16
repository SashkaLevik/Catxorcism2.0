using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class PlayerStats
    {
        public int Leadership;
        public List<DiceFaceData> FrontDiceFaces;
        public List<DiceFaceData> BackDiceFaces;
        public List<string> OpenPlayers;

        public PlayerStats()
        {
            Leadership = 0;
            OpenPlayers = new List<string>();
            FrontDiceFaces = new List<DiceFaceData>();
            BackDiceFaces = new List<DiceFaceData>();
        }

    }
}