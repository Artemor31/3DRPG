using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using UnityEngine.Serialization;
using RPG.Stats;

namespace RPG.Combat
{
    internal class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [FormerlySerializedAs("_leftHandTransform")] [SerializeField] private Transform leftHandTransform = null;
        [FormerlySerializedAs("_rightHandTransform")] [SerializeField] private Transform rightHandTransform = null;
        [FormerlySerializedAs("_defaultWeapon")] [SerializeField] private Weapon defaultWeapon = null;

        private float _timeSInceLastAttack = Mathf.Infinity;

        private Health _target;
        private Mover _mover;
        private Weapon _currentWeapon = null;

        private void Start()
        {
            _mover = GetComponent<Mover>();

           // if (_currentWeapon == null)
                EquipWeapon(defaultWeapon);
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
            if (_timeSInceLastAttack > _currentWeapon.GetDelay())
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

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        // IAction
        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            _target = null;
        }

        public bool CanAttack(GameObject cTarget)
        {
            if (cTarget == null) return false;

            Health htarget = cTarget.GetComponent<Health>();

            return htarget != null && !htarget.Dead();
        }

        public Health GetCurrentTarget()
        {
            return _target;
        }
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            _currentWeapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        //animator melee event
        private void Hit()
        {
            if (_target == null) return;

            float damageBonus = GetComponent<BaseStats>().GetStat(Progression.Stat.bonusDamage);

            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.CreateProjectile(rightHandTransform, leftHandTransform, _target, gameObject, damageBonus);                
            }
            else
            {
                _target.TakeDamage(gameObject, _currentWeapon.GetDamage() * damageBonus);
            }

            GetComponent<Animator>().SetTrigger("stopAttack");
        }
        //animator bow event
        private void Shoot()
        {
            Hit();
        }

        public object CaptureState()
        {
            if (_currentWeapon != null)
                return _currentWeapon.name;
            else
            {
                string s = "Unarmed";
                return s;
            }
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon =  UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
