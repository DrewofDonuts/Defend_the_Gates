using UnityEngine;

namespace Etheral
{
    public class VFXEffect : BaseEffect
    {
        public bool childToSpellObject;
        public Vector3 offset;
        public float delayBeforeVFX;
        public float duration;
        public GameObject[] vfxPrefabs;

        float timer;
        bool isVFXPlayed;

        public override void Tick(float deltaTime)
        {
            if (!isVFXPlayed)
            {
                timer += deltaTime;
                if (timer >= delayBeforeVFX)
                {
                    PlayVFX();
                    isVFXPlayed = true;
                }
            }
        }

        void PlayVFX()
        {
            if (vfxPrefabs != null)
            {
                foreach (var vfxPrefab in vfxPrefabs)
                {
                    if (vfxPrefab != null)
                    {
                        var vfx = Object.Instantiate(vfxPrefab, spellObject.transform.position + offset,
                            spellObject.transform.rotation);
                        if (childToSpellObject)
                            vfx.transform.SetParent(spellObject.transform);

                        if (spellObject != null)
                        {
                            Debug.Log("Should destroy VFX");
                            Object.Destroy(vfx, duration);
                        }
                        else
                        {
                            Debug.LogWarning("SpellObject is null");
                        }
                    }
                }
            }
        }
    }
}