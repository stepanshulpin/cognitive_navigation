using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

    private void OnSceneGUI() {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360.0f, fow.viewRadius);

        Vector3 viewAngleLeft = fow.DirectionFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleRight = fow.DirectionFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleLeft * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleRight * fow.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleDynamicObstacle in fow.visibleDynamicObstacles) {
            Handles.DrawLine(fow.transform.position, visibleDynamicObstacle.position);
        }
    }
}
