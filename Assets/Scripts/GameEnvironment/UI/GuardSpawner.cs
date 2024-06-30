using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class GuardSpawner : MonoBehaviour
    {
        private Button _summonButton;

        public event UnityAction<GuardSpawner> SpawnerTriggered;

        private void Awake()
        {
            _summonButton = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            _summonButton.onClick.AddListener(OpenGuards);
        }

        private void OnDestroy()
        {
            _summonButton.onClick.RemoveListener(OpenGuards);
        }

        private void OpenGuards()
        {
            SpawnerTriggered?.Invoke(this);
        }
    }
}
