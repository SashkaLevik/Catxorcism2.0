using Assets.Scripts.Data;
using Assets.Scripts.Infrastructure.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Assets.Scripts.GameEnvironment.UI
{
    public class VolumeSettings : MonoBehaviour, ISaveProgress
    {

        private const string MasterVolume = "MasterVolume";
        private const string MusicVolume = "MusicVolume";
        private const string SFXVolume = "SFXVolume";

        [SerializeField] private Slider _masterSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _SFXSlider;
        [SerializeField] private AudioMixer _mixer;

        private float _masterVolume;
        private float _musicVolume;
        private float _SFXVolume;

        private void Awake()
        {
            _masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
            _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _SFXSlider.onValueChanged.AddListener(ChangeSoundVolume);
        }

        private void OnDestroy()
        {
            _masterSlider.onValueChanged.RemoveListener(ChangeMasterVolume);
            _musicSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
            _SFXSlider.onValueChanged.RemoveListener(ChangeSoundVolume);
        }

        public void ChangeMasterVolume(float volume)
        {
            _mixer.SetFloat(MasterVolume, Mathf.Log10(volume) * 20);
            _masterVolume = volume;
        }

        public void ChangeMusicVolume(float volume)
        {
            _mixer.SetFloat(MusicVolume, Mathf.Log10(volume) * 20);
            _musicVolume = volume;
        }

        public void ChangeSoundVolume(float volume)
        {
            _mixer.SetFloat(SFXVolume, Mathf.Log10(volume) * 20);
            _SFXVolume = volume;
        }

        public void Load(PlayerProgress progress)
        {
            _masterVolume = progress.WorldData.MasterVolume;
            _musicVolume = progress.WorldData.MusicVolume;
            _SFXVolume = progress.WorldData.SFXVolume;
            _masterSlider.value = _masterVolume;
            _musicSlider.value = _musicVolume;
            _SFXSlider.value = _SFXVolume;
            _mixer.SetFloat(MasterVolume, Mathf.Log10(_masterVolume) * 20);
            _mixer.SetFloat(MusicVolume, Mathf.Log10(_musicVolume) * 20);
            _mixer.SetFloat(SFXVolume, Mathf.Log10(_SFXVolume) * 20);
        }

        public void Save(PlayerProgress progress)
        {
            progress.WorldData.MasterVolume = _masterVolume;
            progress.WorldData.MusicVolume = _musicVolume;
            progress.WorldData.SFXVolume = _SFXVolume;
        }       
    }
}