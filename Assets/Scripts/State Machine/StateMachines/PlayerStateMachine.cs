using System;
using System.Collections;
using Etheral.DefendTheGates;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Etheral
{
    public class PlayerStateMachine : StateMachine, IGameStateListener
    {
        [field: BoxGroup("Components")]
        [field: SerializeField] public PlayerComponents PlayerComponents { get; private set; }

        [field: FoldoutGroup("Player Stuff")]
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: FoldoutGroup("Player Stuff")]
        [field: SerializeField] public PlayerAttributes PlayerCharacterAttributes { get; private set; }
        [field: FoldoutGroup("Player Stuff")]
        [field: SerializeField] public LayerMask LayerToIgnore { get; private set; }
        [field: FoldoutGroup("Player Stuff")]
        [field: SerializeField] public bool IsGodMode { get; private set; }
        [field: FoldoutGroup("Common Components")]
        [field: SerializeField] public ForceReceiver ForceReceiver { get; protected set; }
        [field: FoldoutGroup("Common Components")]
        [Header("Testing")]
        public Transform followTarget;
        public bool isThirdPerson;


        public GameState CurrentGameState { get; set; }
        readonly PlayerProcessor processor = new();

        public bool isWalking { get; private set; }

        float impactTimer;
        float timeBetweenImpacts = .50f;
        bool isStartImpactTimer;
        public bool canBeImpacted { get; private set; } = true;

        public bool IsBulletTime { get; private set; }

        //testing
        public bool isBeingGroundAttacked { get; private set; }


        public void SetIsBeingGroundAttacked(bool isGroundAttacking)
        {
            isBeingGroundAttacked = isGroundAttacking;
        }

        void Awake()
        {
            EventBusPlayerController.PlayerStateMachine = this;

            //Disable character Controller collision
        }


        void Start()
        {
            Animator.updateMode = AnimatorUpdateMode.Normal;

            RegisterWithCharacterManager();

            SwitchState(new PlayerIdleState(this));
            PlayerComponents.LockOnController.OnCurrentTarget += SetCurrentTarget;

            StartCoroutine(WaitForCharacterManager());

            EventBusPlayerController.OnWalkEvent += SetIfWalking;
            EventBusPlayerController.OnGroundAttackingEvent += SetIsBeingGroundAttacked;

            if (PlayerCharacterAttributes == null)
                Debug.LogError("Character Attributes is null in PlayerStateMachine");

            InputReader.PauseEvent += OnPauseButtonDown;
            RegisterListener();
        }

        void OnDestroy()
        {
            EventBusPlayerController.OnWalkEvent -= SetIfWalking;
            EventBusPlayerController.OnGroundAttackingEvent -= SetIsBeingGroundAttacked;
            InputReader.PauseEvent -= OnPauseButtonDown;
            PlayerComponents.LockOnController.OnCurrentTarget -= SetCurrentTarget;
            UnregisterListener();
        }


        void OnPauseButtonDown()
        {
            GameMenu.Instance.OnPause();

            if (GameMenu.Instance.isPause)
                SwitchState(new PlayerUIState(this));
            else
                SwitchState(new PlayerIdleState(this));
        }


        public void SetCurrentTarget(ITargetable currentEnemyTarget)
        {
            if (CombatManager.Instance != null)
                CombatManager.Instance.SetPlayerCurrentTarget(currentEnemyTarget);
        }

        protected override void HandleTakeHit(IDamage iDamage)
        {
            processor.TakeHit(iDamage, this);
        }

        void NewImpactTimer()
        {
            if (isStartImpactTimer)
            {
                impactTimer += Time.deltaTime;

                if (impactTimer >= timeBetweenImpacts)
                {
                    canBeImpacted = true;
                    isStartImpactTimer = false;
                }
                else
                {
                    canBeImpacted = false;
                }
            }
        }

        public void SetIfWalking(bool shouldWalk)
        {
            isWalking = shouldWalk;
        }


        protected override void RegisterWithCharacterManager()
        {
            if (CharacterManager.Instance == null) return;
            CharacterManager.Instance.Register(this);
        }

        protected override void DeRegisterWithCharacterManager()
        {
            CharacterManager.Instance.Unregister(this);
        }

        public override void HandleGettingBlocked() { }

        public override void TriggerImpactTimer()
        {
            //If the timer is not started, start it
            if (!isStartImpactTimer)
                isStartImpactTimer = true;

            //Reset the timer
            if (isStartImpactTimer)
            {
                impactTimer = 0;
            }
        }

        public override void AddForce(Vector3 force)
        {
            ForceReceiver.AddForce(force);
        }


        protected override void DisableCanvasGroup() => PlayerComponents.GetCanvasGroup().SetActive(false);

        new void  Update()
        {
            base.Update();

            // SyncOrbitalCameras();

            // if (IsBulletTime)
            //     CompensateForTimeScaleInAnimator();
            // else
            // {
            //     Animator.speed = 1;
            // }

            NewImpactTimer();

            // ImpactTimer();
            // RotateAim();
        }

        void CompensateForTimeScaleInAnimator()
        {
            // Animator.speed = Time.timeScale + .50 so that the animation is not too fast
            Animator.speed = Mathf.Min(Time.timeScale / 1 + .50f, 1);
        }

        public void ToggleBulletTime()
        {
            if (!IsBulletTime)
                IsBulletTime = true;
            else
                IsBulletTime = false;

            // Time.timeScale = IsBulletTime ? 0.5f : 1f;
        }


        protected override void HandleBlock(IDamage iDamage)
        {
            processor.HandleBlock(iDamage, this);
        }

        public override void SwitchState(State state)
        {
            // if (gameObject.CompareTag("Player") && DialogueManager.isConversationActive) return;
            base.SwitchState(state);
        }

        protected override void HandleDeath(IHaveHealth health)
        {
            CharacterAudio.PlayRandomOneShot(CharacterAudio.OneShotSource, CharacterAudio.AudioLibrary.DeathBlow,
                AudioType.death);

            ForceReceiver.SetIgnoreLedgeCheck(true);

            SwitchState(new PlayerDeadState(this));

            // Health.SetExecution();

            DisableCanvasGroup();

            Health.ResetHealthValueAfterDeath();
            SetCharacterControllerCollisionLayer(true);
            StartCoroutine(WaitBeforeChangeSceneAfterDeath());
        }

        IEnumerator WaitBeforeChangeSceneAfterDeath()
        {
            yield return new WaitForSeconds(4f);
            EtheralSceneManager.Instance.RestartScene();
        }


        public void SetCharacterControllerCollisionLayer(bool shouldIgnore)
        {
            if (shouldIgnore)
            {
                PlayerComponents.GetCC().excludeLayers = PlayerComponents.GetCharacterCollision().LayersToIgnore;
            }
            else
            {
                PlayerComponents.GetCC().excludeLayers = default;
            }
        }


        public void OnGameStateChanged(GameState newGameState)
        {
            Debug.Log($"OnGameStateChanged: {newGameState}");
            CurrentGameState = newGameState;
            ChangePerspective(newGameState is GameState.AttackPhase or GameState.ExplorePhase or GameState.WavePhase );
        }

        public void RegisterListener() => GameStateManager.Instance?.RegisterListener(this);
        public void UnregisterListener() => GameStateManager.Instance?.UnregisterListener(this);


        public void ChangePerspective(bool _isThirdPerson)
        {
            if (_isThirdPerson)
            {
                SwitchState(new PlayerOffensiveState(this));
                EventBusPlayerController.NearCamera(this);
            }
            else
            {
                SwitchState(new PlayerTowerMovementState(this));
                EventBusPlayerController.FarCamera(this);
            }
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), GUIColor(.25f, .50f, 0f)]
        public void LoadComponents()
        {
            LoadCommonComponents();
            PlayerComponents = GetComponent<PlayerComponents>();

            InputReader = GetComponentInChildren<InputReader>();
        }

        [Button("Switch to PlayerOffensiveState")]
        public void SwitchToOffensiveState()
        {
            SwitchState(new PlayerOffensiveState(this));
        }


#endif

    }
}