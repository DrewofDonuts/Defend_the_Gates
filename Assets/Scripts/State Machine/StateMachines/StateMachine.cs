using System;
using System.Collections;
using Interfaces;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public abstract class StateMachine : MonoBehaviour
    {
        public State currentState { get; protected set; }
        public StateType StateType;

        public delegate void OnStateChangeHandler(StateType newStateType);
        public event OnStateChangeHandler OnStateChange;

        [BoxGroup("Components")]
        [SerializeField] ComponentHandler components;

        [field: FoldoutGroup("Common Components")]
        [field: SerializeField] public Animator Animator { get; protected set; }

        [field: FoldoutGroup("Common Components")]
        [field: SerializeReference] public Health Health { get; protected set; }
        [field: FoldoutGroup("Common Components")]
        [field: SerializeField] public WeaponInventory WeaponInventory { get; protected set; }

        [field: FoldoutGroup("Common Components")]
        [field: SerializeField] public CharacterAudio CharacterAudio { get; protected set; }

        [field: FoldoutGroup("Common Components")]
        [field: SerializeField] public WeaponHandler WeaponHandler { get; protected set; }


        // [field: FoldoutGroup("Common Components")]
        // [field: SerializeField] public CharacterAttributes BaseAttributes { get; protected set; }

        public ComponentHandler GetCharComponents() => components;

        // [field: SerializeField] public HighlightEffect HighlightEffect { get; protected set; }

        [FoldoutGroup("Testing")]
        public float testFloat;
        [field: FoldoutGroup("Testing")]
        [field: SerializeField] public Image stateIndicator { get; set; }
        [field: FoldoutGroup("Testing")]
        [field: SerializeField] public Transform Target { get; protected set; }

        [Header("Temp until Multiplayer Implementation")]
        public int charID;


        public SurfaceType CurrentSurface { get; protected set; }
        public event Action<SurfaceType> SurfaceChangeEvent;

        public bool ActiveAbility { get; protected set; }

        public event Action<int> OnAnimationFeedback;

        protected abstract void HandleDeath(IHaveHealth health);
        protected abstract void HandleTakeHit(IDamage iDamage);

        protected abstract void HandleBlock(IDamage iDamage);

        // protected abstract void HandleTakeGroundHit(IDamage iDamage);
        public abstract void HandleGettingBlocked();

        public abstract void TriggerImpactTimer();
        public abstract void AddForce(Vector3 force);

        public void TriggerFeedbackFromAnimation(int feedbackIntensity)
        {
            OnAnimationFeedback?.Invoke(feedbackIntensity);
        }

        protected virtual void OnEnable()
        {
            if (Health == null) return;
            Health.OnTakeHit += HandleTakeHit;
            Health.OnBlock += HandleBlock;
            Health.OnDie += HandleDeath;

            // Health.OnExecuted += DisableCanvasGroup;

            // components.GetCC().detectCollisions = false;
        }


        void OnDisable()
        {
            if (Health == null) return;
            Health.OnTakeHit -= HandleTakeHit;
            Health.OnBlock -= HandleBlock;
            Health.OnDie -= HandleDeath;

            // Health.OnExecuted -= DisableCanvasGroup;
        }

        public void OnChangeStateMethod(StateType newStateType)
        {
            OnStateChange?.Invoke(newStateType);
            StateType = newStateType;
        }

        public StateType GetStateType() => StateType;
        public StateType SetStateType(StateType newStateType) => StateType = newStateType;

        protected abstract void DisableCanvasGroup();


        protected void Update()
        {
            if (GameMenu.Instance != null && GameMenu.Instance.isPause) return;

            currentState?.Tick(Time.deltaTime);
        }

        public virtual void SwitchState(State newState)
        {
            if (Health.IsDead) return;
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }

        protected virtual void RegisterWithCharacterManager() { }
        protected virtual void DeRegisterWithCharacterManager() { }


        public void SetSurface(SurfaceType surfaceType)
        {
            CurrentSurface = surfaceType;
            SurfaceChangeEvent?.Invoke(CurrentSurface);
        }

        public bool CheckFrontAngle(Transform targetObject, float maxAngle, float maxDistance)
        {
            Vector3 direction = targetObject.position - transform.position;

            // Get the angle between the forward vector of this object and the direction vector
            float angle = Vector3.Angle(transform.forward, direction);

            // Get the distance between this object and the target object
            float distance = Vector3.Distance(transform.position, targetObject.position);

            // Check if the target object is within the specified distance angle
            return angle >= 0 && angle <= maxAngle && distance < maxDistance;
        }

        public bool AreGameObjectsFacingEachOther(Transform obj1, Transform obj2, float maxAngle)
        {
            Vector3 directionToOther = (obj2.position - obj1.position).normalized; // Direction from obj1 to obj2
            float angle =
                Vector3.Angle(obj1.forward, directionToOther); // Angle between obj1's forward and the direction to obj2

            // Check if obj1 is facing towards obj2 within the threshold angle
            return angle <= maxAngle;
        }

        public void SetActiveAbility(bool ability)
        {
            ActiveAbility = ability;
        }

        protected IEnumerator WaitForCharacterManager()
        {
            yield return new WaitUntil(() => CharacterManager.Instance != null);
        }


#if UNITY_EDITOR
        public void LoadCommonComponents()
        {
            Animator = GetComponent<Animator>();
            WeaponInventory = GetComponent<WeaponInventory>();
            WeaponInventory.LoadComponents();
            WeaponHandler = GetComponent<WeaponHandler>();
            WeaponHandler.LoadComponents();
            Health = GetComponent<Health>();
            CharacterAudio = GetComponentInChildren<CharacterAudio>();

            // Health.LoadStatBars();
            WeaponInventory.LoadComponents();
            WeaponHandler.LoadComponents();
        }
#endif
        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;
    }
}