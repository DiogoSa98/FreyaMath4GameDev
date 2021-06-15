using UnityEditor;
using UnityEngine;

public class RegularPolygonDrawer : MonoBehaviour
{
    const float TAU = 6.283185307179586f;

    [SerializeField] [Range(2, 20)] int regularPolygonSides;
    [SerializeField] [Range(1, 19)] int skipVerticeDepth; // max shoud be regularPolygonSides - 1
    void DrawRay(Vector3 p, Vector3 dir) => Handles.DrawAAPolyLine(p, p + dir);
    public void OnValidate()
    {
        skipVerticeDepth = Mathf.Clamp(skipVerticeDepth, 1, regularPolygonSides - 1);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        
        DrawUnitCircle();

        var polygonPoints = DivideCircle();
        DrawPolygon(polygonPoints);
    }

    void DrawUnitCircle()
    {
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
    Vector3[] DivideCircle()
    {
        Vector3[] points = new Vector3[regularPolygonSides];
        for (int i = 0; i < regularPolygonSides; i++)
        {
            var circleSlicePercentage = ((float)1 / (float)regularPolygonSides) * i;
            var angleRad = circleSlicePercentage * TAU;
            points[i] = DrawPointInCircle(AngleToDir(angleRad));
        }

        return points;
    }

    Vector3 AngleToDir(float angleRad)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    Vector3 DrawPointInCircle(Vector3 unitCircleVector)
    {
        var p = transform.position + unitCircleVector;
        Gizmos.DrawSphere(p, 0.05f);
        return p;
    }

    void DrawPolygon(Vector3[] polygonPoints)
    {
        Handles.color = Color.cyan;
        for (int i = 0; i < polygonPoints.Length; i++)
        {
            Handles.DrawAAPolyLine(polygonPoints[i], polygonPoints[(i + skipVerticeDepth) % polygonPoints.Length]);
        }
    }
}
