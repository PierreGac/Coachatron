using UnityEngine;
using UnityEditor;

namespace CoachSimulator
{
    [CustomEditor(typeof(SocksGame))]
    public class SocksGameEditor : Editor
    {
        private void OnSceneGUI()
        {
            SocksGame t = target as SocksGame;

            if (t == null)
            {
                return;
            }

            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.transform.TransformPoint(t.spawnStart), t.transform.rotation, 0.1f);
            Vector3 position = Handles.PositionHandle(t.transform.TransformPoint(t.spawnStart), t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.spawnStart = t.transform.InverseTransformPoint(position);
            }

            Handles.color = Color.blue;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.transform.TransformPoint(t.spawnEnd), t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.transform.TransformPoint(t.spawnEnd), t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.spawnEnd = t.transform.InverseTransformPoint(position);
            }

            Handles.color = Color.yellow;
            Handles.DrawLine(t.transform.TransformPoint(t.spawnStart), t.transform.TransformPoint(t.spawnEnd));



            Handles.color = Color.cyan;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.transform.position + Vector3.right * t.xBoundMin, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.transform.position + Vector3.right * t.xBoundMin, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.xBoundMin = (t.transform.position - position).x;
            }

            Handles.color = Color.magenta;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.transform.position + Vector3.right * t.xBoundMax, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.transform.position + Vector3.right * t.xBoundMax, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.xBoundMax = (t.transform.position - position).x;
            }
        }
    }
}