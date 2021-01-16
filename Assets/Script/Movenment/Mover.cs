using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float _maxSpeed = 6f;
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
        public void StartMove(Vector3 destination, float fraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, fraction);
        }
        public void MoveTo(Vector3 destination, float fraction)
        {
            _agent.destination = destination;
            _agent.speed = _maxSpeed * Mathf.Clamp01(fraction);
            _agent.isStopped = false;
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
