using UnityEngine;

namespace WindiBridge
{
    public class RolloverTumbler : MonoBehaviour
    {
        public int sideCount;
        public Transform sourceNeedle;
        float lastAngle;
        int currFace;

        public void FixedUpdate()
        {
            var currAngle = sourceNeedle.localEulerAngles.z;
            if (currAngle > lastAngle + 300)
            {
                currFace++;
            }
            else if (currAngle < lastAngle - 300)
            {
                currFace--;
            }
            //if (currFace < 0) currFace = 0;

            lastAngle = currAngle;
        }

        public void Update()
        {
            float num = (360 / sideCount) * currFace;
            //float newFace = Mathf.Min(num * Mathf.Floor(Mathf.Abs(sourceNeedle.localEulerAngles.z / 360)), 360 - num);
            float speedMult = num * 2 / Mathf.Abs(transform.localEulerAngles.z - num);
            //transform.localEulerAngles = new Vector3(0f, 90, Mathf.Lerp(transform.localEulerAngles.z, num, Time.deltaTime * Mathf.Pow(speedMult, 1.5f)));
            transform.localEulerAngles = new Vector3(0f, 90, num);


        }
    }
}
