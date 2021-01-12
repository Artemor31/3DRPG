using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AICotroller : MonoBehaviour
    {
        [SerializeField] float _chaseDistance = 5f;


        GameObject _player;
        Fighter _fighter;
        Health _health;
        Mover _mover;
        Vector3 _startPosition;


        private void Start()
        {
            _mover = GetComponent<Mover>();
            _startPosition = transform.position;
            _player = GameObject.FindGameObjectWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }
        private void Update()
        {
            if (_health.Dead()) return;
            if (DistanceToPlayer() && _fighter.CanAttack(_player))
            {
                _fighter.Attack(_player);
            }
            else
            {
                _mover.StartMove(_startPosition);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.Lerp(Color.red, Color.green, 0.3f);
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }

        private bool DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position) < _chaseDistance;
        }
    }
}
