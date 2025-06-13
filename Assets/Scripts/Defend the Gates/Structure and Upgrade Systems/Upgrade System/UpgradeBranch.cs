using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    //Used to hold data for a specific upgrade branch in the upgrade system.
    //Branches are determined by what each upgrade level is
    //EX: all level 1 upgrades are in one branch, all level 2 upgrades are in another branch, etc.
    [Serializable]
    public class UpgradeBranch<T> where T :  ScriptableObject
    {
        public List<T> upgradeDataList = new ();
    }
}