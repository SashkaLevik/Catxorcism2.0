using Assets.Scripts.Data;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class BuyButton : MonoBehaviour
    {
        private CardData _cardData;

        public CardData Guard => _cardData;

        public void GetCard(CardData cardData) =>
            _cardData = cardData;
        
    }
}