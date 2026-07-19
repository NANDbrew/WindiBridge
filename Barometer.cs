using System;

using UnityEngine;

namespace WindiBridge
{
    internal class Barometer : MonoBehaviour
    {
        ShipItem shipItem;
        HangableItem hangable;
        public Transform needle;
        public float minAngle = 10f;
        public float maxAngle = 170f;
        static float maxDist = 34000f;
        public float angleStep = 0.2f;
        float errorTime = 0f;
        float currentError = 0f;
        float rangeMult = 1f;

        public void Start()
        {
            shipItem = GetComponent<ShipItem>();
            hangable = GetComponent<HangableItem>();

            UpdateAngle(true);
        }

        private void Update()
        {
            //Debug.Log("angle step = " + angleStep);
            maxDist = RegionBlender.instance.blendedRegion.stormRange * rangeMult;
            UpdateAngle(false);
            if (errorTime <= 0f)
            {
                currentError = GetError();
                errorTime = UnityEngine.Random.Range(1, 5);
            }
            errorTime -= Time.deltaTime;
        }

        private void UpdateAngle(bool instant)
        {
            float normalizedDist = Mathf.InverseLerp(maxDist, 0f, WeatherStorms.currentStormDistance);
            float angle = Mathf.Lerp(minAngle, maxAngle, Mathf.Clamp01(normalizedDist + currentError));
            float lerpedAngle = Mathf.Lerp(needle.localEulerAngles.z, angle, Time.deltaTime * angleStep);

            //float derp = instant ? angle : Mathf.MoveTowardsAngle(needle.localEulerAngles.z, angle, angleStep);

            needle.localEulerAngles = new Vector3(0f, 0f, lerpedAngle);
            
            shipItem.description = Math.Round(normalizedDist, 3).ToString() + "\n" + Math.Round(Mathf.Lerp(31f, 28f, normalizedDist), 2).ToString();

        }

        private float GetError()
        {
            if (hangable != null)
            {
                if (!hangable.IsHanging())
                {
                    //TODO: if not hanging, add random inaccuracies by verticality
                    var tilt = Vector3.Angle(transform.up.normalized, Vector3.up);
                    if (tilt > 3f)
                    {
                        float error = tilt / 360;
                        return UnityEngine.Random.Range(-error, error);
                    }
                }
            }
            return 0f;
        }

    }
}
