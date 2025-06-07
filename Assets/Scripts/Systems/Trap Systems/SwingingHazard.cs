using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class SwingingHazard : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float swingSpeed = 1f;
        [SerializeField] float amplitudeShape = 1f;

        [Header("Axis Angles")]
        [SerializeField] float zAngle = 0f;
        [SerializeField] float xAngle = 45f;
        
        [Header("Audio")]
        [SerializeField] AudioSource whooshSource;
        [SerializeField] float whooshTriggerAngle = 10f; // Angle range near center to trigger sound
        
        
        [Header("State")]
        [SerializeField] float delayBeforeStart;
        [SerializeField] bool isDelayBeforeStart;
        [SerializeField] bool isActive;

        [Header("References")]
        [SerializeField] Transform pivotPoint;

        float time;
        float lastAngle;
        bool hasPlayedWhooshThisPass;



        void Update()
        {
            if (!isActive) return;

            time += Time.deltaTime * swingSpeed;

            if (isDelayBeforeStart && !isActive)
            {
                if (time < delayBeforeStart) return;
                isActive = true;
            }
            
            // Core swing value [-1, 1]
            float sinValue = Mathf.Sin(time);

            // Sharpen near middle, flatten at ends to simulate 'pause'
            float shapedValue = Mathf.Sign(sinValue) * Mathf.Pow(Mathf.Abs(sinValue), amplitudeShape);

            // Apply to rotation
            float angleZ = shapedValue * zAngle;
            float angleX = shapedValue * xAngle;
            
            transform.localRotation = Quaternion.Euler(angleX, 0f, angleZ);
            
            
            // Detect passing through center
            if (Mathf.Abs(angleX) <= whooshTriggerAngle)
            {
                if (!hasPlayedWhooshThisPass)
                {
                    whooshSource?.Play();
                    hasPlayedWhooshThisPass = true;
                }
            }
            else
            {
                // Reset once outside the trigger zone
                hasPlayedWhooshThisPass = false;
            }

            lastAngle = angleX;
            
        }
    }
}