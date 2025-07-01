using Data;
using GameEnvironment.GameLogic.DiceFolder;
using UnityEngine;

namespace GameEnvironment.LevelRoutMap.RoutEventWindows
{
    public class DiceLayoutButton : MonoBehaviour
    {
        public Dice Dice { get; private set; }
        public DiceFaceData FaceData { get; private set; }

        public void SetDiceData(Dice dice, DiceFaceData faceData)
        {
            Dice = dice;
            FaceData = faceData;
        }
    }
}