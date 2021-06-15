using UnityEditor;
using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    const float TAU = 6.283185307179586f;

    [SerializeField] Camera cam;
    [SerializeField] Transform[] objects;
    void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);
    float RadiansToDegree(float angRad) => angRad * 360 / TAU;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        var furthestObject = GetFurthestObject(out float opposite, out float hipothenuse);
        Handles.color = Color.cyan;
        Handles.DrawAAPolyLine(cam.transform.position, furthestObject);

        cam.fieldOfView = RadiansToDegree(Mathf.Asin(opposite / hipothenuse)) * 2;
    }

    Vector3 GetFurthestObject(out float maxDistance, out float relativeVecDistance)
    {
        maxDistance = 0f;
        relativeVecDistance = 0f;
        var camUp = cam.transform.up;
        Vector3 furthestObject = Vector3.zero;
        foreach (var o in objects)
        {
            var relativePos = o.position - cam.transform.position;
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(cam.transform.position, o.position);

            var distanceY = Mathf.Abs(Vector3.Dot(camUp, relativePos));
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(o.position, o.position + cam.transform.up * -Vector3.Dot(camUp, relativePos));

            if (distanceY > maxDistance)
            {
                relativeVecDistance = relativePos.magnitude;
                maxDistance = distanceY;
                furthestObject = o.position;
            }
        }

        return furthestObject;
    }
}
