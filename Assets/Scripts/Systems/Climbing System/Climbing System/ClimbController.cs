using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class ClimbController : MonoBehaviour
    {
        [SerializeField] EnvironmentScanner environmentScanner;
        [SerializeField] ClimbingAction[] climbingActions;


        public ClimbData GetClimbingAction(ClimbActionType actionType)
        {
            var climb = Array.Find(climbingActions, action => action.ActionType == actionType);
            
            return BindClimbingData(climb);
        }

        public bool ClimbLedgeCheck(Vector3 moveDir, out RaycastHit climbPointHIt, out ClimbPoint climbPoint)
        {
            if (environmentScanner.ClimbLedgeCheck(moveDir, out climbPointHIt))
            {
                climbPoint = GetNearestClimbPoint(climbPointHIt);

                Debug.Log(climbPoint.name);
                if (climbPoint.IsDismountBottomPoint)
                    return true;
            }

            climbPoint = default;
            return false;
        }

        public bool LeapOrSwingCheck(out RaycastHit climbPointHit)
        {
            if (environmentScanner.LeapOrSwingCheck(out climbPointHit))
            {
                float angle = Quaternion.Angle(transform.rotation, climbPointHit.transform.rotation);
                if (angle <= 45)
                {
                    var climbPoint = climbPointHit.transform.GetComponent<ClimbPoint>();
                    Debug.Log($"Climbpoint neighbor is {climbPoint.GetLeapOrSwingConnection().climbPoint.name}");

                    return true;
                }
            }

            return false;
        }


        public bool ClimbDownCheck(out RaycastHit climbPointHit, out ClimbPoint climbPoint)
        {
            climbPoint = default;
            if (environmentScanner.DropLedgeCheck(out climbPointHit))
            {
                climbPoint = GetNearestClimbPoint(climbPointHit);

                if (climbPoint != null && climbPoint.IsTopPoint)
                {
                    // Debug.Log(climbPoint.transform.name);
                    return true;
                }
            }


            return false;
        }

        ClimbPoint GetNearestClimbPoint(RaycastHit hit)
        {
            var points = hit.transform.GetComponentsInChildren<ClimbPoint>();
            ClimbPoint nearestPoint = null;
            float nearestPointDistance = Mathf.Infinity;

            foreach (var point in points)
            {
                var distance = Vector3.Distance(point.transform.position, hit.point);
                if (distance < nearestPointDistance)
                {
                    nearestPoint = point;
                    nearestPointDistance = distance;
                }
            }

            return nearestPoint;
        }


        //Returns the position of the hand + the ledge position after the offset has been applied
        public Vector3 GetHandPosition(Transform ledge, AvatarTarget hand, Vector3 offset = default)
        {
            // Vector3 dir = hand == AvatarTarget.RightHand ? ledge.right : -ledge.right;
            Vector3 dir = Vector3.zero;


            if (hand == AvatarTarget.RightHand || hand == AvatarTarget.RightFoot)
                dir = ledge.right;
            else if (hand == AvatarTarget.LeftHand || hand == AvatarTarget.LeftFoot)
                dir = -ledge.right;


            return (ledge.forward * offset.z + Vector3.up * offset.y -
                    dir * offset.x) + ledge.position;
        }

        ClimbData BindClimbingData(ClimbingAction action)
        {
            ClimbData _climbData = new ClimbData();
            _climbData.animName = action.AnimName;
            _climbData.matchStartTime = action.MatchStartTime;
            _climbData.matchTargetTime = action.MatchTargetTime;
            _climbData.limbOffset = action.Offset;

            //this means we will weight to match all three axes  
            _climbData.matchBodyPart = action.MatchBodyPart;
            
            return _climbData;
        }
        
        public ClimbData BindClimbingData(string anim, Vector3 ledgePoint, AvatarTarget avatarTarget,
            Vector3 handOffset,
            float matchStartTime,
            float matchTargetTime)
        {
            ClimbData _climbData = new ClimbData();
            _climbData.animName = anim;
            _climbData.ledgePoint = ledgePoint;
            _climbData.matchStartTime = matchStartTime;
            _climbData.matchTargetTime = matchTargetTime;
            _climbData.limbOffset = handOffset;

            //this means we will weight to match all three axes  
            _climbData.matchBodyPart = avatarTarget;
            return _climbData;
        }
    }

    public struct ClimbData
    {
        public string animName;
        public float matchStartTime;
        public float matchTargetTime;
        public AvatarTarget matchBodyPart;
        public Vector3 ledgePoint;

        //This is the offset we determine for each animation and is arbitrary. Used to calculate the handPlacementAfterOffset
        [FormerlySerializedAs("handOffset")] public Vector3 limbOffset;

        //This is the position of the hand + the ledge position after the offset has been applied in GetHandPosition
        [FormerlySerializedAs("handPlacementAfterOffset")]
        public Vector3 limbPlacementAfterOffset;
    }
}