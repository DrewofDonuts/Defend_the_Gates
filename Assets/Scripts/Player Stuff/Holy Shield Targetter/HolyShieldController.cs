using UnityEngine;
using UnityEngine.Serialization;


namespace Etheral
{
    public class HolyShieldController : MonoBehaviour
    {
        [SerializeField] public RotatingTargetter rotatingTargetter;
        [field: SerializeField] public HolyShield HolyShieldPrefab { get; private set; }

        [field: Header("Targetter Settings")]
        [field: SerializeField] public Transform TargeterPositionToSpawnShield { get; private set; }


        public void EnableTargetter()
        {
            rotatingTargetter.gameObject.SetActive(true);
        }

        public void DisableTargetter()
        {
            rotatingTargetter.gameObject.SetActive(false);
        }

        public void InstantiateShield()
        {
            var holySHield = Instantiate(HolyShieldPrefab, TargeterPositionToSpawnShield.position,
                rotatingTargetter.transform.rotation);
            
            holySHield.transform.position = TargeterPositionToSpawnShield.position;
            holySHield.transform.rotation = rotatingTargetter.transform.rotation;
            
            

            Debug.Log($"Shield Position: {TargeterPositionToSpawnShield.position}");
        }
    }
}