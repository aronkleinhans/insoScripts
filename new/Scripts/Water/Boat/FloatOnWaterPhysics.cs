using UnityEngine;

public class FloatOnWaterPhysics : MonoBehaviour
{
    //Drags
    public GameObject underWaterObject;

    // Script that's doing everything needed with the object's mesh to figurer out whats under/above water
    ModifyFloatingObjectMesh modifyObjectMesh;

    // Mesh for debugging
    Mesh underwaterMesh;

    // The floating objects rigidbody
    Rigidbody objectRigidbody;

    // The density of the water
    const float rhoWater = 1027f;

    private void Start()
    {
        // Get the object's rigidbody
        objectRigidbody = GetComponent<Rigidbody>();

        // Initialize the mesh modifier script
        modifyObjectMesh = new ModifyFloatingObjectMesh(gameObject);

        // All meshes
        modifyObjectMesh.DisplayMesh(underwaterMesh, "Underwater Mesh", modifyObjectMesh.underwaterTriangleData);
    }
}
