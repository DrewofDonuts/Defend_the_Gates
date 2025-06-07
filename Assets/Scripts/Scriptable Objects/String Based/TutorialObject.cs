using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "Tutorial Object", menuName = "Etheral/Text Stuff/Tutorial Object")]
    [InlineEditor]
    public class TutorialObject : ScriptableObject
    {
        public string text;
        public InputIcons inputIcons;
    }
}