using UnityEngine;
using UnityEditor;

namespace CoachSimulator
{
    [CustomEditor(typeof(Curling))]
    public class CurlingAreaEditor : Editor
    {
        private void OnSceneGUI()
        {
            Curling t = target as Curling;

            if(t == null)
            {
                return;
            }

            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.dSideStart, t.transform.rotation, 0.1f);
            Vector3 position = Handles.PositionHandle(t.dSideStart, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.dSideStart = position;
            }

            EditorGUI.BeginChangeCheck();
            Handles.SphereCap(0, t.dSideEnd, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.dSideEnd, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.dSideEnd = position;
            }

            Handles.DrawLine(t.dSideStart, t.dSideEnd);

            Handles.color = Color.red;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.uSideStart, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.uSideStart, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.uSideStart = position;
            }

            EditorGUI.BeginChangeCheck();
            Handles.SphereCap(0, t.uSideEnd, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.uSideEnd, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.uSideEnd = position;
            }

            Handles.DrawLine(t.uSideStart, t.uSideEnd);
        }
    }
}