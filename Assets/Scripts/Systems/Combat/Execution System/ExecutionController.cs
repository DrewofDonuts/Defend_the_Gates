using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class ExecutionController : MonoBehaviour
    {
        [SerializeField] LockOnController lockOnController;

        [SerializeField] float executionCooldown = 5f;

        [Header("References")]
        [SerializeField] Image cooldownField;

        float executionTimer;
        bool isOnCooldown;


        void Update()
        {
            if (isOnCooldown)
            {
                UpdateExecutionCooldown(Time.unscaledDeltaTime);
            }
        }

        public void StartExecutionCooldown()
        {
            isOnCooldown = true;
            executionTimer = 0;
            cooldownField.fillAmount = 0f;
        }

        public void UpdateExecutionCooldown(float deltaTime)
        {
            if (isOnCooldown)
            {
                executionTimer += deltaTime;
                cooldownField.fillAmount = executionTimer / executionCooldown;

                if (executionTimer >= executionCooldown)
                {
                    cooldownField.fillAmount = 1f;
                    isOnCooldown = false;

                    StartCoroutine(PingPongCooldownOnce());
                }
            }
        }

        IEnumerator PingPongCooldownOnce()
        {
            float elapsed = 0f;
            float pulseDuration = 0.5f;
            float maxScale = 1.2f;

            while (elapsed < pulseDuration)
            {
                float t = Mathf.PingPong(elapsed * (1f / pulseDuration) * 2f, 1f);
                float scale = Mathf.Lerp(1f, maxScale, t);
                cooldownField.transform.localScale = new Vector3(scale, scale, 1f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            cooldownField.transform.localScale = Vector3.one; // reset scale just in case
        }


        public bool CheckIfCanExecute(EnemyStateMachine _enemyStateMachine, PlayerStateMachine stateMachine)
        {
            if (isOnCooldown)
                return false;

            if (CheckStateMachinesIfCanExecute(_enemyStateMachine, stateMachine))
                return false;

            if (!IsTargetInFront(stateMachine.transform, _enemyStateMachine.transform, 77f))
                return false;

            if (!CalculateExecution(_enemyStateMachine, stateMachine))
                return false;

            if (!DamageUtil.CalculateIfInRange(stateMachine.transform, _enemyStateMachine.transform, 2.1f))
                return false;


            StartExecutionCooldown();

            return true;
        }


        bool CalculateExecution(EnemyStateMachine _enemyStateMachine, PlayerStateMachine stateMachine)
        {
            return _enemyStateMachine.AIAttributes.CanBeExecuted && !_enemyStateMachine.Health.IsDead &&
                   _enemyStateMachine.Health.IsLowHealth;
        }


        bool CheckStateMachinesIfCanExecute(EnemyStateMachine _enemyStateMachine, PlayerStateMachine stateMachine)
        {
            return _enemyStateMachine == null || _enemyStateMachine.Health == null ||
                   _enemyStateMachine.Health.IsDead || _enemyStateMachine.Health.IsSturdy;
        }

        bool IsTargetInFront(Transform origin, Transform target, float angleThreshold)
        {
            Vector3 directionToTarget = (target.position - origin.position).normalized;
            Vector3 forward = origin.transform.forward;

            // Calculate the angle between forward and directionToTarget
            float angle = Vector3.Angle(forward, directionToTarget);

            Debug.Log("Angle: " + angle);

            return angle <= angleThreshold;
        }
    }
}