using Etheral.Audio;
using UnityEngine;

namespace Etheral
{
    public class OldDoor : MonoBehaviour, IBind<DoorData>
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }
        [field: SerializeField] public GameObject[] Roofing { get; private set; }
        public DoorData doorData;

        public bool IsDoorIsOpen;


        public void Bind(DoorData _data)
        {
            doorData = _data;

            if (doorData.isOpen)
                IsDoorIsOpen = true;
            else
                IsDoorIsOpen = false;
        }

        [ContextMenu("Load Components")]
        public void LoadComponents()
        {
            Animator = GetComponent<Animator>();
            AudioSource = GetComponent<AudioSource>();
        }

        public void OpenDoor()
        {
            Animator.Play("Open");

            IsDoorIsOpen = true;
            doorData.isOpen = true;


            if (AudioClip != null)
                AudioProcessor.PlaySingleClip(AudioSource, AudioClip);

            // Door.Animator.CrossFadeInFixedTime("OpenDoor", 0.1f);
        }

        public void CloseDoor()
        {
            Animator.Play("Close");

            IsDoorIsOpen = false;
            doorData.isOpen = false;

            if (AudioClip != null)
                AudioProcessor.PlaySingleClip(AudioSource, AudioClip);
        }
    }
}