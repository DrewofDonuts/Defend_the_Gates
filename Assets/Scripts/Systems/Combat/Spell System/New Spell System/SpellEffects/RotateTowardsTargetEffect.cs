using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace Etheral
{
    [Serializable] [MovedFrom("Etheral")]
    public class RotateTowardsTargetEffect : BaseEffect
    {
        [FormerlySerializedAs("delayBeforeMoving")] public float delayBeforeRotating = 1f;
        public float rotationSpeed = 10f;
        float delayTimer;

        public override void Tick(float deltaTime)
        {
            if (target == null) return;

            delayTimer += deltaTime;
            if (delayTimer < delayBeforeRotating) return;

            Vector3 direction = target.position - spellObject.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            spellObject.transform.rotation =
                Quaternion.RotateTowards(spellObject.transform.rotation, toRotation, rotationSpeed * deltaTime);
        }
    }
}