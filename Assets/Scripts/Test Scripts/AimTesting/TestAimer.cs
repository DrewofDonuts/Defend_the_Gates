using System;
using Etheral;
using UnityEngine;
using UnityEngine.Serialization;

public class TestAimer : MonoBehaviour
{
    public LayerMask groundMask;
    public InputObject inputObject;
    public Transform aimObject;
    public float maxDistanceFromPlayer;
    Transform Player;
    public LineRenderer lineRenderer;
    bool isKeyboard = true;
    
    GameObject projectile;

    void Start()
    {
        Player = transform;
    }

    void Update()
    {
        if (isKeyboard)
            AimWithMouse();
    }
    
    


    void AimWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(inputObject.MousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            Vector3 targetPosition;
            var direction = (hitInfo.point - Player.position).normalized;
            var distance = Vector3.Distance(hitInfo.point, Player.position);

            if (distance <= maxDistanceFromPlayer)
            {
                targetPosition = hitInfo.point;
            }
            else
            {
                // Clamp to max distance while preserving direction
                targetPosition = Player.position + direction * maxDistanceFromPlayer;
            }

            aimObject.position = new Vector3(targetPosition.x, targetPosition.y + 1f, targetPosition.z);
        }

        UpdateAimLaser();
    }

    void UpdateAimLaser()
    {
        var laserDirection = transform.forward * 4f;
        var endPoint = transform.position + new Vector3(0, transform.localPosition.y + .5f) + laserDirection;

        lineRenderer.SetPosition(0, transform.position + new Vector3(0, transform.localPosition.y + .5f, 0));
        lineRenderer.SetPosition(1, aimObject.position);
    }
}