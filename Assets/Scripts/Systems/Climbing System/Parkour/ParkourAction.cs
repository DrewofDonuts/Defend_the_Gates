using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "ParkourAction", menuName = "Etheral/Parkour and Climbing System/Parkour Action",
        order = 1)]
    [InlineEditor]
    public class ParkourAction : ScriptableObject
    {
        [field: SerializeField] public string AnimName { get; set; }
        [field: SerializeField] public string ObstacleTag { get; set; }
        [field: SerializeField] public bool RotateTowardsObstacle { get; set; }

        [field: BoxGroup("Obstacle Config")]
        [field: Tooltip("Should be equal or greater than CharacterController's stepOffset")]
        [field: SerializeField] public float minHeight { get; set; }
        [field: BoxGroup("Obstacle Config")]
        [field: SerializeField] public float maxHeight { get; set; }

        [field: BoxGroup("Obstacle Config")]
        [field: SerializeField] public float PauseTime { get; set; }


        [field: BoxGroup("Target Matching")]
        [field: SerializeField] public bool EnableTargetMatching { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public AvatarTarget MatchBodyPart { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public AvatarTarget OverrideMatchBodyPart { get; protected set; }
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public float MatchStartTime { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: Tooltip("Must be greater than MatchStartTime")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public float MatchTargetTime { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public Vector3 MatchPosWeight { get; set; } = new Vector3(0, 1, 0);
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]

        public bool MirrorAnimation { get; protected set; }
        
        public Quaternion targetRotation { get; set; } = Quaternion.identity;
        public Vector3 matchPosition { get; set; } = Vector3.zero;

        public virtual bool CheckIfPossible(ObstacleHitData hitData, Transform player)
        {
            //Check Tag - if both are not empty and not equal, return false
            if (!string.IsNullOrEmpty(ObstacleTag) && hitData.forwardHit.transform.tag != ObstacleTag)
                return false;


            //Height Check
            var heightRelativeToPlayer = hitData.heightHit.point.y - player.position.y;
            if (heightRelativeToPlayer < minHeight || heightRelativeToPlayer > maxHeight)
                return false;

            //Rotation Check
            if (RotateTowardsObstacle)
                targetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);

            //Target Matching Check
            if (EnableTargetMatching)
                matchPosition = hitData.heightHit.point;


            return true;
        }
        
        
    }
}