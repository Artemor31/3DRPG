using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Stats", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] progressionCharacterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookUpTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUpTable();

            float[] levels = _lookUpTable[characterClass][stat];

            if (levels.Length <= level)
                return levels[levels.Length - 1];
            //////////////////////////////////
            float a = 0;
            try
            {
                a = levels[level - 1];
            }
            catch (IndexOutOfRangeException)
            {
                Debug.Log(a + " " + level + " " + levels.Length);
            }

            return a;
        }

        private void BuildLookUpTable()
        {
            if (_lookUpTable != null) return;

            _lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progCharClass in progressionCharacterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat pStat in progCharClass.stats)
                {
                    statLookUpTable[pStat.stat] = pStat.level;
                }

                _lookUpTable[progCharClass.characterClass] = statLookUpTable;
            }

        }

        public int GetLevelsLenght(CharacterClass characterClass, Stat stat)
        {
            BuildLookUpTable();
            float[] levels = _lookUpTable[characterClass][stat];
            return levels.Length;
        }


        [Serializable]
        private class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [Serializable]
        private class ProgressionStat 
        { 
            public Stat stat;
            public float[] level;
        }

        public enum Stat
        {
            Health,
            ExpReward,
            ExpToLevelUp,
            bonusDamage
        }
    }
}
