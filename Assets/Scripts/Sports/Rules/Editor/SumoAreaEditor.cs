using UnityEngine;
using UnityEditor;

namespace CoachSimulator
{
    [CustomEditor(typeof(Sumo))]
    public class SumoEditor : Editor
    {
        private void OnSceneGUI()
        {
            Sumo t = target as Sumo;

            if(t == null)
            {
                return;
            }
            #region Player01
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.player1P1, t.transform.rotation, 0.1f);
            Vector3 position = Handles.PositionHandle(t.player1P1, t.transform.rotation);

            if(EditorGUI.EndChangeCheck())
            {
                t.player1P1 = position;
            }

            EditorGUI.BeginChangeCheck();
            Handles.SphereCap(0, t.player1P2, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.player1P2, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.player1P2 = position;
            }
            Handles.DrawLine(t.player1P1, t.player1P2);

            #endregion

            #region Player02
            Handles.color = Color.blue;

            EditorGUI.BeginChangeCheck();
            Handles.CubeCap(0, t.player2P1, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.player2P1, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.player2P1 = position;
            }

            EditorGUI.BeginChangeCheck();
            Handles.SphereCap(0, t.player2P2, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.player2P2, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.player2P2 = position;
            }
            Handles.DrawLine(t.player2P1, t.player2P2);

            #endregion

            Handles.color = Color.cyan;

            EditorGUI.BeginChangeCheck();
            Handles.SphereCap(0, t.fightArea, t.transform.rotation, 0.1f);
            position = Handles.PositionHandle(t.fightArea, t.transform.rotation);

            if (EditorGUI.EndChangeCheck())
            {
                t.fightArea = position;
            }
        }
    }
}