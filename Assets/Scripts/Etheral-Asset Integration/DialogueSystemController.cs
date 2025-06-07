using UnityEngine;

namespace Etheral
{
    public class DialogueSystemController : MonoBehaviour
    {
        [SerializeField] CharacterAudio characterAudio;
        // [SerializeField] StateSelector stateSelector;
        [SerializeField] AnimationHandler animationHandler;

        AnimatorStateInfo originalState;
        string nextState;
        Animator animator;
        bool isPlayingDialogue;
        float normalizedTime;

        // Start is called before the first frame update
        void Start()
        {
            if (characterAudio == null)
                characterAudio = GetComponent<CharacterAudio>();
        }

        void Update()
        {
            if (isPlayingDialogue)
                normalizedTime = animationHandler.GetNormalizedTime(nextState);


            if (normalizedTime >= 1)
                StopAnimationDialogue();
        }

        //Called by Unity Event
        public void EnterDialogueState()
        {
            Debug.Log("Removed EnterDialogue");

            // stateSelector.EnterDialogueState();
        }

        //Called by Unity Event
        public void StartDialogueAudio(int index)
        {
            characterAudio.PlayDialogueClip(index);
            isPlayingDialogue = true;
        }

        //Called by Unity Event
        public void StartAnimationDialogue(string animationName)
        {
            var randomNumber = UnityEngine.Random.Range(1, 5);
            // stateSelector.EnterDialogueStateWithAnimation(animationName + randomNumber);
        }

        public void StopAnimationDialogue()
        {
            animationHandler.CrossFadeInFixedTime(originalState.fullPathHash, 0.2f);

            // stateSelector.EnterIdleState();
            isPlayingDialogue = false;
            normalizedTime = 0;
        }
    }
}