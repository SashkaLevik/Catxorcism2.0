using Assets.Scripts.GameEnvironment.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.GameEnvironment.UI;

namespace Assets.Scripts.GameEnvironment.GameLogic
{
    public class DeckSpawner : MonoBehaviour
    {
        private const string FirstWaveDeck = "Deck/FirstWave";
        private const string Shields = "Deck/Shields";

        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _deckSpawnPos;
        [SerializeField] private List<RectTransform> _firstRaw;
        [SerializeField] private List<RectTransform> _secondRaw;
        [SerializeField] private TMP_Text _remainingCards;
        [SerializeField] private Button _endTurn;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private RectTransform _pos; 

        private int _waveCount;
        private List<Card> _firstWaveDeck = new List<Card>();
        private List<Card> _currentDeck = new List<Card>();
        private List<Card> _spawnedCards = new List<Card>();
        private Card _spawnedCard;
        private Card _bossCard;
        private Card _movedCard;

        public List<RectTransform> FirstRaw => _firstRaw;

        private void Awake()
        {
            _firstWaveDeck = Resources.LoadAll<Card>(FirstWaveDeck).OrderBy(x=>Random.value).ToList();
            SetBossPosition(_firstWaveDeck);
        }

        private void Start()
        {          
            SpawnDeck(_firstWaveDeck);
            Spawn();
            _endTurn.onClick.AddListener(Attack);
        }

        private void OnDestroy()
        {
            _endTurn.onClick.RemoveListener(Attack);
        }

        public void CreateDeck()
        {

        }

        public void Attack()
        {
            StartCoroutine(StartAttack());
        }

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
            Spawn();
            yield return new WaitForSeconds(0.3f);
            MoveOnFirst();            
            ActivateRaw();
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

        private void Spawn()
        {
            if (IsFirstRawFree())
            {
                SpawnCards(_firstRaw);
                UpdateActivation();
            }
            else
                SpawnCards(_secondRaw);
        }

        private void SpawnDeck(List<Card> cards)
        {
            _waveCount = cards.Count;
            //_remainingCards.text = _waveCount.ToString();

            for (int i = 0; i < cards.Count; i++)
            {
                _spawnedCard = Instantiate(cards[i], _deckSpawnPos);
                _currentDeck.Add(_spawnedCard);

                if (_battleHud.Player.Type == PlayerType.Knight)
                {
                    foreach (var card in _currentDeck)
                    {
                        if (card.GetComponent<Shield>())
                            card.GetComponent<Shield>().ChangeSprite();
                    }
                }
                //if (_spawnedCard.GetComponent<Item>())
                //{
                //    _spawnedCard.GetComponent<Item>().Init(_battleHud.Player);
                //}
            }
        }

        private void SpawnCards(List<RectTransform> positions)
        {
            var cardsToSpawn = 3;

            if (_currentDeck.Count < cardsToSpawn)
                cardsToSpawn = _currentDeck.Count;

            for (int i = 0; i < cardsToSpawn; i++)
            {               
                _spawnedCard = _currentDeck[i];

                if (_spawnedCard.GetComponent<Enemy>())
                    _spawnedCard.GetComponent<Enemy>().InitPlayer(_battleHud.Player);

                StartCoroutine(DrawCards(_currentDeck[i], GetCardPosition(positions).transform.position));
                _spawnedCard.transform.SetParent(GetCardPosition(positions));
                //_spawnedCard.transform.SetParent(positions[i]);
                //StartCoroutine(DrawCards(_currentDeck[i], positions[i].transform.position));
                _spawnedCards.Add(_spawnedCard);                               
            }

            foreach (var card in _spawnedCards)
            {
                _currentDeck.Remove(card);
            }
            _spawnedCards.Clear();
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

        private void UpdateActivation()
        {
            foreach (var pos in _firstRaw)
            {
                var card = pos.GetComponentInChildren<Card>();
                if (card != null)
                    card.Activate();
            }
        }

        private void SetBossPosition(List<Card> cards)
        {
            foreach (var card in cards)
            {
                if (card.GetComponent<Boss>() != null)
                    _bossCard = card;
            }

            if (_bossCard != null)
            {
                cards.Remove(_bossCard);
                cards.Add(_bossCard);
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