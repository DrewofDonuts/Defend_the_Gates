using System.Collections;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Etheral
{
    public class EnemyStateMachine : AIStateMachine, IBind<EnemyData>, IAmPoolObject<EnemyStateMachine>
    {
        [field: Header("Object Pool")]
        [field: SerializeField] public EnemyStateMachine Prefab { get; set; }
        [field: SerializeField] public string PoolKey { get; set; }


        public EnemyData enemyData;
        AIHealth aiHealth => Health as AIHealth;

        public void Bind(EnemyData _data)
        {
            enemyData = _data;
            if (enemyData.isDead)
                gameObject.SetActive(false);
        }

        EnemyProcessor processor = new();

        #region Impact Counter Variables
        [Header("Impact Counter Stuff")]
        public bool canPreventImpacts;
        public float impactCounterTime = 5f;
        public int impactCounterMax = 3;
        [ReadOnly]
        public float impactCounterTimer;
        [ReadOnly]
        public int impactCounter;
        [ReadOnly]
        public bool isStartcounterTimer;
        [ReadOnly]
        public bool counterActionReady;
        #endregion

        //Testing
        // public Image stateIndicator;

        public bool CheckIfCounterActionIsReady() => counterActionReady;

        public void SetIsGateAttacking(bool isGateAttacking)
        {
            aiComponents.GetOverrideStateController().SetIsGateAttackingOverride(isGateAttacking);
        }

        protected override IEnumerator Start()
        {
            yield return base.Start();

            GetAIComponents().GetHeadExecutionPoint().OnDestroyed += Health.TriggerOnDie;
            GetAIComponents().GetHeadExecutionPoint().OnDestroyed += Health.SetIsDead;
            aiHealth.OnFallingToDeath += HandleFallingAfterHit;


            if (currentState == null)
                EnterStartingState();

            if (!AITestingControl.displayStateIndicator)
                stateIndicator.enabled = false;
        }

        void HandleFallingAfterHit()
        {
            //Disabled for testing 04/24/25
            SwitchState(new EnemyFallingState(this));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventBusEnemyController.EnemySpawn(name, this);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetAIComponents().GetHeadExecutionPoint().OnDestroyed -= Health.TriggerOnDie;
            GetAIComponents().GetHeadExecutionPoint().OnDestroyed -= Health.SetIsDead;
            aiHealth.OnFallingToDeath -= HandleFallingAfterHit;
            Release();
        }

        
        

        public override ITargetable GetLockedOnTarget()
        {
            // if (!CharacterManager.Instance.IsReady) return default;

            if (GetAIComponents().GetAILockOnController() == null)
            {
                Debug.LogError("Enemy Lock On Controller is null");
                return null;
            }

            var target = GetAIComponents().GetAILockOnController().GetTarget();

            if (target != null && target.Transform != Target)
                Target = target.Transform;

            // Target = target.GetCharComponents().GetHead().transform;

            return target;
        }

        public void EnterSpawnedState()
        {
            SwitchState(new EnemySpawnedState(this));
        }

        protected override void EnterStartingState()
        {
            // if (currentState is EnemySpawnedState)
            //     return;

            // EnterSpawnedState();

            var overrideController = GetAIComponents().GetOverrideStateController();

            if (overrideController.CheckIfGateAttackingOverride())
                SwitchState(new EnemyMoveToGateState(this));
            else if (overrideController.CheckIfStartingStateOverride())
                overrideController.SwitchToOverrideStartingState(this);
            else if (overrideController.CheckIfIdleOverride())
                overrideController.SwitchToOverrideIdleState(this);


            // SwitchState(new EnemyIdleState(this));
        }

        protected override void HandlePlayerStateChange(StateType newstatetype)
        {
            playerStateType = newstatetype;
        }

        protected override void RegisterWithCharacterManager()
        {
            CharacterManager.Instance.Register(this);
        }


        protected override void DisableCanvasGroup()
        {
            GetAIComponents().GetCanvasGroup().SetActive(false);
        }

        float updateInterval = 0.1f; // run every 100ms (10 FPS)
        float updateTimer = 0f;


        //TODO: MUST OPTIMIZE
        protected new void Update()
        {
            base.Update();

            updateTimer += Time.deltaTime;
            if (updateTimer < updateInterval) return;
            updateTimer = 0f;
            
            if (aiComponents.GetAILockOnController() == null) return;
            if(currentState == null) return;
            
            currentTarget = GetLockedOnTarget();
            currentState.SetCurrentTarget(currentTarget);


            // if (aiComponents.GetNavMeshAgentController().GetAgent().isOnOffMeshLink &&
            //     StateType != StateType.JumpOffMeshLink)
            // {
            //     SwitchState(new EnemyCrossLinkJumpState(this));
            // }

            ImpactTimer();
        }

        protected override void HandleTakeHit(IDamage iDamage)
        {
            if (AITestingControl.blockSwitchState) return;
            
            // stateMachineProcessor.TakeHit(iDamage, this);
            processor.TakeHit(iDamage, this);
            
        }

        protected override void HandleBlock(IDamage iDamage)
        {
            processor.HandleBlock(iDamage, this);
        }


        protected override void HandleDeath(IHaveHealth health)
        {
            EventBusEnemyController.EnemyDeath(name, this);
            enemyData.isDead = true;

            processor.HandleDead(this);

            // HighlightEffectController.ToggleHighlightEffect(HighlightEffectController.MultiuseHighlightEffect, false);

            DisableCanvasGroup();
            RemoveTokenAndQueue();
        }

        public void HandleExecution(int index)
        {
            SwitchState(new EnemyExecutedState(this, index));
            RemoveTokenAndQueue();
        }

        public void RemoveTokenAndQueue()
        {
            if (TokenManager.Instance)
            {
                if (currentToken != null)
                    ReturnToken();

                if (TokenManager.Instance.requesterQueue.Contains(this))
                    TokenManager.Instance.DeQueueRequester(this);
            }
        }


        public override void TriggerImpactTimer()
        {
            // if (!AIAttributes.CanCounterAction) return;

            if (!isStartcounterTimer)
                isStartcounterTimer = true;


            if (isStartcounterTimer)
            {
                impactCounter++;
                impactCounterTimer = 0;

                if (impactCounter >= AIAttributes.HitsBeforeCounterAction)
                {
                    //set to false from CounterImpact on Exit and within timer
                    counterActionReady = true;
                }
            }
        }


        void ImpactTimer()
        {
            // if (!AIAttributes.CanCounterAction) return;

            if(!isStartcounterTimer && currentTarget  == null) return;
    
                impactCounterTimer += Time.deltaTime;

                if (impactCounterTimer >= impactCounterTime)
                {
                    counterActionReady = false;
                    impactCounter = 0;
                    impactCounterTimer = 0;
                    isStartcounterTimer = false;
                }
            
        }

        void OnDrawGizmosSelected()
        {
            if (AIAttributes == null || base.GetAIComponents() == null ||
                GetAIComponents().GetNavMeshAgentController().GetAgent() == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AIAttributes.DetectionRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position,
                GetAIComponents().GetNavMeshAgentController().GetAgent().destination);
        }


#if UNITY_EDITOR

        [Button(ButtonSizes.Medium), GUIColor(.45f, .20f, 0f)]
        public void TestImpactState()
        {
            DamageData damageData = new DamageData();
            damageData.damage = 10;
            damageData.knockBackForce = 10;
            damageData.audioImpact = AudioImpact.Null;
            damageData.Direction = Vector3.zero;
            damageData.isShieldBreak = false;
            damageData.feedbackType = FeedbackType.Light;
            damageData.isExecution = false;
            damageData.knockDownForce = 10;

            Health.TakeHit(damageData);
        }

        [Button(ButtonSizes.Medium), GUIColor(.45f, .20f, 0f)]
        public void TestAbsoluteChase()
        {
            SwitchState(new EnemyAbsoluteChaseState(this));
        }

        [Button(ButtonSizes.Medium), GUIColor(.25f, .50f, 0f)]
        public void LoadComponents()
        {
            LoadCommonComponents();
            aiComponents = GetComponent<AIComponentHandler>();
            GetAIComponents().LoadAIComponents();
            ForceReceiver = GetComponent<AIForceReceiver>();
            ForceReceiver.LoadComponents();
        }
#endif

        public void Release()
        {
            if (ObjectPoolManager.Instance == null) return;
            ObjectPoolManager.Instance.ReleaseObject(this);
        }
    }
}