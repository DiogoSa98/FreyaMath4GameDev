using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class TransformPointSpace : MonoBehaviour
{
    [SerializeField] Transform worldPoint;
    [SerializeField] Transform localPoint;
    [SerializeField] bool displayLocalSpaceCoords;
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 localPointPosition;

        Vector3 worldPointPosition;
        if (displayLocalSpaceCoords)
        {
            localPointPosition = localPoint.localPosition;

            var baseLocalPos = worldPoint.position - transform.position;
            // projecting to localSpace
            var x = Vector2.Dot(transform.right, baseLocalPos);
            var y = Vector2.Dot(transform.up, baseLocalPos);
            worldPointPosition = new Vector3(x, y, baseLocalPos.z);
        }
        else
        {
            worldPointPosition = worldPoint.position;

            // projecting to localSpace
            Vector2 worldOffset = transform.right * localPoint.localPosition.x + transform.up * localPoint.localPosition.y;
            localPointPosition = new Vector3(worldOffset.x, worldOffset.y, localPoint.position.z) + transform.position;
        }


        Gizmos.color = new Color(0f, 0.8f, 0.1f);
        Gizmos.DrawSphere(localPoint.position, 0.04f);
        Handles.Label(new Vector3(localPoint.position.x, localPoint.position.y - 0.06f, localPoint.position.z), "local point: (" + Math.Round(localPointPosition.x, 3) + ", " + Math.Round(localPointPosition.y, 3) + ", " + Math.Round(localPointPosition.z, 3) + ")");

        Gizmos.color = new Color(1f, 0.5f, 0.0f);
        Gizmos.DrawSphere(worldPoint.position, 0.04f);
        Handles.Label(new Vector3(worldPoint.position.x, worldPoint.position.y+0.06f, worldPoint.position.z), "world point: ("+ Math.Round(worldPointPosition.x, 3)+", "+ Math.Round(worldPointPosition.y, 3) + ", "+ Math.Round(worldPointPosition.z, 3) + ")");

        Gizmos.color = new Color(0.5f, 0.5f, 0.0f);
        Gizmos.DrawLine(Vector3.zero, worldPoint.position); // vector from world space
        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawLine(Vector3.zero, Vector3.right * 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 0.2f);
        Gizmos.color = new Color(0f, 1f, 0f);
        Gizmos.DrawLine(Vector3.zero, Vector3.up * 0.2f);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.2f);
        Gizmos.color = new Color(0.0f, 0.5f, 0.5f);
        Gizmos.DrawLine(transform.position, worldPoint.position); // vector from local space
    }
#endif
}
