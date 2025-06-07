using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    public class UISliders : MonoBehaviour
    {
        [field: SerializeField] public bool IsHealthBar { get; private set; }
        [field: SerializeField] public bool IsDefenseBar { get; private set; }
        [field: SerializeField] public bool IsStaminaBar { get; private set; }
        
        [field: SerializeField] public Slider Slider { get; private set; }
        
    }
}