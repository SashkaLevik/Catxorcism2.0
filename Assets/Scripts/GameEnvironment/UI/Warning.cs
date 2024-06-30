using UnityEngine;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class Warning : MonoBehaviour
    {
        [SerializeField] private GameObject _warning;

        public void Show()
        {
            _warning.SetActive(true);
            Invoke(nameof(Hide), 2.5f);
        }

        private void Hide() =>
            _warning.SetActive(false);
    }
}