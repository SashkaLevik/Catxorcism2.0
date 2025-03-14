using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using Infrastructure.Services;
using UnityEngine;

namespace GameEnvironment.GameLogic
{
    public class DeckCreator : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private SkillCard _skillPrefab;
        [SerializeField] private DragController _dragController;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _playerSpawnPos;
        [SerializeField] private RectTransform _redrawPos;
        [SerializeField] private RectTransform _deckSpawnPos;
        [SerializeField] private RectTransform _hand;
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private AudioSource _tossCard;

        private int _handCapacity = 4;
        private Canvas _canvas;
        private Player _player;
        private Guard _spawnedGuard;
        private PlayerProgress _progress;
        private SkillCard _currentSkill;
        private List<Guard> _playerGuards = new List<Guard>();
        private List<Card> _currentDeck = new List<Card>();
        private List<Card> _handCards = new List<Card>();
        private List<Card> _discardCards = new List<Card>();

        private void Start()
        {
            if (_progress.WorldData.IsNewRun) 
                _playerGuards = _player.PlayerGuards.ToList();

            _canvas = GetComponent<Canvas>();
            _dragController.GuardPlaced += OnGuardPlaced;
            CreateDeck();
        }
        
        public void Construct(Player player)
        {
            _player = player;
        }
        
        public void DrawHand() => 
            StartCoroutine(DrawHandCards());

        private void CreateDeck()
        {
            foreach (var guard in _playerGuards)
            {
                _spawnedGuard = Instantiate(guard, _playerSpawnPos);
                _spawnedGuard.GetComponent<Health>().Died += OnGuardDie;
                _currentDeck.Add(_spawnedGuard);
                Move(_spawnedGuard, _deckSpawnPos);
            }
        }

        private void OnGuardPlaced(Guard guard)
        {
            _handCards.Remove(guard);
            StartCoroutine(CreateSkillCards(guard));
        }
        
        private void OnGuardDie()
        {
            _player.RestoreLeadership();
        }

        private IEnumerator CreateSkillCards(Guard guard)
        {
            foreach (var skillCard in guard.SkillCards)
            {
                var spawnPos = guard.GetComponentInParent<RectTransform>();
                _currentSkill = Instantiate(skillCard, spawnPos);
                _currentSkill.InitHud(_battleHud);
                yield return new WaitForSeconds(0.2f);
                Move(_currentSkill, _deckSpawnPos);
                yield return new WaitForSeconds(0.2f);
                _currentDeck.Add(_currentSkill);
            }
            
            /*foreach (var skillData in guard.Skills)
            {
                var spawnPos = guard.GetComponentInParent<RectTransform>();
                _currentSkill = Instantiate(_skillPrefab, spawnPos);
                _currentSkill.Init(skillData);
                yield return new WaitForSeconds(0.2f);
                Move(_currentSkill, _deckSpawnPos);
                yield return new WaitForSeconds(0.2f);
                _currentDeck.Add(_currentSkill);
            }*/
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
                
                Move(_currentDeck[0], _hand);
                yield return new WaitForSeconds(0.2f);
                _currentDeck[0].Activate();
                _handCards.Add(_currentDeck[0]);
                _currentDeck.Remove(_currentDeck[0]);
            }
        }

        private IEnumerator Discard()
        {
            foreach (var card in _handCards)
            {
                card.transform.SetParent(_canvas.transform);
                Move(card, _discardPos);
                yield return new WaitForSeconds(0.2f);
                card.Disactivate();
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
            _playerGuards = progress.PlayerStats.PlayerGuards.ToList();
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.PlayerGuards = _playerGuards.ToList();
        }
    }
}