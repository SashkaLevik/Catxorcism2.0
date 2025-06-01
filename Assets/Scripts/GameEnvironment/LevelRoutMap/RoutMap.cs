using System.Collections.Generic;
using System.Linq;
using GameEnvironment.LevelRoutMap.RoutEventWindows;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace GameEnvironment.LevelRoutMap
{
    public class RoutMap : MonoBehaviour
    {
        [SerializeField] private List<RoutEvent> _routEvents;
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private GameObject _pathPrefab;

        private int _layers = 6;
        private int _maxButtonsPerLayer = 3;
        private float _xSteep = 3;
        private float _verticalSpacing = 2f;
        private float _curveHeight = 1f;
        private float _leftMarginPercent = 0.1f;
        private EventButton _currentButton;
        private Camera _camera;
        private List<EventButton> _allButtons = new List<EventButton>();
        private List<MapPath> _allPaths = new List<MapPath>();

        public event UnityAction BattleEntered;

        private void Start()
        {
            _camera = Camera.main;
            GenerateMap();
            if (_allButtons.Count > 0) _allButtons[0].SetReachable(true);
        }

        public void OpenEvent(EventButton targetButton)
        {
            /*if (targetButton.EventType == EventType.Battle) 
                BattleEntered?.Invoke();*/
            
            foreach (var routEvent in _routEvents.Where(routEvent => targetButton.routEventType == routEvent.routEventType))
                routEvent.gameObject.SetActive(true);
            
            _currentButton = targetButton;
            UpdateReachableEvents();
        }

        private void GenerateMap()
        {
            //foreach (Transform child in transform) Destroy(child.gameObject);
            _allButtons.Clear();
            _allPaths.Clear();

            Vector3 leftEdge = _camera.ViewportToWorldPoint(new Vector3(_leftMarginPercent, 0.5f, 0));
            //float xSteep = 3;//(Screen.width * 0.7f) / _layers;
            List<EventButton> previousLayerButtons = new List<EventButton>();

            for (int layer = 0; layer < _layers; layer++)
            {
                int layerButtonsCount = (layer == 0 || layer == _layers - 1)
                    ? 1 : Random.Range(1, _maxButtonsPerLayer + 1);
                
                List<EventButton> currentLayerButtons = new List<EventButton>();

                for (int i = 0; i < layerButtonsCount; i++)
                {
                    Vector3 position = new Vector3(leftEdge.x + (_xSteep * layer),
                        (i - (layerButtonsCount - 1) * 0.5f) * _verticalSpacing, 0);

                    EventButton button = CreateButton(position, layer, i);
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

        private EventButton CreateButton(Vector3 position, int layer, int index)
        {
            GameObject buttonObj = Instantiate(_buttonPrefab, position, Quaternion.identity, transform);
            EventButton button = buttonObj.GetComponent<EventButton>();

            if (layer == 0) button.SetType(RoutEventType.Battle);
            else if (layer == _layers -1) button.SetType(RoutEventType.Boss);
            else button.SetType(GetRandomEventType(layer));
            
            return button;
        }

        private RoutEventType GetRandomEventType(int currentLayer)
        {
            float[] weights =
            {
                40f,
                Mathf.Clamp(currentLayer * 10, 5, 25),
                Mathf.Clamp(30 - currentLayer * 3, 10, 30),
                Mathf.Clamp(currentLayer * 5, 5, 20),
                15f
            };

            float total = 0;
            foreach (var w in weights) total += w;

            float randomPoint = Random.Range(0, total);
            for (int i = 0; i < weights.Length; i++)
            {
                if (randomPoint < weights[i]) return (RoutEventType) i;
                randomPoint -= weights[i];
            }

            return RoutEventType.Battle;
        }
        
        private int GetButtonLayer(EventButton button)
        {
            float xStart = _camera.ViewportToWorldPoint(new Vector3(_leftMarginPercent, 0.5f, 0)).x;
            return (int) (Mathf.RoundToInt(button.transform.position.x - xStart) / _xSteep);
        }
        
        private MapPath CreatePath(EventButton from, EventButton to)
        {
            GameObject pathObj = Instantiate(_pathPrefab, transform);
            MapPath path = pathObj.GetComponent<MapPath>();
            path.Initialize(from, to, _curveHeight);
            _allPaths.Add(path);
            from.AddConnectedButton(to);
            return path;
        }

        public void SetCurrentButton(EventButton button)
        {
            _currentButton = button;
            _currentButton.SetReachable(false);

            foreach (EventButton connected in _currentButton.ConnectedButtons) 
                connected.SetReachable(true);
        }

        private void UpdateReachableEvents()
        {
            foreach (var button in _allButtons) 
                button.SetReachable(false);

            foreach (var button in _currentButton.ConnectedButtons) 
                button.SetReachable(true);
        }
    }
}