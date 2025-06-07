using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "Custom Parkour Action",
        menuName = "Etheral/Parkour and Climbing System/Custom Mirror Action",
        order = 1)]

    //This class is used to create custom parkour actions
    //In this case, whether to play left or right animation
    [InlineEditor] public class CustomMirrorParkourAction : ParkourAction
    {
        public override bool CheckIfPossible(ObstacleHitData hitData, Transform player)
        {
            if (!base.CheckIfPossible(hitData, player))
                return false;

            //check if hitting left or right side
            var hitPoint = hitData.forwardHit.transform.InverseTransformPoint(hitData.forwardHit.point);

            //if approaching from behind and left side
            if (hitPoint is { z: < 0, x: < 0 } or { z: > 0, x: > 0 })
            {
                MirrorAnimation = true;
                OverrideMatchBodyPart = OverrideMatchBodyPart;
            }
            else
            {
                MirrorAnimation = false;
            }

            return true;
        }
    }
}