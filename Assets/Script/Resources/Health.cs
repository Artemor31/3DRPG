using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoints = -1f;
        [SerializeField] private Progression progression;

        private bool _isDead = false;

        private void Start()
        {
            GetComponent<BaseStats>().OnLevelUp += UpdadeHealthOnLevelUp;

            if (healthPoints < 0)
                healthPoints = GetComponent<BaseStats>().GetStat(Progression.Stat.Health);
        }

        private void UpdadeHealthOnLevelUp()
        {
            if (progression == null) return;

            int currentLevel = Math.Max(GetComponent<BaseStats>().GetLevel(), 1);

            float currentMaxHp = progression.GetStat(Progression.Stat.Health, CharacterClass.Player, currentLevel);
            float currentMaxHpPlus = progression.GetStat(Progression.Stat.Health, CharacterClass.Player, currentLevel+1);

            healthPoints += currentMaxHpPlus - currentMaxHp;
        }

        public void TakeDamage(GameObject damager, float damage)
        {
            Debug.Log(damage);
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints != 0) return;
            Die();
            AwardExp(damager);
        }

        private void AwardExp(GameObject damager)
        {
            var exp = damager.GetComponent<Experience>();

            if (exp != null)
                exp.GainExperience(GetComponent<BaseStats>().GetStat(Progression.Stat.ExpReward));
        }

        public bool Dead()
        {
            return _isDead;
        }
        
        private void Die()
        {
            if (!_isDead)
            {
                _isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }

        public float GetInPercentage()
        {
            return 100 * (healthPoints / GetComponent<BaseStats>().GetStat(Progression.Stat.Health));
        }
        public object CaptureState()
        {
            return healthPoints;
        }


        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
            else
            {
                _isDead = false;
                GetComponent<Animator>().Play("Move");
            }
        }
    }
}
