using System;
using System.Collections;
using UnityEngine;

namespace WindiBridge
{
    [RequireComponent(typeof(Collider))]
    public class Anemometer : Weathervane
    {
        public Transform needle;
        public Transform spinner;
        public float spinnerMult = 50f;
        public float needleMult = 7.2f;
        public float responseMult = 0.5f;
        public float value = 0f;
        float lastValue = 0f;
        float startAngle = 0;
        bool inInventory;
        public CollisionTracker colTracker;
        public ShipItem shipItem;

        public void Start()
        {
            startAngle = needle.localEulerAngles.z;
            if (colTracker != null) StartCoroutine(InitColTracker());
        }

        public void OnEnterInventory()
        {
            inInventory = true;
        }
        public void OnLeaveInventory()
        {
            inInventory = false;
        }

        private IEnumerator InitColTracker()
        {
            yield return new WaitUntil(() => shipItem.itemRigidbodyC != null);
            colTracker.transform.SetParent(shipItem.itemRigidbodyC.transform, false);
        }

        public override void LateUpdate()
        {
            if (GameState.wasInSettingsMenu || !GameState.playing || GameState.sleeping) return;
            if (!shipItem.held && colTracker && colTracker.collisions > 0)
            {
                value = 0;
                lastValue = Mathf.Lerp(lastValue, value, Time.deltaTime * (responseMult * 5));
                var tangle = startAngle + lastValue * needleMult;
                needle.localEulerAngles = new Vector3(0f, 0f, tangle);
                return;
            }
            if (shadowedBy)
            {
                value = worldVelocity.magnitude;
            }
            else if (!inInventory)
            {
                value = (Wind.currentWind - worldVelocity).magnitude;
            }
            else value = 0;

            lastValue = Mathf.Lerp(lastValue, value, Time.deltaTime * responseMult);
            var angle = startAngle + lastValue * needleMult;
            needle.localEulerAngles = new Vector3(0f, 0f, angle);

            spinner.Rotate(0f, Time.deltaTime * lastValue * spinnerMult, 0f);

            shipItem.description = Math.Round(value, 1).ToString() + "\ntarget angle: " + Mathf.Round(angle).ToString();
        }

    }

}
