using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.PlayerSkills;
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
        [SerializeField] private List<PlayerSkill> _playerSkills;

        private int _leadership;
        private DragController _dragController;
        private PlayerProgress _progress;

        public int Level { get; private set; }
        public PlayerType Type => _playerType;
        public List<Guard> PlayerGuards => _playerGuards;
        public List<PlayerSkill> PlayerSkills => _playerSkills;

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
            Health.HealthChanged -= UpdateHealth;
            Health.DefenceChanged -= UpdateDefence;
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
            Level = progress.PlayerStats.Level;
            
            _leadership = _progress.WorldData.IsNewRun ? _cardData.ActionPoints 
                : progress.PlayerStats.Leadership;
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.Leadership = _leadership;
        }
    }
}