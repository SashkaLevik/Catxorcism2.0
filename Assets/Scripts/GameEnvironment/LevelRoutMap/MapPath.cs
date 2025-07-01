using Data;
using GameEnvironment.LevelRoutMap.RoutEventWindows;
using UnityEngine;

namespace GameEnvironment.LevelRoutMap
{
    public class MapPath : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public PathData PathData;
        public EventButton StartButton;
        public EventButton EndButton;
        private float _curveHeight = 1f;
        private Transform _buttonContainer;
        private Vector3[] _curvePoints;

        public void Initialize(EventButton start, EventButton end, Transform container)
        {
            StartButton = start;
            EndButton = end;
            _buttonContainer = container;
            _lineRenderer.useWorldSpace = false;
            GenerateCurvedPoints();
            UpdateVisuals();
        }

        public void UpdateVisuals()
        {
            if (_curvePoints == null || _curvePoints.Length == 0) return;
        
            _lineRenderer.positionCount = _curvePoints.Length;
            _lineRenderer.SetPositions(_curvePoints);
        }
        
        private void GenerateCurvedPoints()
        {
            Vector3 startLocal = _buttonContainer.InverseTransformPoint(StartButton.transform.position);
            Vector3 endLocal = _buttonContainer.InverseTransformPoint(EndButton.transform.position);
        
            Vector3 controlPoint = (startLocal + endLocal) / 2 + Vector3.up * _curveHeight;
        
            _curvePoints = new Vector3[12];
            for (int i = 0; i < 12; i++)
            {
                float t = i / 11f;
                _curvePoints[i] = CalculateBezierPoint(t, startLocal, controlPoint, endLocal);
            }
        }

        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }
        
        /*public void UpdateVisuals(EventButton start, EventButton end)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, start.transform.position);
            _lineRenderer.SetPosition(1, end.transform.position);
        }*/
    }
}