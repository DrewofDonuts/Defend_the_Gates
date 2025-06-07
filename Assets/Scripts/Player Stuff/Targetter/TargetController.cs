using UnityEngine;


namespace Etheral
{
    public class TargetController : MonoBehaviour
    {
        [field: Header(" Raycast Start and End Points")]
        [field: SerializeField] public Transform TopRayPoint { get; private set; }
        [field: Tooltip("Use the child object of the targetter as the end point")]
        [field: SerializeField] public Transform BottomRayPoint { get; private set; }
        [field: Tooltip("Triangle pointer to show the direction of the targetter")]
        [field: SerializeField] public GameObject DirectionPointer { get; private set; }


        [field: Header("Parabolic Raycast Settings")]
        [field: SerializeField] public float Height { get; private set; } = 4;
        [field: Range(50, 400)]
        [field: SerializeField] public int NumSteps { get; private set; } = 50;
        [field: SerializeField] public LayerMask layerMask { get; private set; }

        [field: SerializeField] public Targetter Targetter { get; private set; }
        [field: SerializeField] public LineRenderer LineRenderer { get; private set; }

        [field: Header("Targetter Settings")]
        [field: SerializeField] public float TargetSpeed { get; private set; } = 5f;
        [field: SerializeField] public Transform StartOfTrailRendererPosition { get; private set; }
        [field: SerializeField] public float MaxDistance { get; private set; } = 10f;
        [field: SerializeField] public float MaxDistanceSpeed { get; private set; } = 1f;


        Vector3 leapTarget;
        Vector3 highestGroundPoint;

        const float DISTANCE_BEFORE_LINE_RENDERER = 2.5f;

        void Start()
        {
            Targetter.gameObject.SetActive(false);
            LineRenderer.useWorldSpace = false;
            LineRenderer.enabled = false;

            if (DirectionPointer != null)
                DirectionPointer.SetActive(false);
        }

        public void ToggleDirectionPointer(bool activate)
        {
            DirectionPointer.SetActive(activate);
        }


        void Update()
        {
            if (Targetter.enabled)
            {
                // leapTarget = ParabolicRaycast();
                leapTarget = StraightLinecast();

                if (Vector3.Distance(transform.parent.position, Targetter.transform.position) >
                    DISTANCE_BEFORE_LINE_RENDERER)
                {
                    if (!LineRenderer.enabled)
                        LineRenderer.enabled = true;
                    UpdateLineRenderer();
                }

                if (Vector3.Distance(transform.parent.position, Targetter.transform.position) <
                    DISTANCE_BEFORE_LINE_RENDERER ||
                    !Targetter.isActiveAndEnabled)
                {
                    if (LineRenderer.useWorldSpace)
                        LineRenderer.useWorldSpace = false;

                    if (LineRenderer.enabled)
                        LineRenderer.enabled = false;
                }
            }
        }

        public void EnableTargetter()
        {
            Targetter.gameObject.SetActive(true);
            Targetter.HandleSettings(StartOfTrailRendererPosition.position, MaxDistanceSpeed, MaxDistance, TargetSpeed);
            Targetter.SetCanMove(true);
            LineRenderer.enabled = true;
        }

        public void DisableTargetter()
        {
            LineRenderer.useWorldSpace = false;
            LineRenderer.enabled = false;
            Targetter.gameObject.SetActive(false);
        }

        public Vector3 GetHitLocation()
        {
            Targetter.SetCanMove(false);
            var point = Targetter.RaycastAgainstGround(out float rayDistance);

            //prevents the targetter from going below the ground
            if (rayDistance > .25f)
                return transform.position;

            return point;
        }


        Vector3 StraightLinecast()
        {
            if (Physics.Linecast(TopRayPoint.position, BottomRayPoint.position + Vector3.down * 10, out var hit,
                    layerMask))
                return hit.point;
            else
                return default;
        }

        void UpdateLineRenderer()
        {
            if (!LineRenderer.useWorldSpace)
                LineRenderer.useWorldSpace = true;

            Vector3 startPos = transform.parent.position;
            Vector3 endPos = BottomRayPoint.position;
            float distance = Vector3.Distance(startPos, endPos);
            float maxHeight = distance * 0.5f;
            Vector3 midPos = new Vector3((startPos.x + endPos.x) * 0.5f, startPos.y + maxHeight,
                (startPos.z + endPos.z) * 0.5f);

            int segments = 20;
            Vector3[] points = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float t = (float)i / (float)segments;
                Vector3 point = Vector3.Lerp(Vector3.Lerp(startPos, midPos, t), Vector3.Lerp(midPos, endPos, t), t);
                points[i] = point;
            }

            LineRenderer.positionCount = points.Length;
            LineRenderer.SetPositions(points);
        }

        public void LoadComponents(Transform playerTransform)
        {
            LineRenderer = GetComponent<LineRenderer>();
        }
    }
}