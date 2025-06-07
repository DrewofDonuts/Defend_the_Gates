using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    /// <summary>
    /// Checks if there is an override for the idle state.
    /// </summary>
    /// <returns>True if there is an idle state override, otherwise false.</returns>
    /// 
    public class StateOverrideController : MonoBehaviour
    {
        [SerializeField] BaseAIStateOverrides _aiStateOverrides;


        public BaseAIStateOverrides GetStateOverrides()
        {
            if (_aiStateOverrides != null)
                return _aiStateOverrides;

            return default;
        }

        public bool CheckIfGateAttackingOverride()
        {
            if (_aiStateOverrides == null)
                return false;
            return _aiStateOverrides.IsGateAttacking;
        }

        public bool CheckIfStartingStateOverride()
        {
            if (_aiStateOverrides == null)
                return false;
            return _aiStateOverrides.IsOverrideStartingState;
        }


        public bool CheckIfIdleOverride()
        {
            if (_aiStateOverrides == null)
                return false;
            return _aiStateOverrides.IsOverrideIdleState;
        }


        //ST
        public void SwitchToOverrideStartingState(EnemyStateMachine stateMachine) =>
            _aiStateOverrides.StartingOverrideState(stateMachine);


        public void SwitchToOverrideIdleState(EnemyStateMachine stateMachine) =>
            _aiStateOverrides.IdleOverrideState(stateMachine);
    }
}