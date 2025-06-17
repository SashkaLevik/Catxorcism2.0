using System;

namespace Data
{
    [Serializable]
    public class PathData
    {
        public int StartButtonIndex;
        public int EndButtonIndex;

        public PathData()
        {
            StartButtonIndex = 0;
            EndButtonIndex = 0;
        }
    }
}