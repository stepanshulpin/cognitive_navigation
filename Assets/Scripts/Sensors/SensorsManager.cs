using UnityEngine;

public class SensorsManager : MonoBehaviour {
    public int distanceSensorsCount = 4;

    public float minDistance = 0.01f;

    public float maxDistance = 5.0f;

    [Range(0, 360)]
    public float viewAngle;

    public GameObject distanceSensorPrefab;

    public float[] SensorsOutput {
        get {
            return this.distanceSensorsOutput;
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal) {
        if (!isGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Start() {
        this.distanceSensors = new DistanceSensor[this.distanceSensorsCount];
        this.distanceSensorsOutput = new float[this.distanceSensorsCount];
        if (this.distanceSensorsCount > 1) {
            this.InitializeDistanceSensors();
        } else if (this.distanceSensorsCount > 0) {
            this.InitializeSingleDistanceSensor();
        }
    }

    private void FixedUpdate() {
        for (int i = 0; i < this.distanceSensorsCount; i++) {
            this.distanceSensorsOutput[i] = this.distanceSensors[i].Output;
        }
    }

    private void InitializeSingleDistanceSensor() {
        GameObject distanceSensor = Instantiate(this.distanceSensorPrefab);
        distanceSensor.transform.parent = this.transform;
        distanceSensor.GetComponent<DistanceSensor>().Initialize(this.transform.forward, minDistance, maxDistance);
        this.distanceSensors[0] = distanceSensor.GetComponent<DistanceSensor>();
    }

    private void InitializeDistanceSensors() {
        float viewAngleStep = this.viewAngle / (this.distanceSensorsCount - 1);
        float minViewAngle = -this.viewAngle / 2;
        for (int i = 0; i < this.distanceSensorsCount; i++) {
            Vector3 sensorDirection = this.DirectionFromAngle(minViewAngle + i * viewAngleStep, false);
            GameObject distanceSensor = Instantiate(this.distanceSensorPrefab);
            distanceSensor.transform.parent = this.transform;
            distanceSensor.GetComponent<DistanceSensor>().Initialize(sensorDirection, minDistance, maxDistance);
            this.distanceSensors[i] = distanceSensor.GetComponent<DistanceSensor>();
        }
    }

    private DistanceSensor[] distanceSensors;

    private float[] distanceSensorsOutput;
}
