using System.Collections.Generic;
using UnityEngine;

namespace WindiBridge
{
    [RequireComponent(typeof(Collider))]
    public class CollisionTracker : MonoBehaviour
    {
        public int collisions;
        private List<Collider> colliders = new List<Collider>();
        public GameObject item;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger && other.gameObject != item && other.gameObject != transform.parent && !other.CompareTag("Boat"))
            {
                colliders.Add(other);
                collisions++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (colliders.Contains(other))
            {
                colliders.Remove(other);
                collisions--;
            }
        }
    }
}
