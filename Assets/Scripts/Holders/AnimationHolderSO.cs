using System;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "AnimationHolderSO", menuName = "Etheral/Holders/AnimationHolderSO")]
    public class AnimationHolderSO : ScriptableObject
    {
        public AnimationType[] animationTypes;
    }
}