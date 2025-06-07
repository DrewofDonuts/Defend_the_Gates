using System;
using Etheral;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    public InputObject inputObject;
    public CharacterController characterController;

    public float moveSpeed = 5f;

    Vector3 moveDirection;

    void Update()
    {
        moveDirection = new Vector3(inputObject.MovementValue.x, 0, inputObject.MovementValue.y);
        moveDirection.Normalize();
        moveDirection *= moveSpeed;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}