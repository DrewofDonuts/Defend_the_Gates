using UnityEngine;

namespace Etheral
{
    public class LayerSelector : MonoBehaviour
    {
        public string layerName;
        
        void Start()
        {
            gameObject.layer =LayerMask.NameToLayer(layerName);
        }
    }
}