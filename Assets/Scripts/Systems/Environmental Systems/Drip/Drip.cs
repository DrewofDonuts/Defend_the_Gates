using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Drip : MonoBehaviour
    {
        [SerializeField] GameObject[] splashObjects;
        [SerializeField] LayerMask layersToHit;

        [SerializeField] Collider _collider;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] float fadeTime = 4f;

        WaitForSeconds waitTime = new(10f);
        int splashIndex;


       public void LoadNextSplash()
        {
            splashIndex = Random.Range(0, splashObjects.Length);
        }


        void OnTriggerEnter(Collider other)
        {
            if (layersToHit == (layersToHit | (1 << other.gameObject.layer)))
            {
                Vector3 collisionPoint = other.ClosestPointOnBounds(transform.position);
                collisionPoint.y += 0.1f;
              
                
                GameObject splash = Instantiate(splashObjects[splashIndex], collisionPoint,
                    splashObjects[splashIndex].transform.rotation);
                splash.transform.SetParent(other.transform);
                GetMaterialToFad(splash);
            }
        }


        void GetMaterialToFad(GameObject splash)
        {
            Renderer _renderer = splash.GetComponent<Renderer>();
            var materialInstance = _renderer.material;
            materialInstance.color = _renderer.material.color;
            StartCoroutine(FadeOutOverTime(materialInstance));
        }

        IEnumerator FadeOutOverTime(Material materialInstance)
        {
            yield return waitTime;

            float elapsedTime = 0f;
            Color startColor = materialInstance.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            while (elapsedTime < fadeTime)
            {
                materialInstance.color = Color.Lerp(startColor, endColor, (elapsedTime / fadeTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}