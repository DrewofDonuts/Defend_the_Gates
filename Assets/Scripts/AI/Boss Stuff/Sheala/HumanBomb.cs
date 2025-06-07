using System;
using System.Collections;
using HighlightPlus;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Etheral
{
    public class HumanBomb : MonoBehaviour, ICastSpell, IHaveHealth, ITakeHit, IAmPoolObject<HumanBomb>
    {
        [Header("Object Pool Settings")]
        [field: SerializeField] public HumanBomb Prefab { get; set; }
        [field: SerializeField] public string PoolKey { get; set; }

        public void Release()
        {
            ObjectPoolManager.Instance.ReleaseObject(this);
        }

        [Header("Health")]
       [field: SerializeField] public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        [Header("Settings")]
        public float slowSpeed = 1f;
        public float maxSpeed = 5f;
        public float explosionTime = .5f;
        public float distanceBeforeExplode = 1f;

        [Header("References")]
        public AnimationHandler animationHandler;
        public Spell bombSpell;
        public NavMeshAgent agent;
        public HighlightEffect highlightEffect;


        public Transform Target { get; set; }
        public Transform Caster => transform;

        public event Action OnDie;
        GameObject model;

        public Affiliation Affiliation { get; set; }

        // bool hasLoadedModelData;
        bool hasStartedExplodeProcess;
        bool isDead;

        void Start()
        {
            // yield return new WaitUntil(() => hasLoadedModelData);

            Target = EventBusPlayerController.PlayerStateMachine.transform;


            animationHandler.CrossFadeInFixedTime("Locomotion", 0.2f);
            highlightEffect.highlighted = true;
            StartCoroutine(AlternateSpeed());
        }

        IEnumerator AlternateSpeed()
        {
            while (gameObject.activeSelf)
            {
                agent.speed = slowSpeed;
                yield return new WaitForSeconds(.5f);
                agent.speed = maxSpeed;
                yield return new WaitForSeconds(.5f);
            }
        }

        public void Initialize(Transform _target)
        {
            Target = _target;
        }

        void Update()
        {
            if (Target == null) return;

            // while (!hasLoadedModelData)
            //     return;


            if (!hasStartedExplodeProcess)
                agent.SetDestination(Target.position);


            if (isDead) return;

            if (Vector3.Distance(transform.position, Target.position) < distanceBeforeExplode &&
                !hasStartedExplodeProcess)
            {
                agent.isStopped = true;
                agent.ResetPath();
                PreExplode();
            }
        }

        void OnDisable()
        {
            Release();
        }

        void PreExplode()
        {
            hasStartedExplodeProcess = true;
            animationHandler.CrossFadeInFixedTime("Explode", .2f);
            StartCoroutine(ExplodeAfterTime());
        }

        IEnumerator ExplodeAfterTime()
        {
            yield return new WaitForSeconds(.5f);
            var bombSpellObject = Instantiate(bombSpell.spellObject, transform.position, Quaternion.identity);
            bombSpellObject.InitializeSpellObject(this);

            yield return new WaitForSeconds(explosionTime);
            transform.GetChild(1).gameObject.SetActive(false);
            EventBusPlayerController.FeedbackBasedOnDistanceFromPlayer(this, transform.position, FeedbackType.Heavy);

            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }

        public void OnSucessfulCast() { }
        public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;


        public void TakeHit(IDamage damage, float angle = default)
        {
            if (isDead) return;
            CurrentHealth -= damage.Damage;
            highlightEffect.HitFX();
            
            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            StopAllCoroutines();
            agent.isStopped = true;
            isDead = true;
            animationHandler.CrossFadeInFixedTime("Death");
        }

        public void TakeDotDamage(float damage)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(float healthToAdd) { }
        public bool isHooked { get; set; }

        [ContextMenu("Set Pool Key")]
        public void SetPoolKey()
        {
            PoolKey = gameObject.name;
        }
    }
}