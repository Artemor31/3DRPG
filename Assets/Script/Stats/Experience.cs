using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action OnExpGained;

        [SerializeField] private float experience = 0;

        public void GainExperience(float exp)
        {
            experience += exp;
            OnExpGained();
        }

        public float GetExpPoints()
        {
            return experience;
        }

        public object CaptureState()
        {
            return experience;
        }

        public void RestoreState(object state)
        {
            experience = (float)state;
        }
    } 
}
