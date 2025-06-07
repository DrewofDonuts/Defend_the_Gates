using UnityEngine;

namespace Etheral
{
#if UNITY_EDITOR
    [CreateAssetMenu(fileName = "AnimatorEditorHolder",
        menuName = "Etheral/Data Objects/Animation/AnimatorEditorHolder")]
    public class AnimatorEditorHolder : ScriptableObject
    {
        public AnimatorEditor animatorEditor;
    }
#endif
}