using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using RayFire;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;


namespace Etheral
{
    [RequireComponent(typeof(AudioSource))]
    public class Destructible : TriggerSuccessOrFailureMonoBehavior, ITakeHit
    {
        [BoxGroup("Configuration")]
        [SerializeField] bool destroyOnCollision;

        [BoxGroup("Configuration")]
        [SerializeField] float health;

        [BoxGroup("Configuration")]
        [SerializeField] GameObject destroyedFX;

        [BoxGroup("Configuration")]
        [SerializeField] GameObject damageFX;

        [BoxGroup("Configuration")]
        [SerializeField] AudioMixerGroup audioMixerGroup;

        [BoxGroup("Configuration")]
        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation affiliation) => Affiliation = affiliation;


        [BoxGroup("Configuration")]
        [SerializeField] float minimumForce = 20;

        [BoxGroup("Configuration")]
        [SerializeField] ColliderTypes colliderType;


        [FoldoutGroup("Components")]
        [SerializeField] AudioSource audioSource;

        [BoxGroup("Configuration")]
        [SerializeField] ObjectAudio objectAudio;

        [FoldoutGroup("Components")]
        [SerializeField] Rigidbody rigidBody;

        [FoldoutGroup("Components")]
        [SerializeField] Renderer childRenderer;


        [FoldoutGroup("Components")]
        [SerializeField] MMF_Player hitFeedback;

        [FoldoutGroup("Components")]
        [SerializeField] MMF_Player destroyFeedback;


        [FoldoutGroup("Components")]
        [SerializeField] RayfireRigid rigid;

        [FoldoutGroup("RigidFire Settings")]
        [SerializeField] MaterialType materialType;

        [FoldoutGroup("RigidFire Settings")]
        [LabelText("MUST BE RUNTIME IF TOGGLING GRAVITY")]
        [SerializeField] DemolitionType demolitionType;

        [FoldoutGroup("RigidFire Settings")]
        [SerializeField] bool isGravityAtStart = true;

        [FoldoutGroup("RigidFire Settings")]
        [SerializeField]
        public event Action OnDie;
        public bool isHooked { get; set; }

        public List<RayfireRigid> fragmentList = new();

        RayfireRigid destroyedRigid;

        bool alreadyDestroyed;


        void OnEnable()
        {
            if (rigidBody != null)
            {
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
            }

            if (rigid != null)
            {
                rigid.demolitionEvent.LocalEvent += GetReferenceToFragments;

                rigid.fading.fadeType = RayFire.FadeType.SimExclude;
                rigid.fading.fadeTime = 1f;
                rigid.fading.lifeTime = 3f;
            }

            audioSource.outputAudioMixerGroup = audioMixerGroup;

            rigid.Initialize();

            if (destroyOnCollision)
                GetComponent<Collider>().isTrigger = true;
        }

