using System.Collections;
using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.GameLogic
{
    public class DeckSpawner : MonoBehaviour
    {
        [SerializeField] DeckCreator _deckCreator;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _deckSpawnPos;
        [SerializeField] private List<RectTransform> _firstRaw;
        [SerializeField] private List<RectTransform> _secondRaw;
        [SerializeField] private TMP_Text _remainingCards;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private RectTransform _pos;
        [SerializeField] private AudioSource _tossCard;

        private float _tossDelay = 0.2f;
        private float _attackDelay = 0.4f;
        private int _waveNumber;
        private int _maxWave = 2;
        private Boss _bossCard;
        private Card _spawnedCard;
        private Card _movedCard;
        private Player _player;
        private List<Card> _currentDeck = new List<Card>();
        private List<Card> _spawnedCards = new List<Card>();
        private Coroutine _moveCoroutine;

        public List<RectTransform> FirstRaw => _firstRaw;

        public event UnityAction<int> BossDied;

        private void Start()
        {
            SetDeckToSpawn(_waveNumber);
            StartCoroutine(SpawnCards(_firstRaw));
            _player = _battleHud.Player;
        }

        public void DrawNextDeck()
        {
            if (_waveNumber != _maxWave)
            {
                SetDeckToSpawn(_waveNumber);
                StartCoroutine(SpawnCards(_firstRaw));
            }
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

            foreach (var pos in _firstRaw)
            {                
                if (pos.GetComponentInChildren<Enemy>() != null)
                {
                    pos.GetComponentInChildren<Enemy>().Attack();
                    yield return new WaitForSeconds(_attackDelay);
                }
                yield return null;
            }

            yield return new WaitForSeconds(_tossDelay);
            yield return StartCoroutine(MoveOnFirst());
            yield return new WaitForSeconds(_tossDelay);
            yield return StartCoroutine(SpawnCards(_secondRaw));
            yield return new WaitForSeconds(_tossDelay);
            yield return StartCoroutine(MoveOnFirst());
            ActivateRaw();
            _battleHud.ResetCooldown();
        }        

        private IEnumerator MoveOnFirst()
        {
            for (int i = 0; i < _secondRaw.Count; i++)
            {
                if (_secondRaw[i].GetComponentInChildren<Card>() != null
                    && _firstRaw[i].GetComponentInChildren<Card>() == null)
                {
                    _movedCard = _secondRaw[i].GetComponentInChildren<Card>();
                    _movedCard.transform.SetParent(_firstRaw[i]);
                    StartCoroutine(MoveCards(_movedCard, _firstRaw[i].transform.position));
                    yield return new WaitForSeconds(0.2f);
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
            if (_player != null)
            {
                _waveNumber++;
                ClearRaw(_firstRaw);
                ClearRaw(_secondRaw);                
                Invoke(nameof(DelayBossDeath), 1f);
            }
        }

        private void DelayBossDeath() =>
            BossDied?.Invoke(_waveNumber);        

        private IEnumerator SpawnCards(List<RectTransform> positions)
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
                    StartCoroutine(MoveCards(_spawnedCard, GetCardPosition(positions).transform.position));
                    _spawnedCard.transform.SetParent(GetCardPosition(positions));
                    _spawnedCards.Add(_spawnedCard);
                    _tossCard.Play();
                    yield return new WaitForSeconds(_tossDelay);
                }
            }

            foreach (var card in _spawnedCards)
                _currentDeck.Remove(card);

            ActivateRaw();
            _spawnedCards.Clear();
            _remainingCards.text = _currentDeck.Count.ToString();
        }                          

        private IEnumerator MoveCards(Card card, Vector3 newPos)
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
    }
}