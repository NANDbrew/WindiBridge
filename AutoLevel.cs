using System.Collections;
using UnityEngine;

namespace WindiBridge
{
    public class AutoLevel : MonoBehaviour
    {
        ShipItem shipItem;
        Quaternion shopRot;
        bool locked;
        bool locking;

        private void Start()
        {
            shipItem = transform.parent.GetComponent<ShipItem>();
            shopRot = base.transform.rotation;
        }

        private void LateUpdate()
        {
            if (shipItem.held)
            {
                locked = false;
            }
            else if (!locking && !locked)
            {
                if (!shipItem.sold)
                {
                    StartCoroutine(ShopLockRoutine());
                }
                else
                {
                    StartCoroutine(LockRoutine());
                }
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
        public IEnumerator ShopLockRoutine()
        {
            locking = true;
            Quaternion startRot = base.transform.rotation;
            for (float t = 0f; t < 1f; t += Time.deltaTime * 2f)
            {
                base.transform.rotation = Quaternion.Lerp(startRot, shopRot, t);
                yield return new WaitForEndOfFrame();
            }
            locked = true;
            locking = false;
        }
    }
}
