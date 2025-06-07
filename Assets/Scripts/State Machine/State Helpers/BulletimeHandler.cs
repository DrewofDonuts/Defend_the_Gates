using System.Collections;
using UnityEngine;

namespace Etheral
{
    /// <summary>
    /// To handle objects that you do not want to be affected by bullet time:
    ///     var _speed = speed;
    ///    if (Time.timeScale < 1) _speed /= Time.timeScale;
    /// 
    /// </summary>
    public class BulletimeHandler
    {
        PlayerBaseState playerBaseState;

        public BulletimeHandler(PlayerBaseState playerBaseState)
        {
            this.playerBaseState = playerBaseState;
        }

        public void StartBulletTime(float bulletTime = .2f, float timeAtBulletTime = .5f)

        {
            playerBaseState.stateMachine.StartCoroutine(
                BulletTimeAcceleration(bulletTime, timeAtBulletTime));
            playerBaseState.stateMachine.ToggleBulletTime();
        }

        public void EndBulletTime(float recoveryRate = 1)

        {
            Time.timeScale = Mathf.MoveTowards(.2f, 1, recoveryRate);
            Time.fixedDeltaTime = 0.02f;
            playerBaseState.stateMachine.ToggleBulletTime();
        }

        public IEnumerator BulletTimeAcceleration(float bulletTime, float timeMultiplierForLerp = .5f)
        {
            Time.timeScale = bulletTime;
            Time.fixedDeltaTime = bulletTime * 0.02f;

            float timer = 0f;

            while (Time.timeScale < 1)
            {
                timer += Time.unscaledDeltaTime * timeMultiplierForLerp;
                Time.timeScale = Mathf.Lerp(bulletTime, 1, timer);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                yield return null;
            }

            Time.timeScale = 1;
            playerBaseState.stateMachine.ToggleBulletTime();
        }
    }
}