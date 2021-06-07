using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] Transform targetObject;
    [SerializeField] [Range(0, 1)] float lookAngleTreshold;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0.0f);
        Gizmos.DrawSphere(targetObject.position, 0.05f);

        Gizmos.color = LookingAt(targetObject.position) ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);

        Gizmos.DrawSphere(transform.position, 0.05f);
    }
#endif

    bool LookingAt(Vector3 targetPos)
    {
        var look = transform.position + transform.right;
        var dotProd = look.x * targetPos.x +
                    look.y * targetPos.y +
                    look.z * targetPos.z;

        return dotProd >= lookAngleTreshold;
    }
}
