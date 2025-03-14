using System.Collections.Generic;
using GameEnvironment.GameLogic.CardFolder;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class ActionPointsViewer : MonoBehaviour
    {
        [SerializeField] private Sprite _fullSprite;
        [SerializeField] private Sprite _emptySprite;
        [SerializeField] private GameObject _container;
        [SerializeField] private Image _imagePrefab;

        private int _actionPoints;
        private int _requiredAP;
        private Image _apImage;
        private Guard _guard;
        private List<Image> _apImages = new List<Image>();

        private void Start()
        {
            _guard = GetComponent<Guard>();
            _actionPoints = _guard.CardData.ActionPoints;
            _guard.APChanged += UpdateAP;

            for (int i = 0; i < _actionPoints; i++)
            {
                _apImage = Instantiate(_imagePrefab, _container.transform);
                _apImage.sprite = _fullSprite;
                _apImages.Add(_apImage);
            }
        }

        public void ResetAP()
        {
            foreach (var image in _apImages) 
                image.sprite = _fullSprite;
        }
        
        private void UpdateAP(int requiredAP)
        {
            _requiredAP = requiredAP;
            
            if (_requiredAP > _apImages.Count) 
                _requiredAP = _apImages.Count;
            
            for (int i = 0; i < _requiredAP; i++) 
                _apImages[i].sprite = _emptySprite;
        }
    }
}