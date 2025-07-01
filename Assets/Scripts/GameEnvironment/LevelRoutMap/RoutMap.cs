using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameEnvironment.GameLogic.CardFolder;
using GameEnvironment.LevelRoutMap.RoutEventWindows;
using GameEnvironment.UI;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GameEnvironment.LevelRoutMap
{
    public class RoutMap : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private MapMovementController _mapMover;
        [SerializeField] private List<RoutEvent> _routEvents;
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private GameObject _pathPrefab;
        [SerializeField] private RectTransform _buttonContainer;
        [SerializeField] private RectTransform _tokenStartPos;

        public MapData MapData;
        private int _layers = 9;
        private int _maxButtonsPerLayer = 3;
        private int _buttonsInLayer;
        private float _xSteep = 3;
        private float _verticalSpacing = 2f;
        private float _curveHeight = 1f;
        private float _leftMarginPercent = 0.25f;
        private Player _player;
        private BattleHud _battleHud;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoadService;
        private Canvas _canvas;
        private EventButton _currentButton;
        private Camera _camera;
        private List<EventButton> _allButtons = new List<EventButton>();
        private List<MapPath> _allPaths = new List<MapPath>();

        public GameObject PlayerToken { get; private set; }
        
        public EventButton CurrentButton => _currentButton;

        public List<EventButton> AllButtons => _allButtons;

        public event UnityAction BattleEntered;

        public event UnityAction OnMapGenerated;
        public event UnityAction<EventButton> OnButtonChanged; 

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = Camera.main;
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void Start()
        {
            _camera = Camera.main;
            _mapMover.MapMoved += OpenEvent;
            
            if (MapData.SavedButtons.Count > 0 && MapData.CurrentButtonIndex >= 0)
                LoadMap();
            else
            {
                GenerateMap();
                if (_allButtons.Count > 0) _allButtons[0].SetReachable(true);
            }

            foreach (var routEvent in _routEvents) 
                routEvent.Closed += ShowMap;
        }

        private void OnDestroy()
        {
            _mapMover.MapMoved -= OpenEvent;
            
            foreach (var routEvent in _routEvents) 
                routEvent.Closed -= ShowMap;
        }

        private void ShowMap() => 
            _buttonContainer.gameObject.SetActive(true);


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _saveLoadService.SaveProgress();
            }
        }

        public void Construct(Player player, BattleHud battleHud)
        {
            _player = player;
            _battleHud = battleHud;
            
            foreach (var routEvent in _routEvents) 
                routEvent.Construct(_player, _battleHud);
        }
        
        public void SetCurrentButton(EventButton button)
        {
            _currentButton = button;
            _currentButton.SetReachable(false);

            foreach (EventButton connected in _currentButton.ConnectedButtons) 
                connected.SetReachable(true);
            
            OnButtonChanged?.Invoke(button);
        }
        
        private void OpenEvent(EventButton targetButton)
        {
            if (targetButton.RoutEventType == RoutEventType.Battle) 
                BattleEntered?.Invoke();
            
            foreach (var routEvent in _routEvents.Where(routEvent => targetButton.RoutEventType == routEvent.routEventType))
            {
                routEvent.gameObject.SetActive(true);
                
                if (routEvent.routEventType != RoutEventType.Battle) 
                    _buttonContainer.gameObject.SetActive(false);
            }

            UpdateReachableEvents();
        }
        
        private void GenerateMap()
        {
            _allButtons.Clear();
            _allPaths.Clear();

            Vector3 leftEdge = _camera.ViewportToWorldPoint(new Vector3(_leftMarginPercent, 0.5f, 0));
            List<EventButton> previousLayerButtons = new List<EventButton>();

            for (int layer = 0; layer < _layers; layer++)
            {
                _buttonsInLayer = (layer == 0 || layer == _layers - 1)
                    ? 1 : Random.Range(1, _maxButtonsPerLayer + 1);
                
                List<EventButton> currentLayerButtons = new List<EventButton>();

                for (int i = 0; i < _buttonsInLayer; i++)
                {
                    Vector3 position = new Vector3(leftEdge.x + (_xSteep * layer),
                        (i - (_buttonsInLayer - 1) * 0.5f) * _verticalSpacing, 0);

                    EventButton button = CreateButton(position, layer, i, _layers);
                    button.Construct(this);
                    currentLayerButtons.Add(button);
                    _allButtons.Add(button);

                    if (previousLayerButtons.Count > 0)
                    {
                        CreateLayerConnections(previousLayerButtons, button);
                        ValidateLayerConnection(previousLayerButtons, currentLayerButtons);
                    }
                }
                
                previousLayerButtons = currentLayerButtons;
            }
            
            ValidateFullMapConnectivity();
            OnMapGenerated?.Invoke();
            CreatePlayerToken();
        }

        private void CreatePlayerToken()
        {
            PlayerToken = Instantiate(_player.PlayerToken, _currentButton == null 
                ? _tokenStartPos.transform.position : _currentButton.transform.position, Quaternion.identity, transform);
        }
        
        private EventButton CreateButton(Vector3 position, int layer, int index, int totalLayers)
        {
            GameObject buttonObj = Instantiate(_buttonPrefab, position, Quaternion.identity, _buttonContainer);
            EventButton button = buttonObj.GetComponent<EventButton>();
            button.SetType(DetermineEventType(layer, totalLayers));

            return button;
        }

        private EventButton LoadSavedButton(Vector3 position, RoutEventType eventType)
        {
            GameObject button = Instantiate(_buttonPrefab, position, Quaternion.identity, _buttonContainer);
            EventButton eventButton = button.GetComponent<EventButton>();
            eventButton.SetType(eventType);
            eventButton.Construct(this);
            return eventButton;
        }
        
        private RoutEventType DetermineEventType(int layer, int totalLayers)
        {
            int battleChance = Random.Range(0, 11);
            
            if (layer == 0) return RoutEventType.Battle;
            if (layer == totalLayers - 1) return RoutEventType.Boss;

            bool isBattleLayer = (layer % 2 == 0);

            if (!isBattleLayer && _buttonsInLayer > 1)
            {
                if (battleChance > 7)
                    return RoutEventType.Battle;
            }
            
            return isBattleLayer ? RoutEventType.Battle : GetRandomEventType(layer);
        }

        private void ValidateFullMapConnectivity()
        {
            List<EventButton> unvisitedButtons = new List<EventButton>(_allButtons);
            Queue<EventButton> buttonQueue = new Queue<EventButton>();
            
            if (_allButtons.Count > 0)
            {
                buttonQueue.Enqueue(_allButtons[0]);
                unvisitedButtons.Remove(_allButtons[0]);
            }

            while (buttonQueue.Count > 0)
            {
                EventButton currentButton = buttonQueue.Dequeue();
                foreach (var connectedButton in currentButton.ConnectedButtons)
                {
                    if (unvisitedButtons.Contains(connectedButton))
                    {
                        buttonQueue.Enqueue(connectedButton);
                        unvisitedButtons.Remove(connectedButton);
                    }
                }
            }

            if (unvisitedButtons.Count > 0)
            {
                foreach (var button in unvisitedButtons)
                {
                    EventButton closestReachable = FindClosestReachable(button);

                    if (closestReachable != null)
                    {
                        CreatePath(closestReachable, button);
                    }
                }
            }
        }
        
        private void ValidateLayerConnection(List<EventButton> previousLayerButtons, List<EventButton> currentLLayerButtons)
        {
            foreach (var prevButton in previousLayerButtons)
            {
                if (prevButton.ConnectedButtons.Count == 0)
                {
                    EventButton randomButton = currentLLayerButtons[Random.Range(0, currentLLayerButtons.Count)];
                    CreatePath(prevButton, randomButton);
                }
            }

            foreach (var currButton in currentLLayerButtons)
            {
                bool hasConnection = false;
                foreach (var prevButton in previousLayerButtons)
                {
                    if (prevButton.ConnectedButtons.Contains(currButton))
                    {
                        hasConnection = true;
                        break;
                    }
                }

                if (!hasConnection)
                {
                    EventButton randomButton = previousLayerButtons[Random.Range(0, previousLayerButtons.Count)];
                    CreatePath(randomButton, currButton);
                }
            }
        }

        private void CreateLayerConnections(List<EventButton> previousLayerButtons, EventButton currentButton)
        {
            int connections = Mathf.Min(Random.Range(1, 3), previousLayerButtons.Count);
            List<EventButton> availableButtons = new List<EventButton>(previousLayerButtons);

            for (int i = 0; i < connections; i++)
            {
                if (availableButtons.Count == 0) break;

                int randomIndex = Random.Range(0, availableButtons.Count);
                MapPath path = CreatePath(availableButtons[randomIndex], currentButton);
            }
        }

        private EventButton FindClosestReachable(EventButton targetButton)
        {
            EventButton closestButton = null;
            float minDistance = Mathf.Infinity;
            int targetLayer = GetButtonLayer(targetButton);

            for (int layer = targetLayer - 1; layer >= 0; layer--)
            {
                foreach (var button in _allButtons)
                {
                    if (GetButtonLayer(button) == layer &&
                        button.ConnectedButtons.Count > 0 &&
                        !button.ConnectedButtons.Contains(targetButton))
                    {
                        float distance = Vector3.Distance(button.transform.position,
                            targetButton.transform.position);

                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestButton = button;
                        }
                    }
                }

                if (closestButton != null) break;
            }

            return closestButton;
        }

        private RoutEventType GetRandomEventType(int currentLayer)
        {
            float[] weights =
            {
                Mathf.Clamp(40 - currentLayer * 2, 15, 40), // Decrease
                Mathf.Clamp(currentLayer * 8, 10, 35), //Increase
                25f
            };

            float total = weights[0] + weights[1] + weights[2];
            float randomPoint = Random.Range(0, total);

            if (randomPoint < weights[0]) return RoutEventType.DiceSmith;
            if (randomPoint < weights[0] + weights[1]) return RoutEventType.FirePlace;
            
            return RoutEventType.TreasureBox;
        }
        
        private int GetButtonLayer(EventButton button)
        {
            float xStart = _camera.ViewportToWorldPoint(new Vector3(_leftMarginPercent, 0.5f, 0)).x;
            return (int) (Mathf.RoundToInt(button.transform.position.x - xStart) / _xSteep);
        }
        
        private MapPath CreatePath(EventButton from, EventButton to)
        {
            GameObject pathObj = Instantiate(_pathPrefab, _buttonContainer);
            MapPath path = pathObj.GetComponent<MapPath>();
            path.Initialize(from, to, _buttonContainer);
            _allPaths.Add(path);
            from.AddConnectedButton(to);
            return path;
        }

        private void UpdateReachableEvents()
        {
            foreach (var button in _allButtons) 
                button.SetReachable(false);

            foreach (var button in _currentButton.ConnectedButtons) 
                button.SetReachable(true);
        }

        private void LoadMap()
        {
            _layers = MapData.CurrentLayerCount;
            Dictionary<int, EventButton> indexToButton = new Dictionary<int, EventButton>();
            
            for (int i = 0; i < MapData.SavedButtons.Count; i++)
            {
                EventButtonData buttonData = MapData.SavedButtons[i];
                EventButton eventButton = LoadSavedButton(buttonData.Position, buttonData.EventType);
                eventButton.SetReachable(buttonData.IsReachable);
                _allButtons.Add(eventButton);
                indexToButton[i] = eventButton;
            }

            for (int i = 0; i < MapData.SavedButtons.Count; i++)
            {
                EventButton button = _allButtons[i];
                foreach (var connectedIndex in MapData.SavedButtons[i].ConnectedButtonIndices)
                {
                    if (indexToButton.ContainsKey(connectedIndex))
                    {
                        button.AddConnectedButton(indexToButton[connectedIndex]);
                    }
                }
            }

            foreach (var pathData in MapData.SavedPaths)
            {
                if (indexToButton.ContainsKey(pathData.StartButtonIndex) 
                    && indexToButton.ContainsKey(pathData.EndButtonIndex))
                {
                    CreatePath(indexToButton[pathData.StartButtonIndex],
                        indexToButton[pathData.EndButtonIndex]);
                }
            }

            if (MapData.CurrentButtonIndex >=0 && MapData.CurrentButtonIndex < _allButtons.Count)
            {
                _currentButton = _allButtons[MapData.CurrentButtonIndex];
                UpdateReachableEvents();
            }
            
            CreatePlayerToken();
        }

        public void Load(PlayerProgress progress)
        {
            MapData = progress.MapData;
        }

        public void Save(PlayerProgress progress)
        {
            progress.MapData.SavedButtons.Clear();
            progress.MapData.SavedPaths.Clear();
            progress.MapData.CurrentLayerCount = _layers;
            Dictionary<EventButton, int> buttonToIndex = new Dictionary<EventButton, int>();
            
            for (int i = 0; i < _allButtons.Count; i++)
            {
                EventButton button = _allButtons[i];
                buttonToIndex[button] = i;
                button.ButtonData.IsReachable = button.IsReachable;
                button.ButtonData.Position = button.transform.position;
                button.ButtonData.EventType = button.RoutEventType;
                
                foreach (var connected in button.ConnectedButtons)
                {
                    if (buttonToIndex.ContainsKey(connected))
                        button.ButtonData.ConnectedButtonIndices.Add(buttonToIndex[connected]);
                }
                
                progress.MapData.SavedButtons.Add(button.ButtonData);
            }
            
            foreach (var mapPath in _allPaths)
            {
                if (buttonToIndex.ContainsKey(mapPath.StartButton) && buttonToIndex.ContainsKey(mapPath.EndButton))
                {
                    mapPath.PathData.StartButtonIndex = buttonToIndex[mapPath.StartButton];
                    mapPath.PathData.EndButtonIndex = buttonToIndex[mapPath.EndButton];
                    progress.MapData.SavedPaths.Add(mapPath.PathData);
                }
            }

            if (_currentButton != null && buttonToIndex.ContainsKey(_currentButton))
                progress.MapData.CurrentButtonIndex = buttonToIndex[_currentButton];
        }
    }
}