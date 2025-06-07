using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "EnemyObjectData", menuName = "Etheral/CharactersAI/Enemy Object Data")]
    public class EnemyPrefabSO : ScriptableObject
    {
        [SerializeField] EnemyStateMachine enemyStateMachine;
        public EnemyStateMachine EnemyStateMachine => enemyStateMachine;
    }
}