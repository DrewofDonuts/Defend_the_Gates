using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Etheral
{
    //Place on objects that need to be faded
    public class ObjectFader : MonoBehaviour
    {
        [field: SerializeField] public float FadeSpeed { get; private set; } = 1f;
        [field: SerializeField] public float FadeAmount { get; private set; } = 0f;
        [field: SerializeField] public float OriginalOpacity { get; private set; } = 1f;

        [field: SerializeField] public bool HasChildren { get; private set; }

        [field: SerializeField] public bool HasDoor { get; private set; }

        [field: ShowIf("HasDoor")]
        [field: SerializeField] public OldDoor OldDoor { get; private set; }

        public bool DoFade { get; private set; }


        [field: SerializeField] public new Renderer renderer { get; private set; }
        [field: SerializeField] public Renderer[] renderers { get; private set; }
        [field: SerializeField] public List<Material> materials { get; private set; } = new();
        [field: SerializeField] public List<float> originalOpacities { get; private set; } = new();


        void Start()
        {
            renderer = GetComponent<Renderer>();

            if (renderer != null)
                foreach (var material in renderer.materials)
                {
                    if (materials.Contains(material)) continue;
                    materials.Add(material);
                }


            if (HasChildren)
            {
                renderers = GetComponentsInChildren<Renderer>();

                foreach (var _renderer in renderers)
                {
                    foreach (var material in _renderer.materials)
                    {
                        if (materials.Contains(material)) continue;
                        materials.Add(material);
                    }
                }
            }

            if (materials.Count == 0) return;
            foreach (var material in materials)
            {
                // OriginalOpacity = material.color.a;
                if (originalOpacities.Contains(material.color.a)) continue;
                originalOpacities.Add(material.color.a);
            }
        }

        void Update()
        {
            if (DoFade && !HasDoor)
            {
                FadeAll();
            }
            else if (DoFade && HasDoor && OldDoor.IsDoorIsOpen)
            {
                FadeAll();
            }
            else
            {
                ResetFadeAll();
            }
        }

        public void SetFade(bool fade)
        {
            DoFade = fade;
        }

        void FadeAll()
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].color.a > FadeAmount)
                {
                    Color currentColor = materials[i].color;
                    Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                        Mathf.Lerp(currentColor.a, FadeAmount, FadeSpeed * Time.deltaTime));
                    materials[i].color = smoothColor;
                }
                else
                {
                    DoFade = false;
                }
            }
        }

        void ResetFadeAll()
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (materials[i].color.a < OriginalOpacity)
                {
                    Color currentColor = materials[i].color;
                    Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                        Mathf.Lerp(currentColor.a, OriginalOpacity, FadeSpeed * Time.deltaTime));
                    materials[i].color = smoothColor;
                }
                else
                {
                    DoFade = false;
                }
            }
        }
        
        
        public void SetHasDoor(bool hasDoor)
        {
            HasDoor = hasDoor;
        }

#if UNITY_EDITOR


        [Button("Load Materials")]
        public void LoadMaterials()
        {
            renderer = GetComponent<Renderer>();

            if (renderer != null)
                foreach (var material in renderer.materials)
                {
                    if (materials.Contains(material)) continue;
                    materials.Add(material);
                }


            if (HasChildren)
            {
                renderers = GetComponentsInChildren<Renderer>();

                foreach (var _renderer in renderers)
                {
                    foreach (var material in _renderer.materials)
                    {
                        if (materials.Contains(material)) continue;
                        materials.Add(material);
                    }
                }
            }

            if (materials.Count == 0) return;
            foreach (var material in materials)
            {
                // OriginalOpacity = material.color.a;
                if (originalOpacities.Contains(material.color.a)) continue;
                originalOpacities.Add(material.color.a);
            }
        }
#endif
    }
}