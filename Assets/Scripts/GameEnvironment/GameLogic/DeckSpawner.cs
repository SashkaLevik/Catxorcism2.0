using Assets.Scripts.GameEnvironment.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.GameEnvironment.UI;
using UnityEngine.Events;

namespace Assets.Scripts.GameEnvironment.GameLogic
{
    public class DeckSpawner : MonoBehaviour
    {
        [SerializeField] DeckCreator _deckCreator;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _deckSpawnPos;
        [SerializeField] private List<RectTransform> _firstRaw;
        [SerializeField] private List<RectTransform> _secondRaw;
        [SerializeField] private TMP_Text _remainingCards;
        [SerializeField] private Button _endTurn;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private RectTransform _pos;

        private int _waveNumber;
        private int _maxWave = 2;
        private Boss _bossCard;
        private Card _spawnedCard;
        private Card _movedCard;
        private List<Card> _currentDeck = new List<Card>();
        private List<Card> _spawnedCards = new List<Card>();

        public event UnityAction<int> BossDied;

        private void Start()
        {
            SetDeckToSpawn(_waveNumber);
            SpawnCards();
            _endTurn.onClick.AddListener(Attack);
        }

        private void OnDestroy()
        {
            _endTurn.onClick.RemoveListener(Attack);
        }        

        public void ActivateRaw()
        {
            foreach (var pos in _firstRaw)
            {
                if (pos.GetComponentInChildren<Card>() != null)
                    pos.GetComponentInChildren<Card>().Activate();
            }
        }

        public void DisactivateRaw()
        {
            foreach (var pos in _firstRaw)
            {
                if (pos.GetComponentInChildren<Card>() != null)
                    pos.GetComponentInChildren<Card>().Disactivate();
            }
        }

        public void Attack()=>
            StartCoroutine(StartAttack());

        private IEnumerator StartAttack()
        {
            DisactivateRaw();
            yield return new WaitForSeconds(0.3f);

            foreach (var pos in _firstRaw)
            {                
                if (pos.GetComponentInChildren<Enemy>() != null)
                {
                    pos.GetComponentInChildren<Enemy>().Attack();
                    yield return new WaitForSeconds(0.3f);
                }
                yield return null;
            }

            MoveOnFirst();
            yield return new WaitForSeconds(0.3f);
            SpawnCards();
            yield return new WaitForSeconds(0.3f);
            MoveOnFirst();            
            ActivateRaw();
            _battleHud.ResetCooldown();
        }        

        private void MoveOnFirst()
        {
            for (int i = 0; i < _secondRaw.Count; i++)
            {
                if (_secondRaw[i].GetComponentInChildren<Card>() != null
                    && _firstRaw[i].GetComponentInChildren<Card>() == null)
                {
                    _movedCard = _secondRaw[i].GetComponentInChildren<Card>();
                    _movedCard.transform.SetParent(_firstRaw[i]);
                    StartCoroutine(DrawCards(_movedCard, _firstRaw[i].transform.position));
                    UpdateActivation();
                }
            }
        }

        private void SetDeckToSpawn(int value)
        {
            if (value == 0)
                SpawnDeck(_deckCreator.PortDeck);
            else if (value == 1)
                SpawnDeck(_deckCreator.MarketDeck);
        }

        private void SpawnDeck(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                _spawnedCard = Instantiate(cards[i], _deckSpawnPos);
                _currentDeck.Add(_spawnedCard);

                if (_spawnedCard.GetComponent<Boss>())
                {
                    _bossCard = _spawnedCard.GetComponent<Boss>();
                    _bossCard.GetComponent<Health>().Died += OnBossDie;
                }

                foreach (var card in _currentDeck)
                {
                    if (card.GetComponent<Shield>())
                        card.GetComponent<Shield>().ChangeSprite(_battleHud.Player.Type);
                }               
            }
        }

        private void OnBossDie()
        {
            _waveNumber++;
            BossDied?.Invoke(_waveNumber);
            _bossCard.GetComponent<Health>().Died -= OnBossDie;
            ClearRaw(_firstRaw);
            ClearRaw(_secondRaw);

            if (_waveNumber != _maxWave)
            {
                SetDeckToSpawn(_waveNumber);
                Invoke(nameof(SpawnCards), 1f);
            }                
        }

        private void ClearRaw(List<RectTransform> raw)
        {
            foreach (var pos in raw)
            {
                if (pos.GetComponentInChildren<Card>() != null)
                {
                    var card = pos.GetComponentInChildren<Card>();
                    Destroy(card.gameObject);
                }                    
            }
        }

        private void SpawnCards()
        {
            if (IsFirstRawFree())
            {
                Spawn(_firstRaw);
                UpdateActivation();
            }
            else
                Spawn(_secondRaw);
        }

        private void Spawn(List<RectTransform> positions)
        {
            var cardsToSpawn = 3;

            if (_currentDeck.Count < cardsToSpawn)
                cardsToSpawn = _currentDeck.Count;

            for (int i = 0; i < cardsToSpawn; i++)
            {
                _spawnedCard = _currentDeck[i];

                if (_spawnedCard.GetComponent<Enemy>())
                    _spawnedCard.GetComponent<Enemy>().InitPlayer(_battleHud.Player);

                if (GetCardPosition(positions) != null)
                {
                    StartCoroutine(DrawCards(_spawnedCard, GetCardPosition(positions).transform.position));
                    _spawnedCard.transform.SetParent(GetCardPosition(positions));
                    _spawnedCards.Add(_spawnedCard);
                }
            }

            foreach (var card in _spawnedCards)
                _currentDeck.Remove(card);

            _spawnedCards.Clear();
            _remainingCards.text = _currentDeck.Count.ToString();
        }               

        private IEnumerator DrawCards(Card card, Vector3 newPos)
        {            
            while (card.transform.position != newPos)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, newPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (card.GetComponent<Enemy>())
                card.GetComponent<Enemy>().SetPosition();
        }

        private RectTransform GetCardPosition(List<RectTransform> positions)
        {
            foreach (var pos in positions)
            {
                if (pos.GetComponentInChildren<Card>() == null)
                    return pos;
            }
            return null;
        }

        private void UpdateActivation()
        {
            foreach (var pos in _firstRaw)
            {
                var card = pos.GetComponentInChildren<Card>();
                if (card != null)
                    card.Activate();
            }
        }       

        private bool IsFirstRawFree()
        {
            foreach (var pos in _firstRaw)
            {
                if (pos.GetComponentInChildren<Card>() != null)
                    return false;
            }
            return true;
        }        
    }
}