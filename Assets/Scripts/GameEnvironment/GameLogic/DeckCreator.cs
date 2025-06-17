using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.CardFolder.SkillCards;
using GameEnvironment.UI;
using Infrastructure.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameEnvironment.GameLogic
{
    public class DeckCreator : MonoBehaviour, ISaveProgress
   {
        private const string AllCardsData = "Data/CardData";

        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private SkillCard _skillPrefab;
        [SerializeField] private DragController _dragController;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _playerSpawnPos;
        [SerializeField] private RectTransform _redrawPos;
        [SerializeField] private RectTransform _deckSpawnPos;
        [SerializeField] private RectTransform _handPosition;
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private AudioSource _tossCard;

        public float _cardSpacing = 100f;
        private int _handCapacity;
        private Canvas _canvas;
        private Player _player;
        private Guard _spawnedGuard;
        private PlayerProgress _progress;
        private SkillCard _currentSkill;
        private List<Guard> _fieldGuards = new List<Guard>();
        private List<Card> _currentDeck = new List<Card>();
        private List<Card> _handCards = new List<Card>();
        private List<Card> _discardCards = new List<Card>();
        private List<CardData> _startDeck = new List<CardData>();
        private List<CardData> _allCards = new List<CardData>();
        private List<string> _playerDeck = new List<string>();

        public Player Player => _player;

        public List<Card> HandCards => _handCards;

        public List<Guard> FieldGuards => _fieldGuards;
        
        private void Awake()
        {
            _allCards = Resources.LoadAll<CardData>(AllCardsData).ToList();
        }

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _dragController.GuardPlaced += OnGuardPlaced;
            CreateDeck();
        }

        public void Construct(Player player)
        {
            _player = player;
        }
        
        public void RefreshEnemies() => 
            StartCoroutine(GetNewEnemy());

        private IEnumerator GetNewEnemy()
        {
            yield return new WaitForSeconds(0.4f);
            
            foreach (var guard in _fieldGuards) 
                guard.TryGetEnemy(_battleHud);
        }
        
        public void DrawHand() => 
            StartCoroutine(DrawHandCards());

        public void DrawBuffCard(Guard guard) =>
            StartCoroutine(CreateBuffCard(guard));

        public void DiscardPlayedSkill(SkillCard skillCard)
        {
            skillCard.transform.SetParent(_canvas.transform);
            Move(skillCard, _discardPos);
            skillCard.Inactivate();
            skillCard.GetComponent<Card>().DisableCollider();
            _handCards.Remove(skillCard);
            UpdateCardsShift();
            _discardCards.Add(skillCard);
        }
        
        public void UpdateCardsShift()
        {
            int cardsCount = _handCards.Count;

            for (int i = 0; i < cardsCount; i++)
            {
                float horizontalOffset = _cardSpacing * (i - (cardsCount - 1) / 2f);
                _handCards[i].transform.localPosition = new Vector3(horizontalOffset, 0, 0);
            }
        }

        public void RemoveObstacleSkill(ObstacleSkill obstacleSkill) => 
            _handCards.Remove(obstacleSkill);

        private void CreateDeck()
        {
            foreach (var cardData in _allCards.Where(cardData => _playerDeck.Contains(cardData.EnName)))
                _startDeck.Add(cardData);

            foreach (var cardData in _startDeck)
            {
                _spawnedGuard = Instantiate(cardData.CardPrefab.GetComponent<Guard>(), _deckSpawnPos);
                _spawnedGuard.ConstructUnit(cardData);
                _spawnedGuard.GetCanvas(_canvas, _handPosition);
                _spawnedGuard.Health.Died += OnGuardDie;
                _currentDeck.Add(_spawnedGuard);
                _spawnedGuard.Flip();
            }
        }

        private void OnGuardPlaced(Guard guard)
        {
            _handCards.Remove(guard);
            _fieldGuards.Add(guard);
            StartCoroutine(CreateSkillCards(guard));
        }
        
        private void OnGuardDie(Unit unit)
        {
            unit.Health.Died -= OnGuardDie;
            _currentDeck.Remove(unit);
            _player.RestoreLeadership();
            _enemySpawner.RefreshEnemies();
        }


        private IEnumerator CreateSkillCards(Guard guard)
        {
            foreach (var skill in guard.SkillCards)
            {
                var spawnPos = guard.GetComponentInParent<RectTransform>();
                _currentSkill = Instantiate(skill, spawnPos);
                _currentSkill.GetCanvas(_canvas, _handPosition);
                _currentSkill.InitHud(_battleHud);
                yield return new WaitForSeconds(0.2f);
                Move(_currentSkill, _deckSpawnPos);
                yield return new WaitForSeconds(0.2f);
                _currentDeck.Add(_currentSkill);
            }
        }
        
        private IEnumerator CreateBuffCard(Guard guard)
        {
            var spawnPos = guard.GetComponentInParent<RectTransform>();
            _currentSkill = Instantiate(guard.BuffCard, spawnPos);
            _currentSkill.GetCanvas(_canvas, _handPosition);
            _currentSkill.InitHud(_battleHud);
            yield return new WaitForSeconds(0.2f);
            Move(_currentSkill, _deckSpawnPos);
            yield return new WaitForSeconds(0.2f);
            _currentDeck.Add(_currentSkill);
            yield return null;
        }

        private IEnumerator DrawHandCards()
        {
            if (_handCards.Count > 0)
                yield return StartCoroutine(Discard());

            yield return new WaitForSeconds(0.05f);
            
            for (int i = 0; i < _handCapacity; i++)
            {
                if (_currentDeck.Count == 0)
                {
                    yield return StartCoroutine(RedrawPlayed());
                    yield return new WaitForSeconds(0.2f);
                    _discardCards.Clear();
                }

                yield return StartCoroutine(MoveCards(_currentDeck[0], _handPosition));
                _handCards.Add(_currentDeck[0]);
                _currentDeck.Remove(_currentDeck[0]);
                UpdateCardsShift();
            }

            yield return new WaitForSeconds(0.3f);

            foreach (var card in _handCards)
            {
                card.Activate();
            }
        }

        private IEnumerator Discard()
        {
            foreach (var card in _handCards) 
                card.DisableCollider();
            
            foreach (var card in _handCards)
            {
                card.transform.SetParent(_canvas.transform);
                Move(card, _discardPos);
                yield return new WaitForSeconds(0.2f);
                card.Inactivate();
                _discardCards.Add(card);
            }

            yield return new WaitForSeconds(0.1f);
            _handCards.Clear();
        }
        
        private IEnumerator RedrawPlayed()
        {
            _currentDeck = _discardCards.OrderBy(x => Random.value).ToList();
            
            foreach (var card in _discardCards)
            {
                Move(card, _redrawPos);
                yield return new WaitForSeconds(0.1f);
            }
            
            foreach (var card in _discardCards)
            {
                Move(card, _deckSpawnPos);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void Move(Card card, RectTransform newPos)
        {
            StartCoroutine(MoveCards(card, newPos));
        }
        
        private IEnumerator MoveCards(Card card, RectTransform newPos)
        {            
            while (card.transform.position != newPos.transform.position)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, newPos.transform.position,
                    _moveSpeed * Time.deltaTime);
                yield return null;
            }
            card.Flip();
            card.transform.SetParent(newPos);
        }
        
        public void Load(PlayerProgress progress)
        {
            _progress = progress;
            _handCapacity = progress.PlayerStats.HandCapacity;
            _playerDeck = progress.PlayerStats.PlayerDeck.ToList();
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.PlayerDeck = _playerDeck.ToList();
        }
    }
}