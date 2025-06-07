using UnityEngine;

namespace Etheral
{
    public abstract class StateMachineProcessor<T> where T : StateMachine
    {
        protected float impactTimer;

        public bool CheckIfKnockedDown(float knockDownForce, float knockDownDefense, bool isSturdy)
        {
            //if current defense is less than 2% of knockdown defense, then return true
            
            return (knockDownForce > knockDownDefense) && !isSturdy;
            
            // return true;
        }

        public bool CheckIfImpact(bool canImpact, bool isSturdy, float currentProtection)
        {
            return canImpact && !isSturdy && currentProtection <= 0;
        }
        
        public bool CheckIfImpact(bool canImpact, bool isSturdy)
        {
            return canImpact && !isSturdy;
        }

        public bool CheckIfKnockedBack(float knockBackForce, float knockBackDefense)
        {
            return Mathf.Abs(knockBackForce) > Mathf.Abs(knockBackDefense);
        }

        public abstract void TakeHit(IDamage iDamage, T stateMachine);
        public abstract void HandleBlock(IDamage iDamage, T stateMachine);
        public abstract void HandleDead(T stateMachine);
    }
}