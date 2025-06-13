using UnityEngine;

namespace Etheral.DefendTheGates
{
    public interface IUpgradable
    {
        int Level { get; }
        string Name { get; }
        GameObject Prefab { get; }
        string Description { get; }
    }
    
}