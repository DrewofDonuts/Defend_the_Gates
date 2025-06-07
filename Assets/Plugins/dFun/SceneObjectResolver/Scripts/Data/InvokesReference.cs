using System;
using DFun.Invoker;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DFun.UnityDataTypes
{
    [Serializable]
    public class InvokesReference
    {
        [SerializeField] private InvokesList invokesList = new InvokesList();
        public InvokesList InvokesList => invokesList;

        public bool ContainsData => invokesList.ContainsData;

        public InvokesReference()
        {
        }

        public InvokesReference(InvokesReference copyFromInvokesReference)
        {
            invokesList = new InvokesList(copyFromInvokesReference.InvokesList);
        }

        public void AddInvoke(InvokeData invokeData)
        {
            invokesList.Add(invokeData);
        }

        public void Invoke(Object objectInstance, bool catchExceptions = true, bool logExceptions = true)
        {
            invokesList.Invoke(objectInstance, catchExceptions, logExceptions);
        }

        public void Invoke(Type type, bool catchExceptions = true, bool logExceptions = true)
        {
            invokesList.Invoke(type, catchExceptions, logExceptions);
        }
    }
}