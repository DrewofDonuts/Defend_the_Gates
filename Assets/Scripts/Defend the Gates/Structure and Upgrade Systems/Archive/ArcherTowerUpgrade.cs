﻿namespace Etheral.DefendTheGates
{
    public class ArcherTowerUpgrade : DefenseUpgrade
    {
        public override void ApplyUpgrade()
        {
            currentUpgradeStuff.gameObject.SetActive(false);
            upgradeStuff[upgradeLevel].SetActive(true);
            
            
            upgradeLevel++;
            
            
        }
    }
}