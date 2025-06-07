using UnityEngine;

namespace PampelGames.GoreSimulator.Demo
{
    
    /// <summary>
    ///     A simple script to demonstrate cut, execution and reset of a character.
    /// </summary>
    public class DemoExecuteMesh : MonoBehaviour
    {
        private void Reset()
        {
            layerMask |= 1 << LayerMask.NameToLayer("Default");
        }
        
        public KeyCode ResetKey = KeyCode.Space;
        public LayerMask layerMask;
        public float explosionForce = 250f;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
                {
                    if (!hit.collider.transform.TryGetComponent<IGoreObject>(out var goreObject)) return;
                    goreObject.ExecuteCut(hit.point);
                }
            }
            
            
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
                {
                    if (!hit.collider.transform.TryGetComponent<IGoreObject>(out var goreObject)) return;
                    goreObject.ExecuteExplosion(explosionForce);
                }
            }
            
            
            if (Input.GetKeyDown(ResetKey))
            {
                var goreSims = FindObjectsByType<GoreSimulator>(FindObjectsSortMode.None);
                foreach (var goreSimulator in goreSims)
                {
                    goreSimulator.ResetCharacter();
                }
            }
        }
    }
}
