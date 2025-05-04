using GameEnvironment.GameLogic;
using UnityEngine;
using UnityEngine.UI;

namespace Data
{
    [CreateAssetMenu(fileName = "DiceFaceData", menuName = "DiceFaceData")]
    public class DiceFaceData : ScriptableObject
    {
        public SuitType SuitType;
        public Sprite SuitImage;
        public Material Material;
    }
}