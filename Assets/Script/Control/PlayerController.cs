using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        void Update()
        {
            InteractWithCombat();
            InnteractWithMovement();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack();
                }
            }
        }

        private void InnteractWithMovement()
        {
            if (Input.GetMouseButton(0))
            {
                MoveToCoursor();
            }
        }

        void MoveToCoursor()
        {
            RaycastHit hit;
            bool isHitted = Physics.Raycast(GetMouseRay(), out hit);

            if (isHitted)
                GetComponent<Mover>().MoveTo(hit.point);
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
