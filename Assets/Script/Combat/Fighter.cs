using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] Transform _leftHandTransform = null;
        [SerializeField] Transform _rightHandTransform = null;
        [SerializeField] Weapon _defaultWeapon = null;

        float _timeSInceLastAttack = Mathf.Infinity;

        Health _target;
        Mover _mover;
        Weapon _currentWeapon = null;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            EquipWeapon(_defaultWeapon);
        }

        private void Update()
        {
            _timeSInceLastAttack += Time.deltaTime;

            if (_target == null) return;
            if (_target.Dead()) return;

            if (!EnemyInRange())
            {
                _mover.MoveTo(_target.GetComponent<Transform>().position, 1);
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
            if (_timeSInceLastAttack > _defaultWeapon.GetDelay())
            {
                GetComponent<Animator>().ResetTrigger("stopAttack");
                GetComponent<Animator>().SetTrigger("attack");
                _timeSInceLastAttack = 0;
            }
        }

        private bool EnemyInRange()
        {
            return Vector3.Distance(transform.position, _target.GetComponent<Transform>().position) < _currentWeapon.GetRange();
        }

        public void Attack(GameObject CombatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = CombatTarget.GetComponent<Health>();
        }

        // IAction
        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            _target = null;
        }

        public bool CanAttack(GameObject _cTarget)
        {
            if (_cTarget == null) return false;

            Health _Htarget = _cTarget.GetComponent<Health>();

            return _Htarget != null && !_Htarget.Dead();
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            _currentWeapon.Spawn(_rightHandTransform, _leftHandTransform, animator);
        }

        //animator melee event
        void Hit()
        {
            if (_target == null) return; 

            if (_currentWeapon.hasProjectile())
            {
                _currentWeapon.CreateProjectile(_rightHandTransform, _leftHandTransform, _target);                
            }
            else
            {
                _target.TakeDamage(_currentWeapon.GetDamage());
            }

            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        //animator bow event
        void Shoot()
        {
            Hit();
        }
    }
}
