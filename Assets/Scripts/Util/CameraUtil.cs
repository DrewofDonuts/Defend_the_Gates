using UnityEngine;

namespace Etheral
{
    public static class CameraUtil
    {
        public static bool IsVisibleFrom(Camera camera, Renderer objectRenderer)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

            //Disable camera check for testing
            return GeometryUtility.TestPlanesAABB(planes, objectRenderer.bounds);
        }
    }
}