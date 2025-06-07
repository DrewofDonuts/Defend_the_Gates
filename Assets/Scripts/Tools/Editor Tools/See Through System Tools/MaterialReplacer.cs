#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Etheral
{
    public class MaterialReplacer : MonoBehaviour
    {
        public static void ReplaceMaterials(string directory = null)
        {
            // Get all GameObjects in the scene
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Check if the GameObject or its parent has an ObjectFader component
                if (obj.GetComponent<ObjectFader>() != null || (obj.transform.parent != null &&
                                                                obj.transform.parent.GetComponent<ObjectFader>() !=
                                                                null))
                {
                    // Get the Renderer component
                    Renderer renderer = obj.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        // Get the materials of the Renderer
                        Material[] materials = renderer.sharedMaterials;

                        for (int i = 0; i < materials.Length; i++)
                        {
                            // Get the material with the same name + "_isTransparent"

                            // $"Assets/Etheral/Art/Transparent Materials/{materials[i].name}"

                            string materialDirectory = $"{directory}/{materials[i].name}" + "_transparent.mat";
                            Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialDirectory);


                            // If the new material exists, replace the old material with the new material
                            if (newMaterial != null)
                            {
                                materials[i] = newMaterial;
                            }
                        }

                        // Assign the new materials to the Renderer
                        renderer.sharedMaterials = materials;
                    }
                }
            }
        }
    }
}
#endif