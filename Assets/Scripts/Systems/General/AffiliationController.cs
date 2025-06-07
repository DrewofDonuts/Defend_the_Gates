using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    [DisallowMultipleComponent]
    public class AffiliationController : MonoBehaviour
    {
        [SerializeField] Affiliation affiliation;

        void Start()
        {
            SetAffiliation();
        }
        
        void SetAffiliation()
        {
            var components = GetComponentsInChildren<IAffiliate>();
            var siblingComponents = GetComponents<IAffiliate>();

            foreach (var component in components)
            {
                component.Affiliation = affiliation;
            }

            foreach (var component in siblingComponents)
            {
                component.Affiliation = affiliation;
            }
        }

#if UNITY_EDITOR

        [Button]
        void SetAffiliationEditor()
        {
            SetAffiliation();
        }

#endif
    }
}