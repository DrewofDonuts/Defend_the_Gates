using System;
using Sirenix.OdinInspector;
using Etheral;
using UnityEngine;
using UnityEngine.UI;


namespace Etheral
{
    public class Cooldown : MonoBehaviour
    {
        [SerializeField] Sprite cooldownSprite;
        [field: SerializeField] public Image IconImage { get; private set; }
        [field: SerializeField] public Image FillImage { get; set; }

        [field: SerializeField] public float MaxFillAmount { get; set; }
        [field: SerializeField] public AudioClip CooldownReady { get; set; }
        [field: SerializeField] public AudioSource AudioSource { get; set; }


        float currentFill;
        bool playedAudio;
        bool shouldFade;
        float currentAlpha;
        float fadeTimer;
        Color currentColor;


        void Start()
        {
            // if(IconImage.sprite == null)
            //     IconImage.sprite = cooldownSprite;

            FillImage.fillMethod = Image.FillMethod.Radial360;
            FillImage.fillOrigin = (int)Image.Origin360.Top;
            FillImage.fillClockwise = false;
            // currentFill = MaxFillAmount;

            currentAlpha = IconImage.color.a;
            currentColor = IconImage.color;
        }

        void Update()
        {
            currentFill -= Time.deltaTime;

            FillImage.fillAmount = currentFill / MaxFillAmount;

            if (currentFill <= 0 && !shouldFade)
            {
                HandleEndCooldown();
                shouldFade = true;
            }

            if (shouldFade)
                HandleAlpha();
        }

        void HandleEndCooldown()
        {
            if (!playedAudio && CooldownReady != null)
                AudioProcessor.PlaySingleOneShot(AudioSource, CooldownReady, AudioType.cooldown);

            playedAudio = true;
        }

        void HandleAlpha()
        {
            fadeTimer += Time.deltaTime;
            float normalizedTimer = fadeTimer / 1f;
            normalizedTimer = Mathf.Clamp(normalizedTimer, 0, 1);

            currentColor.a = Mathf.Lerp(currentAlpha, 0, normalizedTimer);
            IconImage.color = currentColor;


            if (IconImage.color.a <= 0)
                Destroy(gameObject, 1);
        }


        public void SetFillAmount(float maxFillAmount)
        {
            MaxFillAmount = maxFillAmount;
            currentFill = maxFillAmount;
        }
    }
}