using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class FlightPathCurveSODrawer : MonoBehaviour {

	public FlightPathCurveSO CurveSO;
    public Transform DemoObj;
    [Range(0f, 1f)]
    public float DemoSlider;

    private List<PathPointData> PathPointsData;

    private void OnValidate()
    {
        if (PathPointsData == null || DemoObj == null)
            return;

        float currDistance = Mathf.Lerp(
            PathPointsData[0].Distance,
            PathPointsData[PathPointsData.Count - 1].Distance,
            DemoSlider);

        PathPointData prevPoint = PathPointsData[0];
        PathPointData nextPoint = PathPointsData[0];

        foreach (PathPointData pointData in PathPointsData)
        {
            if (pointData.Distance <= currDistance)
                prevPoint = pointData;
            else
            {
                nextPoint = pointData;
                break;
            }
        }

        float localLerpPos = Mathf.InverseLerp(
            prevPoint.Distance,
            nextPoint.Distance,
            currDistance);

        DemoObj.position = Vector3.Lerp(
            prevPoint.Position,
            nextPoint.Position,
            localLerpPos);
        DemoObj.rotation = Quaternion.Lerp(
            prevPoint.Rotation,
            nextPoint.Rotation,
            localLerpPos);
    }

        private void OnDrawGizmos()
    {
        if (CurveSO == null)
            return;
        if (CurveSO.CurveNodes == null)
            return;
        if (CurveSO.CurveNodes.Count <= 1)
            return;

        PathPointsData = CurveSO.GetPathData(0f, transform.position);
        

        Vector3 prevPosition = PathPointsData[0].Position;
        foreach (PathPointData pointData in PathPointsData)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                pointData.Position,
                pointData.Position + pointData.Rotation * Vector3.right * 10f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(
                prevPosition,
                pointData.Position);

            prevPosition = pointData.Position;
        }
    }
}
