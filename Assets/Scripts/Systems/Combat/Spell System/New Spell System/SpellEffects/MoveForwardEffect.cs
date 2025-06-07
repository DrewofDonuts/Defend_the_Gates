using System;
using UnityEngine;

namespace Etheral
{
    public class MoveForwardEffect : BaseEffect
    {
        public float delayMovement;
        public float speed = 20f;
        public float acceleration = .2f;

        float delayTimer;

        public override void Tick(float deltaTime)
        {
            if (spellObject == null) return;

            delayTimer += deltaTime;

            if (delayTimer < delayMovement) return;
            // spellObject.transform.Translate(Vector3.forward * (speed * deltaTime));
            
            speed += acceleration * deltaTime;
            spellObject.transform.Translate(Vector3.forward * (speed * deltaTime));
        }
    }
}