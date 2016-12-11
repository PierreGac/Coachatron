using UnityEngine;
using UnityEditor;

namespace CoachSimulator
{
    [CustomEditor(typeof(Petanque))]
    public class PetanqueAreaEditor : Editor
    {
        private void OnSceneGUI()
        {
            Petanque t = target as Petanque;

            if(t == null)
            {
                return;
            }

            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.p1, t.transform.rotation, 0.1f);
            Vector3 position = Handles.PositionHandle(t.p1, t.transform.rotation);

            if(EditorGUI.EndChangeCheck())
            {
                t.p1 = position;
            }

            Handles.color = Color.blue;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.p2, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.p2, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.p2 = position;
            }

            Handles.color = Color.yellow;
            Handles.DrawLine(t.p1, t.p2);
        }
    }
}