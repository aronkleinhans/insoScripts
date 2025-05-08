using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModifyFloatingObjectMesh : MonoBehaviour
{
    Transform objectTransform;


    public List<TriangleData> underwaterTriangleData = new List<TriangleData>();
    public ModifyFloatingObjectMesh(GameObject floatingObject)
    {

    }

    public void DisplayMesh(Mesh mesh, string name, List<TriangleData> triangleData)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Build the mesh
        for (int i = 0; i < triangleData.Count; i++)
        {
            //From global coordinates to local
            Vector3 p1 = objectTransform.InverseTransformPoint(triangleData[i].p1);
        }
    }
}
