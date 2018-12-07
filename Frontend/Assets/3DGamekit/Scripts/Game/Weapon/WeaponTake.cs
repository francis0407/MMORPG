using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gamekit3D
{
    [RequireComponent(typeof(Collider))]
    public class WeaponTake : MonoBehaviour
    {
        public UnityEvent OnEnter, OnExit;
        public GameObject weapon;
        static private bool hasTaken = false;
        void Reset()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            ExecuteOnEnter(other);
        }

        protected virtual void ExecuteOnEnter(Collider other)
        {
            if (hasTaken)
                return;
            //PlayerMyController sender = other.GetComponent<PlayerMyController>();
            //if (sender == null)
            //{
            //    return;
            //}
            //sender.PlayerTakeWeapon(weapon);
            //sender.PlayerTakeItem();
            //OnEnter.Invoke();
            hasTaken = true;
            FrontEnd.World.Instance.fPlayer.CreateItem();
        }

        void OnTriggerExit(Collider other)
        {
            ExecuteOnExit(other);
        }

        protected virtual void ExecuteOnExit(Collider other)
        {
            OnExit.Invoke();
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "InteractionTrigger", false);
        }

        void OnDrawGizmosSelected()
        {
            //need to inspect events and draw arrows to relevant gameObjects.
        }

    }
}
