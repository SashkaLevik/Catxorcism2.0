using System;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic.DiceFolder
{
    public class DiceFace : MonoBehaviour
    {
        [SerializeField] private SuitType _suitType;
        [SerializeField] private DiceFaceData _faceData;
        [SerializeField] private Material _material;

        private MeshRenderer _meshRenderer;
        
        public SuitType SuitType => _suitType;
        public DiceFaceData FaceData => _faceData;

        private void Awake() => 
            _meshRenderer = GetComponent<MeshRenderer>();

        public void Init(DiceFaceData faceData)
        {
            _faceData = faceData;
            _suitType = _faceData.SuitType;
            _meshRenderer.material = _faceData.Material;
        }
    }
}