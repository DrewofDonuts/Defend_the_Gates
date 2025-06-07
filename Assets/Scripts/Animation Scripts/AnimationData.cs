using System;
using UnityEngine;

namespace Etheral
{
    [Serializable]
    public class AnimationData
    {
        public AnimationData(string animationName, AnimationClip animationClip, bool isEnabled = true)
        {
            this.animationName = animationName;
            this.animationClip = animationClip;
            this.isEnabled = isEnabled;
        }

        public string animationName;
        public AnimationClip animationClip;
        public bool isEnabled = true;
    }
}