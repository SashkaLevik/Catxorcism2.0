using System;
using Data;
using UnityEngine;

namespace GameEnvironment.GameLogic.DiceFolder
{
    public class DiceFace : MonoBehaviour
    {
        [SerializeField] private SuitType _suitType;
        [SerializeField] private DiceFaceData _faceData;
        [SerializeField] private Material _material;

        public SuitType SuitType => _suitType;

        private void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
        }

        public void Init(DiceFaceData faceData)
        {
            _faceData = faceData;
            _suitType = _faceData.SuitType;
            _material = _faceData.Material;
        }
    }
}