using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    public class StatsBinder : MonoBehaviour, IBind<PlayerAbilityAndResourceData>
    {
        [SerializeField] string characterName;

        public string Name => characterName;
        public PlayerAbilityAndResourceData playerAbilityAndResourceData = new();

        int startingAtonement;
        public event Action OnNewCharacter;

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

                if (!_data.isSavedData)
                {
                    Debug.Log("Data is not saved data");
                    OnNewCharacter?.Invoke();

                    // Clear values, but do NOT assign a new instance
                    playerAbilityAndResourceData.Name = Name;
                    playerAbilityAndResourceData.playerStatsData = new PlayerStatsData();
                    playerAbilityAndResourceData.GatheredResources = new PermanentResources();
                }
            }
        }
        
    }
}