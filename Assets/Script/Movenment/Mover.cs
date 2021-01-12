using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent _agent;
        Health _health;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        void Update()
        {
            _agent.enabled = !_health.Dead();
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            _agent.destination = destination;
            _agent.isStopped = false;
        }
        public void StartMove(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }
        public void Cancel()
        {
            _agent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = _agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }
    }
}
