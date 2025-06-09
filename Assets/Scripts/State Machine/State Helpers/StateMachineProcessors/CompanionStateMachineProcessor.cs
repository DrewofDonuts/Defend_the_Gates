using System.Collections;
using System.Collections.Generic;
using Etheral;
using UnityEngine;

public class CompanionStateMachineProcessor : StateMachineProcessor<CompanionStateMachine>
{
    public override void TakeHit(IDamage iDamage, CompanionStateMachine stateMachine)
    {
        stateMachine.CharacterAudio.AudioSelector.WeaponHitAudioHandler(iDamage.AudioImpact);
        var health = stateMachine.Health;

        if (CheckIfKnockedDown(iDamage.KnockDownForce, stateMachine.AIAttributes.KnockDownDefense, health.IsSturdy))
        {
            stateMachine.SwitchState(new NpcKnockedDownState(stateMachine));
        }
        else if (!stateMachine.Health.IsSturdy)
        {
            stateMachine.SwitchState(new NpcImpactState(stateMachine));
        }
    }

    public override void HandleBlock(IDamage iDamage, CompanionStateMachine stateMachine)
    {
    }

    public override void HandleDead(CompanionStateMachine stateMachine)
    {
        stateMachine.SwitchState(new NpcDownedState(stateMachine));
    }
}