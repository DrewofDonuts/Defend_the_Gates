using System;
using System.Collections;
using UnityEngine;

namespace Etheral
{
    public class Hook : MonoBehaviour, IAffiliate
    {
        [Header("References")]
        public GameObject hookMesh;
        public Transform hookBase;
        public SphereCollider triggerCollider;
        public Transform startingPosition;
        public WeaponHandler weaponHandler;

        [Header("Hook Configuration")]
        public float hookSpeed = 10f;
        public float returnHookSpeed;
        public DamageData damageData;

        [Header("Testing")]
        public Transform currentHit;

        public bool canHook;
        public bool isForward;
        public bool isHooked;
        public bool isReturn;
        public bool isCompletelyReturned { get; private set; }

        public event Action OnReturnHook;
        public event Action OnComplete;


        public Affiliation Affiliation { get; set; }
        public void SetAffiliation(Affiliation _affiliation) { }
        ITakeHit currentTakeHit;

        void OnEnable()
        {
            hookMesh.SetActive(false);
            damageData.Transform = hookBase;
        }

        public void EnableProjectile()
        {
            transform.position = startingPosition.position;
            transform.rotation = startingPosition.rotation;
            canHook = true;
            isForward = true;
            hookMesh.SetActive(true);
            isCompletelyReturned = false;

            // weaponHandler._currentRightHandDamage.gameObject.SetActive(false);
            weaponHandler.SetWeaponGO(true, false);
        }

        void Update()
        {
            if (isForward && !isHooked)
                transform.Translate((Vector3.forward + Vector3.left * .1f) * hookSpeed * Time.deltaTime, Space.Self);
            else if (isForward && isHooked)
                transform.Translate(Vector3.zero * hookSpeed * Time.deltaTime);
            else if (isReturn)
                transform.Translate((Vector3.back + Vector3.right * .1f) * hookSpeed * Time.deltaTime, Space.Self);

            if (CheckHookDistanceFromBase() && !isHooked)
            {
                PrepareHookToReturn();
            }


            if (isReturn && Vector3.Distance(transform.position, startingPosition.position) < 1.5f)
                ResetHookToStartingState();
        }

        void OnTriggerEnter(Collider other)
        {
            CheckBroaderHit(other);

            CheckIfHitWallLayer(other);
        }


        void CheckBroaderHit(Collider other)
        {
            if (!other.TryGetComponent(out ITakeHit takeHit)) return;
            if (takeHit.Affiliation == Affiliation || takeHit.Affiliation == Affiliation.Neutral) return;
            if (!canHook) return;
            if (takeHit.isHooked) return;


            transform.parent = other.transform;
            transform.position = other.transform.position;

            currentHit = other.transform;
            currentTakeHit = takeHit;
            isHooked = true;

            //clean up how isHooked is set 12/24/24
            if (!takeHit.isHooked)
                takeHit.isHooked = true;


            StartCoroutine(ReturnHook(isForward, takeHit));
        }

        void CheckIfHitWallLayer(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Debug.Log("Hit wall");
                PrepareHookToReturn();

                //Play sound
                //Display hit effect
            }
        }

        bool CheckHookDistanceFromBase()
        {
            return Vector3.Distance(transform.position, hookBase.position) >= 6f && !isReturn;
        }

        IEnumerator ReturnHook(bool hitOnForward, ITakeHit hit = null)
        {
            if (hitOnForward)
            {
                //Time to wait depends if the hook is returning or not
                var timeToWait = isReturn ? 0 : 3f;
                yield return new WaitForSeconds(timeToWait);

                damageData.Direction =
                    DamageUtil.CalculateKnockBack(hookBase, currentHit.transform, damageData.KnockBackForce);

                if (hit != null)
                {
                    hit.isHooked = false;
                    hit.TakeHit(damageData, DamageUtil.CalculateAngleToTarget(hookBase.transform, currentHit));
                }


                //Only return the hook if it's not already returning
                if (!isReturn)
                    PrepareHookToReturn();
            }
            else
                PrepareHookToReturn();


            // else
            // {
            //     PrepareHookToReturn();
            // }
        }

        void PrepareHookToReturn()
        {
            // transform.position = startingPosition.position;
            transform.rotation = startingPosition.rotation;

            canHook = false;
            isReturn = true;
            isForward = false;
            transform.parent = hookBase;
            OnReturnHook?.Invoke();
        }

        public void ResetHookToStartingState()
        {
            if (currentTakeHit != null)
                currentTakeHit.isHooked = false;
            currentTakeHit = null;
            isReturn = false;
            isCompletelyReturned = true;
            hookMesh.SetActive(false);
            transform.position = startingPosition.position;
            transform.rotation = startingPosition.rotation;
            OnComplete?.Invoke();
            weaponHandler._currentRightHandDamage.gameObject.SetActive(true);
            weaponHandler.SetWeaponGO(true, true);
            isHooked = false;
            canHook = false;
        }
    }
}