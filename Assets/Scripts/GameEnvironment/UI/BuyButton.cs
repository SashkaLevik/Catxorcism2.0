using Data;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class BuyButton : MonoBehaviour
    {
        private CardData _cardData;

        public CardData Guard => _cardData;

        public void GetCard(CardData cardData) =>
            _cardData = cardData;
        
    }
}