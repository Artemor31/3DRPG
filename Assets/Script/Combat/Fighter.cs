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

        float _timeSInceLastAttack = Mathf.Infinity;

        Health _target;
        Mover _mover;

        private void Start()
        {
            _mover = GetComponent<Mover>();
        }

        private void Update()
        {
            _timeSInceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.Dead()) return;

            if (!EnemyInRange())
            {
                _mover.MoveTo(_target.GetComponent<Transform>().position);
            }
            else
            {
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);
            if (_timeSInceLastAttack > _attackDelay)
            {
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
                _timeSInceLastAttack = 0;
            }
        }

        private bool EnemyInRange()
        {
            return Vector3.Distance(transform.position, _target.GetComponent<Transform>().position) < _range;
        }

        public void Attack(GameObject CombatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = CombatTarget.GetComponent<Health>();
        }
        // IAction
        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            _target = null;
        }

        public bool CanAttack(GameObject _cTarget)
        {
            if (_cTarget == null) return false;

            Health _Htarget = _cTarget.GetComponent<Health>();

            return _Htarget != null && !_Htarget.Dead();
        }
        //animator event
        void Hit()
        {
            if(_target != null)
            _target.TakeDamage(Damage);
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
