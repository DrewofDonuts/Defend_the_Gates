using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Etheral
{
    public abstract class Health : MonoBehaviour, IHaveHealth
    {
        [field: SerializeField] public CharacterAttributes CharacterAttributes;
        [field: SerializeField] public StatBars StatBars { get; private set; }
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] protected PrefabContainer bloodPrefabs;

        public abstract float CurrentHealth { get; set; }
        public abstract float CurrentDefense { get; set; }
        public abstract float CurrentHolyCharge { get; set; }

        //FLAGS
        public bool IsBlocking { get; private set; }
        public bool IsVulnerable { get; private set; }

        //sturdy determines if character can go into impact state or not
        [field: SerializeField] public bool IsSturdy { get; private set; }
        public bool IsDead { get; private set; }

        //EVENTS
        public event Action<float> OnUseHolyWill;
        public event Action<IDamage> OnTakeHit;
        public event Action<IDamage> OnTakeGroundHit;
        public event Action<IDamage> OnBlock;
        public event Action<float> OnDamageHealth; //Float events are for HealthProcessor and Stats UI
        public event Action<Health> OnDie;
        public event Action OnCurrentStatChanged;

        public Transform Transform { get; set; }
        public abstract float MaxHealth { get; }
        public abstract float MaxDefense { get; }
        public abstract float MaxHolyCharge { get; }
        public bool IsLowHealth { get; protected set; }

        bool lastHitHadBlood;
        public bool IsExecuted { get; private set; }

        public abstract void TakeHit(IDamage damage, float angle = default);

        //HealthProcessor
        public HealthProcessor healthProcessor { get; protected set; }

        protected virtual void Start()
        {
            // if (gameObject.activeSelf)
            //     InitializeHealth();
            Transform = transform;

            if (CharacterAttributes == null)
                Debug.LogError("Character Attributes not assigned to " + gameObject.name);
        }

        protected virtual void OnDisable()
        {
            if (healthProcessor != null)
                healthProcessor.OnDisableHealth();

            StatBars.OnDisableHealth();
        }

        public void HideCanvasGroup()
        {
            if (canvasGroup != null)
                canvasGroup.alpha = 0;
        }

        protected void InitializeHealthProcessor(CharacterData characterData)
        {
            healthProcessor = new HealthProcessor(this, characterData);
        }

        protected void SetStatBars()
        {
            if (StatBars.HealthSlider == null || StatBars.DefenseSlider == null || StatBars.HolyWillSlider == null)
                LoadStatBars();

            StatBars.StartingMethod(this);
        }

        protected virtual void Update() { }


        protected bool IsWithinBlockAngle(float angle)
        {
            float blockingAngle = CharacterAttributes.BlockingAngle;
            return angle >= 0f && angle <= blockingAngle;
        }

        #region Blocking and Defense
        protected void RaiseOnBlock(IDamage damage) => OnBlock?.Invoke(damage);
        protected void RaiseOnDamgeHealth(float damage) => OnDamageHealth?.Invoke(damage);


        protected void HandleBlock(IDamage iDamage)
        {
            DamageData newDamageData = new DamageData
            {
                damage = iDamage.Damage,
                Direction = DamageUtil.CalculateKnockBack(iDamage.Transform, transform, iDamage.KnockBackForce),
                knockBackForce = iDamage.KnockBackForce,
                knockDownForce = iDamage.KnockDownForce,
                isShieldBreak = iDamage.IsShieldBreak,
                feedbackType = iDamage.FeedbackType,
                isExecution = iDamage.IsExecution,
                audioImpact = iDamage.AudioImpact,
                doesImpact = iDamage.DoesImpact
            };


            RaiseOnBlock(newDamageData);

            // RaiseOnDamgeHealth(newDamageData.Damage / 4);

            //SFX and VFX to indicate successful block


            if (CurrentHealth <= 0)
            {
                EnableBlood();
                TriggerOnDie();
            }


            iDamage.IsBlocked(true);
        }
        #endregion

        #region Hit Damage
        protected void HandleDotDamage(float damage)
        {
            OnDamageHealth?.Invoke(damage);

            if (CurrentHealth <= 0)
                TriggerOnDie();
        }

        public void ValueChanged()
        {
            OnCurrentStatChanged?.Invoke();
        }


        protected virtual void HandleHitDamage(IDamage iDamage, bool preventDeathToCheckLedge = false)
        {
            DamageData newDamageData = new DamageData
            {
                damage = iDamage.Damage,
                Direction = DamageUtil.CalculateKnockBack(iDamage.Transform, transform, iDamage.KnockBackForce),
                knockBackForce = iDamage.KnockBackForce,
                knockDownForce = iDamage.KnockDownForce,
                isShieldBreak = iDamage.IsShieldBreak,
                feedbackType = iDamage.FeedbackType,
                isExecution = iDamage.IsExecution,
                audioImpact = iDamage.AudioImpact,
                doesImpact = iDamage.DoesImpact
            };
            
            TriggerOnTakeDamage(newDamageData);
            if (CurrentHealth <= 0 && !preventDeathToCheckLedge)
                TriggerOnDie();

            TriggerOnHit(newDamageData);
            
            
            if (bloodPrefabs != null && bloodPrefabs.Prefabs.Length > 0 && CurrentDefense <= 0f)
                EnableBlood();

        }


        public void TakeDot(float damage) => HandleDotDamage(damage);
        #endregion


        #region Flags
        public void SetBlocking(bool isBlocking)
        {
            IsBlocking = isBlocking;
        }

        public void SetSturdy(bool isSturdy)
        {
            IsSturdy = isSturdy;
        }

        public void SetIsInvulnerable(bool isVulernable)
        {
            IsVulnerable = isVulernable;
        }
        #endregion


        protected void HandleGroundDamage(IDamage damage)
        {
            OnTakeGroundHit?.Invoke(damage);

            if (bloodPrefabs != null && bloodPrefabs.Prefabs.Length > 0)
                EnableBlood();
            else
                Debug.LogWarning("No Blood VFX Prefabs assigned to " + gameObject.name);

            if (CurrentHealth <= 0)
            {
                TriggerOnDie();
            }
        }


        public void Heal(float healthToAdd)
        {
            healthProcessor.HealHealth(healthToAdd);
        }

        protected void TriggerOnTakeDamage(DamageData newDamageData)
        {
            OnDamageHealth?.Invoke(newDamageData.damage);
        }

        public void TriggerOnHit(DamageData damageData)
        {
            OnTakeHit?.Invoke(damageData);
        }

        public void TriggerOnDie()
        {
            OnDie?.Invoke(this);
            Destroy(gameObject, 10f); // Delay to allow for death animations or effects
        }

        public void UseStamina(float staminaUsed)
        {
            OnUseHolyWill?.Invoke(staminaUsed);
        }

        public void EnableBlood()
        {
            if (bloodPrefabs == null || bloodPrefabs.Prefabs.Length == 0) return;
            var index = Random.Range(0, bloodPrefabs.Prefabs.Length);
            var offset = new Vector3(0, .5f, 0);

            if (lastHitHadBlood)
            {
                lastHitHadBlood = false;
            }
            else
            {
                Instantiate(bloodPrefabs.Prefabs[index], transform.position + offset,
                    Quaternion.identity);

                lastHitHadBlood = true;
            }
        }


        public void SetExecution()
        {
            // OnExecuted?.Invoke();

            IsExecuted = true;
            TriggerOnDie();

            // IDamage damage = new DamageData
            // {
            //     damage = 100000
            // };
            // HandleHitDamage(damage);
        }

        public void SetIsDead()
        {
            IsDead = true;
            HideCanvasGroup();
        }


        public void LoadStatBars()
        {
            StatBars.LoadHealth(this);

            var UISliders = GetComponentsInChildren<UISliders>();

            Slider healthSlider = default;
            Slider defenseSlider = default;
            Slider staminaSlider = default;

            foreach (var slider in UISliders)
            {
                if (slider.IsHealthBar)
                    healthSlider = slider.Slider;

                if (slider.IsDefenseBar)
                    defenseSlider = slider.Slider;

                if (slider.IsStaminaBar)
                    staminaSlider = slider.Slider;
            }

            StatBars.LoadHealthAndDefenseSliders(healthSlider, defenseSlider);

            if (staminaSlider != null)
                StatBars.LoadStaminaSlider(staminaSlider);
        }

        public void ResetHealthValueAfterDeath()
        {
            healthProcessor.SetInitialCurrentHealth();
        }

#if UNITY_EDITOR

        [Button("Load Components")]
        public void LoadComponents()
        {
            LoadStatBars();
        }

#endif
        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;
    }
}