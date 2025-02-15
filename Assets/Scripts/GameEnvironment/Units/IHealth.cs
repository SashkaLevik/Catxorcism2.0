using UnityEngine.Events;

namespace GameEnvironment.Units
{
    public interface IHealth
    {
        event UnityAction<int> HealthChanged;
        event UnityAction<int> DefenceChanged;
        int CurrentHP { get; set; }
        int MaxHP { get; set; }
        int Defence { get; set; }

        void TakeDamage(int damage);
        //void TakeDirectDamage(float damage);
        //void BreakeDefence(float value);
    }
}
