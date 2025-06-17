using GameEnvironment.GameLogic;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardData")]

    public class CardData : ScriptableObject
    {
        public GameObject CardPrefab;
        public Sprite RawPosition;
        public SuitType SuitType;
        public Sprite SuitIcon;
        public string EnName;
        public string RuName;
        public int Damage;
        public int Health;
        public int ActionPoints;
        public int UpgradePrice;
        public int Level;
    }
}
