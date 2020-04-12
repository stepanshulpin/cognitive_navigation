using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SensorsManager))]
public class SensorsEditor : Editor {

    private void OnSceneGUI() {
        SensorsManager sensorsManager = (SensorsManager)target;
        Handles.color = Color.white;
        if (sensorsManager.distanceSensorsCount > 1) {
            this.VisualizeSensors(sensorsManager);
        } else if (sensorsManager.distanceSensorsCount == 1) {
            this.VisualizeSingleSensor(sensorsManager);
        }
    }

    private void VisualizeSensors(SensorsManager sensorsManager) {
        float viewAngleStep = sensorsManager.viewAngle / (sensorsManager.distanceSensorsCount - 1);
        float minViewAngle = -sensorsManager.viewAngle / 2;
        for (int i = 0; i < sensorsManager.distanceSensorsCount; i++) {
            Vector3 sensorDirection = sensorsManager.DirectionFromAngle(minViewAngle + i * viewAngleStep, false);
            Handles.DrawLine(sensorsManager.transform.position,
                sensorsManager.transform.position + sensorDirection * sensorsManager.maxDistance);
        }
    }

    private void VisualizeSingleSensor(SensorsManager sensorsManager) {
        Handles.DrawLine(sensorsManager.transform.position,
                sensorsManager.transform.position + sensorsManager.transform.forward * sensorsManager.maxDistance);
    }
}
