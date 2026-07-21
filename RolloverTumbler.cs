using UnityEngine;

namespace WindiBridge
{
    internal class RolloverTumbler : MonoBehaviour
    {
        public int sideCount;
        public Transform sourceNeedle;

        public void Update()
        {
            float num = 360 / sideCount;
            float newFace = Mathf.Min(num * Mathf.Floor(Mathf.Abs(sourceNeedle.localEulerAngles.z / 360)), 360 - num);
            float speedMult = num * 2 / Mathf.Abs(transform.localEulerAngles.z - newFace);
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, Mathf.Lerp(transform.localEulerAngles.z, newFace, Time.deltaTime * Mathf.Pow(speedMult, 1.8f)));
            
        }
    }
}
