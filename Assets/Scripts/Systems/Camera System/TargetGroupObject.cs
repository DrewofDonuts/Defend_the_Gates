using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Etheral
{
    public class TargetGroupObject : MonoBehaviour
    {
        [SerializeField] float weight;
        [SerializeField] float radius;
        [SerializeField] Transform target;
        
        public float Weight => weight;
        public float Radius => radius;
        public Transform Target => target;
        
        
        
    }
}