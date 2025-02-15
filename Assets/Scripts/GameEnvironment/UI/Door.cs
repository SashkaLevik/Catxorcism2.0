using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.UI
{
    public class Door : MonoBehaviour
    {
        private const string IsOpen = "IsOpen";

        [SerializeField] private TMP_Text _doorTitle;
        [SerializeField] private AudioSource _doorOpen;
        [SerializeField] private AudioSource _doorClose;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnEnter()
        {
            if (GetComponent<Button>().interactable == true)
            {
                _animator.SetBool(IsOpen, true);
                _doorOpen.Play();
                _doorTitle.enabled = false;
            }            
        }

        public void OnExit()
        {
            if (GetComponent<Button>().interactable == true)
            {
                _animator.SetBool(IsOpen, false);
                Invoke(nameof(ShowTitle), 0.5f);
                _doorClose.Play();
            }
        }

        public void OnClick()
        {
            if (GetComponent<Button>().interactable == true)
            {
                _animator.SetBool(IsOpen, false);
                Invoke(nameof(ShowTitle), 0.5f);
                _doorClose.Play();
            }
        }

        private void ShowTitle() =>
            _doorTitle.enabled = true;
    }
}