using HighlightPlus;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class HighlightEffectControllerAI : HighlightEffectControllerBase
    {
        [Header("References")]
        [SerializeField] AIHealth health;

        [Header("AI Highlight Effects")]
        [SerializeField] HighlightEffect attackEffect;
        [SerializeField] HighlightEffect lockedOnLowHealthEffect;
        [SerializeField] HighlightEffect lockedOnEffect;
        
        
        
        protected override void Start()
        {
            base.Start();
            // attackEffect.SetTarget(rootTarget);
            attackEffect.highlighted = true;
            lockedOnEffect.SetTarget(rootTarget);
            lockedOnLowHealthEffect.SetTarget(rootTarget);
        }

        public void TriggerAttackEffect() => attackEffect.HitFX();


        [Button("Toggle Outline Effect")]
        public void ToggleLockedOnEffect(bool isLocked)
        {
            if (isLocked)
            {
                if (health.IsLowHealth)
                    lockedOnLowHealthEffect.highlighted = true;
                else
                    lockedOnEffect.highlighted = true;
            }
            else
            {
                if (health.IsLowHealth)
                    lockedOnLowHealthEffect.highlighted = false;
                else
                    lockedOnEffect.highlighted = false;
            }
        }
    }
}