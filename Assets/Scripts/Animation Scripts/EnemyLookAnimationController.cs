using System;
using System.Collections;
using FIMSpace.FLook;
using UnityEngine;

namespace Etheral
{
    public class EnemyLookAnimationController : MonoBehaviour
    {
        [SerializeField] EnemyLockOnController lockOnController;
        [SerializeField] Transform target;
        [SerializeField] FLookAnimator lookAnimator;


        IEnumerator Start()
        {
            yield return new WaitWhile(() => lockOnController.GetTarget() == null);
            target = lockOnController.GetTarget().Transform;

            if (target != null)
                lookAnimator.SetLookTarget(target);
        }

        // void Update()
        // {
        //     if (target == null)
        //         target = lockOnController.GetTarget().gameObject.GetComponentInChildren<Head>().transform;
        //    
        //     if (target == null && lookAnimator.GetLookAtTransform() == null)
        //         lookAnimator.SetLookTarget(target);
        // }
    }
}