using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float _range = 2f;
        [SerializeField] float _attackDelay = 1f;
        [SerializeField] float Damage = 10f;

        float _timeToAttack;

        Transform _target;
        Mover _mover;

        private void Start()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            _timeToAttack += Time.deltaTime;

            if (_target == null) return;

            if (!EnemyInRange())
            {
                _mover.MoveTo(_target.position);
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if (_timeToAttack >= _attackDelay)
            {
                GetComponent<Animator>().SetTrigger("attack");
                _timeToAttack = 0;
            }
        }

        private bool EnemyInRange()
        {
            return Vector3.Distance(transform.position, _target.position) < _range;
        }

        public void Attack(CombatTarget CombatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = CombatTarget.transform;
        }
        // IAction
        public void Cancel()
        {
            _target = null;
        }

        //animator event
        void Hit()
        {
            _target.GetComponent<Health>().TakeDamage(Damage);
        }
    }
}
