using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    const float TAU = 6.283185307179586f;

    [SerializeField] Camera cam;
    [SerializeField] Transform[] objects;
    List<float> objectsRadius = new List<float>();
    void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);
    float RadiansToDegree(float angRad) => angRad * 360 / TAU;

    private void OnDrawGizmos()
    {
        GiveObjectsRadius();
        //var furthestObject = GetFurthestObject(out float opposite, out float hipothenuse);
        var fovAngle = GetBiggerAngle(out Vector3 selectedObject);
        Handles.color = Color.cyan;
        Handles.DrawAAPolyLine(cam.transform.position, selectedObject);

        cam.fieldOfView = RadiansToDegree(fovAngle) * 2;
    }

    void GiveObjectsRadius()
    {
        Random.InitState(8);
        Gizmos.color = Color.grey;
        for (int i = 0; i < objects.Length; i++)
        {
            var radius = Random.Range(0.4f, 1.5f);
            objectsRadius.Add(radius);
            Gizmos.DrawWireSphere(objects[i].position, radius);
        }
    }

    float GetBiggerAngle(out Vector3 selectedObject)
    {
        var maxAngle = 0f;
        selectedObject = Vector3.zero;
        
        var camUp = cam.transform.up;

        foreach (var o in objects)
        {
            var relativePos = o.position - cam.transform.position;
            Handles.color = Color.gray;
            Handles.DrawAAPolyLine(cam.transform.position, o.position);

            var distanceY = Mathf.Abs(Vector3.Dot(camUp, relativePos));
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(o.position, o.position + cam.transform.up * -Vector3.Dot(camUp, relativePos));

            var angle = Mathf.Asin(distanceY / relativePos.magnitude);
            if (angle > maxAngle)
            {
                selectedObject = o.position;
                maxAngle = angle;
            }
        }

        return maxAngle;
    }
}
