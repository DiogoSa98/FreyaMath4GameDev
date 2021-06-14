using UnityEditor;
using UnityEngine;

public class MeshAreaCalculator : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    private void OnDrawGizmos()
    {
        float area = 0;
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;
        for (int i = 0; i < triangles.Length; i+=3)
        {
            var p0 = vertices[triangles[i]];
            var p1 = vertices[triangles[i+1]];
            var p2 = vertices[triangles[i+2]];

            var v1 = p1 - p0;
            var v2 = p2 - p0;
            var triangleArea = Vector3.Cross(v1, v2).magnitude / 2f;
            
            area += triangleArea;
        }

        Handles.Label(new Vector3(transform.position.x - 0.5f, transform.position.y + 1f, transform.position.z), "Area: " + area + "m2");
    }
}
