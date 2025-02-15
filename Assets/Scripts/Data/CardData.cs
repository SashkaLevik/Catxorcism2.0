using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.Units;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardData")]

    public class CardData : ScriptableObject
    {
        public Card CardPrefab;
        public string RuName;
        public string EnName;
        public string RuDescription;
        public string EnDescription;
        public int Damage;
        public int Health;
        public int ActivatePrice;
        public int SummonPrice;
        public int UpgradePrice;
        public int Level;
    }
}
