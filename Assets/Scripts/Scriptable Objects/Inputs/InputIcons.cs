using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Input Icons", menuName = "Etheral/Input/Input Icons")]
    [InlineEditor]
    public class InputIcons : ScriptableObject
    {
        public Sprite XboxIcon;
        public Sprite PlayStationIcon;
        public Sprite KeyboardIcon;
    }
}