using Assets.Scripts.GameEnvironment.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.GameLogic
{
    public class DeckCreator : MonoBehaviour
    {
        private const string PortDeckShaffled = "Deck/PortDeck";
        private const string MarketDeckShaffled = "Deck/MarketDeck";

        private Boss _bossCard;
        private List<Card> _portDeck = new List<Card>();
        private List<Card> _marketDeck = new List<Card>();

        public List<Card> PortDeck => _portDeck;
        public List<Card> MarketDeck => _marketDeck;


        private void Awake()
        {
            _portDeck = Resources.LoadAll<Card>(PortDeckShaffled).OrderBy(x => Random.value).ToList();
            _marketDeck = Resources.LoadAll<Card>(MarketDeckShaffled).OrderBy(x => Random.value).ToList();
            SetBossPosition(_portDeck);
            SetBossPosition(_marketDeck);
        }

        private void SetBossPosition(List<Card> cards)
        {
            foreach (var card in cards)
            {
                if (card.GetComponent<Boss>() != null)
                    _bossCard = card.GetComponent<Boss>();
            }

            if (_bossCard != null)
            {
                cards.Remove(_bossCard);
                cards.Add(_bossCard);
            }
        }       
    }
}