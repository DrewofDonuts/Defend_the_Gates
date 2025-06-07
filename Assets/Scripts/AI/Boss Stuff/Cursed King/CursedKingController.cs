using System;
using Etheral;
using UnityEngine;

namespace Etheral
{
    public class CursedKingController : BasePhaseController<PhaseInfoCursedKing>
    {
        bool startSkeletonTimer;
        bool startWraithTimer;
        public float skeletonTimer;
        public float wraithTimer;

        void Update()
        {
            UpdateTimers();
        }

        public void StartSkeletonTimer() => startSkeletonTimer = true;
        public void StartWraithTimer() => startWraithTimer = true;

        public void ResetSkeletonTimer()
        {
            skeletonTimer = 0;
            startSkeletonTimer = false;
        }

        public void ResetWraithTimer()
        {
            wraithTimer = 0;
            startWraithTimer = false;
        }

        protected override void UpdateTimers()
        {
            base.UpdateTimers();
            if (startSkeletonTimer)
                skeletonTimer += Time.deltaTime;

            if (startWraithTimer)
                wraithTimer += Time.deltaTime;
        }
    }

    [Serializable]
    public class PhaseInfoCursedKing : PhaseInfo
    {
        [Header("Raise Skeleton Arhcers")]
        public float minRaiseSkeletonTime;
        public float maxRaiseSkeletonTime;
        public int spawnNumber;

        [Header("Call Wraiths")]
        public float minCallWraithTime;
        public float maxCallWraithTime;

        public PhaseInfoCursedKing(int phase, int howManySpawns, float minSkel, float maxSkel, float minWraith,
            float maxWraith) : base(phase)
        {
            this.phase = phase;
            minRaiseSkeletonTime = minSkel;
            maxRaiseSkeletonTime = maxSkel;
            minCallWraithTime = minWraith;
            maxCallWraithTime = maxWraith;
            spawnNumber = howManySpawns;
        }
    }
}