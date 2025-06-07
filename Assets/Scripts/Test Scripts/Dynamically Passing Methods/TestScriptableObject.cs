using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(menuName = "Etheral/Test/States", fileName = "Test SO")]
    public class TestScriptableObject : ScriptableObject
    {
        [SerializeField] EnemyBaseState EnemyCounterActionState;

        [SerializeReference] public List<EnemyBaseState> EnemyBaseStates;


        // [ContextMenu(nameof(AddImpactCounter))]
        // void AddImpactCounter()
        // {
        //     EnemyBaseStates.Add( new EnemyImpactCounterState());
        // }


    }
    
    
    
}