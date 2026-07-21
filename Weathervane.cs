using UnityEngine;

namespace WindiBridge
{
    [RequireComponent(typeof(Collider))]
    public class Weathervane : MonoBehaviour
    {
        private Vector3 previousPosition;
        public Vector3 worldVelocity;
        public Transform shadowedBy;
        public float responseSpeed = 0.1f;
        public bool pivotLocal = true;

        public virtual void Awake()
        {
            base.GetComponent<Collider>().isTrigger = true;
        }
        public void Update()
        {
            if (GameState.justStarted || GameState.currentlyLoading)
            {
                previousPosition = transform.position;
            }
            else if (shadowedBy != null)
            {
                Vector3 localPos = transform.position - shadowedBy.position;
                worldVelocity = (localPos - previousPosition) / Time.deltaTime;
                previousPosition = localPos;
            }
            else
            {
                var newVelocity = (transform.position - previousPosition) / Time.deltaTime;
                //worldVelocity = newVelocity.magnitude < 50 ? newVelocity : worldVelocity;
                if (newVelocity.magnitude < 500)
                {
                    worldVelocity = newVelocity;
                }

                previousPosition = transform.position;
            }
        }
        public virtual void LateUpdate()
        {
            if (GameState.wasInSettingsMenu || !GameState.playing || GameState.sleeping) return;
            if (shadowedBy)
            {
                base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(-worldVelocity, Vector3.up), Time.deltaTime * worldVelocity.magnitude * responseSpeed);
            }
            else
            {
                base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(Wind.currentWind - worldVelocity, base.transform.parent.up), Time.deltaTime * (Wind.currentWind.magnitude - worldVelocity.magnitude) * responseSpeed);
            }

            if (pivotLocal) transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            else transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<InteriorEffectsTrigger>() != null)
            {
                shadowedBy = other.transform;
                previousPosition = transform.position - shadowedBy.position;
                //Debug.Log("flag entered interior");
            }
        }
        public virtual void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<InteriorEffectsTrigger>() != null)
            {
                shadowedBy = null;
                previousPosition = transform.position;
                //Debug.Log("flag exited interior");
            }
        }

    }

}
