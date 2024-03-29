using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BouncingLaser : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] int bounceCount;
    private void OnDrawGizmos()
    {
        void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);

        Vector3 emmissionPos = transform.position;
        Vector3 lookDir = transform.forward;

        Handles.color = Color.blue;
        DrawRay(emmissionPos, lookDir);
        
        for (int bounce = 0; bounce < bounceCount; bounce++)
        {
            if (Physics.Raycast(emmissionPos, lookDir, out RaycastHit hit))
            {
                Vector3 hitPos = hit.point;
                Vector3 normal = hit.normal;
                Handles.color = Color.cyan;
                Handles.DrawAAPolyLine(emmissionPos, hitPos);

                Handles.color = Color.green;
                DrawRay(hitPos, normal); // up
                Handles.color = Color.red; 
                var right = Vector3.Cross(normal, lookDir).normalized;
                DrawRay(hitPos, right);
                Handles.color = Color.blue;
                var forward = Vector3.Cross(right, normal).normalized;
                DrawRay(hitPos, forward);

                emmissionPos = hitPos;
                //lookDir = BounceDirection(lookDir, normal);
                lookDir = BounceDirectionQuickMath(lookDir, normal);
            }
            else
            {
                Handles.color = Color.cyan;
                DrawRay(emmissionPos, lookDir);
                break;
            }
        }
    }

    private Vector3 BounceDirection(Vector3 dir1, Vector3 normal)
    {
        var yLocal = Vector3.Dot(normal, -dir1);
        var right = Vector3.Cross(normal, dir1).normalized; // x local always 0
        var forward = Vector3.Cross(right, normal).normalized;
        var zLocal = Vector3.Dot(forward, dir1);
        var localDir = new Vector4(0, yLocal, zLocal, 0); // last point 0 cause only dir

        //covert local dir to world dir
        var matrix = new Matrix4x4();
        matrix.SetColumn(0, right);     
        matrix.SetColumn(1, normal);
        matrix.SetColumn(2, forward);
        //matrix.SetColumn(3, new Vector4(hitPos.x, hitPos.y, hitPos.z, 1)); not needed cause only converting dir
        return matrix.MultiplyPoint3x4(localDir);
    }

    private Vector3 BounceDirectionQuickMath(Vector3 dir, Vector3 normal)
    {
        var yProj = Vector3.Dot(normal, dir) * normal;
        return dir - 2 * yProj;
    }
}
