using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.DiceFolder;
using UnityEngine;
using UnityEngine.UI;

namespace GameEnvironment.LevelRoutMap.RoutEventWindows
{
    public class DiceSmith : RoutEvent
    {
        [SerializeField] private int _forgePrice;
        [SerializeField] private Dice _frontDice;
        [SerializeField] private Dice _backDice;
        [SerializeField] private List<DiceLayoutButton> _frontDiceLayout;
        [SerializeField] private List<DiceLayoutButton> _backDiceLayout;
        [SerializeField] private List<DiceFace> _smithDiceLayout;
        [SerializeField] private Image _choosedSmithFace;
        [SerializeField] private Image _choosedPlayerFace;
        [SerializeField] private Button _forge;

        private DiceFaceData _dataToAdd;
        private DiceFaceData _dataToRemove;
        private Dice _currentDice;
        private DiceLayoutButton _currentButton;

        private void Start()
        {
            _frontDice = _battleHud.PlayerFrontDice;
            _backDice = _battleHud.PlayerBackDice;
            CreateDiceLayout(_frontDice, _frontDiceLayout);
            CreateDiceLayout(_backDice, _backDiceLayout);
            _forge.onClick.AddListener(ForgeDiceFace);

            foreach (var diceFace in _smithDiceLayout)
                diceFace.GetComponent<Button>().onClick.AddListener(() => SetFaceToAdd(diceFace));
        }

        private void ForgeDiceFace()
        {
            if (_playerMoney.Coins >= _forgePrice)
            {
                _playerMoney.RemoveCoin(_forgePrice, _battleHud.CoinsAmount);
                _battleHud.ChangeDiceFace(_currentDice, _dataToRemove, _dataToAdd);
                _currentButton.GetComponent<Image>().sprite = _dataToAdd.SuitImage;
            }
            else
                _warning.Show(_warning.NoCoins);
        }

        private void SetFaceToAdd(DiceFace diceFace)
        {
            _dataToAdd = diceFace.DiceFaceData;
            _choosedSmithFace.sprite = _dataToAdd.SuitImage;
        }

        private void CreateDiceLayout(Dice dice, List<DiceLayoutButton> layoutButtons)
        {
            for (int i = 0; i < dice.Faces.Count; i++)
            {
                layoutButtons[i].SetDiceData(dice, dice.Faces[i].DiceFaceData);
                Image faceIcon = layoutButtons[i].GetComponent<Image>();
                faceIcon.sprite = dice.Faces[i].SuitIcon;
            }

            foreach (var button in layoutButtons)
                button.GetComponent<Button>().onClick.AddListener(() => SetFaceToRemove(button));
        }

        private void SetFaceToRemove(DiceLayoutButton layoutButton)
        {
            _currentDice = layoutButton.Dice;
            _dataToRemove = layoutButton.FaceData;
            _currentButton = layoutButton;
            _choosedPlayerFace.sprite = _dataToRemove.SuitImage;
        }
    }
}