using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "FlightPathSO/Curve")]
public class FlightPathCurveSO : FlightPathBaseSO
{
    public List<FlightPathCurveNode> CurveNodes;

    public override List<PathPointData> GetPathData(float prevDistance, Vector3 prevPosition)
    {
        List<PathPointData> pathData = new List<PathPointData>();
        int iterations = 20;
        Vector3 prevPoint;
        Vector3 currPoint;
        float distance = prevDistance;

        for (int nodeIndex = 0; nodeIndex < CurveNodes.Count - 1; nodeIndex++)
        {
            prevPoint = CurveNodes[nodeIndex].NodePosiion;

            for (int i = 1; i <= iterations; i++)
            {
                currPoint = CurveMath.NodeLerpPostion(
                    CurveNodes[nodeIndex],
                    CurveNodes[nodeIndex + 1],
                    (float)i / (float)iterations);

                pathData.Add(
                    new PathPointData(
                        distance,
                        prevPoint + prevPosition,
                        CurveMath.NodeLerpRotation(
                            CurveNodes[nodeIndex],
                            CurveNodes[nodeIndex + 1],
                            currPoint - prevPoint,
                            (float)i / (float)iterations)));

                distance += (currPoint - prevPoint).magnitude;
                prevPoint = currPoint;
            }
        }

        pathData.Add(
            new PathPointData(
                distance,
                CurveNodes[CurveNodes.Count - 1].NodePosiion + prevPosition,
                CurveNodes[CurveNodes.Count - 1].GetMainRotation()));

        return pathData;
    }
}



[System.Serializable]
public class FlightPathCurveNode
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

    public Quaternion GetMainRotation()
    {
        if (DirectionMode == FlightCurvePointerNodeDirectionMode.Aligned)
            return Quaternion.LookRotation(Vector3.forward,
                Quaternion.Euler(Vector3.forward * 90f) * MainHandlePositionReleative);
        else
            return Quaternion.Euler(CustomDirection);
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

    public static Vector3 NodeLerpPostion(FlightPathCurveNode startNode, FlightPathCurveNode endNode, float t)
    {
        return CubicLerp(
                    startNode.NodePosiion,
                    startNode.GetMainHandlePos(),
                    endNode.GetPrevHandlePos(),
                    endNode.NodePosiion,
                    t);
    }

    public static Quaternion NodeLerpRotation(FlightPathCurveNode startNode, FlightPathCurveNode endNode, Vector3 pointDirection, float t)
    {
        return (startNode.TransitionMode == FlightCurvePointerNodeTransitionMode.Aligned) ?
                    Quaternion.LookRotation(
                        Vector3.forward, 
                        Quaternion.Euler(Vector3.forward * 90f) * pointDirection) :
                    Quaternion.Lerp(
                        startNode.GetMainRotation(),
                        endNode.GetMainRotation(),
                        t);
    }

    public static float PathLength(FlightPathCurveNode startNode, FlightPathCurveNode endNode, int iterations = 10)
    {
        float length = 0f;
        Vector3 prevPoint = startNode.NodePosiion;
        Vector3 currPoint;

        for (int i = 1; i <= iterations; i++)
        {
            currPoint = NodeLerpPostion(startNode, endNode, (float)i / (float)iterations);
            length += Vector3.Distance(prevPoint, currPoint);
            prevPoint = currPoint;
        }

        return length;
    }
}
