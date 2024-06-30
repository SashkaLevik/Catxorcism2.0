using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class WorldData
    {
        public string Level;
        public int Stage;
        public int FirstLvlStages;
        public int LevelNumber;
        public bool IsNewGame;
        public bool IsFirstRun;
        public float MasterVolume;
        public float MusicVolume;
        public float SFXVolume;

        public WorldData(string level)
        {
            Level = level;
            Stage = 1;
            FirstLvlStages = 6;
            LevelNumber = 1;
            IsNewGame = true;
            IsFirstRun = true;
            MasterVolume = 1;
            MusicVolume = 1;
            SFXVolume = 1;
        }
    }
}
