using UnityEngine;

namespace Etheral
{
    //Key methods
    //Getting Phase Info
    //Update Timers
    //Update Phase
    //Create Boss Phase Info that inherits from PhaseInfo

    //In Boss Idle.Enter() state
    //Start Phase Timers
    //UpdateTimers by Current Phase

    public abstract class BasePhaseController<T> : MonoBehaviour where T : PhaseInfo
    {
        [Header("Phase Settings")]
        [Range(0.1f, 1.5f)]
        [SerializeField] protected float phase2HealthPercent = .5f;
        [Range(0.1f, 1f)]
        [SerializeField] protected float phase3HealthPercent = .2f;
        [field: SerializeField] public int CurrentPhase { get; protected set; } = 1;

        [SerializeField] protected T[] phaseInfos;
        [SerializeField] protected Health health;

        protected virtual void OnDie(Health health) { }

        protected virtual void Start()
        {
            if (health == null)
                health = GetComponent<Health>();

            health.OnDie += OnDie;
            health.OnTakeHit += damage => TakeHit(damage);
        }

        protected virtual void UpdateTimers()
        {
            if (GameMenu.Instance != null && GameMenu.Instance.isPause) return;
        }

        protected virtual void TakeHit(IDamage damage)
        {
            GetPhaseInfo();
        }

        public T GetPhaseInfo()
        {
            if (health.CurrentHealth > health.MaxHealth * phase2HealthPercent)
            {
                CurrentPhase = 1;

                // health.SetSturdy(true);
                return phaseInfos[0];
            }

            if (health.CurrentHealth <= health.MaxHealth * phase2HealthPercent)
            {
                CurrentPhase = 2;

                // health.SetSturdy(true);

                return phaseInfos[1];
            }

            if (health.CurrentHealth <= health.MaxHealth * phase3HealthPercent)
            {
                CurrentPhase = 3;

                // health.SetSturdy(false);
                return phaseInfos[2];
            }

            return phaseInfos[0];
        }
    }
}