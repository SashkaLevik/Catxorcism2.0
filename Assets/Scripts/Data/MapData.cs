using System;
using System.Collections.Generic;
using GameEnvironment.LevelRoutMap.RoutEventWindows;

namespace Data
{
    [Serializable]
    public class MapData
    {
        public int CurrentLayerCount;
        public int CurrentButtonIndex;
        public List<EventButtonData> SavedButtons;
        public List<PathData> SavedPaths;

        public MapData()
        {
            CurrentLayerCount = 0;
            CurrentButtonIndex = -1;
            SavedButtons = new List<EventButtonData>();
            SavedPaths = new List<PathData>();
        }
    }
}