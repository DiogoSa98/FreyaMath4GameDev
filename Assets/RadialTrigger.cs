using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialTrigger : MonoBehaviour
{
    [SerializeField] Transform targetObject;
    [SerializeField] [Range(1, 5)] float triggerRadius;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.5f, 0.0f);
        Gizmos.DrawSphere(targetObject.position, 0.05f);

        Gizmos.color = ObjectInTrigger(targetObject.position) ? Color.green :  Color.red;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
#endif

    bool ObjectInTrigger(Vector3 objPos)
    {
        var diff = objPos - transform.position;

        // optimized for treshold, no actual distance required, just use squared version of equation
        var diffDist = diff.x * diff.x + diff.y * diff.y + diff.z * diff.z;
        return diffDist < triggerRadius * triggerRadius;
    }
}
