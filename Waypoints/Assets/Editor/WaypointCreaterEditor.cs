using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(WaypointCreater))]
public class WaypointCreaterEditor : Editor
{
    private WaypointCreater m_WaypointCreater;

    private SerializedObject m_SerializedObject;
    private SerializedProperty m_PropPointRadius;
    private SerializedProperty m_PropColor;
    private SerializedProperty m_PropWaypoints;

    private void OnEnable()
    {
        m_WaypointCreater = FindObjectOfType<WaypointCreater>();

        m_SerializedObject = serializedObject;
        m_PropPointRadius = m_SerializedObject.FindProperty("WaypointRadius");
        m_PropColor = m_SerializedObject.FindProperty("PointColor");
        m_PropWaypoints = m_SerializedObject.FindProperty("Waypoints");

        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        m_SerializedObject.Update();
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(m_PropPointRadius);

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(m_PropColor);
        GUILayout.Space(20);

        if (GUILayout.Button("Add Waypoint"))
        {
            m_WaypointCreater.CreateWaypoint();
        }

        GUILayout.Space(10);
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            if (m_PropWaypoints.arraySize > 0)
            {
                ReadWaypointList();
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Remove Waypoint"))
        {
            // REDO

            m_WaypointCreater.RemoveWaypointFromList(0);
        }

        if (m_SerializedObject.ApplyModifiedProperties())
        {
            SceneView.RepaintAll();
        }
    }

    private void ReadWaypointList()
    {
        if (m_PropWaypoints.arraySize > 0)
        {
            for (int index = 0; index < m_PropWaypoints.arraySize; index++)
            {
                //GUILayout.Toggle(false, null, );
                EditorGUILayout.Vector3Field("Point" + index.ToString(), m_PropWaypoints.GetArrayElementAtIndex(index).vector3Value);
            }
        }
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        m_SerializedObject.Update();
        for (int i = 0; i < m_WaypointCreater.Waypoints.Count; i++)
        {
            m_WaypointCreater.Waypoints[i] = Handles.PositionHandle(m_WaypointCreater.Waypoints[i], Quaternion.identity);
            Handles.color = m_PropColor.colorValue;
            Handles.SphereHandleCap(-1, m_WaypointCreater.Waypoints[i], Quaternion.identity, m_PropPointRadius.floatValue, EventType.Repaint);
        }
        m_SerializedObject.ApplyModifiedProperties();

    }
}
