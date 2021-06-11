using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class TransformPointSpace : MonoBehaviour
{
    [SerializeField] Transform point;
    [SerializeField] Transform validationPoint;
    [SerializeField] bool displayLocalSpaceCoords;
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.8f, 0.1f);
        var validationPointPosition = displayLocalSpaceCoords ? validationPoint.localPosition : validationPoint.position;
        Gizmos.DrawSphere(validationPoint.position, 0.04f);
        Handles.Label(new Vector3(validationPoint.position.x, validationPoint.position.y - 0.06f, validationPoint.position.z), "validation point: (" + Math.Round(validationPointPosition.x, 3) + ", " + Math.Round(validationPointPosition.y, 3) + ", " + Math.Round(validationPointPosition.z, 3) + ")");
        
        Vector3 pointPosition;
        if (displayLocalSpaceCoords)
        {
            var baseLocalPos = point.position - transform.position;
            // accounting for rotation
            var x2 = transform.right.x * baseLocalPos.x - transform.right.y * baseLocalPos.y;
            var y2 = transform.right.y * baseLocalPos.x + transform.right.x * baseLocalPos.y;
            pointPosition = new Vector3(x2, y2, baseLocalPos.z);
        }
        else
        {
            pointPosition = point.position;
        }

        Gizmos.color = new Color(1f, 0.5f, 0.0f);
        //Gizmos.DrawSphere(point.position, 0.02f);
        Gizmos.DrawSphere(point.position, 0.02f);
        Handles.Label(new Vector3(point.position.x, point.position.y+0.06f, point.position.z), "test point: ("+ Math.Round(pointPosition.x, 3)+", "+ Math.Round(pointPosition.y, 3) + ", "+ Math.Round(pointPosition.z, 3) + ")");

        Gizmos.color = new Color(0.5f, 0.5f, 0.0f);
        Gizmos.DrawLine(Vector3.zero, point.position); // vector from world space
        Gizmos.color = new Color(0.0f, 0.5f, 0.5f);
        Gizmos.DrawLine(transform.position, point.position); // vector from local space
    }
#endif
}
