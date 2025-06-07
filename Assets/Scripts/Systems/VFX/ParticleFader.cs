using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    public class ParticleFader : MonoBehaviour
    {
        [SerializeField] ParticleSystem[] particleSystems;
        [SerializeField] MeshRenderer[] meshRenderers;
        [SerializeField] float fadeSpeed = 3f;
        [FormerlySerializedAs("timeToFade")] [SerializeField]
        float timeBeforeFading = 3f;

        [ReadOnly]
        public float timer;
        [ReadOnly]
        public bool isFading;
        [ReadOnly]
        public bool stopFading;

         Material[] instanceMaterials;

        // Start is called before the first frame update
        void Awake()
        {
            // Create instance materials for each renderer
            instanceMaterials = new Material[meshRenderers.Length];
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                instanceMaterials[i] = new Material(meshRenderers[i].sharedMaterial);
                meshRenderers[i].material = instanceMaterials[i];
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (stopFading) return;
            timer += Time.deltaTime;

            if (timer >= timeBeforeFading && !isFading)
            {
                StartCoroutine(FadeOut());
                isFading = true;
            }
        }

        IEnumerator FadeOut()
        {
            // Fade mesh renderers
            foreach (var meshRenderer in meshRenderers)
            {
                Color color = meshRenderer.material.color;
                while (color.a > 0)
                {
                    color.a -= Time.deltaTime * fadeSpeed;
                    meshRenderer.material.color = color;
                    yield return null;
                }
            }

            foreach (var meshRenderer in meshRenderers)
            {
                Color color = meshRenderer.material.color;
                while (color.a > 0)
                {
                    color.a -= Time.deltaTime * fadeSpeed;
                    meshRenderer.material.color = color;
                    yield return null;
                }
            }

            foreach (var partSystem in particleSystems)
            {
                var main = partSystem.main;

                // var currentGradient = main.startColor;
                // var minColor = currentGradient.colorMin;
                // var maxColor = currentGradient.colorMax;
                //
                // while (minColor.a > 0)
                // {
                //     minColor.a -= Time.deltaTime * fadeSpeed;
                //     maxColor.a -= Time.deltaTime * fadeSpeed;
                //     main.startColor = new ParticleSystem.MinMaxGradient(minColor, maxColor);
                //     yield return null;
                // }
                //
                // while (maxColor.a > 0)
                // {
                //     maxColor.a -= Time.deltaTime * fadeSpeed;
                //     main.startColor = new ParticleSystem.MinMaxGradient(minColor, maxColor);
                //     yield return null;
                // }

                while (main.startColor.color.a > 0)
                {
                    var color = main.startColor.color;
                    color.a -= Time.deltaTime * fadeSpeed;
                    main.startColor = color;
                    yield return null;
                }
            }
        }

#if UNITY_EDITOR
        [Button("Get Particle Systems")]
        public void GetParticleSystems()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
#endif
        public void StopFading()
        {
            StopAllCoroutines();
            isFading = false;
            stopFading = true;
            timer = 0;

            foreach (var partSystem in particleSystems)
            {
                var main = partSystem.main;
                var color = main.startColor.color;
                color.a = 1;
                main.startColor = color;
            }
        }
    }
}