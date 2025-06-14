using System;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral.DefendTheGates
{
    public class StructureController : MonoBehaviour
    {
        [SerializeField] Structure structure;
        [SerializeField] DefensesHealth defensesHealth;
        [SerializeField] List<GameObject> intactMeshes = new();
        [SerializeField] List<GameObject> destroyedMeshes = new();
        [SerializeField] List<GameObject> destroyedVFX = new();


        void Start()
        {
            defensesHealth.OnDie += HandleStructureDestroyed;
        }

        void OnDisable() =>
            defensesHealth.OnDie -= HandleStructureDestroyed;


        void HandleStructureDestroyed()
        {
            structure.HandleDestroyed();
            foreach (var mesh in intactMeshes)
            {
                mesh.SetActive(false);
            }

            if (destroyedMeshes.Count == 0)
                foreach (var mesh in destroyedMeshes)
                {
                    mesh.SetActive(true);
                }

            //Once we have VFX, we can enable them here
            // foreach (var vfx in destroyedVFX)
            // {
            //     vfx.SetActive(true);
            // }
        }
    }
}