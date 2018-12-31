using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FrontEnd;
using Gamekit3D.Network;
using Common;
namespace Gamekit3D
{
    [RequireComponent(typeof(Collider))]
    public class InteractOnTrigger : MonoBehaviour
    {
        public LayerMask layers;
        public UnityEvent OnEnter, OnExit;
        new Collider collider;
        public InventoryController.InventoryChecker[] inventoryChecks;
        private bool used = false;
        object used_lock = new object();
        void Reset()
        {
            layers = LayerMask.NameToLayer("Everything");
            collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (0 != (layers.value & 1 << other.gameObject.layer))
            {
                ExecuteOnEnter(other);
                SendTrigerMessage();
            }
        }

        protected virtual void ExecuteOnEnter(Collider other)
        {
            OnEnter.Invoke();
            for (var i = 0; i < inventoryChecks.Length; i++)
            {
                inventoryChecks[i].CheckInventory(other.GetComponentInChildren<InventoryController>());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (0 != (layers.value & 1 << other.gameObject.layer))
            {
                ExecuteOnExit(other);
            }
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

        public void SendTrigerMessage()
        {
            lock (used_lock)
            {
                if (used)
                    return;
                used = true;
            }
            CTrigerOnEnter msg = new CTrigerOnEnter();
            string name = gameObject.name;
            if (name.Contains("PressurePad"))
            {
                msg.pressurePad = new PressurePad(false, 0, name);
            }
            if (name.Contains("Switch"))
            {
                msg.switchCrystal = new SwitchCrystal(false, 0, name);
            }
            Client.Instance.Send(msg);
        }

    }
}
