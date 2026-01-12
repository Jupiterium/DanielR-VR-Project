using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshCollider))]
public class LineColliderGenerator : MonoBehaviour
{
    LineRenderer lineRenderer;
    MeshCollider meshCollider;
    Mesh mesh;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = new Mesh();

        // If your power lines never move, baking once in Start is enough.
        // If they sway in the wind, move this line to Update().
        BakeLineToMesh();
    }

    void BakeLineToMesh()
    {
        // 1. Tell the LineRenderer to output its geometry to our mesh object
        lineRenderer.BakeMesh(mesh, true);

        // 2. Assign that mesh to the collider
        meshCollider.sharedMesh = mesh;
    }
}