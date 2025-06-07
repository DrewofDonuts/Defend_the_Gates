using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class DripController : MonoBehaviour
    {
        [SerializeField] Drip dripPrefab;
        [SerializeField] Renderer _renderer;

        [SerializeField] float dripRate = 4f;
        [SerializeField] bool testOverride;
        
        
        public bool shouldDrip = true;
        Plane[] planes;

        void Start()
        {
            planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            
            StartCoroutine(DripTiming());
        }

        IEnumerator DripTiming()
        {
            while (shouldDrip && IsOnScreen())
            {
                yield return new WaitForSeconds(dripRate);
                Drip drip = Instantiate(dripPrefab, transform.position, dripPrefab.transform.rotation);
                drip.LoadNextSplash();
            }
        }
        
        protected bool IsOnScreen()
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);


            if (testOverride)
                return true;
            
            return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 &&
                   screenPoint.y < 1;
        }
    }
}