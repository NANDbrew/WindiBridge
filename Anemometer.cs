using System;
using System.Collections;
using UnityEngine;

namespace WindiBridge
{
    [RequireComponent(typeof(Collider))]
    public class Anemometer : Weathervane
    {
        // magic multiplier to get chiplog knots. old value 1.94384f. (from the built-in "speedometer"/sailinfo mod. new value is emperical)
        const float knotsConversion = 1.865f;
        const float spinnerSpeed = 50f;

        public Transform needle;
        public Transform spinner;
        public float needleMult = 1f;
        public float smoothedWind { get; private set; }
        bool inInventory;
        public CollisionTracker colTracker;
        public ShipItem shipItem;

        public Transform tumbler;
        public int tumblerSides = 4;

        public void Start()
        {
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

            float wind = 0;
            if (GameState.wasInSettingsMenu || !GameState.playing || GameState.sleeping) return;
            if (!shipItem.held && colTracker && colTracker.collisions > 0)
            {
                smoothedWind = Mathf.Lerp(smoothedWind, wind, Time.deltaTime * (responseSpeed * 5));
                needle.localEulerAngles = new Vector3(0f, 0f, smoothedWind * needleMult);
                return;
            }
            if (shadowedBy)
            {
                wind = worldVelocity.magnitude * knotsConversion;
            }
            else if (!inInventory)
            {
                wind = (Wind.currentWind - worldVelocity).magnitude * knotsConversion;
            }

            smoothedWind = Mathf.Lerp(smoothedWind, wind, Time.deltaTime * responseSpeed);
            var angle = smoothedWind * needleMult;
            needle.localEulerAngles = new Vector3(0f, 0f, angle);

            spinner.Rotate(0f, Time.deltaTime * smoothedWind * spinnerSpeed, 0f);

            if (tumbler)
            {
                float num = 360 / tumblerSides;
                float newFace = Mathf.Min(num * Mathf.Floor(Mathf.Abs(angle / 360)), 360 - num);
                float speedMult = num * 2 / Mathf.Abs(tumbler.localEulerAngles.z - newFace);
                tumbler.localEulerAngles = new Vector3(0f, tumbler.localEulerAngles.y, Mathf.Lerp(tumbler.localEulerAngles.z, newFace, Time.deltaTime * Mathf.Pow(speedMult, 1.8f)));

            }
#if DEBUG
            shipItem.description = Math.Round(smoothedWind, 1).ToString() + "\ntarget angle: " + Mathf.Round(angle).ToString();
#endif

        }

    }

}
