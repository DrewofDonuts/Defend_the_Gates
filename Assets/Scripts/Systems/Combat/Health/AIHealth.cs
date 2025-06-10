using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class AIHealth : Health
    {
        [Range(0, 1)]
        [SerializeField] float lowHealthThreshold = 0.40f;
        [SerializeField] GameObject lowHealthSkull;
        [SerializeField] public AIComponentHandler aIComponentHandler;
        [SerializeField] AIForceReceiver forceReceiver;
        [field: SerializeField] public bool IsNPC { get; private set; }
        public bool canBeExecuted = true;

        public event Action OnFallingToDeath;

        public override float MaxHealth => CharacterAttributes.MaxHealth;
        public override float MaxDefense => CharacterAttributes.MaxDefense;
        public override float MaxHolyCharge => CharacterAttributes.MaxHolyWill;


        public EnemyData enemyData = new();
        public override float CurrentHolyCharge { get; set; }
        public override float CurrentHealth
        {
            get => enemyData.health;
            set => enemyData.health = value;
        }
        public override float CurrentDefense
        {
            get => enemyData.defense;
            set => enemyData.defense = value;
        }

        protected override void Start()
        {
            base.Start();
            InitializeHealthProcessor(enemyData);
            healthProcessor.SetInitialCurrentHealth(aIComponentHandler.GetStatHandler()
                .GetModfiedMaxHealth(CharacterAttributes));
            healthProcessor.SetInitialCurrentDefense(aIComponentHandler.GetStatHandler()
                .GetModifiedMaxDefense(CharacterAttributes));

            if (StatBars.HealthSlider == null || StatBars.DefenseSlider == null)
                LoadStatBars();

            StatBars.StartingMethod(this);
            ValueChanged();

            HandleLowHealth();
        }


        void HandleLowHealth()
        {
            if (canBeExecuted && CurrentHealth / MaxHealth <= lowHealthThreshold && !lowHealthSkull.activeSelf)
            {
                // lowHealthSkull.SetActive(true);
                IsLowHealth = true;
                StartCoroutine(PingPongLowHealthSkull());
            }
        }

        new void Update()
        {
            if (IsNPC || CharacterAttributes == null) return;
            base.Update();
        }

        public override void TakeHit(IDamage damage, float angle = default)
        {
            if (CurrentHealth <= 0 || IsVulnerable) return;


            if (IsBlocking && IsWithinBlockAngle(angle))
            {
                HandleBlock(damage);
            }
            else
            {
                HandleHitDamage(damage, true);

                HandleLowHealth();
            }
        }

        protected override void HandleHitDamage(IDamage iDamage, bool preventDeathToCheckLedge = false)
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
                doesImpact = iDamage.DoesImpact,
                attackerID = iDamage.AttackerID
            };

            // EventBusPlayerController.FeedbackIgnoringDistanceFromPlayer(this, newDamageData.FeedbackType);

            //Checks if there is any feedback to play, otherwise it will not call the event
            if (newDamageData.FeedbackType != FeedbackType.None && newDamageData.AttackerID != -1)
                EventBusPlayerController.FeedbackBasedOnIDAndDistance(this, transform.position, newDamageData);


            var isLedgeAhead = forceReceiver.IsLedgeAhead(newDamageData.Direction, 2f);

            // if (Math.Abs(newDamageData.KnockBackForce) > Mathf.Abs(CharacterAttributes.KnockBackDefense))
            //     forceReceiver.AddForce(newDamageData.Direction);

            TriggerOnHit(newDamageData);
            if (bloodPrefabs != null && bloodPrefabs.Prefabs.Length > 0 && CurrentDefense <= 0f)
                EnableBlood();

            if (forceReceiver.AddForceAndCheckIfShouldFallOffLedge(newDamageData.Direction, 2f, IsLowHealth))
            {
                OnFallingToDeath?.Invoke();
                HideCanvasGroup();
                return;
            }

            TriggerOnTakeDamage(newDamageData);
            if (CurrentHealth <= 0)
                TriggerOnDie();
        }


        IEnumerator PingPongLowHealthSkull()
        {
            float pulseSpeed = 1.5f;
            float minScale = 1f;
            float maxScale = 1.3f;

            while (!IsDead)
            {
                float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
                float scale = Mathf.Lerp(minScale, maxScale, t);
                lowHealthSkull.transform.localScale = new Vector3(scale, scale, 1f);
                yield return null;
            }
        }
    }
}