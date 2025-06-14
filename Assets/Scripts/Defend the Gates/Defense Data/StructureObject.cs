using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    [InlineEditor]
    public abstract class StructureObject : ScriptableObject
    {
        public abstract IUpgradable UpgradeData { get; }
    }
}