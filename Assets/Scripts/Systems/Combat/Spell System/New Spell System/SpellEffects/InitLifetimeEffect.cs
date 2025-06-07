using UnityEngine;

namespace Etheral
{
    // This effect is used to destroy the spell object after a certain amount of time
    //This enables us to allow other effects be active for other durations
    public class InitLifetimeEffect : BaseEffect
    {
        public float globalLifetime = 5f;
        
        public override void Initialize(SpellObject _spellObject, ICastSpell iCastSpell)
        {
            base.Initialize(_spellObject, iCastSpell);
            Object.Destroy(spellObject.gameObject, globalLifetime);
        }
        
    }
} 