using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMovement : MonoBehaviour
{
    struct HermiteSpline
    {
        public Vector3 startPoint, endPoint;
        public Vector3 startTangent, endTangent;
    }

    HermiteSpline spline = new HermiteSpline();

    public void StartMovement(Vector3 start, Vector3 end, Vector3 tan1, Vector3 tan2)
    {
        spline.startPoint = start;
        spline.endPoint = end;
        spline.startTangent = tan1;
        spline.endTangent = tan2;

        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        for (float t = 0.0f; t < 1.0f; t += 0.01f)
        {
            //float ease = Ease(t);
            float ease = CubicEase(t, 0.0f, 0.55f, 1.0f);
            this.transform.position = GetPosition(ease);
            this.transform.rotation = GetOrientation(ease);
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    public void StopMovement()
    {
        StopCoroutine("Move");
    }

    Vector3 GetPosition(float t)
    {
        // Clamp parameter to valid range
        if (t < 0.0f) { t = 0.0f; }
        if (t > 1.0f) { t = 1.0f; }

        float t2 = t*t;
        float t3 = t2*t;

        Vector3 p0 = spline.startPoint;
        Vector3 p1 = spline.endPoint;
        Vector3 m0 = spline.startTangent - p0;
        Vector3 m1 = spline.endTangent - p1;

        Vector3 pos = (((2.0f*t3 - 3.0f*t2 + 1.0f) * p0)
                    + ((t3 - 2.0f*t2 + t) * m0)
                    + ((-2.0f*t3 + 3.0f*t2) * p1)
                    + ((t3-t2) * m1));

        return new Vector3(pos.x, this.transform.position.y, pos.z);
    }

    Quaternion GetOrientation(float t)
    {
        // Get orientation from tangent along the curve
        Vector3 curve_tan = GetPosition(t + 0.01f) - GetPosition(t);
        curve_tan.Normalize();

        // Check if we are close to the last point along the path
        if (t >= 0.99f){
            // The last point does not have a well-defined tangent, so use the one of the curve
            curve_tan = spline.endTangent;
        }

        // Create orientation from the tangent
        Quaternion orient = new Quaternion();
        orient.SetLookRotation(curve_tan, Vector3.up);

        return orient;
    }

    float Ease(float t)
    {
        return (Mathf.Sin(t * Mathf.PI - Mathf.PI / 2.0f) + 1.0f) / 2.0f;
    }

    float CubicEase(float t, float b, float c, float d)
    {
        //t = current time
        //b = start value
        //c = change in value
        //d = duration
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
    }
}
