using System;
using FIMSpace.FLook;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class ShealaController : MonoBehaviour
    {
        [Header("Phase Settings")]
        [Range(0.1f, 1.5f)]
        [SerializeField] float phase2HealthPercent = .5f;
        [Range(0.1f, 1f)]
        [SerializeField] float phase3HealthPercent = .2f;
        [field: SerializeField] public int CurrentPhase { get; private set; } = 1;

        [SerializeField] PhaseInfoSheala[] phaseInfos;

        [SerializeField] Health health;
        [SerializeField] HumanBombController humanBombController;
        public HumanBombController HumanBombController => humanBombController;


        public float currentTimeBeforeProjectile { get; private set; }
        public float currentTImeBeforeHumanBomb { get; private set; }

        bool startProjectileTimer;
        bool startHumanBombTimer;

        public float projectileTimer;
        public float humanBombTimer;

        void Start()
        {
            if (health == null)
                health = GetComponent<Health>();
            health.OnDie += OnDie;
            health.OnTakeHit += damage => TakeHit(damage);
        }

        void Update()
        {
            if (startProjectileTimer)
                projectileTimer += Time.deltaTime;

            if (startHumanBombTimer)
                humanBombTimer += Time.deltaTime;
        }

        public void StartProjectileTimer() => startProjectileTimer = true;

        public void ResetProjectileTimer()
        {
            startProjectileTimer = false;
            projectileTimer = 0;
        }

        public void StartHumanBombTimer() => startHumanBombTimer = true;

        public void ResetHumanBombTimer()
        {
            humanBombTimer = 0;
            startHumanBombTimer = false;
        }

        public void UpdateTimersByCurrentPhase()
        {
            if (CurrentPhase == 1)
            {
                currentTimeBeforeProjectile =
                    Random.Range(phaseInfos[0].minProjectileTime, phaseInfos[0].maxProjectileTime);
                currentTImeBeforeHumanBomb =
                    Random.Range(phaseInfos[0].minHumanBombTime, phaseInfos[0].maxHumanBombTime);
            }
            else if (CurrentPhase == 2)
            {
                currentTimeBeforeProjectile =
                    Random.Range(phaseInfos[1].minProjectileTime, phaseInfos[1].maxProjectileTime);
                currentTImeBeforeHumanBomb =
                    Random.Range(phaseInfos[1].minHumanBombTime, phaseInfos[1].maxHumanBombTime);
            }
            else if (CurrentPhase == 3)
            {
                currentTimeBeforeProjectile =
                    Random.Range(phaseInfos[2].minProjectileTime, phaseInfos[2].maxProjectileTime);
                currentTImeBeforeHumanBomb =
                    Random.Range(phaseInfos[2].minHumanBombTime, phaseInfos[2].maxHumanBombTime);
            }
        }


        void TakeHit(IDamage damage)
        {
            GetPhaseInfo();
        }

        public PhaseInfoSheala GetPhaseInfo()
        {
            if (health.CurrentHealth > health.MaxHealth * phase2HealthPercent)
            {
                CurrentPhase = 1;
                return phaseInfos[0];
            }

            if (health.CurrentHealth <= health.MaxHealth * phase2HealthPercent)
            {
                CurrentPhase = 2;
                return phaseInfos[1];
            }

            if (health.CurrentHealth <= health.MaxHealth * phase3HealthPercent)
            {
                CurrentPhase = 3;
                return phaseInfos[2];
            }

            return phaseInfos[0];
        }

        void OnDie(Health health1) { }
    }

    [Serializable]
    public class PhaseInfoSheala : PhaseInfo
    {
        [Header("Projectile Settings")]
        public float minProjectileTime;
        public float maxProjectileTime;

        [Header("HumanBomb Settings")]
        public float minHumanBombTime;
        public float maxHumanBombTime;

        public PhaseInfoSheala(int phase, float minProjectileTime, float maxProjectileTime) : base(phase)
        {
            this.phase = phase;
            this.minProjectileTime = minProjectileTime;
            this.maxProjectileTime = maxProjectileTime;
        }
    }

    public abstract class PhaseInfo
    {
        public  int phase;

        public PhaseInfo(int phase)
        {
            this.phase = phase;
        }
    }
}