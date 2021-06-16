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

    private void OnDrawGizmos()
    {
        GiveObjectsRadius();

        //var biggerAngle = GetBiggerAngle(out Vector3 selectedObject);
        var biggerAngle = GetBiggerAngle3D(out Vector3 selectedObject);
        Handles.color = Color.cyan;
        Handles.DrawAAPolyLine(cam.transform.position, selectedObject);

        cam.fieldOfView = biggerAngle * 2 * Mathf.Rad2Deg;
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

    float GetBiggerAngle3D(out Vector3 selectedObject) // convert everything to camera local space
    {
        var maxAngle = float.MinValue;
        selectedObject = Vector3.zero;

        //var camUp = cam.transform.up;
        var camUp = Vector3.up;

        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            var relativePos = cam.transform.InverseTransformPoint(obj.position);
            Handles.color = Color.gray;
            Handles.DrawAAPolyLine(Vector3.zero, relativePos);

            // project point to 2D space camera (forward, up) 
            var relativePosFlat = new Vector3(relativePos.z, relativePos.y, 0f);
            
            var distanceY = Mathf.Abs(Vector3.Dot(camUp, relativePosFlat));
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(obj.position, obj.position + Vector3.up * -Vector3.Dot(camUp, relativePosFlat));

            var angleToPoint = Mathf.Asin(distanceY / relativePosFlat.magnitude);
            var angleToRadius = Mathf.Asin(1f / relativePosFlat.magnitude);
            //Debug.Log(objectsRadius[i]); objectsRadius list is fucked...
            var angle = angleToPoint + angleToRadius;
            Handles.Label(obj.position + Vector3.up * 0.7f, (angleToRadius * Mathf.Rad2Deg).ToString());
            Handles.Label(obj.position - Vector3.up * 0.5f, (angleToPoint * Mathf.Rad2Deg).ToString());

            if (angle > maxAngle)
            {
                selectedObject = obj.position;
                maxAngle = angle;
            }
        }

        return maxAngle;
    }

    //float GetBiggerAngle(out Vector3 selectedObject)
    //{
    //    var maxAngle = float.MinValue;
    //    selectedObject = Vector3.zero;

    //    var camUp = cam.transform.up;

    //    for (int i=0; i<objects.Length; i++)
    //    {
    //        var obj = objects[i];
    //        var relativePos = obj.position - cam.transform.position;
    //        Handles.color = Color.gray;
    //        Handles.DrawAAPolyLine(cam.transform.position, obj.position);

    //        var distanceY = Mathf.Abs(Vector3.Dot(camUp, relativePos));
    //        Handles.color = Color.red;
    //        Handles.DrawAAPolyLine(obj.position, obj.position + cam.transform.up * -Vector3.Dot(camUp, relativePos));

    //        var angleToPoint = Mathf.Asin(distanceY / relativePos.magnitude);
    //        var angleToRadius = Mathf.Asin(1f / relativePos.magnitude);
    //        //Debug.Log(objectsRadius[i]); objectsRadius list is fucked...
    //        var angle = angleToPoint + angleToRadius;
    //        Handles.Label(obj.position + Vector3.up * 0.7f, (angleToRadius * Mathf.Rad2Deg).ToString());
    //        Handles.Label(obj.position - Vector3.up * 0.5f, (angleToPoint * Mathf.Rad2Deg).ToString());

    //        if (angle > maxAngle)
    //        {
    //            selectedObject = obj.position;
    //            maxAngle = angle;
    //        }
    //    }

    //    return maxAngle;
    //}
}
