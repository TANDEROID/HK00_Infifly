using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(FlightCurve))]
public class FlightCurveEditor : Editor {

    private void OnSceneGUI()
    {
        FlightCurve curve = target as FlightCurve;

        Handles.color = Color.gray;

        for (int i = 0; i < curve.CurveNodes.Count; i++)
        {
            FlightCurveNode node = curve.CurveNodes[i];

            ChangeNodePosition(curve, node);

            if (node.HandlesMode != FlightCurveNodeHandlesMode.None)
            {
                ChangeMainHandlePosition(curve, node);
                ChangeSubHandlePositon(curve, node);
            }

            Handles.Label(node.NodePosiion + curve.transform.position, i.ToString());
        }
    }

    private void ChangeNodePosition(FlightCurve curve, FlightCurveNode node)
    {
        EditorGUI.BeginChangeCheck();
        Vector3 newNodePosition =
            Handles.FreeMoveHandle(
                node.NodePosiion + curve.transform.position,
                Quaternion.identity,
                HandleUtility.GetHandleSize(node.NodePosiion + curve.transform.position) / 7f,
                Vector3.zero,
                Handles.RectangleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change Node Position");
            node.NodePosiion = newNodePosition - curve.transform.position;
            EditorUtility.SetDirty(curve);
        }
    }

    private void ChangeMainHandlePosition(FlightCurve curve, FlightCurveNode node)
    {
        EditorGUI.BeginChangeCheck();
        Handles.DrawLine(node.NodePosiion + curve.transform.position, node.GetMainHandlePos() + curve.transform.position);
        Vector3 newMainHandlePositionReleative =
            Handles.FreeMoveHandle(
                node.GetMainHandlePos() + curve.transform.position,
                Quaternion.identity,
                HandleUtility.GetHandleSize(node.NodePosiion + curve.transform.position) / 10f,
                Vector3.zero,
                Handles.CircleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change Main Handle Position");
            node.MainHandlePositionReleative = newMainHandlePositionReleative - node.NodePosiion - curve.transform.position;
            if (node.HandlesMode == FlightCurveNodeHandlesMode.Similiar)
                node.SubHandlePositionReleative = -node.MainHandlePositionReleative;
            else if (node.HandlesMode == FlightCurveNodeHandlesMode.CoDirected)
                node.SubHandlePositionReleative = -node.MainHandlePositionReleative.normalized * node.SubHandlePositionReleative.magnitude;

            EditorUtility.SetDirty(curve);
        }
    }

    private void ChangeSubHandlePositon(FlightCurve curve, FlightCurveNode node)
    {
        EditorGUI.BeginChangeCheck();
        Handles.DrawLine(node.NodePosiion + curve.transform.position, node.GetPrevHandlePos() + curve.transform.position);
        Vector3 newPrevHandlePositionReleative =
            Handles.FreeMoveHandle(
                node.GetPrevHandlePos() + curve.transform.position,
                Quaternion.identity,
                HandleUtility.GetHandleSize(node.NodePosiion + curve.transform.position) / 10f,
                Vector3.zero,
                Handles.CircleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Change Sub Handle Position");
            node.SubHandlePositionReleative = newPrevHandlePositionReleative - node.NodePosiion - curve.transform.position;
            if (node.HandlesMode == FlightCurveNodeHandlesMode.Similiar)
                node.MainHandlePositionReleative = -node.SubHandlePositionReleative;
            else if (node.HandlesMode == FlightCurveNodeHandlesMode.CoDirected)
                node.MainHandlePositionReleative = -node.SubHandlePositionReleative.normalized * node.MainHandlePositionReleative.magnitude;

            EditorUtility.SetDirty(curve);
        }
    }
}
