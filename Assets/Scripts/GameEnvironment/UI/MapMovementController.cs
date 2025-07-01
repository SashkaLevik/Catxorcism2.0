using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEnvironment.LevelRoutMap;
using GameEnvironment.LevelRoutMap.RoutEventWindows;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.UI
{
    public class MapMovementController : MonoBehaviour
    {
        [SerializeField] private Transform _mapContainer;
        [SerializeField] private RoutMap _routMap;

        private int _mapCenterX = -15;
        private float _tokenMovementDuration = 0.5f;
        private float _moveDuration = 1f;
        private float _horizontalPadding = 2.0f;
        private float _mapMinX;
        private float _mapMaxX;
        private GameObject _playerToken;
        private AnimationCurve _moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private AnimationCurve _tokenCurve = AnimationCurve.EaseInOut(0,0,1,1);
        private Coroutine _moveCoroutine;
        private Coroutine _tokenCoroutine;
        private List<MapPath> _allPaths = new List<MapPath>();

        public event UnityAction<EventButton> MapMoved;

        private void Start()
        {
            _playerToken = _routMap.PlayerToken;
            _routMap.OnMapGenerated += InitializeMap;
            _routMap.OnButtonChanged += MoveTokenToButton;
        }
        
        private void OnDestroy()
        {
            _routMap.OnMapGenerated -= InitializeMap;
            _routMap.OnButtonChanged -= MoveTokenToButton;
        }

        private void InitializeMap()
        {
            _mapMinX = float.MaxValue;
            _mapMaxX = float.MinValue;

            foreach (var eventButton in _routMap.AllButtons)
            {
                float x = eventButton.transform.position.x;
                
                if (x < _mapMinX) _mapMinX = x;
                if (x > _mapMaxX) _mapMaxX = x;
            }

            _mapMinX -= _horizontalPadding;
            _mapMaxX += _horizontalPadding;
            _allPaths.Clear();
            _allPaths.AddRange(_mapContainer.GetComponentsInChildren<MapPath>());
        }

        private void MoveTokenToButton(EventButton button)
        {
            if (_tokenCoroutine != null) StopCoroutine(_tokenCoroutine);
            _tokenCoroutine = StartCoroutine(MoveToken(button));
        }

        private void ShiftMap()
        {
            if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);

            if (_mapContainer.position.x > _mapCenterX) 
                _moveCoroutine = StartCoroutine(MoveMap());
        }
        
        private void UpdatePaths()
        {
            foreach (var path in _allPaths.Where(path => path != null)) path.UpdateVisuals();
        }

        private IEnumerator MoveToken(EventButton targetButton)
        {
            RectTransform tokenParent = targetButton.GetComponent<RectTransform>();
            Vector3 startPos = _playerToken.transform.position;
            Vector3 endPos = targetButton.transform.position;
            float elapsed = 0f;

            while (elapsed < _tokenMovementDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _tokenMovementDuration);
                float curveT = _tokenCurve.Evaluate(t);

                _playerToken.transform.position = Vector3.Lerp(startPos, endPos, curveT);
                yield return null;
            }
            
            _playerToken.transform.position = endPos;
            _playerToken.transform.SetParent(tokenParent);
            _playerToken.transform.localScale = Vector3.one;
            ShiftMap();
            
            MapMoved?.Invoke(targetButton);
        }
        
        private IEnumerator MoveMap()
        {
            Vector3 startPos = _mapContainer.position;
            Vector3 targetPos = startPos + new Vector3(-3, 0, 0);
            float elapsed = 0f;
        
            while (elapsed < _moveDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _moveDuration);
                float curveT = _moveCurve.Evaluate(t);
                
                _mapContainer.position = Vector3.Lerp(startPos, targetPos, curveT);
                UpdatePaths();
                yield return null;
            }
            
            _mapContainer.position = targetPos;
            _moveCoroutine = null;
            UpdatePaths();
        }
    }
}