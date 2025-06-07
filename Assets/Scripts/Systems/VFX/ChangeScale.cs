using System;
using UnityEngine;

namespace Etheral
{
    public class ChangeScale : MonoBehaviour
    {
        public int scaleMultiplier = 1;


        void Update()
        {
            transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
        }
    }
}