using System;
using System.Collections.Generic;
using GameEnvironment.LevelRoutMap.RoutEventWindows;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class RoutMapData
    {
        [Serializable]
        public class RoutButtonData
        {
            public Vector3 position;
            public RoutEventType EventType;
            public bool isVisited;
            public bool isReachable;
            public List<int> connectedNodeIndices;
        }
        
        [Serializable]
        public class PathData
        {
            public int StartButtonIndex;
            public int EndButtonIndex;
        }

        public int CurrentLayerCount;
        public int CurrentButtonIndex = -1;
        public List<RoutButtonData> SavedButtons = new List<RoutButtonData>();
        public List<PathData> SavedPaths = new List<PathData>();

        public void ResetMapData()
        {
            SavedButtons.Clear();
            SavedPaths.Clear();
            CurrentButtonIndex = -1;
        }
    }
}