using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class DoNotDestroy : MonoBehaviour
    {
        void Awake()
        {
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
