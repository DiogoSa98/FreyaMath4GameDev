using UnityEditor;
using UnityEngine;

public class TurretPlacement : MonoBehaviour
{
    [SerializeField] [Range(0.2f, 2f)] float height;
    [SerializeField] [Range(0.2f, 2f)] float gunDistance;
    [SerializeField] [Range(0.2f, 2f)] float barrelLength;
    private void OnDrawGizmos()
    {
        Vector3 head = transform.position;
        Vector3 lookDir = transform.forward;

        if (Physics.Raycast(head, lookDir, out RaycastHit hit))
        {
            Vector3 hitPos = hit.point;
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(head, hitPos);

            var turretMatrix = TurretLocalSpace(hitPos, hit.normal, lookDir);

            DrawBoundingBox(turretMatrix);
            DrawTurret(turretMatrix);
        }
    }
    void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);

    Matrix4x4 TurretLocalSpace(Vector3 hitPos, Vector3 hitNormal, Vector3 lookDir)
    {
        Vector3 up = hitNormal;
        Handles.color = Color.green;
        DrawRay(hitPos, up);
        var right = Vector3.Cross(up, lookDir).normalized;
        Handles.color = Color.red;
        DrawRay(hitPos, right);
        var forward = Vector3.Cross(right, up).normalized;
        Handles.color = Color.blue;
        DrawRay(hitPos, forward);

        var matrix = new Matrix4x4();
        matrix.SetColumn(0, right);
        matrix.SetColumn(1, hitNormal);
        matrix.SetColumn(2, forward);
        matrix.SetColumn(3, new Vector4(hitPos.x, hitPos.y, hitPos.z, 1));
        return matrix;
    }
    void DrawBoundingBox(Matrix4x4 spaceToDraw)
    {
        Vector3[] corners = new Vector3[]{
            // bottom 4 positions:
            new Vector3( 1, 0, 1 ),
            new Vector3( -1, 0, 1 ),
            new Vector3( -1, 0, -1 ),
            new Vector3( 1, 0, -1 ),
            // top 4 positions:
            new Vector3( 1, 2, 1 ),
            new Vector3( -1, 2, 1 ),
            new Vector3( -1, 2, -1 ),
            new Vector3( 1, 2, -1 )
        };

        // convert corners to turret world space
        Vector3[] cornersWorldSpace = new Vector3[corners.Length];
        for (int i = 0; i < corners.Length; i++)
        {
            cornersWorldSpace[i] = spaceToDraw.MultiplyPoint(corners[i]);
        }

        // draw box
        foreach (var item in cornersWorldSpace)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(item, 0.05f);
        }

        Handles.color = Color.yellow;
        Handles.DrawAAPolyLine(cornersWorldSpace[0], cornersWorldSpace[1]);
        Handles.DrawAAPolyLine(cornersWorldSpace[0], cornersWorldSpace[3]);
        Handles.DrawAAPolyLine(cornersWorldSpace[0], cornersWorldSpace[4]);
        Handles.DrawAAPolyLine(cornersWorldSpace[2], cornersWorldSpace[1]);
        Handles.DrawAAPolyLine(cornersWorldSpace[2], cornersWorldSpace[3]);
        Handles.DrawAAPolyLine(cornersWorldSpace[2], cornersWorldSpace[6]);
        Handles.DrawAAPolyLine(cornersWorldSpace[5], cornersWorldSpace[4]);
        Handles.DrawAAPolyLine(cornersWorldSpace[5], cornersWorldSpace[1]);
        Handles.DrawAAPolyLine(cornersWorldSpace[5], cornersWorldSpace[6]);
        Handles.DrawAAPolyLine(cornersWorldSpace[7], cornersWorldSpace[4]);
        Handles.DrawAAPolyLine(cornersWorldSpace[7], cornersWorldSpace[3]);
        Handles.DrawAAPolyLine(cornersWorldSpace[7], cornersWorldSpace[6]);
    }

    void DrawTurret(Matrix4x4 spaceToDraw)
    {
        Handles.color = Color.cyan;
        var spawnPoint = spaceToDraw.GetColumn(3);

        // base
        var up = spaceToDraw.GetColumn(1).normalized;
        var heightPoint = (height * up) + spawnPoint;
        Handles.DrawAAPolyLine(spawnPoint, heightPoint);

        // barrel holder
        var right = spaceToDraw.GetColumn(0).normalized;
        var holderLength = gunDistance / 2f;
        var holderRight = (right * holderLength) + heightPoint;
        Handles.DrawAAPolyLine(heightPoint, holderRight);
        var holderLeft = (-right * holderLength) + heightPoint;
        Handles.DrawAAPolyLine(heightPoint, holderLeft);

        //barrels
        var forward = spaceToDraw.GetColumn(2).normalized;
        var barrelRight = (forward * barrelLength) + holderRight;
        Handles.DrawAAPolyLine(holderRight, barrelRight);
        var barrelLeft = (forward * barrelLength) + holderLeft;
        Handles.DrawAAPolyLine(holderLeft, barrelLeft);
    }
}
