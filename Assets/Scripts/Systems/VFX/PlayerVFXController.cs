using UnityEngine;
using UnityEngine.Serialization;

namespace Etheral
{
    /// <summary>
    /// Will need this in the future when using some type of Mesh VFX
    /// </summary>
    
    public class PlayerVFXController : MonoBehaviour
    {
        [SerializeField] PSMeshRendererUpdater leftWeaponVFX;
        [SerializeField] PSMeshRendererUpdater rightWeaponVFX;


        public void SetLeftWeaponVFX(GameObject meshObject)
        {
            leftWeaponVFX.MeshObject = meshObject;
        }

        public void SetRightWeaponVFX(GameObject meshObject)
        {
            rightWeaponVFX.MeshObject = meshObject;
        }

        public void SetRightVFX(bool isActive)
        {
            rightWeaponVFX.gameObject.SetActive(isActive);
            rightWeaponVFX.IsActive = isActive;
            rightWeaponVFX.UpdateMeshEffect();
        }

        public void SetLeftVFX(bool isActive)
        {
            leftWeaponVFX.gameObject.SetActive(isActive);
            leftWeaponVFX.IsActive = isActive;
            leftWeaponVFX.UpdateMeshEffect();
        }
    }
}