using System.Collections;
using UnityEngine;

namespace WindiBridge
{
    [RequireComponent(typeof(Cloth))]
    public class WindFlag : Weathervane
    {
        private Cloth cloth;
        public GameObject rolledCloth;

        public override void Awake()
        {
            base.Awake();
            cloth = GetComponent<Cloth>();
            //rolledCloth = base.transform.parent.Find("rolled_cloth").gameObject;
            rolledCloth.SetActive(false);
            multiplier = 1f;
        }

        public override void LateUpdate()
        {
            if (!shadowedBy)
            {
                cloth.externalAcceleration = (Wind.currentWind - worldVelocity) * multiplier;
            }
            else
            {
                cloth.externalAcceleration = -worldVelocity * multiplier;
            }
        }
        public void OnEnable()
        {
            rolledCloth?.SetActive(false);
            this.StartCoroutine(ResetCloth());
        }
        public void OnDisable()
        {
            rolledCloth?.SetActive(true);
        }

        private IEnumerator ResetCloth()
        {
            cloth.enabled = false;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            cloth.enabled = true;
        }


    }
}
