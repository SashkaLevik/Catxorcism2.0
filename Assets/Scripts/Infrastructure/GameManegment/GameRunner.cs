using UnityEngine;

namespace Assets.Scripts.Infrastructure.GameManegment
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;

        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper != null)
                return;

            Instantiate(BootstrapperPrefab);
        }

        private void OnEnable()
        {
            //WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        private void OnDisable()
        {
            //WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
        }
    }
}
