using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Object Container", menuName = "Etheral/Data Objects/Prefab Container")]
    public class PrefabContainer : ScriptableObject
    {
        [SerializeField] string title;

        [TextArea(3, 8)]
        [SerializeField] string description;

        [field: SerializeField] public GameObject[] Prefabs { get; private set; }
    }
}