using System.Collections;
using UnityEngine;

namespace WindiBridge
{
    public class AutoLevel : MonoBehaviour
    {
        ShipItem shipItem;
        bool locked;
        bool locking;

        private void Start()
        {
            shipItem = transform.parent.GetComponent<ShipItem>();
        }

        private void LateUpdate()
        {
            if (shipItem.held)
            {
                locked = false;
            }
            else if (!locking && !locked)
            {
                StartCoroutine(LockRoutine());
            }
            if (!locked)
            {
                if (shipItem.currentActualBoat != null)
                {
                    base.transform.up = shipItem.currentActualBoat.up;
                }
                else
                {
                    base.transform.up = Vector3.up;
                }
            }
        }

        public IEnumerator LockRoutine()
        {
            locking = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            locked = true;
            locking = false;
        }
    }
}
