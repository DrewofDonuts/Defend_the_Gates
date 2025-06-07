using System;
using UnityEngine;

public class RotateAroundTest : MonoBehaviour
{
    public Transform _player;
    public float _angle;
    public float _smoothDamping = 1;

    void Update()
    {
        transform.RotateAround(_player.position, Vector3.up,
            _angle * _smoothDamping * Time.deltaTime);
    }
}