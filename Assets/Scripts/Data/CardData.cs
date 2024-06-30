using Assets.Scripts.GameEnvironment.Units;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [CreateAssetMenu(fileName = "CardData", menuName = "CardData")]

    public class CardData : ScriptableObject
    {
        public Card CardPrefab;
        public int Damage;
        public int Health;
        public int AP;
        public int ActivatePrice;
        public int SummonPrice;
        public int UpgradePrice;
        public int Level;
    }
}
