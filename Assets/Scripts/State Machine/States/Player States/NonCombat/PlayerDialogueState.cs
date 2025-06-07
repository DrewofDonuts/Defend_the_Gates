using UnityEngine;

namespace Etheral
{
    public class PlayerDialogueState : PlayerBaseState
    {
        bool isDialogue;


        public PlayerDialogueState(PlayerStateMachine stateMachine, bool isDialogue) : base(stateMachine)
        {
            this.isDialogue = isDialogue;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime("Idle", 0.2f);
            stateMachine.Animator.SetBool("IsMoving", false);
        }

        public override void Tick(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);

            // if (!DialogueManager.isConversationActive && isDialogue)
            //     ReturnToLocomotion();
            //
            // if (!stateMachine.PlayerComponentHandler.PlayerDialogueSystemController.QuestJournal.questJournalUI.isVisible && !isDialogue)
            //     ReturnToLocomotion();
        }

        public override void Exit() { }
    }
}