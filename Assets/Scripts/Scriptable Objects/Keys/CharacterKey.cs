using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New Character Key", menuName = "Etheral/Keys/New Character Key")]
    public class CharacterKey : StringObject
    {
        public string CharacterName => Value;
        

        // void OnValidate()
        // {
        //     CharacterName = "";
        //     CharacterName = NameConverter.ConvertToName(Value);
        // }
        
    }
}