using UnityEngine;
using System.Collections.Generic;

public abstract class FlightPathBaseSO : ScriptableObject
{
    public abstract List<PathPointData> GetPathData(float prevDistance, Vector3 prevPosition);
}

[System.Serializable]
public class PathPointData
{
    public float Distance;
    public Vector3 Position;
    public Quaternion Rotation;

    public PathPointData()
    {
        Distance = 0f;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }

    public PathPointData(float distance, Vector3 position, Quaternion rotation)
    {
        Distance = distance;
        Position = position;
        Rotation = rotation;
    }
}