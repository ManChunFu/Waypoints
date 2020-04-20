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
    private SerializedProperty m_PropGroundLevel;

    private Tool m_LastTool = Tool.None;
    private const float m_ScreenSize = 10f;

    private void OnEnable()
    {
        m_LastTool = Tools.current;
        Tools.current = Tool.None;

        m_WaypointCreater = FindObjectOfType<WaypointCreater>();

        m_SerializedObject = serializedObject;
        m_PropPointRadius = m_SerializedObject.FindProperty("WaypointRadius");
        m_PropColor = m_SerializedObject.FindProperty("PointColor");
        m_PropWaypoints = m_SerializedObject.FindProperty("Waypoints");
        m_PropGroundLevel = m_SerializedObject.FindProperty("MinGroundLevel");

        Selection.selectionChanged += Repaint;
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        Tools.current = m_LastTool;

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
            GUILayout.Space(10);
            GUILayout.Toggle(false, new GUIContent("Set Ground Level height"));//, GUILayout.Width(100));
            GUILayout.Space(10);

            if (m_PropWaypoints.arraySize > 0)
            {
               ReadWaypointList();
            }

            GUILayout.Space(10);
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
        for (int index = 0; index < m_PropWaypoints.arraySize; index++)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Toggle(false, (string)null, GUILayout.Width(30));
                GUILayout.Label("Point " + index.ToString(), GUILayout.Width(60));
                EditorGUILayout.PropertyField(m_PropWaypoints.GetArrayElementAtIndex(index), new GUIContent(""));
            }
        }
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        m_SerializedObject.Update();
        for (int i = 0; i < m_PropWaypoints.arraySize; i++)
        {
            SerializedProperty prop = m_PropWaypoints.GetArrayElementAtIndex(i);
            prop.vector3Value = Handles.PositionHandle(prop.vector3Value, Quaternion.identity);
            Handles.color = m_PropColor.colorValue;
            Handles.SphereHandleCap(-1, prop.vector3Value, Quaternion.identity, m_PropPointRadius.floatValue, EventType.Repaint);
            Handles.Label(prop.vector3Value, "Point " + i.ToString());
            Handles.DrawDottedLine(prop.vector3Value, m_PropWaypoints.GetArrayElementAtIndex(m_WaypointCreater.GetNextIndex(i)).vector3Value, m_ScreenSize);
        }
        m_SerializedObject.ApplyModifiedProperties();
    }
}