        void OnDisable()
        {
            // RFDemolitionEvent.GlobalEvent -= HandleFragments;
            if (rigid != null)
                rigid.demolitionEvent.LocalEvent -= GetReferenceToFragments;

            if (rigidBody != null)
            {
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (destroyOnCollision && !alreadyDestroyed)
            {
                if (other.TryGetComponent(out StateMachine stateMachine))
                {
                    HandleCollisionDemolish();
                }
            }
        }

        [ContextMenu("Destroy")]
        public void DemolishObject()
        {
            rigid.Demolish();
        }


        public void TakeHit(IDamage damage, float angle)
        {
            Debug.Log("Take Hit");
            HandleDamage(damage);
            if (hitFeedback != null)
                hitFeedback.PlayFeedbacks();
        }

        public void TakeDotDamage(float damage)
        {
            health -= damage;

            if (health <= 0)
                HandleDeadByDOT();
        }


        public void HandleDamage(IDamage damage)
        {
            health -= damage.Damage;
            HandleHitAudio();

            if (health <= 0)
                HandleDeadByAttack(damage);
        }

        void HandleCollisionDemolish()
        {
            alreadyDestroyed = true;
            DemolishObject();
            DestroyAudio();
            DestroyGameObject();
        }

        void HandleDeadByDOT()
        {
            if (!rigid.physics.gr)
                rigid.physics.gr = true;
            
            DemolishObject();
            HandleEvents();
            HandleRigidBody();
            HandleFeedback();
        }


        void HandleDeadByAttack(IDamage damage)
        {
            if (!rigid.physics.gr)
                rigid.physics.gr = true;

            DemolishObject();
            HandleEvents();
            HandleRigidBody();
            HandleFeedback();

            if (damage != null)
                HandleFragmentKnockback(damage);


            var target = GetComponent<ITargetable>();
            if (target != null)
                target.HandleDeadTarget();

            DestroyGameObject();
        }

        void DestroyGameObject()
        {
            Destroy(gameObject, 5f);
        }

        void HandleFeedback()
        {
            if (destroyFeedback != null)
                destroyFeedback.PlayFeedbacks();
        }

        void HandleEvents()
        {
            OnTriggerEvent();
            OnDie?.Invoke();
        }

        void HandleRigidBody()
        {
            if (rigidBody != null)
            {
                rigidBody.isKinematic = false;
                rigidBody.useGravity = true;
            }
        }


        void GetReferenceToFragments(RayfireRigid rigid)
        {
            destroyedRigid = rigid;

            foreach (var debri in destroyedRigid.fragments)
            {
                debri.physics.gr = true;
                fragmentList.Add(debri);
            }
        }

        void HandleFragmentKnockback(IDamage damage)
        {
            if (damage != null)
            {
                var direction = (transform.position - damage.Transform.position).normalized;

                foreach (var fragment in fragmentList)
                {
                    if (fragment == null)
                        continue;
                    fragment.physics.rb.AddForce(direction * Mathf.Max(minimumForce, damage.KnockBackForce));
                }
            }
        }

        void HandleHitAudio()
        {
            if (objectAudio == null)
            {
                Debug.LogError("No ObjectAudio found on " + gameObject.name);
                return;
            }

            if (health <= 0)
            {
                DestroyAudio();
            }
            else if (health > 0 && health < health * .15f)
            {
                AudioProcessor.PlayRandomClips(audioSource, objectAudio.HitAudio, .85f, 1.15f);
            }
            else
            {
                AudioProcessor.PlayRandomClips(audioSource, objectAudio.DamagedAudio, .85f, 1.15f);
            }
        }

        public void DestroyAudio()
        {
            AudioProcessor.PlayRandomClips(audioSource, objectAudio.DestroyedAudio, .85f, 1.15f);
        }


#if UNITY_EDITOR

        [Button("Get Components"), GUIColor(0.4f, 0.8f, 1)]
        public void GetComponents()
        {
            audioSource = GetComponent<AudioSource>();
            rigidBody = GetComponent<Rigidbody>();


            rigid = GetComponentInChildren<RayfireRigid>();

            hitFeedback = GetComponentInChildren<MMF_Player>();
            destroyFeedback = GetComponentInChildren<MMF_Player>();
        }


        [Button("SizeCollidersToChildren"), GUIColor(0.4f, 0.8f, 1)]
        public void SetCollider()
        {
            var currentCollider = GetComponent<Collider>();
            if (currentCollider != null)
                DestroyImmediate(currentCollider);

            var originalScale = transform.localScale;


            transform.localScale = Vector3.one;

            ColliderConfiguration colliderConfiguration = new ColliderConfiguration();

            switch (colliderType)
            {
                case ColliderTypes.BoxCollider:
                    currentCollider = colliderConfiguration.CreateBoxCollider(gameObject, childRenderer);
                    break;
                case ColliderTypes.SphereCollider:
                    currentCollider = colliderConfiguration.CreateSphereCollider(gameObject, childRenderer);
                    break;
                case ColliderTypes.CapsuleCollider:
                    currentCollider = colliderConfiguration.CreateCapsuleCollider(gameObject, childRenderer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            currentCollider.excludeLayers = LayerMask.GetMask("Fracture");

            transform.localScale = originalScale;
        }

        [Button("Configure RayFire"), GUIColor(0.4f, 0.8f, 1)]
        public void ConfigureRayfire()
        {
            //Set rigid type
            rigid.physics.mt = materialType;

            //Set demolition type
            rigid.demolitionType = demolitionType;

            //Set layer for fragments
            rigid.meshDemolition.prp.lay = LayerMask.NameToLayer("Fracture");

            rigid.gameObject.layer = LayerMask.NameToLayer("Fracture");

            //by collision
            rigid.limitations.col = false;

            //ignore near
            rigid.physics.ine = true;

            //set gravity
            rigid.physics.gr = isGravityAtStart;
        }
#endif

        public void ShowThatIsTargeted(bool enable) { }
    }
}