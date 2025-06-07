using UnityEngine;

namespace Etheral
{
    public class LineController : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField] AIHealth aiHealth;
        [SerializeField] Transform startPoint;


        [Header("Wave Settings")]
        [SerializeField] int pointCount = 20; // Number of points along the line
        [SerializeField] float waveAmplitude = 0.5f; // Height of the wave
        [SerializeField] float waveFrequency = 2f; // Number of waves along the line
        [SerializeField] float waveSpeed = 2f; // Speed of the wave animation

        Transform target;
        AIHealth currentEnemySpawner;
        bool isDead;

        void Awake()
        {
            lineRenderer.enabled = false;
            aiHealth.OnDie += HandleDeath;
            lineRenderer.positionCount = pointCount; // Ensure the Line Renderer has the correct number of points
        }

        void OnDisable()
        {
            aiHealth.OnDie -= HandleDeath;
        }

        void HandleDeath(Health health)
        {
            isDead = true;

            if (lineRenderer != null)
                lineRenderer.enabled = false;
        }

        public void SetTarget(AIHealth _target, Transform _transform)
        {
            currentEnemySpawner = _target;
            target = _transform;
            lineRenderer.enabled = true;
            currentEnemySpawner.OnDie += HandleDeath;
        }

        public void SetTarget(Transform _target)
        {
            target = _target;
            lineRenderer.enabled = true;
        }

        void Update()
        {
            if (target == null) return;
            if (isDead) return;

            UpdateLineRenderer();
        }

        void UpdateLineRenderer()
        {
            Vector3 start = startPoint.position; // Start position of the line
            Vector3 end = target.position; // End position of the line

            for (int i = 0; i < pointCount; i++)
            {
                float t = (float)i / (pointCount - 1); // Interpolation factor [0, 1]
                Vector3 position = Vector3.Lerp(start, end, t);

                // Add waving effect
                Vector3 offset = Vector3.up * Mathf.Sin(t * waveFrequency * Mathf.PI * 2 + Time.time * waveSpeed) *
                                 waveAmplitude;
                lineRenderer.SetPosition(i, position + offset);
            }
        }
    }
}