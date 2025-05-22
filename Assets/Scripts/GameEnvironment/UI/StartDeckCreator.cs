using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using Infrastructure.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace GameEnvironment.UI
{
    public class StartDeckCreator : BaseWindow, ISaveProgress
    {
        [SerializeField] private Academy _academy;
        [SerializeField] private RectTransform _hiredGuardsContainer;
        [SerializeField] private Warning _warning;
        [SerializeField] private GameObject _map;
        [SerializeField] private Button _mapButton;
        [SerializeField] private GuardDescription _guardDescription;
        [SerializeField] private List<RectTransform> _slots;
        [SerializeField] private TMP_Text _deckCount;

        private int _deckCapacity;
        private float _moveSpeed = 30f;
        private Guard _currentGuard;
        private List<CardData> _availableGuards = new List<CardData>();
        private List<CardData> _hiredGuards = new List<CardData>();
        private List<Guard> _spawnedGuards = new List<Guard>();
        private Camera _camera;
        private ISaveLoadService _saveLoadService;

        //public List<Guard> HiredGuards => _hiredGuards;

        private void Start()
        {
            _camera = Camera.main;
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 clickPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(clickPosition, Vector2.zero, Single.PositiveInfinity);

                foreach (var hit in hitInfo)
                {
                    if (hit.collider.TryGetComponent(out Guard guard))
                    {
                        _guardDescription.Show(guard);
                    }
                }
            }
        }

        private void OnEnable()
        {
            _mapButton.onClick.AddListener(OpenMap);
            _deckCount.text = _deckCapacity.ToString();
            _availableGuards = _academy.AvailableGuards.ToList();
            InstallAvailableGuards();
        }

        private void OnDisable()
        {
            foreach (var guard in _spawnedGuards) 
                Destroy(guard.gameObject);
            
            foreach (var slot in _slots.Where(slot => slot.GetComponentInChildren<Guard>() != null))
                Destroy(slot.GetComponentInChildren<Guard>().gameObject);
            
            _mapButton.onClick.RemoveListener(OpenMap);
            _availableGuards.Clear();
            _spawnedGuards.Clear();
            _hiredGuards.Clear();
        }

        private void InstallAvailableGuards()
        {
            for (int i = 0; i < _availableGuards.Count; i++)
            {
                _currentGuard = (Guard) Instantiate(_availableGuards[i].CardPrefab, _slots[i]);
                _currentGuard.SetSlotIndex(_slots.IndexOf(_slots[i]));
                _spawnedGuards.Add(_currentGuard);
                _currentGuard.OnGuardPressed += AddInDeck;
            }
        }

        private void AddInDeck(Guard guard)
        {
            if (_hiredGuards.Count <= _deckCapacity - 1)
            {
                _currentGuard = guard;
                _currentGuard.OnGuardPressed -= AddInDeck;
                _currentGuard.OnGuardPressed += RemoveFromDeck;
                _hiredGuards.Add(_currentGuard.CardData);
                StartCoroutine(MoveCards(guard, _hiredGuardsContainer));
                CheckDeckCapacity();
                _guardDescription.Hide();
            }
            else
                _warning.Show(_warning.FullDeckCapacity);
        }

        private void RemoveFromDeck(Guard guard)
        {
            _currentGuard = guard;
            _currentGuard.OnGuardPressed -= RemoveFromDeck;
            _currentGuard.OnGuardPressed += AddInDeck;
            _hiredGuards.Remove(_currentGuard.CardData);
            StartCoroutine(MoveCards(guard, _slots[_currentGuard.SlotIndex]));
            CheckDeckCapacity();
        }
        
        private IEnumerator MoveCards(Card card, RectTransform newPos)
        {            
            while (card.transform.position != newPos.transform.position)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, newPos.transform.position,
                    _moveSpeed * Time.deltaTime);
                yield return null;
            }
            card.transform.SetParent(newPos);
        }

        private void OpenMap()
        {
            _map.SetActive(true);
            _saveLoadService.SaveProgress();
        }

        private void CheckDeckCapacity()
        {
            _deckCount.text = (_deckCapacity - _hiredGuards.Count).ToString();
            _mapButton.interactable = _hiredGuards.Count > 0;
        }
        
        public void Load(PlayerProgress progress)
        {
            _deckCapacity = progress.PlayerStats.DeckCapacity;
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.StartingGuards = _hiredGuards.ToList();
        }
    }
}