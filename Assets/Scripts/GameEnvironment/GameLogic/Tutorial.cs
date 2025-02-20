using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Video;

namespace GameEnvironment.GameLogic
{
    public class Tutorial : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private VideoPlayer _player;
        [SerializeField] private List<VideoClip> _enClips;
        [SerializeField] private List<VideoClip> _ruClips;
        [SerializeField] private GameObject _tutorialWindow;

        private int _clipNumber;
        private PlayerProgress _progress;
        private List<VideoClip> _clips = new List<VideoClip>();

        private void Start()
        {
            _clips = GetLocalizedClips().ToList();

            if (_progress.WorldData.IsFirstRun == true)
                OpenTutorial();
        }

        private void OpenTutorial() =>
            Invoke(nameof(StartTutorial), 1f);

        private void StartTutorial()
        {
            _tutorialWindow.SetActive(true);
            StartCoroutine(WaitNextClip());
        }

        private IEnumerator WaitNextClip()
        {
            while (_clipNumber != _clips.Count)
            {
                ShowNext();
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                yield return null;
            }

            _progress.WorldData.IsFirstRun = false;
            _tutorialWindow.SetActive(false);
        }

        private void ShowNext()
        {
            _player.Stop();
            _player.clip = GetLocalizedClips()[_clipNumber];
            _player.Play();            
            _clipNumber++;
        }

        private List<VideoClip> GetLocalizedClips()
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return _enClips;

            return _ruClips;
        }

        public void Save(PlayerProgress progress) { }

        public void Load(PlayerProgress progress) =>
            _progress = progress;
    }
}