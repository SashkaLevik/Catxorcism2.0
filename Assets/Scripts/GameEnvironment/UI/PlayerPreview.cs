using Data;
using TMPro;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class PlayerPreview : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthAmount;
        [SerializeField] private CardData _cardData;
        [SerializeField] private GameObject _prefab;

        public CardData CardData => _cardData;

        private void Start()
        {
            _healthAmount.text = _cardData.Health.ToString();
        }
    }
}