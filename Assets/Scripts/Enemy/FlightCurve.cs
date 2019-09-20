using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class FlightCurve : MonoBehaviour {

    public List<FlightCurveNode> CurveNodes;

    private void OnDrawGizmos()
    {
        if (CurveNodes == null)
            return;


        if (CurveNodes.Count <= 1)
            return;

        int iterations = 20;

        Vector3 prevPoint;
        Vector3 currPoint;


        for (int nodeIndex = 0; nodeIndex < CurveNodes.Count - 1; nodeIndex++)
        {
            prevPoint = CurveNodes[nodeIndex].NodePosiion;
            for (int i = 1; i <= iterations; i++)
            {
                currPoint = CurveMath.NodeLerp(
                    CurveNodes[nodeIndex],
                    CurveNodes[nodeIndex + 1],
                    (float)i / (float)iterations);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(prevPoint + transform.position, currPoint + transform.position);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(
                    prevPoint + transform.position,
                    prevPoint + transform.position + Vector3.Normalize(Vector3.Lerp(
                        CurveNodes[nodeIndex].MainHandlePositionReleative,
                        CurveNodes[nodeIndex + 1].MainHandlePositionReleative,
                        (float)i / (float)iterations))
                    );

                prevPoint = currPoint;
            }
        }
    }
}

public static class CurveMath
{
    public static Vector3 TripleLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return Vector3.Lerp(
            Vector3.Lerp(a, b, t),
            Vector3.Lerp(b, c, t),
            t);
    }

    public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        return Vector3.Lerp(
            TripleLerp(a, b, c, t),
            TripleLerp(b, c, d, t),
            t);
    }

    public static Vector3 NodeLerp(FlightCurveNode startNode, FlightCurveNode endNode, float t)
    {
        return CubicLerp(
                    startNode.NodePosiion,
                    startNode.GetMainHandlePos(),
                    endNode.GetPrevHandlePos(),
                    endNode.NodePosiion,
                    t);
    }

    public static float PathLength(FlightCurveNode startNode, FlightCurveNode endNode, int iterations = 10)
    {
        float length = 0f;
        Vector3 prevPoint = startNode.NodePosiion;
        Vector3 currPoint;

        for (int i = 1; i <= iterations; i++)
        {
            currPoint = NodeLerp(startNode, endNode, (float)i / (float)iterations);
            length += Vector3.Distance(prevPoint, currPoint);
            prevPoint = currPoint;
        }

        return length;
    }
}

public class FlightCurvePart
{
    public float Length { get; private set; }
    public FlightCurveNode StartNode;
    public FlightCurveNode EndNode;

    public void UpdatePath()
    {
        Length = 0f;
    }
}


[System.Serializable]
public class FlightCurveNode
{
    public FlightCurveNodeHandlesMode HandlesMode;
    public Vector3 NodePosiion;
    public Vector3 MainHandlePositionReleative;
    public Vector3 SubHandlePositionReleative;
    public FlightCurvePointerNodeDirectionMode DirectionMode;
    public FlightCurvePointerNodeTransitionMode TransitionMode;
    public Vector3 CustomDirection;

    public Vector3 GetMainHandlePos()
    {
        if (HandlesMode != FlightCurveNodeHandlesMode.None)
            return MainHandlePositionReleative + NodePosiion;
        else
            return NodePosiion;
    }

    public Vector3 GetPrevHandlePos()
    {
        switch (HandlesMode)
        {
            case FlightCurveNodeHandlesMode.Similiar:
                return -MainHandlePositionReleative + NodePosiion;
            case FlightCurveNodeHandlesMode.CoDirected:
                return -MainHandlePositionReleative.normalized * SubHandlePositionReleative.magnitude + NodePosiion;
            case FlightCurveNodeHandlesMode.Different:
                return SubHandlePositionReleative + NodePosiion;
            default:
                return NodePosiion;
        }
    }
}

    public enum FlightCurveNodeHandlesMode
{
    Similiar,
    CoDirected,
    Different,
    None
}

public enum FlightCurvePointerNodeDirectionMode
{
    Aligned,
    Custom
}

public enum FlightCurvePointerNodeTransitionMode
{
    Aligned,
    Lerped
}
