using System;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public static class UIFillUtility
    {
        //!!! You MUST pass a float from the calling code and reassign it to the return value!!!

        
        /// <summary>
        /// Call every frame to update a radial fill.
        /// </summary>
        /// <param name="radialFill">The Image to update.</param>
        /// <param name="isHolding">Whether the user is currently holding the button.</param>
        /// <param name="holdProgress">The current progress value (0 to 1).</param>
        /// <param name="holdTime">Time in seconds required to complete the fill.</param>
        /// <param name="resetSpeed">Speed at which the fill resets when not held.</param>
        /// <param name="onComplete">Callback invoked when holdProgress reaches 1.</param>
        /// <returns>The updated holdProgress value (should be reassigned by caller).</returns>
        /// 
        public static float UpdateRadialFill(Image radialFill, bool isHolding, float holdProgress, float holdTime, float resetSpeed, Action onComplete)
        {
            if (isHolding)
            {
                holdProgress += Time.deltaTime / holdTime;
                if (holdProgress >= 1f)
                {
                    holdProgress = 0f;
                    onComplete?.Invoke();
                }
            }
            else if (holdProgress > 0f)
            {
                holdProgress -= Time.deltaTime * resetSpeed;
            }

            if (radialFill != null)
                radialFill.fillAmount = Mathf.Clamp01(holdProgress);

            return Mathf.Clamp01(holdProgress);
        }
    }
}