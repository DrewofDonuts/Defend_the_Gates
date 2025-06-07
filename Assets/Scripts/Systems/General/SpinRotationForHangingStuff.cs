using System.Collections;
using UnityEngine;

namespace Etheral
{
    //THIS ISN'T WORKING
    public class SpinRotationForHangingStuff : MonoBehaviour
    {
        public float speed = 1.0f;
        public float maxRotation = 45.0f;
        public float zOffset = 0.0f;

        Quaternion _start, _end;

        // Use this for initialization
        void Start()
        {
            // Set the start and end rotations
            _start = Quaternion.AngleAxis(-maxRotation, Vector3.up);
            _end = Quaternion.AngleAxis(maxRotation, Vector3.up);

            // Start the pendulum routine
            StartCoroutine(Spin());
        }

        void Update()
        {
        }

        IEnumerator Spin()
        {
            while (true)
            {
                // Rotate from -maxRotation to maxRotation over time 't' with easing in and out
                for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
                {
                    float rotationY = maxRotation * (Mathf.Sin(t * Mathf.PI - Mathf.PI / 2) + 1) / 2;
                    transform.rotation = Quaternion.Euler(0, rotationY, zOffset);
                    yield return null;
                }

                // Rotate from maxRotation to -maxRotation over time 't' with easing in and out
                for (float t = 0f; t < 1f; t += Time.deltaTime * speed)
                {
                    float rotationY = maxRotation * (Mathf.Sin(t * Mathf.PI + Mathf.PI / 2) + 1) / 2;
                    transform.rotation = Quaternion.Euler(0, rotationY, zOffset);
                    yield return null;
                }
            }
        }
    }
}