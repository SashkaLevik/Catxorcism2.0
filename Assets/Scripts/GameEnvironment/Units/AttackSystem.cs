using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameEnvironment.Units
{
    public class AttackSystem : MonoBehaviour
    {
        [SerializeField] protected AudioSource _attackSound;
        [SerializeField] private float _moveSpeed;
        [SerializeField] protected Projectile _projectile;

        public void Attack(Unit player, Unit enemy)
        {
            if (player.AttackType == AttackType.Melee)
                StartCoroutine(MeleeAttack(player, enemy));
            else if (player.AttackType == AttackType.Range)
                RangeAttack(player, enemy);

            _attackSound.Play();
        }

        private void RangeAttack(Unit player, Unit enemy)
        {
            Projectile projectile = Instantiate(_projectile, player.FirePos.transform.position, Quaternion.identity, player.FirePos);
            projectile.Init(enemy, player.Damage);
            projectile.Shoot();
        }

        private IEnumerator MeleeAttack(Unit player, Unit enemy)
        {
            yield return StartCoroutine(Move(enemy.transform.position));
            yield return null;
            enemy.GetComponent<Health>().TakeDamage(player.Damage);
            player.GetComponent<Health>().TakeDamage(enemy.Damage);
            yield return StartCoroutine(Move(player.StartPosition));
        }

        private IEnumerator Move(Vector3 newPos)
        {
            while (transform.position != newPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPos, _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}