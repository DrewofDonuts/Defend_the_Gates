using HighlightPlus;
using UnityEngine;

namespace Etheral
{
    public class HighlightEffectControllerBase : MonoBehaviour
    {
        [Header("Highlight Effects")]
        [SerializeField] protected HighlightEffect hitEffect;
        [SerializeField] protected HighlightEffect defenseHitEffect;
        [SerializeField] protected HighlightEffect outlineEffect;

        [Header("Targets")]
        [SerializeField] protected Transform rootTarget;
        
        protected virtual void  Start()
        {
            outlineEffect.SetTarget(rootTarget);
            hitEffect.SetTarget(rootTarget);
            defenseHitEffect.SetTarget(rootTarget);

            outlineEffect.highlighted = true;
            hitEffect.highlighted = true;
            defenseHitEffect.highlighted = true;
        }

        public void TriggerTakeHitEffect() => hitEffect.HitFX();
        public void TriggerDefenseHitEffect() => defenseHitEffect.HitFX();
    }
}