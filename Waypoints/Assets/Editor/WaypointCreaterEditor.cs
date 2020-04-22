using System;
using System.Runtime;
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
    private SerializedProperty m_PropCheckBoxs;
    private SerializedProperty m_PropGroundLevel;
    private SerializedProperty m_PropIsSetGround;
    private SerializedProperty m_PropInsertPoint;

    private Tool m_LastTool = Tool.None;
    private const float m_ScreenSize = 10f;
    private bool m_ShowMessage = false;

    private void OnEnable()
    {
        m_LastTool = Tools.current;
        Tools.current = Tool.None;

        m_WaypointCreater = (WaypointCreater)target;

        m_SerializedObject = serializedObject;
        m_PropPointRadius = m_SerializedObject.FindProperty("WaypointRadius");
        m_PropColor = m_SerializedObject.FindProperty("PointColor");
        m_PropWaypoints = m_SerializedObject.FindProperty("Waypoints");
        m_PropCheckBoxs = m_SerializedObject.FindProperty("WaypointsCheckBox");
        m_PropGroundLevel = m_SerializedObject.FindProperty("MinGroundLevel");
        m_PropIsSetGround = m_SerializedObject.FindProperty("IsSetGroundLevel");
        m_PropInsertPoint = m_SerializedObject.FindProperty("IsInsertPoint");

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
        var style = EditorStyles.wordWrappedLabel;
        style.fontStyle = FontStyle.Bold;
        EditorGUILayout.LabelField("Add waypoint by click the button under or Shit + mouse click at the scene.", style);

        GUILayout.Space(5);
        if (GUILayout.Button("Add Waypoint"))
        {
            m_WaypointCreater.CreateWaypoint();
        }

        GUILayout.Space(10);
        if (m_PropWaypoints.arraySize > 0 && m_PropCheckBoxs.arraySize > 0)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Space(10);
                SetGroundHeight();

                GUILayout.Space(10);
                ReadWaypointList();

                GUILayout.Space(10);
                EditorGUILayout.LabelField("Insert waypoints at the scene. Check the box under to activate the function.", style);
                GUILayout.Space(5);
                m_PropInsertPoint.boolValue = EditorGUILayout.Toggle("Break-Way-Insert", m_PropInsertPoint.boolValue);
                GUILayout.Space(10);
                if (m_PropInsertPoint.boolValue)
                {
                    EditorGUILayout.HelpBox("Function activated.", MessageType.Info);
                }
            }

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Remove a waypoint by click the button under or Ctrl + mouse click on the point at the scene.", style);
            GUILayout.Space(5);
            if (GUILayout.Button("Remove Selected Waypoint"))
            {
                m_ShowMessage = false;

                if (m_PropCheckBoxs.arraySize > 0)
                {
                    if (m_WaypointCreater.IsAnySelected())
                    {
                        m_WaypointCreater.RemoveWaypoint();
                    }
                    else
                    {
                        m_ShowMessage = true;
                    }
                }
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Remove All"))
            {
                m_WaypointCreater.RemoveAll();
                m_PropIsSetGround.boolValue = false;
                m_PropInsertPoint.boolValue = false;
                m_ShowMessage = false;
            }

            if (m_ShowMessage)
            {
                GUILayout.Space(10);
                EditorGUILayout.HelpBox("Select at least one point to remove.", MessageType.Warning);
            }
        }

        if (m_SerializedObject.ApplyModifiedProperties())
        {
            SceneView.RepaintAll();
        }
    }


    private void SetGroundHeight()
    {
        m_PropIsSetGround.boolValue = EditorGUILayout.Toggle("Set ground height", m_PropIsSetGround.boolValue);

        if (m_PropIsSetGround.boolValue)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(m_PropGroundLevel, GUILayout.Width(180));
                GUILayout.Space(5);
                if (GUILayout.Button("Apply"))
                {
                    SetGroundLevel();
                }
            }
        }
    }

    private void SetGroundLevel()
    {
        m_WaypointCreater.SetGroundLevel(m_PropGroundLevel.floatValue);
    }

    private void ReadWaypointList()
    {
        for (int index = 0; index < m_PropWaypoints.arraySize; index++)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(m_PropCheckBoxs.GetArrayElementAtIndex(index), new GUIContent(""), GUILayout.Width(20));
                GUILayout.Label("Point " + index.ToString(), GUILayout.Width(60));
                EditorGUILayout.PropertyField(m_PropWaypoints.GetArrayElementAtIndex(index), new GUIContent(""));
            }
        }
    }


    private void DuringSceneGUI(SceneView sceneView)
    {
        Selection.activeObject = m_WaypointCreater;
        bool holdingShift = (Event.current.modifiers & EventModifiers.Shift) != 0;
        bool holdingCtl = (Event.current.modifiers & EventModifiers.Control) != 0;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (Event.current.type == EventType.MouseDown && holdingShift)
            {
                if (m_PropInsertPoint.boolValue && m_PropWaypoints.arraySize > 2)
                {
                    m_WaypointCreater.InsertPoint(hit.point);
                }
                else
                {
                    m_WaypointCreater.CreateWaypoint(hit.point);
                }
            }

            if (Event.current.type == EventType.MouseDown && holdingCtl)
            {
                m_WaypointCreater.RemoveWaypoint(hit.point);
            }
        }

        m_SerializedObject.Update();

        if (m_PropWaypoints.arraySize > 0)
        {
            for (int i = 0; i < m_PropWaypoints.arraySize; i++)
            {
                SerializedProperty prop = m_PropWaypoints.GetArrayElementAtIndex(i);
                prop.vector3Value = Handles.PositionHandle(prop.vector3Value, Quaternion.identity);

                Handles.color = m_PropColor.colorValue;
                Handles.SphereHandleCap(-1, prop.vector3Value, Quaternion.identity, m_PropPointRadius.floatValue, EventType.Repaint);

                Handles.Label(prop.vector3Value, "Point " + i.ToString());

                if (m_PropWaypoints.arraySize > 1)
                {
                    Handles.DrawDottedLine(prop.vector3Value, m_PropWaypoints.GetArrayElementAtIndex(m_WaypointCreater.GetNextIndex(i)).vector3Value, m_ScreenSize);
                }
            }
        }
        m_SerializedObject.ApplyModifiedProperties();
    }
}

