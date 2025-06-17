using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class WorldData
    {
        public string Level;
        public int Stage;
        public int LevelNumber;
        public bool IsNewGame;
        public bool IsNewRun;
        public bool IsFirstRun;
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;
        public string CurrentPlayer;
        public List<string> OpenGuards;
        public List<string> RestoredBuildings;
        public List<string> OpenPlayers;

        public WorldData(string level)
        {
            Level = level;
            Stage = 1;
            LevelNumber = 1;
            IsNewGame = true;
            IsFirstRun = true;
            IsNewRun = true;
            MasterVolume = 1;
            MusicVolume = 1;
            SFXVolume = 1;
            RestoredBuildings = new List<string>();
            OpenPlayers = new List<string>();
            OpenGuards = new List<string>();
        }
    }
}
