using System.Collections.Generic;
using Data;
using GameEnvironment.GameLogic.PlayerSkills;
using GameEnvironment.Units;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Events;

namespace GameEnvironment.GameLogic.CardFolder
{
    public class Player : Unit, ISaveProgress
    {
        [SerializeField] private PlayerType _playerType;
        [SerializeField] private List<PlayerSkill> _playerSkills;

        private int _leadership;
        private DragController _dragController;
        private PlayerProgress _progress;

        public int Level { get; private set; }
        public PlayerType Type => _playerType;
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

        private void OnDestroy()
        {
            Health.HealthChanged -= UpdateHealth;
            Health.DefenceChanged -= UpdateDefence;
            Health.Died -= OnPlayerDie;
        }

        public void Construct(DragController dragController)
        {
            _dragController = dragController;
            _dragController.GuardPlaced += OnGuardPlaced;
        }

        public void RestoreLeadership() => 
            Leadership++;

        private void OnGuardPlaced(Guard guard) => 
            Leadership--;

        private void OnPlayerDie(Unit unit)
        {
            _dragController.GuardPlaced -= OnGuardPlaced;
            Health.Died -= OnPlayerDie;
        }

        public void Load(PlayerProgress progress)
        {
            _progress = progress;
            Level = progress.PlayerStats.Level;

            _leadership = progress.PlayerStats.Leadership;
            /*_leadership = _progress.WorldData.IsNewRun ? _cardData.ActionPoints 
                : progress.PlayerStats.Leadership;*/
        }

        public void Save(PlayerProgress progress)
        {
            progress.PlayerStats.Leadership = _leadership;
        }
    }
}