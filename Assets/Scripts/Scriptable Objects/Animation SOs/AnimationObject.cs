using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "AnimationObject", menuName = "Etheral/Data Objects/Animation/Animation Object", order = -1)]
    [InlineEditor]
    public class AnimationObject : ScriptableObject
    {
        public AnimationData animationData;
        // public AnimationClip animationClip;
        
        
    }
}