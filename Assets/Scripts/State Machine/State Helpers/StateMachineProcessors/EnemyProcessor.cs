using Etheral;
using UnityEngine;

public class EnemyProcessor : StateMachineProcessor<EnemyStateMachine>
{
    public override void TakeHit(IDamage iDamage, EnemyStateMachine stateMachine)
    {
        stateMachine.CharacterAudio.AudioSelector.WeaponHitAudioHandler(iDamage.AudioImpact);
        var health = stateMachine.Health;

        if (CheckIfKnockedDown(iDamage.KnockDownForce, stateMachine.AIAttributes.KnockDownDefense, health.IsSturdy) &&
            health.CurrentDefense <= 0)
        {
            stateMachine.SwitchState(new EnemyKnockDownState(stateMachine));
        }
        else if (CheckIfImpact(iDamage.DoesImpact, stateMachine.Health.IsSturdy, health.CurrentDefense))
        {
            stateMachine.TriggerImpactTimer();

            if (stateMachine.canPreventImpacts && stateMachine.impactCounter >= stateMachine.impactCounterMax)
                return;
   
            stateMachine.SwitchState(new EnemyImpactState(stateMachine, iDamage));
        }

        if (health.CurrentDefense > 0)
            stateMachine.GetAIComponents().GetHighlightEffectController().TriggerDefenseHitEffect();
        else
            stateMachine.GetAIComponents().GetHighlightEffectController().TriggerTakeHitEffect();


        // if (CheckIfKnockedBack(iDamage.KnockBackForce, health.CharacterAttributes.KnockBackDefense))
        //     stateMachine.ForceReceiver.AddForce(iDamage.Direction);
    }


    public override void HandleBlock(IDamage iDamage, EnemyStateMachine stateMachine)
    {
        var health = stateMachine.Health;

        if (iDamage.IsShieldBreak)
        {
            stateMachine.SwitchState(new EnemyShieldBreakState(stateMachine));
        }

        if (CheckIfKnockedDown(iDamage.KnockDownForce, stateMachine.AIAttributes.KnockDownDefense, health.IsSturdy))
            stateMachine.SwitchState(new EnemyKnockDownState(stateMachine));
        else if (!iDamage.IsShieldBreak)
        {
            stateMachine.TriggerImpactTimer();
            stateMachine.SwitchState(new EnemyBlockHitState(stateMachine));
        }

        if (CheckIfKnockedBack(iDamage.KnockBackForce, health.CharacterAttributes.KnockBackDefense))
            stateMachine.ForceReceiver.AddForce(iDamage.Direction / 2f);
    }

    public override void HandleDead(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.Health.IsExecuted)
            stateMachine.SwitchState(new EnemyDeadState(stateMachine));
    }
}