using System;
using Etheral;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Vector3 = UnityEngine.Vector3;

public class ForceReceiver : MonoBehaviour
{
    // public GroundCheck groundCheck;
    [SerializeField] protected float _drag = .1f;
    [SerializeField] protected CharacterController characterController;
    [SerializeField] protected LayerMask groundLayers;
    [SerializeField] protected bool ignoreLedgeCheck;

    protected Vector3 _impact { get; set; } //when hit
    protected Vector3 _dampingVelocity;
    public float verticalVelocity { get; protected set; } //used for jumping

    //Forces movement is used for gravity, impacts, and to weight against directional Character movement
    public Vector3 ForcesMovement => _impact + Vector3.up * verticalVelocity;

    //Jumping Checks
    bool isFallingFromJump;
    public bool IsGravity { get; protected set; } = true;
    Vector3 floorPosition;

    //Falling to death events

    
    public void SetIgnoreLedgeCheck(bool ignore)
    {
        ignoreLedgeCheck = ignore;
    }
    
    public void SetGravity(bool isGravity)
    {
        IsGravity = isGravity;
    }

    protected virtual void Update()
    {
        // if (IsGravity)
        HandleGravity();
        _impact = Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag);

        if (IsLedgeAhead(Vector3.SmoothDamp(_impact, Vector3.zero, ref _dampingVelocity, _drag)))
            _impact = Vector3.zero;
    }


    public bool IsGrounded()
    {
        // return IsGroundedCustom(.2f);

        return characterController.isGrounded;
    }


    protected void HandleGravity()
    {
        if (IsGrounded() && verticalVelocity < 0f)
        {
            //keeps the player grounded
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            if (isFallingFromJump)
            {
                verticalVelocity += Physics.gravity.y * 2f * Time.deltaTime;
            }
            else
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime;

                // if (verticalVelocity <= -8f && !eventIsSent)
                // {
                //     Debug.Log($"Falling to death at {verticalVelocity}");
                //     OnFallingToDeath?.Invoke();
                //     eventIsSent = true;
                // }
            }
        }
    }

    public bool IsLedgeAhead(Vector3 direction, float checkDistance = 1f, float maxSafeYDrop = 0.5f,
        float rayLength = 1.5f, float maxSlopeAngle = 50f)
    {
        if (direction == Vector3.zero) return false;
        if (ignoreLedgeCheck) return false;

        // if (canFall) return false;

        Vector3 origin = characterController.transform.position + Vector3.up * 0.3f;
        Vector3 normalizedDirection = direction.normalized;

        float halfWidth = characterController.radius * 0.75f;

// Calculate three horizontal offset positions for raycasting: center, left, and right.
// These are used to check for ground in front of the character, accounting for edge cases like sloped or uneven surfaces.
        Vector3[] offsets =
        {
            Vector3.zero, // Center ray: directly in front of the character

            // Left ray: offset to the left of the movement direction
            Vector3.Cross(Vector3.up, normalizedDirection).normalized * halfWidth,

            // Right ray: offset to the right of the movement direction
            -Vector3.Cross(Vector3.up, normalizedDirection).normalized * halfWidth
        };

        foreach (var offset in offsets)
        {
            Vector3 checkPosition = origin + normalizedDirection * checkDistance + offset;

            if (Physics.Raycast(checkPosition, Vector3.down, out RaycastHit hit, rayLength, groundLayers))
            {
                float yDrop = checkPosition.y - hit.point.y;
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                Debug.DrawRay(checkPosition, Vector3.down * rayLength, Color.green);

                if (yDrop > maxSafeYDrop || slopeAngle > maxSlopeAngle)
                {
                    // Too far down or too steep — treat as ledge
                    return true;
                }
            }
            else
            {
                Debug.DrawRay(checkPosition, Vector3.down * rayLength, Color.red);
                return true; // No ground hit at all — definitely a ledge
            }
        }

        return false; // All checks safe
    }


    public virtual void AddForce(Vector3 force)
    {
        _impact += force;
    }


    public void ResetForces()
    {
        ForcesMovement.Set(0, 0, 0);
        verticalVelocity = 0;
    }

    public void SetIsFallingForJump(bool b)
    {
        isFallingFromJump = b;
    }

    public void ToggleGravity(bool isGravityOn)
    {
        IsGravity = isGravityOn;
    }


#if UNITY_EDITOR
    public void LoadComponents()
    {
        characterController = GetComponentInParent<CharacterController>();
    }
#endif
}