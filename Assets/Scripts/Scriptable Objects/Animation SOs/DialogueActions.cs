using System;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "DialogueActions", menuName = "Etheral/Data Objects/Animation/Dialogue Actions", order = -1)]
    [Obsolete("This was a dumb idea. To Remove")]
    public class DialogueActions : ScriptableObject
    {
        public AnimationData[] happyDialogue;
        public AnimationData[] neutralDialogue;
        public AnimationData[] angryDialogue;
        public AnimationData[] sadDialogue;
        public AnimationData[] scaredDialogue;
        public AnimationData[] surprisedDialogue;
        }
}