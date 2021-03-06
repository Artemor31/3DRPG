using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Health _health;
        private void Start()
        {
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            if ( _health.Dead() ) return;
            if ( InteractWithCombat() ) return;
            if ( InteractWithMovement() ) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;
                
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool isHitted = Physics.Raycast(GetMouseRay(), out hit);

            if (isHitted)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMove(hit.point, 1);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
