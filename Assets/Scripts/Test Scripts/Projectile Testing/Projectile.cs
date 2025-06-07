using System;
using System.Collections;
using Etheral;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Projectile : MonoBehaviour, IAmPoolObject<Projectile>
{
    protected float speed;
    protected float lifeTime;
    Affiliation affiliation;

    [field: SerializeField] public Projectile Prefab { get; set; }
    [field: SerializeField] public string PoolKey { get; set; }
    [FormerlySerializedAs("aimVariablity")] [SerializeField]
    protected float aimOffset;

    protected bool isCollided;

    protected Vector3 eulerAngleVelocity;
    public float CollisionAngle { get; private set; }

    public DamageData DamageData { get; private set; } = new();


    IEnumerator Start()
    {
        yield return new WaitForSeconds(5);
        
        

        if (gameObject.activeSelf)
            Destroy(gameObject);
            // Release();
    }

    // void OnDisable()
    // {
    //     ResetProjectile();
    //     if (gameObject.activeSelf)
    //         Release();
    // }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITakeHit takeHit))
        {
            HandleHittingTarget(other, takeHit);
        }
        else
        {
            HandleHittingObjects(other);
        }
    }

    protected virtual void HandleHittingObjects(Collider other)
    {
        if (other.isTrigger) return;
        // transform.parent = other.transform;
        isCollided = true;
        Destroy(gameObject);
    }

    void HandleHittingTarget(Collider other, ITakeHit takeHit)
    {
        if (takeHit.Affiliation == affiliation) return;

        if (isCollided) return;

        DamageData.Direction = (other.transform.position - transform.position).normalized *
                               DamageData.knockBackForce;

        float angle = DamageUtil.CalculateAngleToTarget(transform, other.transform);

        takeHit.TakeHit(DamageData, angle); //TODO use audio overload
        isCollided = true;

        Destroy(gameObject);
        // Release();
    }

    public void Release()
    {
        ObjectPoolManager.Instance.ReleaseObject(Prefab);
    }

    public void ResetProjectile()
    {
        isCollided = false;
        // transform.SetParent(null);
        transform.position = Vector3.zero;
    }

    public void SetProjectileSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetProjectileStats(float damage, float knockBack, float knockDown, float lifeTime,
        AudioImpact audioImpact, float _aimOffset = 5f, bool doesImpact = false)
    {
        DamageData.damage = damage;
        DamageData.knockBackForce = knockBack;
        DamageData.knockDownForce = knockDown;
        DamageData.doesImpact = doesImpact;
        this.lifeTime = lifeTime;
        aimOffset = _aimOffset;
    }

    public void SetEulerAndCollisionAngles(Vector3 eulerAngleVelocity, float collisionAngle)
    {
        this.eulerAngleVelocity = eulerAngleVelocity;
        CollisionAngle = collisionAngle;
    }

    public void SetTeam(Affiliation team)
    {
        affiliation = team;
    }

    public Affiliation Affiliation { get; set; }

    public void SetAffiliation(Affiliation _affiliation) => Affiliation = _affiliation;
}