using Etheral;
using UnityEngine;
using AudioType = Etheral.AudioType;

public class PlayerProcessor : StateMachineProcessor<PlayerStateMachine>
{
    public override void TakeHit(IDamage iDamage, PlayerStateMachine stateMachine)
    {
        stateMachine.CharacterAudio.AudioSelector.WeaponHitAudioHandler(iDamage.AudioImpact);
        stateMachine.PlayerComponents.GetFeedbackHandler().HandleHapticInput(iDamage.FeedbackType);

        // if (Health.IsSturdy && CheckAngle(iDamage.Transform, 77, 20) &&
        //     !iDamage.IsShieldBreak || Health.IsImmuneToAllDamage) return;

        var health = stateMachine.Health;


        if (CheckIfKnockedBack(iDamage.KnockBackForce, health.CharacterAttributes.KnockBackDefense))
        {
            stateMachine.ForceReceiver.AddForce(iDamage.Direction);
        }


        if (CheckIfKnockedDown(iDamage.KnockDownForce, stateMachine.PlayerCharacterAttributes.KnockDownDefense, health.IsSturdy))
        {
            // stateMachine.Health.SetSturdy(true);
            stateMachine.SwitchState(new PlayerKnockedDownState(stateMachine));
        }
        else if (CheckIfImpact(stateMachine.canBeImpacted, stateMachine.Health.IsSturdy, health.CurrentDefense))
        {
            stateMachine.SwitchState(new PlayerImpactState(stateMachine));
            stateMachine.PlayerComponents.GetHighlightEffectController().TriggerTakeHitEffect();
            stateMachine.TriggerImpactTimer();
        }
        else if(health.CurrentDefense > 0)
            stateMachine.PlayerComponents.GetHighlightEffectController().TriggerDefenseHitEffect();
            
    }
    //
    // public override void TakeGroundHit(IDamage iDamage, PlayerStateMachine stateMachine)
    // {
    //     stateMachine.CharacterAudio.AudioSelector.WeaponHitAudioHandler(iDamage.AudioImpact);
    //     stateMachine.PlayerComponents.GetFeedbackHandler().HandleHapticInput(iDamage.FeedbackType);
    // }

    public override void HandleBlock(IDamage iDamage, PlayerStateMachine playerStateMachine)
    {
        playerStateMachine.PlayerComponents.GetFeedbackHandler().HandleHapticInput(iDamage.FeedbackType);

        if (iDamage.IsShieldBreak)
        {
            //TODO: SOLIDIFY WHERE BLOCK AUDIO IS DETERMINED - HERE OR IN WEAPON DAMAGE?
            playerStateMachine.CharacterAudio.PlayRandomOneShot(playerStateMachine.CharacterAudio.BlockSource,
                playerStateMachine.CharacterAudio.AudioLibrary.BlockImpact, AudioType.block);

            // playerStateMachine.SwitchState(new PlayerShieldBreakState(playerStateMachine));
            playerStateMachine.SwitchState(new PlayerKnockedDownState(playerStateMachine));

            playerStateMachine.ForceReceiver.AddForce(iDamage.Direction);
        }
        else if (!iDamage.IsShieldBreak)
        {
            playerStateMachine.SwitchState(new PlayerBlockState(playerStateMachine));
            playerStateMachine.CharacterAudio.PlayRandomOneShot(playerStateMachine.CharacterAudio.BlockSource,
                playerStateMachine.CharacterAudio.AudioLibrary.BlockImpact, AudioType.block);
            playerStateMachine.ForceReceiver.AddForce(iDamage.Direction / 2);
        }
    }

    public override void HandleDead(PlayerStateMachine stateMachine)
    {
        throw new System.NotImplementedException();
    }
}