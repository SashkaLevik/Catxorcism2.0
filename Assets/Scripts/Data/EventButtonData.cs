using System;
using System.Collections.Generic;
using GameEnvironment.LevelRoutMap.RoutEventWindows;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class EventButtonData
    {
        public Vector3 Position;
        public RoutEventType EventType;
        public bool IsReachable;
        public List<int> ConnectedButtonIndices;

        public EventButtonData()
        {
            Position = Vector3.zero;
            IsReachable = false;
            ConnectedButtonIndices = new List<int>();
        }
    }
}