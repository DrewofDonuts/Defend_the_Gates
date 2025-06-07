using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Etheral
{
    [Serializable] [MovedFrom("Etheral")]
    public class RotateTowardsCaster : BaseEffect
    {
        public float delayBeforeRotating = 1f;
        public float rotationSpeed = 100f;
        [Header("Stop Rotating Settings")]
        [Tooltip("Stop rotation if the spell is close enough to the caster")]
        public bool stopRotatingAtCaster;
        public float distanceBeforeStopping;

        bool isStoppedRotating;


        float delayTimer;

        public override void Tick(float deltaTime)
        {
            if (isStoppedRotating) return;

            delayTimer += deltaTime;

            if (delayTimer < delayBeforeRotating) return;


            Vector3 direction = caster.position - spellObject.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            spellObject.transform.rotation =
                Quaternion.RotateTowards(spellObject.transform.rotation, toRotation, rotationSpeed * deltaTime);

            isStoppedRotating = Vector3.Distance(spellObject.transform.position, caster.transform.position) <=
                                distanceBeforeStopping;
        }
    }
}