using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.Invoker
{
    [Serializable]
    public class InvokesList
    {
        [SerializeField] private List<InvokeData> invokes = new List<InvokeData>(0);

        public List<InvokeData> Invokes => invokes;

        public bool ContainsData => Invokes != null && Invokes.Count > 0;

        public InvokesList()
        {
            
        }
        
        public InvokesList(InvokesList copyFrom)
        {
            invokes = new List<InvokeData>(copyFrom.Invokes.Count);
            for (int i = 0, iSize = copyFrom.Invokes.Count; i < iSize; i++)
            {
                invokes.Add(new InvokeData(copyFrom.Invokes[i]));
            }
        }
        
        public void Add(InvokeData invokeData)
        {
            Invokes.Add(invokeData);
        }

        public void Invoke(Object objectInstance, bool catchExceptions = true, bool logExceptions = true)
        {
            for (int i = 0, iSize = Invokes.Count; i < iSize; i++)
            {
                Invokes[i].Invoke(objectInstance, catchExceptions, logExceptions);
            }
        }
        
        public void Invoke(Type type, bool catchExceptions = true, bool logExceptions = true)
        {
            for (int i = 0, iSize = Invokes.Count; i < iSize; i++)
            {
                Invokes[i].Invoke(type, catchExceptions, logExceptions);
            }
        }
    }
}