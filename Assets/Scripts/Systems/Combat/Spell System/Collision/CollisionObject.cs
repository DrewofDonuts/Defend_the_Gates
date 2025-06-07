using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    public class CollisionObject : MonoBehaviour
    {
        [InlineButton("CreateCollisionConfig", "New")]
        [SerializeField] AudioSource audioSource;



        public void Init(AudioClip audioClip, float lifeTime)
        {
            AudioProcessor.PlaySingleOneShot(audioSource, audioClip, AudioType.spellImpact,.75f, 1.25f);
            Destroy(gameObject, lifeTime);
        }


#if UNITY_EDITOR

        public void CreateCollisionConfig()
        {
            var collisionConfig = AssetCreator.NewCollisionConfig();
            UnityEditor.AssetDatabase.SaveAssets();
        }


#endif
    }
}