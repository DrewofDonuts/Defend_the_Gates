using UnityEngine;
using UnityEngine.AI;

namespace Etheral
{
    //KEPT FOR RECORDS IN CASE OF REVERT
    public class ForceReceiverWithLedgeCheck : MonoBehaviour
    {
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }
        public GroundCheck groundCheck;
        [SerializeField] float _drag = .1f;
        [SerializeField] ParkourController parkourController;
        [SerializeField] CharacterController characterController;

        public Vector3 _impact { get; private set; } //when hit
        Vector3 _dampingVelocity;
        public float verticalVelocity { get; private set; } //used for jumping

        //Forces movement is used for gravity, impacts, and to weight against directional Character movement
        public Vector3 ForcesMovement => _impact + Vector3.up * verticalVelocity;

        //used to send to EnvironmentScanner to check if player is on a ledge
        public Vector3 DesiredDirection { get; private set; }

        //Actual direction the player is moving
        public Vector3 MoveDirection { get; private set; }

        //Checks if the player is on a ledge
        public bool IsOnLedge { get; private set; }

        //Checks if the player can jump based on angle of the ledge
        public bool CanJumpOffLedge { get; private set; }

        //Data used for angle and height
        public LedgeData LedgeData { get; private set; }

        //Jumping Checks
        bool isFalling;
        public bool IsGravity { get; private set; } = true;

        Vector3 floorPosition;

        void Update()
        {
            // if (IsGravity)
            HandleGravity();
            if (parkourController != null)
            {
                // GetLedgeInfo();
            }

            //used to decelerate the impact velocity back to zero
            _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);

            if (Agent != null)
            {
                //once the impact is less than below...
                if (_impact.sqrMagnitude < 0.2f * 0.2f)
                {
                    _impact = Vector3.zero;
                    Agent.enabled = true;
                }
            }
        }

        public bool IsGrounded()
        {
            return characterController.isGrounded;


            // if (groundCheck.IsGrounded(out RaycastHit hit))
            // {
            //     floorPosition = hit.point;
            //
            //     // transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            //
            //     return true;
            // }
            //
            // return false;
        }

        //provides distance to determine if a player can perform air attacks
        public float GetDistanceToGround()
        {
            return groundCheck.GetDistanceToGround();
        }


        void HandleGravity()
        {
            if (IsGrounded() && verticalVelocity < 0f)
            {
                //keeps the player grounded
                verticalVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                if (isFalling)
                    verticalVelocity += Physics.gravity.y * 2f * Time.deltaTime;
                else
                    verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
        }

        public void AddForce(Vector3 force)
        {
            DesiredDirection += _impact + force;
            bool _isOnLedge = false;

            if (parkourController != null)
                _isOnLedge = parkourController.LedgeCheck(ForcesMovement + DesiredDirection, out var ledgeData);


            if (_isOnLedge)
            {
                // Debug.Log("Will Fall");
                _impact += Vector3.zero;
            }
            else
            {
                // Debug.Log("Will Not Fall");
                _impact += force;
            }

            _impact += force;


            if (Agent != null) //for AI only
            {
                Agent.enabled = false;
            }
        }

        public void Jump(float jumpForce)
        {
            isFalling = true;
            verticalVelocity += jumpForce;
        }

        [ContextMenu("Load NavMeshAgent")]
        public void LoadNavMeshAgent()
        {
            Agent = GetComponent<NavMeshAgent>();
        }


        void GetLedgeInfo()
        {
            IsOnLedge = parkourController.LedgeCheck(ForcesMovement + DesiredDirection, out var ledgeData) &&
                        IsGrounded();
            LedgeData = ledgeData;

            CanJumpOffLedge = Vector3.Angle(LedgeData.surfaceHit.normal, DesiredDirection + ForcesMovement) < 45f;
        }


        //used to send to EnvironmentScanner to check if player is on a ledge
        public void SetMotion(Vector3 motion)
        {
            Vector3 clampedMotion = motion;
            clampedMotion.x = Mathf.Clamp(clampedMotion.x, -1f, 1f);
            clampedMotion.z = Mathf.Clamp(clampedMotion.z, -1f, 1f);

            DesiredDirection = clampedMotion;
            MoveDirection = motion;
        }

        public Vector3 GetMotion()
        {
            //Enable below to allow movement when on a ledge
            return MoveDirection;

            //Enable below to prevent movement when on a ledge
            return IsOnLedge ? Vector3.zero : MoveDirection;
        }


        public void ResetForces()
        {
            ForcesMovement.Set(0, 0, 0);
            verticalVelocity = 0;
        }

        public void SetIsFalling(bool b)
        {
            isFalling = b;
        }

        public void ToggleGravity(bool isGravityOn)
        {
            IsGravity = isGravityOn;
        }
    }
}