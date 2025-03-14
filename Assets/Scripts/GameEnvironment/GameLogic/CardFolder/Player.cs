using System.Collections.Generic;
using Data;
using GameEnvironment.UI;
using GameEnvironment.Units;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Player : Unit, ISaveProgress
    {
        [SerializeField] private PlayerType _playerType;
        [SerializeField] private List<Guard> _playerGuards;

        private int _leadership;
        private DragController _dragController;
        private PlayerProgress _progress;
        
        public PlayerType Type => _playerType;
        
        public List<Guard> PlayerGuards => _playerGuards;

        public int Leadership
        {
            get => _leadership;
            set
            {
                _leadership = value;
                LeadershipChanged?.Invoke(Leadership);
            }
        }

        public event UnityAction<int> LeadershipChanged;

        protected override void Start()
        {
            base.Start();
            //_health.Died += OnPlayerDie;
            _dragController.GuardPlaced += OnGuardPlaced;
        }

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHealth;
            _health.DefenceChanged -= UpdateDefence;
            //_health.Died -= OnPlayerDie;
            _dragController.GuardPlaced -= OnGuardPlaced;
        }

        public void Construct(DragController dragController)
        {
            _dragController = dragController;
        }

        public void RestoreLeadership() => 
            Leadership++;

        private void OnGuardPlaced(Guard guard) => 
            Leadership--;

        /*private void OnPlayerDie()
        {
            _dragController.GuardPlaced -= OnGuardPlaced;
            _health.Died -= OnPlayerDie;
        }*/

        public void Load(PlayerProgress progress)
        {
            _progress = progress;

            _leadership = _progress.WorldData.IsNewRun ? _cardData.ActionPoints : progress.PlayerStats.Leadership;
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.Leadership = _leadership;
        }
    }
}