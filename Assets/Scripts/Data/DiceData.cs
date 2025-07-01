using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DiceData
    {
        public List<DiceFaceData> FrontDiceFaces;
        public List<DiceFaceData> BackDiceFaces;

        public DiceData()
        {
            FrontDiceFaces = new List<DiceFaceData>();
            BackDiceFaces = new List<DiceFaceData>();
        }
    }
}