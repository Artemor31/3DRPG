using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        [FormerlySerializedAs("_healthText")] [SerializeField] private Text healthText;

        private void Awake()
        {
            _fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if (_fighter.GetCurrentTarget() == null)
            {
                healthText.text = "...";
                return;
            }
            Health health = _fighter.GetCurrentTarget();
            healthText.text = health.GetInPercentage().ToString() + "%";
        }
    } 
}
