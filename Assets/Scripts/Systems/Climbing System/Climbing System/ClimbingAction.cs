using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "climbing Action", menuName = "Etheral/Parkour and Climbing System/Climbing Action",
        order = 1)]
    [InlineEditor]
    public class ClimbingAction : ScriptableObject
    {
        [field: SerializeField] public string AnimName { get; set; }
        [field: SerializeField] public bool RotateTowardsObstacle { get; set; } = true;
        [field: SerializeField] public ClimbActionType ActionType { get; set; }

        [field: BoxGroup("Target Matching")]
        [field: SerializeField] public bool EnableTargetMatching { get; set; } = true;
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public AvatarTarget MatchBodyPart { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public float MatchStartTime { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: Tooltip("Must be greater than MatchStartTime")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public float MatchTargetTime { get; set; }
        [field: BoxGroup("Target Matching")]
        [field: ShowIf("EnableTargetMatching")]
        [field: SerializeField] public Vector3 MatchPosWeight { get; set; } = Vector3.one;
        [field: BoxGroup("Target Matching")]
        [field: SerializeField] public Vector3 Offset { get; set; }
    }
}