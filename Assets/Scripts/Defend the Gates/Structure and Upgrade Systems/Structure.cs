using UnityEngine;

namespace Etheral.DefendTheGates
{
    public abstract class Structure : MonoBehaviour, IStructure
    {
        public bool IsDestroyed { get; protected set; }

        public abstract void HandleDestroyed();
    }
}