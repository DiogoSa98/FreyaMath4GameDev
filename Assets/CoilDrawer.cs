using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoilDrawer : MonoBehaviour
{
    const float TAU = 6.283185307179586f;

    [SerializeField] [Range (1, 10)] int numberOfTurns;
    [SerializeField] [Range(0.1f, 4f)] float coilHeight;
    [SerializeField] [Range(0.1f, 4f)] float coilRadius;
    void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        

        var heightPerTurn = coilHeight / (float)numberOfTurns;
        Handles.color = Color.green;
        DrawRay(transform.position, transform.up * heightPerTurn * numberOfTurns);
        Handles.color = Color.blue;
        DrawRay(transform.position, transform.forward * coilRadius);

        var p0 = transform.position;
        for (int i = 0; i < numberOfTurns; i++)
        {
            for (float stepTurnPercentage = 0f; stepTurnPercentage < TAU; stepTurnPercentage += 0.05f)
            {
                var angleStep = stepTurnPercentage;
                var turnXZ = AngleToDir(angleStep) * coilRadius;

                var height = heightPerTurn * stepTurnPercentage  / TAU;
                height += heightPerTurn * i;

                var p1 = new Vector3(turnXZ.x, height, turnXZ.y) + transform.position;
                //Gizmos.DrawSphere(p1, 0.02f);
                Handles.color = Color.white;
                Handles.DrawAAPolyLine(p0, p1);

                p0 = p1;
            }
        }
    }

    Vector2 AngleToDir(float angleRad)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
