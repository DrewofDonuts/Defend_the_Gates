using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


namespace Etheral
{
    public class FracturedMemoryObject : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip completedClip;

        [Header("Completed Settings")]
        [SerializeField] float floatSpeed = 1f;
        [SerializeField] float amplitude = 0.25f;

        [Header("Pre-Combine Settings")]
        [SerializeField] float preCombineDuration = 1.0f;
        [SerializeField] float preCombinationSpinSpeed = 10.0f;
        [Header("Fractured Settings")]
        [SerializeField] float combineDuration = 1.0f;
        [Tooltip("Distance Range pieces can fracture from original position")]
        [SerializeField] float fractureDistanceRange = 6.0f;

        [Tooltip("Minimum distance pieces can fracture between each other")]
        [SerializeField] float minDistance = 3.0f;
        [SerializeField] Transform[] memoryPieces;

        [Header("Fractured Float Settings")]
        [SerializeField] float minFractureRotSpeed = 10f;
        [SerializeField] float maxFractureRotSpeed = 50f;        
        

        bool isComplete;

        Quaternion[] originalRotations;
        Vector3[] originalPositions;

        float[] rotationSpeeds;
        Vector3[] rotationDirections;
        Vector3 startPosition;

        bool hasBeenTriggered;

        void Start()
        {
            if (isComplete) return;
            SetStartingPositions();
            SetPiecesInRandomPositionsAndRotations();
            SetRandomRotationSpeedsAndDirections();
            startPosition = transform.position;
        }

        void OnTriggerEnter(Collider other)
        {
            if (hasBeenTriggered) return;
            if (other.CompareTag("Player"))
            {
                CompleteMemory();
                hasBeenTriggered = true;
            }
        }

        void Update()
        {
            if (!isComplete)
                RotatePiecesInSpaces();

            if (isComplete)
                Float();
        }

        void Float()
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        void SetStartingPositions()
        {
            originalRotations = new Quaternion[memoryPieces.Length];
            originalPositions = new Vector3[memoryPieces.Length];

            for (int i = 0; i < memoryPieces.Length; i++)
            {
                originalRotations[i] = memoryPieces[i].rotation;
                originalPositions[i] = memoryPieces[i].position;
            }
        }

        void SetPiecesInRandomPositionsAndRotations()
        {
            SetStartingPositions();

            for (int i = 0; i < memoryPieces.Length; i++)
            {
                Vector3 newPosition;
                bool positionIsValid;

                do
                {
                    newPosition = new Vector3(Random.Range(-fractureDistanceRange, fractureDistanceRange),
                        Random.Range(-fractureDistanceRange, fractureDistanceRange),
                        Random.Range(-fractureDistanceRange, fractureDistanceRange));
                    positionIsValid = true;

                    for (int j = 0; j < i; j++)
                    {
                        if (Vector3.Distance(newPosition, memoryPieces[j].localPosition) < minDistance)
                        {
                            positionIsValid = false;
                            break;
                        }
                    }
                } while (!positionIsValid);

                memoryPieces[i].localPosition = newPosition;
            }
        }

        void SetRandomRotationSpeedsAndDirections()
        {
            rotationSpeeds = new float[memoryPieces.Length];
            rotationDirections = new Vector3[memoryPieces.Length];

            for (int i = 0; i < memoryPieces.Length; i++)
            {
                rotationSpeeds[i] = Random.Range(minFractureRotSpeed, maxFractureRotSpeed);
                
                
                rotationDirections[i] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))
                    .normalized;
            }
        }

        void RotatePiecesInSpaces()
        {
            for (int i = 0; i < memoryPieces.Length; i++)
            {
                memoryPieces[i].Rotate(rotationDirections[i], rotationSpeeds[i] * Time.deltaTime);
            }
        }

        [ContextMenu("Complete Memory")]
        public void CompleteMemory()
        {
            isComplete = true;
            StartCoroutine(SpinFragmentsQuicklyBeforeCombining());
        }

        IEnumerator SpinFragmentsQuicklyBeforeCombining()
        {
            float elapsedTime = 0f;
            float currentSpinSpeed = 0f;


            Vector3[] startPositions = new Vector3[memoryPieces.Length];

            for (int i = 0; i < memoryPieces.Length; i++)
            {
                startPositions[i] = memoryPieces[i].position;
            }

            while (elapsedTime < preCombineDuration)
            {
                elapsedTime += Time.deltaTime;
                // currentSpinSpeed = Mathf.Min(preCombinationSpinSpeed,
                //     currentSpinSpeed + Time.deltaTime * preCombinationSpinSpeed);
                
                currentSpinSpeed = Mathf.Lerp(0, preCombinationSpinSpeed, elapsedTime / preCombineDuration);



                for (int i = 0; i < memoryPieces.Length; i++)
                {
                    memoryPieces[i].Rotate(rotationDirections[i],
                        rotationSpeeds[i] * Time.deltaTime * currentSpinSpeed);


                    Vector3 direction = (memoryPieces[i].position - startPositions[i]).normalized;
                    memoryPieces[i].Translate(-direction * Time.deltaTime * floatSpeed);
                }

                yield return null;
            }

            StartCoroutine(MovePiecesToOriginalPositions());
        }

        IEnumerator MovePiecesToOriginalPositions()
        {
            float elapsedTime = 0f;

            Vector3[] startPositions = new Vector3[memoryPieces.Length];
            for (int i = 0; i < memoryPieces.Length; i++)
            {
                startPositions[i] = memoryPieces[i].position;
            }

            while (elapsedTime < combineDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / combineDuration;

                for (int i = 0; i < memoryPieces.Length; i++)
                {
                    memoryPieces[i].position = Vector3.Lerp(startPositions[i], originalPositions[i], t);
                    memoryPieces[i].rotation = Quaternion.Lerp(memoryPieces[i].rotation, originalRotations[i], t);
                }

                yield return null;
            }

            // Ensure final positions and rotations are set
            for (int i = 0; i < memoryPieces.Length; i++)
            {
                memoryPieces[i].position = originalPositions[i];
                memoryPieces[i].rotation = originalRotations[i];
                audioSource.clip = completedClip;
                audioSource.Play();

                EventBusPlayerController.FeedbackBasedOnDistanceFromPlayer(this, transform.position,
                    FeedbackType.Light);
            }
        }
    }
}