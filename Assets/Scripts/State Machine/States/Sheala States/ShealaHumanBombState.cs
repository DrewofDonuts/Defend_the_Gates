using System.Linq;
using UnityEngine;

namespace Etheral
{
    public class ShealaHumanBombState : EnemyBaseState
    {
        HumanBombController humanBombController;
        ShealaController shealaController;
        PhaseInfoSheala phaseInfoSheala;

        bool hasSpawnedHumanBomb;
        bool hasStartedAttack;
        bool hasEffectSpawned;
        int numberOfHumanBombs;


        public ShealaHumanBombState(EnemyStateMachine _stateMachine) : base(_stateMachine) { }

        public override void Enter()
        {
            shealaController = aiComponents.GetCustomController<ShealaController>();
            phaseInfoSheala = shealaController.GetPhaseInfo();
            humanBombController = shealaController.HumanBombController;
            
            characterAction = stateMachine.AIAttributes.SpecialAbility.FirstOrDefault(x => x.Name == "Human Bomb");

            shealaController.ResetHumanBombTimer();

            if (characterAction != null)
            {
                animationHandler.CrossFadeInFixedTime(characterAction.PreAnimation);
            }
            
            PlayEmote(characterAction, AudioType.attackEmote);

        }

        public override void Tick(float deltaTime)
        {
            var telegraphTime = animationHandler.GetNormalizedTime(characterAction.PreAnimation);
            var normalizedTime = animationHandler.GetNormalizedTime(characterAction.AnimationName);

            if (telegraphTime >= characterAction.TimeBeforeEffect && !hasEffectSpawned)
            {
                Object.Instantiate(characterAction.Effect, shealaController.transform.position + new Vector3(0, .1f, 0),
                    Quaternion.identity);
                hasEffectSpawned = true;
            }

            if (telegraphTime >= 1 && !hasStartedAttack)
            {
                animationHandler.CrossFadeInFixedTime(characterAction.AnimationName);
            }


            if (normalizedTime >= characterAction.TimesBeforeSpells[0] && !hasSpawnedHumanBomb)
            {
                humanBombController.SpawnHumanBomb(HandlePhase());
                hasSpawnedHumanBomb = true;
            }


            if (normalizedTime >= 1)
                enemyStateBlocks.CheckLocomotionStates();
        }

        public int HandlePhase()
        {
            if (phaseInfoSheala.phase == 1)
                return numberOfHumanBombs = 1;
            if (phaseInfoSheala.phase == 2)
                return numberOfHumanBombs = 2;
            if (phaseInfoSheala.phase == 3)
                return numberOfHumanBombs = 3;

            return default;
        }

        public override void Exit()
        {
            shealaController.ResetHumanBombTimer();
        }
    }
}