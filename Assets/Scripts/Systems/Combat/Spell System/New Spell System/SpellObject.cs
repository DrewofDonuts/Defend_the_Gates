using System;
using System.Collections.Generic;
using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Etheral
{
    [RequireComponent(typeof(Rigidbody), typeof(AudioSource))]
    public class SpellObject : MonoBehaviour, IAmPoolObject<SpellObject>
    {
        [Header("Object Pool Settings")]
        [field: SerializeField] public SpellObject Prefab { get; set; }
        [field: SerializeField] public string PoolKey { get; set; }


        [Header("Spell Settings")]
        [SerializeField]
        [SerializeReference] public List<BaseEffect> Effects = new();
        [SerializeField] GameObject[] childObjects;
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [SerializeField] AudioSource audioSource;
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [SerializeField] new Collider collider;
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [SerializeField] Rigidbody rigidBody;

        [Header("IDamage Settings")]
        public DamageData DamageData { get; private set; } = new();

        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;
        public AudioSource AudioSource => audioSource;
        
        ObjectPool<SpellObject> objectPool;

        void Start()
        {
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            if (childObjects.Length == 0)
            {
                int childCount = transform.childCount;
                childObjects = new GameObject[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    childObjects[i] = transform.GetChild(i).gameObject;
                }
            }
        }

        public void DisableChildObjects()
        {
            foreach (var childObject in childObjects)
            {
                childObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            Release();
        }

        public void Release()
        {
            ObjectPoolManager.Instance.ReleaseObject(this);
        }

        
        public void InitializeSpellObject(ICastSpell iCastSpell)
        {
            // Spell.InitializeSpells(this, caster);
            foreach (var spell in Effects)
            {
                spell.Initialize(this, iCastSpell);
            }
        }

        void Update()
        {
            var time = Time.deltaTime;

            foreach (var effect in Effects)
            {
                effect.Tick(time);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            foreach (var effect in Effects)
            {
                effect.ApplyOnTriggerEnter(other);
            }
        }

        void OnTriggerStay(Collider other)
        {
            foreach (var effect in Effects)
            {
                effect.ApplyOnStay(other);
            }
        }

#if UNITY_EDITOR

        void OnValidate()
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            audioSource = GetComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }

        [ContextMenu("Add Projectile Effect")] void AddProjectileEffect() => Effects.Add(new MoveForwardEffect());

        [ContextMenu("Add Heal Over Time Effect")]
        void AddHealOverTimeEffect() => Effects.Add(new HealEffects());

        [ContextMenu("Add Collision Damage Effect")]
        void AddHealEffect() => Effects.Add(new DamageOnHitEffect());

        [ContextMenu("Add Collision DOT Effect")]
        void AddAreaDamageEffect() => Effects.Add(new DOTOnHitEffect());

#endif
    }
}