using System.Collections;
using UnityEngine;

namespace Etheral
{
    //Place on camera
    public class CameraObjectFader : MonoBehaviour
    {
        [field: SerializeField] public LayerMask TargetMask { get; private set; }

        LayerMask includedMasks;

        GameObject player;
        ObjectFader objectFader;
        PlayerStateMachine playerStateMachine;


        void Start()
        {
            // yield return new WaitUntil(() => CharacterManager.Instance.Player != null);

            if (CharacterManager.Instance == null)
                player = FindObjectOfType<PlayerStateMachine>().gameObject
                    .GetComponentInChildren<CameraPlayerReference>().gameObject;
            else
                player = CharacterManager.Instance.Player.gameObject.GetComponentInChildren<CameraPlayerReference>()
                    .gameObject;
        }

        void Update()
        {
            if (player != null)
            {

                var cameraPosition = transform.position;
                var dir = player.transform.position - cameraPosition;
                Ray ray = new Ray(cameraPosition, dir);

                if (Physics.Raycast(ray, out RaycastHit hit, dir.magnitude, TargetMask))
                {
                    if (hit.collider == null) return;

                    if (hit.collider.gameObject == player)
                    {
                        if (objectFader != null)
                        {
                            objectFader.SetFade(false);
                        }
                    }
                    else
                    {
                        objectFader = hit.collider.gameObject.GetComponent<ObjectFader>();
                        if (objectFader != null)
                            objectFader.SetFade(true);
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (player != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
    }
}