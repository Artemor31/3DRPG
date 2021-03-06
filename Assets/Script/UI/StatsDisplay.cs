using RPG.Resources;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RPG.UI
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] private Text healthText;
        [SerializeField] private Text expText;
        [SerializeField] private Text baseStatsText;

        private Health _health;
        private Experience _experience;
        private BaseStats _baseStats;
        
        private void Awake()
        {
            _experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            _health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            _baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            expText.text = $"{_experience.GetExpPoints():0}";
            healthText.text = $"{_health.GetInPercentage():0}%";
            baseStatsText.text = $"{_baseStats.CalculateLevel():0}";
        }
    } 
}
