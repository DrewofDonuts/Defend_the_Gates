using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Etheral
{
    [CreateAssetMenu(fileName = "New GameObject Holder", menuName = "Etheral/GameObject Holder")]
    public class GameObjectHolderSO : ScriptableObject
    {
        public GameObjects[] gameObjects;

        
    }
    
    [Serializable]
    public class GameObjects
    {
        public string type;
        public Object[] gameObjects;
    }
    
}