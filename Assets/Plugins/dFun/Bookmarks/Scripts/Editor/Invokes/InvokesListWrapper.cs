using DFun.Invoker;
using UnityEngine;

namespace DFun.Bookmarks
{
    public class InvokesListWrapper : ScriptableObject
    {
        [SerializeField] private InvokesList invokesList;

        public InvokesList InvokesList
        {
            get => invokesList;
            set => invokesList = value;
        }
    }
}