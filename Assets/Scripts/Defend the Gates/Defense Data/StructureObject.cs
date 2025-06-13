using UnityEngine;

namespace Etheral.DefendTheGates
{
    public abstract class StructureObject : ScriptableObject
    {
        public abstract IUpgradable UpgradeData { get; }
    }
}