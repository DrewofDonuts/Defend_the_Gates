using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New UI  Holder", menuName = "Etheral/UI  Holder")]
    public class UIHolder : ScriptableObject
    {
        public UIImages[] uiImages;

    }

    [Serializable]
    public class UIImages
    {
        public string type;
        [PreviewField]
        public Sprite[] images;
    }
    
}