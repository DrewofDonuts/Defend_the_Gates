using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class FadeOutAndIn : MonoBehaviour
    {
        [field: SerializeField] public Image FadeOutImage { get; private set; }
        [field: SerializeField] public float FadeTime { get; private set; }
        [field: SerializeField] public float FadeInTime { get; private set; }
        [field: SerializeField] public float FadeDelay { get; private set; }
        [field: SerializeField] public bool FadeOutOnStart { get; private set; }


        void Start()
        {
            if (FadeOutOnStart)
            {
                FadeOutImage.gameObject.SetActive(true);
                FadeInImageRoutine();
            }
        }

        public void FadeOutImageRoutine()
        {
            FadeOutImage.gameObject.SetActive(true);
            StartCoroutine(FadeOutImageCoroutine());
        }

        IEnumerator FadeOutImageCoroutine()
        {
            yield return new WaitForSeconds(FadeDelay);

            float elapsedTime = 0;
            Color color = FadeOutImage.color;

            while (elapsedTime < FadeTime)
            {
                color.a = Mathf.Lerp(0, 1, elapsedTime / FadeTime);
                FadeOutImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 1;
            FadeOutImage.color = color;
            
            FadeOutImage.gameObject.SetActive(false);

        }

        public void FadeInImageRoutine()
        {
            FadeOutImage.gameObject.SetActive(true);
            StartCoroutine(FadeInImageCoroutine());
        }

        IEnumerator FadeInImageCoroutine()
        {
            yield return new WaitForSeconds(FadeDelay);

            float elapsedTime = 0;
            Color color = FadeOutImage.color;

            while (elapsedTime < FadeInTime)
            {
                color.a = Mathf.Lerp(1, 0, elapsedTime / FadeInTime);
                FadeOutImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 0;
            FadeOutImage.color = color;
            
            FadeOutImage.gameObject.SetActive(false);

        }

        public void FadeOutAndInImageRoutine()
        {
            FadeOutImage.gameObject.SetActive(true);
            StartCoroutine(FadeOutAndInImageCoroutine());
        }
        
        IEnumerator FadeOutAndInImageCoroutine()
        {
            float elapsedTime = 0;
            Color color = FadeOutImage.color;

            // Fade Out
            while (elapsedTime < FadeTime)
            {
                color.a = Mathf.Lerp(0, 1, elapsedTime / FadeTime);
                FadeOutImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 1;
            FadeOutImage.color = color;
            
            yield return new WaitForSeconds(FadeDelay);

            elapsedTime = 0;
            color = FadeOutImage.color;

            // Fade In
            while (elapsedTime < FadeTime)
            {
                color.a = Mathf.Lerp(1, 0, elapsedTime / FadeTime);
                FadeOutImage.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            color.a = 0;
            FadeOutImage.color = color;
            
            FadeOutImage.gameObject.SetActive(false);

        }

        
        
    }

}