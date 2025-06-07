using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "AbilityUnlockData", menuName = "Etheral/Rewards/AbilityUnlockData")]
    public class AbilityUnlockData : ScriptableObject
    {
        [SerializeField] Sprite abilitySprite;
        [SerializeField] string abilityTitle;
        [SerializeField] string abilityDescription;
        [SerializeField] int startingAbilityCost;
        [SerializeField] PlayerAbilityTypes abilityType;
        
        public Sprite AbilitySprite => abilitySprite;
        public string AbilityTitle => abilityTitle;
        public string AbilityDescription => abilityDescription;
        public int StartingAbilityCost => startingAbilityCost;
        public PlayerAbilityTypes AbilityType => abilityType;
    }
}