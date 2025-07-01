using Data;
using UnityEngine;

namespace GameEnvironment.GameLogic.DiceFolder
{
    public class DiceFace : MonoBehaviour
    {
        [SerializeField] private SuitType _suitType;
        [SerializeField] private Sprite _suitIcon;
        [SerializeField] private Material _material;

        public DiceFaceData DiceFaceData;
        private MeshRenderer _meshRenderer;
        
        public SuitType SuitType => _suitType;

        public Sprite SuitIcon => _suitIcon;

        public Material Material => _material;

        private void Awake() => 
            _meshRenderer = GetComponent<MeshRenderer>();

        private void Start()
        {
            _meshRenderer.material = _material;
        }

        public void ConstructDiceFace(DiceFaceData data)
        {
            DiceFaceData = data;
            _suitType = DiceFaceData.SuitType;
            _suitIcon = DiceFaceData.SuitImage;
            _material = DiceFaceData.Material;
            _meshRenderer.material = _material;
        }
    }
}