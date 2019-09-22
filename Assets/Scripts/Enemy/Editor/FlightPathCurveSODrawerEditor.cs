using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(FlightPathCurveSODrawer))]
public class FlightPathCurveSODrawerEditor : Editor {


    private void OnSceneGUI()
    {
        FlightPathCurveSODrawer curveDrawer = target as FlightPathCurveSODrawer;

        if (curveDrawer.CurveSO == null)
            return;

        Handles.color = Color.gray;
        for (int i = 0; i < curveDrawer.CurveSO.CurveNodes.Count; i++)
        {
            FlightPathCurveNode node = curveDrawer.CurveSO.CurveNodes[i];

            ChangeNodePosition(curveDrawer, node);

            if (node.HandlesMode != FlightCurveNodeHandlesMode.None)
            {
                ChangeMainHandlePosition(curveDrawer, node);
                ChangeSubHandlePositon(curveDrawer, node);
            }

            Handles.Label(node.NodePosiion + curveDrawer.transform.position, i.ToString());
        }
    }

    private void ChangeNodePosition(FlightPathCurveSODrawer curveDrawer, FlightPathCurveNode node)
    {
        EditorGUI.BeginChangeCheck();
        Vector3 newNodePosition =
            Handles.FreeMoveHandle(
                node.NodePosiion + curveDrawer.transform.position,
                Quaternion.identity,
                HandleUtility.GetHandleSize(node.NodePosiion + curveDrawer.transform.position) / 7f,
                Vector3.zero,
                Handles.RectangleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curveDrawer.CurveSO, "Change Node Position");
            node.NodePosiion = newNodePosition - curveDrawer.transform.position;
            EditorUtility.SetDirty(curveDrawer);
        }
    }

    private void ChangeMainHandlePosition(FlightPathCurveSODrawer curveDrawer, FlightPathCurveNode node)
    {
        EditorGUI.BeginChangeCheck();
        Handles.DrawLine(node.NodePosiion + curveDrawer.transform.position, node.GetMainHandlePos() + curveDrawer.transform.position);
        Vector3 newMainHandlePositionReleative =
            Handles.FreeMoveHandle(
                node.GetMainHandlePos() + curveDrawer.transform.position,
                Quaternion.identity,
                HandleUtility.GetHandleSize(node.NodePosiion + curveDrawer.transform.position) / 10f,
                Vector3.zero,
                Handles.CircleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curveDrawer.CurveSO, "Change Main Handle Position");
            node.MainHandlePositionReleative = newMainHandlePositionReleative - node.NodePosiion - curveDrawer.transform.position;
            if (node.HandlesMode == FlightCurveNodeHandlesMode.Similiar)
                node.SubHandlePositionReleative = -node.MainHandlePositionReleative;
            else if (node.HandlesMode == FlightCurveNodeHandlesMode.CoDirected)
                node.SubHandlePositionReleative = -node.MainHandlePositionReleative.normalized * node.SubHandlePositionReleative.magnitude;

            EditorUtility.SetDirty(curveDrawer);
        }
    }

    private void ChangeSubHandlePositon(FlightPathCurveSODrawer curveDrawer, FlightPathCurveNode node)
    {
        EditorGUI.BeginChangeCheck();
        Handles.DrawLine(node.NodePosiion + curveDrawer.transform.position, node.GetPrevHandlePos() + curveDrawer.transform.position);
        Vector3 newPrevHandlePositionReleative =
            Handles.FreeMoveHandle(
                node.GetPrevHandlePos() + curveDrawer.transform.position,
                Quaternion.identity,
                HandleUtility.GetHandleSize(node.NodePosiion + curveDrawer.transform.position) / 10f,
                Vector3.zero,
                Handles.CircleHandleCap);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curveDrawer.CurveSO, "Change Sub Handle Position");
            node.SubHandlePositionReleative = newPrevHandlePositionReleative - node.NodePosiion - curveDrawer.transform.position;
            if (node.HandlesMode == FlightCurveNodeHandlesMode.Similiar)
                node.MainHandlePositionReleative = -node.SubHandlePositionReleative;
            else if (node.HandlesMode == FlightCurveNodeHandlesMode.CoDirected)
                node.MainHandlePositionReleative = -node.SubHandlePositionReleative.normalized * node.MainHandlePositionReleative.magnitude;

            EditorUtility.SetDirty(curveDrawer);
        }
    }
}
