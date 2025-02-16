using GameEnvironment.GameLogic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DiceFaceData", menuName = "DiceFaceData")]
    public class DiceFaceData : ScriptableObject
    {
        public SuitType SuitType;
        public Material Material;
    }
}