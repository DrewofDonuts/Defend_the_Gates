using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Etheral
{
    //Used for things like hanging bodies to move back and forth
    public class PendulumRotation : MonoBehaviour
    {
        public float speed = 1.0f;
        public float maxRotation = 45.0f;
        public float rotationOffset = 0.0f;

        Quaternion _start, _end;

        // Use this for initialization
        void Start()
        {
            float initialRotation = transform.rotation.eulerAngles.z;

            speed = Random.Range(0.1f, speed);

            float randomRotation = Random.Range(0, maxRotation);

            // Set the start and end rotations
            _start = Quaternion.AngleAxis(-randomRotation + initialRotation, Vector3.forward);
            _end = Quaternion.AngleAxis(randomRotation + initialRotation, Vector3.forward);

            // Start the pendulum routine
            StartCoroutine(Pendulum());
        }

        void Update()
        {
            transform.rotation = Quaternion.Euler(rotationOffset, 0, 0);
        }
        
        IEnumerator Pendulum()
        {
            while (true)
            {
                // Slerp from start to end over time 't' with easing in and out
                for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
                {
                    transform.rotation =
                        Quaternion.Slerp(_start, _end, (Mathf.Sin(t * Mathf.PI - Mathf.PI / 2) + 1) / 2);
                    
                    yield return null;
                }
        
                // Slerp from end to start over time 't' with easing in and out
                for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
                {
                    transform.rotation =
                        Quaternion.Slerp(_end, _start, (Mathf.Sin(t * Mathf.PI - Mathf.PI / 2) + 1) / 2);
                    yield return null;
                }
            }
        }
    }
}


// public float speed = 1.0f;
// public float maxRotation = 45.0f;
// public float momentumIncreaseRate = 0.1f;
//
// Quaternion _start, _end;
// float momentum = 0f;
//
// void Start()
// {
//     _start = Quaternion.AngleAxis(-maxRotation, Vector3.forward);
//     _end = Quaternion.AngleAxis(maxRotation, Vector3.forward);
//     StartCoroutine(Pendulum());
// }
//
// IEnumerator Pendulum()
// {
//     while (true)
//     {
//         // Get the controller input
//         float input = Input.GetAxis("Vertical");
//
//         // Increase the momentum based on the controller input
//         momentum += input * momentumIncreaseRate;
//
//         // Clamp the momentum to the maxRotation
//         momentum = Mathf.Clamp(momentum, -maxRotation, maxRotation);
//
//         // Slerp from start to end over time 't' with easing in and out, taking into account the momentum
//         for (float t = 0f; t < 1f; t += Time.deltaTime)
//         {
//             transform.rotation = Quaternion.Slerp(_start, _end, (Mathf.Sin((t + momentum) * Mathf.PI - Mathf.PI / 2) + 1) / 2);
//             yield return null;
//         }
//
//         // Slerp from end to start over time 't' with easing in and out, taking into account the momentum
//         for (float t = 0f; t < 1f; t += Time.deltaTime)
//         {
//             transform.rotation = Quaternion.Slerp(_end, _start, (Mathf.Sin((t + momentum) * Mathf.PI - Mathf.PI / 2) + 1) / 2);
//             yield return null;
//         }
//     }
// }