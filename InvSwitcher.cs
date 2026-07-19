using UnityEngine;

namespace WindiBridge
{
    public class InvSwitcher : MonoBehaviour
    {
        public GameObject toSwitch;
        public ShipItem shipItem;
        public HangableItem hangableItem;
        public Anemometer anemometer;

        public virtual void Start()
        {
            shipItem = GetComponent<ShipItem>() ?? transform.parent.gameObject.GetComponent<ShipItem>();
            hangableItem = GetComponent<HangableItem>() ?? null;

            if (!shipItem.sold)
            {
                toSwitch?.SetActive(false);
            }
        }

        public virtual void OnEnterInventory()
        {
            anemometer?.OnEnterInventory();
        }
        public virtual void OnLeaveInventory()
        {
            anemometer?.OnLeaveInventory();
        }

        public virtual void OnDrop()
        {
            toSwitch?.SetActive(IsHanging());
        }
        public virtual void OnPickup()
        {
            toSwitch?.SetActive(true);
        }

        private bool IsHanging()
        {
            if (shipItem.GetCurrentInventorySlot() > -1)
            {
                return false;
            }
            if (hangableItem != null && hangableItem.IsHanging())
            {
                return true; 
            }
            if (shipItem.wallAttachment && shipItem.GetItemRigidbody() is ItemRigidbody irb && irb.attached)
            {
                return true;
            }
            return false;
        }
    }
}
