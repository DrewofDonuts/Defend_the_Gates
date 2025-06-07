using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class SplineMover : MonoBehaviour
    {
        public EtheralSpline spline;
        public Transform target;

 
        
        void Update()
        {
            transform.position = spline.WhereOnSpline(target.position);
        }
    }
}
