using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public abstract class AIStateMachine : StateMachine
    {
        [BoxGroup("Components")]
        [SerializeField] protected AIComponentHandler aiComponents;
        [field: SerializeField] public AIAttributes AIAttributes { get; protected set; }

        [field: FoldoutGroup("Common Components")]
        [field: SerializeField] public AIForceReceiver ForceReceiver { get; protected set; }

        [field: FoldoutGroup("Testing")]
        [field: SerializeField] public AITestingControl AITestingControl { get; protected set; }


        [field: FoldoutGroup("Token  and Attack Stuff")]
        [SerializeField] bool isHostile;
        [field: FoldoutGroup("Token  and Attack Stuff")]
        [field: SerializeField] public bool UsesToken { get; private set; }
        [field: FoldoutGroup("Token  and Attack Stuff")]
        protected AttackToken currentToken;
        [field: FoldoutGroup("Token  and Attack Stuff")]
        public int attacksBeforeRetreat = 1;
        [field: FoldoutGroup("Token  and Attack Stuff")]
        public int currentAttackCount = 0;
        [field: FoldoutGroup("Token  and Attack Stuff")]
        public string attackTokenName;

        protected ITargetable currentTarget;

        protected float targetCheckInterval = 0.25f;
        protected float targetCheckTimer;


        #region PlayerStuff
        protected PlayerStateMachine player;
        public PlayerStateMachine GetPlayer() => player;
        public Vector3 GetPlayerPosition() => player.transform.position;

        protected StateType playerStateType;
        public StateType GetPlayerStateType() => playerStateType;
        #endregion

        public AIComponentHandler GetAIComponents() => aiComponents;
        public virtual void SetHostile(bool value) => isHostile = value;
        public virtual bool GetHostile() => isHostile;


        protected abstract void EnterStartingState();
        protected abstract void HandlePlayerStateChange(StateType newstatetype);
        public abstract ITargetable GetTarget();


        protected virtual IEnumerator Start()
        {
            // EnterStartingState();
            player = EventBusPlayerController.PlayerStateMachine;

            if (CharacterManager.Instance != null)
            {
                yield return new WaitUntil(() => CharacterManager.Instance.IsReady);
                {
                    // SetPlayer();
                    SubscribeToPlayerStateChange();
                    RegisterWithCharacterManager();

                    // GetTarget();
                }
            }
        }


        public int GetCurrentAttackCount() => currentAttackCount;

        public virtual bool RequestToken()
        {
            if (TokenManager.Instance)
                return TokenManager.Instance.RequestToken(this);

            Debug.LogError("Token Manager is not available");
            return false;
        }

        public void AssignToken(AttackToken token)
        {
            currentToken = token;
            attackTokenName = currentToken.TokenName;
        }


        public virtual AttackToken GetCurrentAttackToken()
        {
            return currentToken;
        }

        public void TrackAttacksForToken()
        {
            // if (!UsesToken) return;
            currentAttackCount++;
        }


        public bool CheckIfShouldRetreat()
        {
            //If doesn't use token, never retreat
            if (!UsesToken || !TokenManager.Instance) return false;

            return CheckIfShouldReturnToken() && TokenManager.Instance.AreThereZeroTokensLeft();
        }

        public bool CheckIfShouldReturnToken()
        {
            if (!UsesToken || !TokenManager.Instance) return false;

            if (currentAttackCount >= attacksBeforeRetreat)
            {
                ReturnToken();
                return true;
            }

            return false;
        }

        public virtual void ReturnToken()
        {
            if (!UsesToken) return;
            TokenManager.Instance.ReturnToken(currentToken);
            currentToken = null;
            attackTokenName = null;
            currentAttackCount = 0;
        }

        void SubscribeToPlayerStateChange()
        {
            player.OnStateChange += HandlePlayerStateChange;
        }

        protected virtual void OnDestroy()
        {
            if (player != null)
                player.OnStateChange -= HandlePlayerStateChange;
        }

        public void ResetAttackCount()
        {
            currentAttackCount = 0;
        }

        public override void HandleGettingBlocked()
        {
            //Logic for when the AI gets blocked
        }

        public override void AddForce(Vector3 force)
        {
            if (ForceReceiver.enabled == false)
                ForceReceiver.enabled = true;
            ForceReceiver.AddForce(force);
        }
    }
}