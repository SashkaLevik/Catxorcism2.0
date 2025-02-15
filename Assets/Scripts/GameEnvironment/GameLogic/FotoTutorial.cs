using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.GameLogic
{
    public class FotoTutorial : MonoBehaviour, ISaveProgress
    {
        [SerializeField] private List<Sprite> _enImages;
        [SerializeField] private List<Sprite> _ruImages;
        [SerializeField] private GameObject _tutorialWindow;
        [SerializeField] private DeckSpawner _deck;
        [SerializeField] private Image _currentImage;

        private int _imageNumber;
        private PlayerProgress _progress;
        private ISaveLoadService _saveLoad;
        private List<Sprite> _images = new List<Sprite>();

        private void Start()
        {
            _saveLoad = AllServices.Container.Single<ISaveLoadService>();
            _images = GetLocalizedImages().ToList();

            if (_progress.WorldData.IsFirstRun == true)
                OpenTutorial();
        }

        private void OpenTutorial() =>
            Invoke(nameof(StartTutorial), 1f);

        private void StartTutorial()
        {
            _deck.DisactivateRaw();
            _tutorialWindow.SetActive(true);
            StartCoroutine(WaitNextClip());
        }

        private IEnumerator WaitNextClip()
        {
            while (_imageNumber != _images.Count)
            {
                ShowNext();
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                yield return null;
            }

            _progress.WorldData.IsFirstRun = false;
            _tutorialWindow.SetActive(false);
            _deck.ActivateRaw();
            _saveLoad.SaveProgress();
        }

        private void ShowNext()
        {
            _currentImage.sprite = _images[_imageNumber];            
            _imageNumber++;
        }

        private List<Sprite> GetLocalizedImages()
        {
            if (Application.systemLanguage == SystemLanguage.English)
                return _enImages;

            return _ruImages;
        }

        public void Save(PlayerProgress progress) { }

        public void Load(PlayerProgress progress) =>
            _progress = progress;
    }
}
