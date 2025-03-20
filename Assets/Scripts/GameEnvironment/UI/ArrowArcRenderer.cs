using System.Collections.Generic;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class ArrowArcRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private GameObject _dotPrefab;

        private int _poolSize = 50;
        private int _dotToSkip = 1;
        public float _spacing = 5;
        private float _arrowAngle = 0;
        private Camera _camera;
        private GameObject _arrow;
        private Vector3 _arrowDirection = new Vector3(0, 0, 0);
        private List<GameObject> _dotPool = new List<GameObject>();

        private void Start()
        {
            _camera = Camera.main;
            _arrow = Instantiate(_arrowPrefab, transform);
            _arrow.transform.localPosition = Vector3.zero;
            InitializePool(_poolSize);
        }

        private void Update()
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 startPos = transform.position;
            Vector3 midPoint = CalculateMidPoint(startPos, mousePos);
            UpdateArc(startPos, midPoint, mousePos);
            PositionAndRotationArrow(mousePos);
        }

        private void PositionAndRotationArrow(Vector3 position)
        {
            _arrow.transform.position = position;
            Vector3 direction = _arrowDirection - position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += _arrowAngle;
            _arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
        {
            int numDots = Mathf.CeilToInt(Vector3.Distance(start, end) / _spacing);

            for (int i = 0; i < numDots && i < _dotPool.Count; i++)
            {
                float t = i / (float) numDots;
                t = Mathf.Clamp(t, 0f, 1f);
                Vector3 position = QuadraticBezierPoint(start, mid, end, t);

                if (i != numDots - _dotToSkip)
                {
                    _dotPool[i].transform.position = position;
                    _dotPool[i].SetActive(true);
                }

                if (i == numDots - (_dotToSkip + 1) && i - _dotToSkip + 1 >= 0)
                {
                    _arrowDirection = _dotPool[i].transform.position;
                }
            }

            for (int i = numDots - _dotToSkip; i < _dotPool.Count; i++)
            {
                if (i > 0) _dotPool[i].SetActive(false);
            }
        }

        private Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = uu * start;
            point += 0.5f * u * t * control;
            point += tt * end;
            return point;
        }

        private Vector3 CalculateMidPoint(Vector3 start, Vector3 end)
        {
            Vector3 midPoint = (start + end) / 1f;
            float arcHeight = Vector3.Distance(start, end) / 10f;
            midPoint.y += arcHeight;
            return midPoint;
        }

        private void InitializePool(int poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject dot = Instantiate(_dotPrefab, Vector3.zero, Quaternion.identity, transform);
                dot.SetActive(false);
                _dotPool.Add(dot);
            }
        }
    }
}