using RayFire;
using Sirenix.OdinInspector;
using UnityEngine;

public class RayfireTest : MonoBehaviour
{
    public RayfireRigid rigid;


#if UNITY_EDITOR

    [Button("Fracture")]
    public void FractureObject()
    {
        rigid.Demolish();
    }

#endif
}