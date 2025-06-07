using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    [CreateAssetMenu(fileName = "CollisionConfig", menuName = "Etheral/Ability System/Collision Config")]
    [InlineEditor]
    public class CollisionConfig : ScriptableObject
    {
        public AudioClip audioClip;
        public CollisionObject effectPrefab;
        public float offset;
        public float lifetime = 2f;

        public void InstantiateEffect(Collider other, Vector3 projectilePosition)
        {
            // // var contact = other.ClosestPointOnBounds(effectPrefab.transform.position);
            // var contact = other.ClosestPoint(projectilePosition);
            var effect = Instantiate(effectPrefab, projectilePosition, Quaternion.identity);

            effect.Init(audioClip, lifetime);
        }
    }
}