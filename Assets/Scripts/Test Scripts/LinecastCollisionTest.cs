using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinecastCollisionTest : MonoBehaviour
{
    public LayerMask layerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        DoLinecast();
        
    }

    public void DoLinecast()
    {
        Physics.Linecast(transform.position, transform.position + Vector3.down * 10, out var hit, layerMask);
        
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 10, Color.red, 5f);
        
        Debug.Log(hit.transform.gameObject.name);
    }
    
    
}
