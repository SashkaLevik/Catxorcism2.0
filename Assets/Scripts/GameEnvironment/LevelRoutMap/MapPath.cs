using GameEnvironment.LevelRoutMap.RoutEventWindows;
using UnityEngine;

namespace GameEnvironment.LevelRoutMap
{
    public class MapPath : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        /*private EventButton _startButton;
        private EventButton _endButton;*/
        //private float _lineThickness = 0.2f;

        public void Initialize(EventButton start, EventButton end, float curveHeight)
        {
            //_lineRenderer.startWidth = _lineThickness;
            //_lineRenderer.endWidth = _lineThickness;
            //DrawCurvedPath(start.transform.position, end.transform.position, curveHeight);

            UpdateVisuals(start, end);
        }

        private void DrawCurvedPath(Vector3 start, Vector3 end, float height)
        {
            Vector3 controlPoint = (start + end) / 2 + Vector3.up * height;

            _lineRenderer.positionCount = 12;

            for (int i = 0; i < 12; i++)
            {
                float t = i / 11f;
                Vector3 position = CalculateBezierPoint(t, start, controlPoint, end);
                _lineRenderer.SetPosition(i, position);
            }
        }

        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }

        private void UpdateVisuals(EventButton start, EventButton end)
        {
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, start.transform.position);
            _lineRenderer.SetPosition(1, end.transform.position);
        }
    }
}