using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    /// <summary>
    /// This is used to bind the RunData to the GameManager
    /// When a run completes, the RunData should be wiped clean
    /// Future state will save the RunData for total runs completed and total resources gathered
    /// </summary>
    public class RunBinder : MonoBehaviour, IBind<PlayerAbilityAndResourceData>, INamed
    {
        [FormerlySerializedAs("runData")] public PlayerAbilityAndResourceData playerAbilityAndResourceData = new PlayerAbilityAndResourceData();
        public event Action OnNewRun;

        int startingAtonement;

        void Awake()
        {
            if (playerAbilityAndResourceData == null)
            {
                Debug.Log("RunData is null");
                playerAbilityAndResourceData = new PlayerAbilityAndResourceData { Name = name };
            }
        }

        public void Bind(PlayerAbilityAndResourceData _data)
        {

            if (_data != null)
            {
                playerAbilityAndResourceData = _data;
            }

            //If RunData doesn't exist at all, create a new one
            if (playerAbilityAndResourceData == null)
            {
                playerAbilityAndResourceData = new PlayerAbilityAndResourceData { Name = name };

                if (_data != null)
                {
                    playerAbilityAndResourceData = _data;
                }
            }
            
            //Disabled because data is no longer available 04/11/2025
            //
            // if (!playerAbilityAndResourceData.IsRunActive)
            //     OnNewRun?.Invoke();
            //
            // playerAbilityAndResourceData.IsRunActive = true;
        }


        public string Name { get; set; }
    }
}