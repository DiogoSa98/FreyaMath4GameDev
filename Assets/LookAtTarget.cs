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
        var look = transform.right;
        var targetLook = (targetPos - transform.position).normalized;
        var dotProd = look.x * targetLook.x +
                    look.y * targetLook.y +
                    look.z * targetLook.z;

        Gizmos.color = new Color(0.1f, 0.5f, 0.0f);

        Gizmos.DrawLine(transform.position, transform.position + targetLook);
        return dotProd >= lookAngleTreshold;
    }
}
