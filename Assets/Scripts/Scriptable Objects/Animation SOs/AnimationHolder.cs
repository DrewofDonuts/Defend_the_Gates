using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "AnimationHolder", menuName = "Etheral/Data Objects/Animation/Animation Holder",
        order = -1)]
    public class AnimationHolder : ScriptableObject
    {
        public AnimationClip[] animationClips;

        public List<AnimationData> overrideAnimations;


        [Button("Create Animation Data Animation Name For Each Clip")]
        public void CreateAnimationDataAnimationNameForEachClip()
        {
            if (animationClips == null)
                return;
            foreach (var overrideAnimation in animationClips)
            {
                if (overrideAnimations.Any(oa => oa.animationName == overrideAnimation.name))
                    continue;
                overrideAnimations.Add(new AnimationData(overrideAnimation.name, overrideAnimation));
            }

            animationClips = null;
        }
    }
}


// void CreateAnimationDataForEveryState()
// {
//     foreach (var layer in animatorController.layers)
//     {
//         foreach (var state in layer.stateMachine.states)
//         {
//             var clip = state.state.motion as AnimationClip;
//
//             // Skip if animationDataList already contains an item with the same animationName
//             if (animationDataList.Any(data => data.animationName == state.state.name))
//             {
//                 continue;
//             }
//
//             animationDataList.Add(new AnimationData
//             {
//                 animationName = state.state.name,
//                 animationClip = clip
//             });
//         }
//     }
//             
//     // animationDatas = animationDataList.ToArray();
//             
// }
//
//
// [Button("Add Animation Data List")]
// public void AddAnimationDataList()
// {
//     CreateAnimationDataForEveryState();
// }