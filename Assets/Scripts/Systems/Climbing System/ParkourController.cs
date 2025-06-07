using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Etheral
{
    public class ParkourController : MonoBehaviour
    {
        [field: SerializeField] public List<ParkourAction> ParkourActions { get; set; }
        [field: SerializeField] public ParkourAction JumpDown { get; set; }
        [SerializeField] CharacterController characterController;
        EnvironmentScanner environmentScanner;

        void Awake()
        {
            environmentScanner = GetComponent<EnvironmentScanner>();
        }
        
        public ClimbAndParkourData CheckIfPossible()
        {
            var hitData = environmentScanner.ObstacleCheck();
            if (hitData.forwardHitFound)
            {
                foreach (var action in ParkourActions)
                {
                    if (action.CheckIfPossible(hitData, characterController.transform))
                    {
                        return BindParkourData(action);
                    }
                }
            }

            return default;
        }

        public ClimbAndParkourData GetJumpDown()
        {
            return BindParkourData(JumpDown);
        }

        public bool LedgeCheck(Vector3 moveDir, out LedgeData ledgeData)
        {
            //used to prevent jumping when there is an obstacle in front of the player
            var hitData = environmentScanner.ObstacleCheck();
            
            return environmentScanner.ObstacleLedgeCheck(moveDir, out ledgeData) && !hitData.forwardHitFound;
        }

        ClimbAndParkourData BindParkourData(ParkourAction parkourAction)
        {
            ClimbAndParkourData _climbAndParkourData = new ClimbAndParkourData();
            _climbAndParkourData.animName = parkourAction.AnimName;
            _climbAndParkourData.targetRotation = parkourAction.targetRotation;
            _climbAndParkourData.rotateTowardsObstacle = parkourAction.RotateTowardsObstacle;
            _climbAndParkourData.enableTargetMatching = parkourAction.EnableTargetMatching;
            _climbAndParkourData.matchBodyPart = parkourAction.MatchBodyPart;
            _climbAndParkourData.matchStartTime = parkourAction.MatchStartTime;
            _climbAndParkourData.matchTargetTime = parkourAction.MatchTargetTime;
            _climbAndParkourData.matchPosition = parkourAction.matchPosition;
            _climbAndParkourData.matchPosWeight = parkourAction.MatchPosWeight;
            _climbAndParkourData.PauseTime = parkourAction.PauseTime;
            _climbAndParkourData.mirrorAnimation = parkourAction.MirrorAnimation;
            _climbAndParkourData.overrideMatchBodyPart = parkourAction.OverrideMatchBodyPart;


            return _climbAndParkourData;
        }
    }

    public struct ClimbAndParkourData
    {
        public string animName;
        public bool rotateTowardsObstacle;
        public Quaternion targetRotation;

        #region Target Matching
        public bool enableTargetMatching;
        public AvatarTarget matchBodyPart;
        public AvatarTarget overrideMatchBodyPart;
        public float matchStartTime;
        public float matchTargetTime;
        public Vector3 matchPosition;
        public Vector3 matchPosWeight;
        public bool mirrorAnimation;
        #endregion

        public double PauseTime { get; set; }
    }
}