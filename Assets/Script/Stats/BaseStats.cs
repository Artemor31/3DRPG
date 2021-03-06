using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action OnLevelUp;
        
        [Range(1,10)]
        [SerializeField]
        private int startLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression;
        [SerializeField] private GameObject levelUpEffect;

        private int _currentLevel;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.OnExpGained += UpdateExp;
            }
        }

        private void UpdateExp()
        {
            if (CalculateLevel() > _currentLevel)
            {
                _currentLevel = CalculateLevel();
                LevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpEffect, transform);
        }

        public int GetLevel()
        {
            if (_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }
            return _currentLevel;
        }

        public float GetStat(Progression.Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startLevel;

            float currentExp = experience.GetExpPoints();
            int levelsLenght = progression.GetLevelsLenght(characterClass, Progression.Stat.ExpToLevelUp);

            for (int level = 1; level <= levelsLenght; level++)
            {
                float expToLvlUp = progression.GetStat(Progression.Stat.ExpToLevelUp, characterClass, level);

                if (currentExp <= expToLvlUp)
                {
                    return level;
                }
            }
            return levelsLenght + 1;
        }
    }
}
