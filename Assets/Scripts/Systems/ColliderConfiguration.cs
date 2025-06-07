using UnityEngine;


//creates colliders for Parent objects of child objects with renderers

namespace Etheral
{
    public class ColliderConfiguration
    {
        public BoxCollider CreateBoxCollider(GameObject gameObject, Renderer childRenderer = null)
        {
            //Get the bounds of the child's object's mesh
            if (childRenderer == null)
                childRenderer = gameObject.GetComponentInChildren<Renderer>();


            //Get the bounds of the child's object's mesh
            Bounds childBounds = childRenderer.bounds;

            //Create new collider
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();

            // //Set the size of the collider to match the bounds of the child object's mesh
            // boxCollider.size = childBounds.size;

            // var size = boxCollider.size.y;

            boxCollider.size = new Vector3(childBounds.size.x * .75f,
                childBounds.size.y,
                childBounds.size.z * .75f);

            //Calculate the center of the child object's bounds
            Vector3 centerOffset = childBounds.center - gameObject.transform.position;

            //Set the center of the collider to match the center of the child object's bounds
            boxCollider.center = centerOffset;

            return boxCollider;
        }

        public SphereCollider CreateSphereCollider(GameObject gameObject, Renderer childRenderer = null)
        {
            //Get the bounds of the child's object's mesh
            if (childRenderer == null)
                childRenderer = gameObject.GetComponentInChildren<Renderer>();

            //Get the bounds of the child's object's mesh
            Bounds childBounds = childRenderer.bounds;

            //Create new collider
            SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();

            //Set the size of the collider to match the bounds of the child object's mesh
            sphereCollider.radius = childBounds.size.x / 2;


            //Calculate the center of the child object's bounds
            Vector3 centerOffset = childBounds.center - gameObject.transform.position;

            //Set the center of the collider to match the center of the child object's bounds
            sphereCollider.center = centerOffset;

            return sphereCollider;
        }

        public CapsuleCollider CreateCapsuleCollider(GameObject gameObject, Renderer childRenderer = null)
        {
            //Get the bounds of the child's object's mesh
            if (childRenderer == null)
                childRenderer = gameObject.GetComponentInChildren<Renderer>();

            //Get the bounds of the child's object's mesh
            Bounds childBounds = childRenderer.bounds;

            //Create new collider
            CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();

            //Set the size of the collider to match the bounds of the child object's mesh
            capsuleCollider.radius = childBounds.size.x / 2;
            capsuleCollider.height = childBounds.size.y;

            //Calculate the center of the child object's bounds
            Vector3 centerOffset = childBounds.center - gameObject.transform.position;

            //Set the center of the collider to match the center of the child object's bounds
            capsuleCollider.center = centerOffset;

            return capsuleCollider;
        }
    }
}