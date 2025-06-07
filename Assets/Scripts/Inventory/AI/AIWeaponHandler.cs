using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public class AIWeaponHandler : WeaponHandler
    {
        //waiting for Inventory to load weapons before referencing Logics
        IEnumerator Start()
        {
            WeaponInventory.EquippedMeleeEvent += LoadCurrentWeaponDamage;
            yield return StartCoroutine(WaitALittle());
        }

        void OnDisable()
        {
            WeaponInventory.EquippedMeleeEvent -= LoadCurrentWeaponDamage;
        }

        IEnumerator WaitALittle()
        {

            yield return new WaitWhile(() => WeaponInventory.IsLoadingEquipment);
            LoadCurrentWeaponDamage();
        }
    }
}