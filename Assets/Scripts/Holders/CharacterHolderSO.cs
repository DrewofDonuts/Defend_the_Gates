using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "CharacterHolderSO", menuName = "Etheral/Holders/CharacterHolderSO")]
    public class CharacterHolderSO : ScriptableObject
    {
        public string episode;
        public CharacterType[] characterTypes;
    }

    
}