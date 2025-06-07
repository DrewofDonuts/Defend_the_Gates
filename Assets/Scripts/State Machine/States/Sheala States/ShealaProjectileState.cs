using System;
using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class ShealaProjectileState : EnemyBaseState
    {
        ShealaController shealaController;
        PhaseInfoSheala phaseInfoSheala;
        int numberOfProjectiles;

        public ShealaProjectileState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            shealaController = aiComponents.GetCustomController<ShealaController>();
            phaseInfoSheala = shealaController.GetPhaseInfo();

            characterAction =
                stateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Blood Dagger");

            // characterAction = stateMachine.AIAttributes.SpecialAbility[0];

            aiComponents.GetCustomController<ShealaController>().ResetProjectileTimer();


            if (characterAction != null)
            {
                actionProcessor.SetupActionProcessorForThisAction(stateMachine, characterAction);
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName);
            }
            else
                Debug.LogError("No Blood Projectile Ability Found");
            
            PlayEmote(characterAction, AudioType.attackEmote);

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateTowardsTargetSmooth(120f);
            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            
            actionProcessor.CastSpells(normalizedTime);


            if (normalizedTime >= 1f)
                enemyStateBlocks.CheckLocomotionStates();
        }

        public override void Exit()
        {
            actionProcessor.ClearAll();
        }


        public int HandlePhase()
        {
            if (phaseInfoSheala.phase == 1)
                return numberOfProjectiles = 1;
            if (phaseInfoSheala.phase == 2)
                return numberOfProjectiles = 2;
            if (phaseInfoSheala.phase == 3)
                return numberOfProjectiles = 3;

            return default;
        }
    }
}