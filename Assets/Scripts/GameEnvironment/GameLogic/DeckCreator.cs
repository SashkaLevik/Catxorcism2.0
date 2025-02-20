using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.GameLogic.DiceFolder;
using GameEnvironment.UI;
using GameEnvironment.Units;
using Infrastructure.Services;
using UnityEngine;

namespace GameEnvironment.GameLogic
{
    public class DeckCreator : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private RectTransform _playerSpawnPos;
        [SerializeField] private RectTransform _redrawPos;
        [SerializeField] private RectTransform _deckSpawnPos;
        [SerializeField] private RectTransform _hand;
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private AudioSource _tossCard;

        private int _handCapacity = 4;
        private Player _player;
        private Guard _spawnedGuard;
        private PlayerProgress _progress;
        private List<Guard> _playerGuards = new List<Guard>();
        private List<Card> _currentDeck = new List<Card>();
        private List<Card> _handCards = new List<Card>();
        private List<Card> _discardCards = new List<Card>();

        private void Start()
        {
            if (_progress.WorldData.IsNewRun) 
                _playerGuards = _player.PlayerGuards.ToList();
            
            CreateDeck();
            _battleHud.PlayerFrontDice.OnDiceResult += DrawFirstGuards;
        }

        public void Construct(Player player)
        {
            _player = player;
        }
        
        private void CreateDeck()
        {
            foreach (var guard in _playerGuards)
            {
                _spawnedGuard = Instantiate(guard, _playerSpawnPos);
                _spawnedGuard.GetComponent<Health>().Died += OnGuardDie;
                _currentDeck.Add(_spawnedGuard);
                StartCoroutine(MoveCards(_spawnedGuard, _deckSpawnPos));
            }
        }

        private void OnGuardDie()
        {
            _player.RestoreLeadership();
        }

        public void DrawNextHand()
        {
            StartCoroutine(DrawHandCards());
        }
        
        private void DrawFirstGuards(DiceFace arg0)
        {
            StartCoroutine(DrawHandCards());
        }
        
        private IEnumerator DrawHandCards()
        {
            if (_handCards.Count > 0)
                yield return StartCoroutine(Discard());
            
            for (int i = 0; i < _handCapacity; i++)
            {
                if (_currentDeck.Count == 0)
                    yield return StartCoroutine(RedrawPlayed());
                
                StartCoroutine(MoveCards(_currentDeck[0], _hand));
                yield return new WaitForSeconds(0.2f);
                _currentDeck[0].Activate();
                _currentDeck.Remove(_currentDeck[0]);
                _handCards.Add(_currentDeck[0]);
            }
            
            yield return null;
        }

        private IEnumerator Discard()
        {
            StartCoroutine(Draw(_handCards, _discardPos));
            yield return null;
        }

        private IEnumerator RedrawPlayed()
        {
            _currentDeck = _discardCards.OrderBy(x => Random.value).ToList();
            yield return StartCoroutine(Draw(_currentDeck, _redrawPos));
            yield return StartCoroutine(Draw(_currentDeck, _deckSpawnPos));
            yield return null;
        }

        private IEnumerator Draw(List<Card> cards, RectTransform position)
        {
            /*foreach (var card in cards) 
                StartCoroutine(MoveCards(card, position));*/

            yield return null;
        }
        
        private IEnumerator MoveCards(Card card, RectTransform newPos)
        {            
            while (card.transform.position != newPos.transform.position)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, newPos.transform.position,
                    _moveSpeed * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
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