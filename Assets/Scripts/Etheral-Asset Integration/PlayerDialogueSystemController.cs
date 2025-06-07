using PixelCrushers.DialogueSystem;
using PixelCrushers.QuestMachine.Wrappers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class PlayerDialogueSystemController : DialogueSystemController
    {
        // [field: SerializeField] public PlayerStateMachine StateMachine { get; private set; }
        [SerializeField] InputObject inputObject;
        [field: SerializeField] public QuestJournal QuestJournal { get; private set; }

        // [field: SerializeField] public SelectFirstButton SelectFirstButton { get; private set; }


        [ReadOnly]
        public DialogueSystemTrigger NPC;

        bool isInConversation;


        void OnEnable()
        {
            inputObject.DialogueEvent += StartDialogueWithNPC;
            inputObject.JournalEvent += ToggleJournal;
        }

        void ToggleJournal()
        {
            QuestJournal.ToggleJournalUI();

            if (QuestJournal.questJournalUI.isVisible)
            {
                // StateMachine.SwitchState(new PlayerDialogueState(StateMachine, false));

                EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.UIState);
            }
            else
            {
                // StateMachine.SwitchState(new PlayerOffensiveState(StateMachine));
                EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.MovementState);
            }
        }

        void OnDisable()
        {
            inputObject.DialogueEvent -= StartDialogueWithNPC;
            inputObject.JournalEvent -= ToggleJournal;
        }


        void StartDialogueWithNPC()
        {
            if (NPC != null && !DialogueManager.isConversationActive)
            {
                NPC.OnUse();
                if (!DialogueManager.isConversationActive) return;
                Debug.Log("Starting conversation with NPC");

                // StateMachine.SwitchState(new PlayerDialogueState(StateMachine, true));

                EventBusPlayerController.ChangePlayerState(this, PlayerControlTypes.UIState);
            }
        }

        void EndDialogueWithNPC()
        {
            if (NPC == null) return;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DialogueSystemTrigger npc))
            {
                NPC = npc;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (NPC != null)
                NPC = null;
        }
    }
}