using TMPro;
using UnityEngine;

namespace GameEnvironment.UI
{
    public class Warning : MonoBehaviour
    {
        [SerializeField] private GameObject _warning;
        [SerializeField] private TMP_Text _wrongRowType;

        public TMP_Text WrongRowType => _wrongRowType;

        public void Show(TMP_Text text)
        {
            text.gameObject.SetActive(true);
            Invoke(nameof(Hide), 2.5f);
        }

        private void Hide(TMP_Text text) =>
            text.gameObject.SetActive(false);
    }
}