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
    [SerializeField] [Range(1f, 8f)] float coilLineWidth;
    [SerializeField] Color coilColorStart;
    [SerializeField] Color coilColorEnd;
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
            for (float stepTurnPercentage = 0f; stepTurnPercentage < TAU; stepTurnPercentage += 0.01f)
            {
                var angleStep = stepTurnPercentage;
                var turnXZ = AngleToDir(angleStep) * coilRadius;

                var height = heightPerTurn * stepTurnPercentage  / TAU;
                height += heightPerTurn * i;

                var p1 = new Vector3(turnXZ.x, height, turnXZ.y) + transform.position;
                //Gizmos.DrawSphere(p1, 0.02f);
                if (p0 != transform.position)
                {
                    Handles.color = Color.Lerp(coilColorStart, coilColorEnd, MapFloat(0f, coilHeight, 0f, 01f, height));
                    Handles.DrawAAPolyLine(coilLineWidth, p0, p1);
                }

                p0 = p1;
            }
        }
    }

    Vector2 AngleToDir(float angleRad)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    float MapFloat(float iMin, float iMax, float oMin, float oMax, float value)
    {
        return oMin + (value - iMin) * (oMax - oMin) / (iMax - iMin);
    }
}
