using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdaptiveFOV : MonoBehaviour
{
    const float TAU = 6.283185307179586f;

    [SerializeField] Camera cam;
    [SerializeField] Transform[] objects;
    List<float> objectsRadius;
    void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);
    float RadiansToDegree(float angRad) => angRad * 360 / TAU;

    private void OnDrawGizmos()
    {
        GiveObjectsRadius();

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
            //float radius = Random.Range(0.4f, 1.5f);
            float radius = 1f;
            objectsRadius.Add(radius);
            Gizmos.DrawWireSphere(objects[i].position, radius);
        }
    }

    float GetBiggerAngle(out Vector3 selectedObject)
    {
        var maxAngle = 0f;
        selectedObject = Vector3.zero;
        
        var camUp = cam.transform.up;

        for (int i=0; i<objects.Length; i++)
        {
            var obj = objects[i];
            var relativePos = obj.position - cam.transform.position;
            Handles.color = Color.gray;
            Handles.DrawAAPolyLine(cam.transform.position, obj.position);

            var distanceY = Mathf.Abs(Vector3.Dot(camUp, relativePos));
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(obj.position, obj.position + cam.transform.up * -Vector3.Dot(camUp, relativePos));

            var angleToPoint = Mathf.Asin(distanceY / relativePos.magnitude);
            var angleToRadius = Mathf.Asin(1 / relativePos.magnitude);
            //Debug.Log(objectsRadius[i]); objectsRadius list is fucked...
            var angle = angleToPoint + angleToRadius;
            Handles.Label(obj.position + Vector3.up * 0.5f, RadiansToDegree(angleToRadius).ToString());
            Handles.Label(obj.position - Vector3.up * 0.5f, RadiansToDegree(angleToPoint).ToString());

            if (angle > maxAngle)
            {
                selectedObject = obj.position;
                maxAngle = angle;
            }
        }

        return maxAngle;
    }
}
