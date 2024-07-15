using Agava.WebUtility;
using Assets.Scripts.GameEnvironment.UI;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Infrastructure.GameManegment
{
    public class WebFocus : MonoBehaviour
    {
        private const string MasterVolume = "MasterVolume";

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private VolumeSettings _volumeSettings;

        public float _currentVolume;        

        private void OnEnable()
        {
            Application.focusChanged += OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
        }        

        private void OnDisable()
        {
            Application.focusChanged -= OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
        }

        public void MuteAudio(bool value)
        {
            _currentVolume = _volumeSettings.Volume;
            _audioMixer.SetFloat(MasterVolume, value ? -80 : Mathf.Log10(_currentVolume) * 20);
        }

        public void PauseGame(bool value)
        {
            Time.timeScale = value ? 0 : 1;
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            MuteAudio(!inApp);
            PauseGame(!inApp);
        }

        private void OnInBackgroundChangeWeb(bool isBackground)
        {
            MuteAudio(isBackground);
            PauseGame(isBackground);
        }             
    }
}