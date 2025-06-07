using Sirenix.OdinInspector;
using UnityEngine;


namespace Etheral
{
    public class BatchShaderChanger : MonoBehaviour
    {
        public Shader newShader; // Drag and drop the new shader here in the Inspector

        void Start()
        {
            ChangeShaderForMaterials();
        }


        [Button("Change Shader For Materials")]
        public void ChangeShaderForMaterials()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer rend in renderers)
            {
                Material[] materials = rend.sharedMaterials;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (newShader != null)
                    {
                        materials[i].shader = newShader;
                    }
                    else
                    {
                        Debug.LogError("New shader is not assigned!");
                    }
                }
            }
        }
    }
}