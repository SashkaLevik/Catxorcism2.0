using TMPro;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class Warning : MonoBehaviour
    {
        [SerializeField] private GameObject _warning;
        [SerializeField] private TMP_Text _wrongRowType;
        [SerializeField] private TMP_Text _noLeadership;
        [SerializeField] private TMP_Text _noAP;
        [SerializeField] private TMP_Text _occupiedSlot;
        [SerializeField] private TMP_Text _fullDeckCapacity;

        public TMP_Text WrongRowType => _wrongRowType;

        public TMP_Text NoLeadership => _noLeadership;

        public TMP_Text NoAP => _noAP;

        public TMP_Text OccupiedSlot => _occupiedSlot;

        public TMP_Text FullDeckCapacity => _fullDeckCapacity;

        public void Show(TMP_Text text)
        {
            _warning.SetActive(true);
            text.gameObject.SetActive(true);
            Invoke(nameof(Hide), 2.5f);
        }

        private void Hide() => 
            _warning.SetActive(false);
    }
}