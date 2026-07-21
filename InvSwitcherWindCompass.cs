using System.Collections;
using UnityEngine;

namespace WindiBridge
{
    public class InvSwitcherWindCompass : InvSwitcher
    {
        public Weathervane indicator;
        Quaternion indicatorFoldedRot;
        Quaternion indicatorStartingRot;
        bool rotating;
        private bool cancel;
        public void Awake()
        {
            indicator = indicator ?? this.GetComponentInChildren<WindiBridge.Weathervane>();
            indicatorStartingRot = indicator.transform.localRotation;
            indicatorFoldedRot = indicatorStartingRot * Quaternion.Euler(Vector3.forward * 89f);
        }

        public override void OnEnterInventory()
        {
            if (rotating) cancel = true;
            StartCoroutine(ToggleIndicator(true));
        }
        public override void OnLeaveInventory()
        {
            if (rotating) cancel = true;
            StartCoroutine(ToggleIndicator(false));
        }

        public IEnumerator ToggleIndicator(bool fold)
        {
            if (cancel)
            {
                yield return new WaitForEndOfFrame();
                cancel = false;
            }
            indicator.enabled = false;
            rotating = true;

            float target = fold ? 85f : 0f;
            Vector3 angles = indicator.transform.localEulerAngles;
            float start = angles.z;

            for (float t = 0f; t < 1f; t += Time.deltaTime * 5f)
            {
                if (cancel) break;
                angles.z = Mathf.Lerp(start, target, t);
                Debug.Log("angle = " + angles.z);
                indicator.transform.localEulerAngles = angles;
                yield return new WaitForEndOfFrame();
            }
            indicator.enabled = !fold;
            rotating = false;
            Debug.Log("rotated.");
        }

    }
}
