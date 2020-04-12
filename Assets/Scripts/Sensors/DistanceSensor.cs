using UnityEngine;
using Sensors;

public class DistanceSensor : MonoBehaviour, ISensor {

    public LayerMask obstaclesMask;

    public float Output {
        get {
            return this.output;
        }
    }

    private void Start() {
        this.transform.localPosition = Vector3.zero + 0.5f * Vector3.forward;
        this.transform.localScale = Vector3.one;
        this.parent = this.transform.parent;
    }

    public void Initialize(Vector3 direction, float minDistance, float maxDistance) {
        Debug.Log("Initial direction " + direction.normalized.ToString());
        this.direction = direction.normalized;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.InitializeRenderer();
    }

    private void FixedUpdate() {
        Vector3 direction = this.parent.rotation * this.direction;

        RaycastHit hit;
        Physics.Raycast(this.parent.position, direction, out hit, this.maxDistance, this.obstaclesMask);
        if (hit.collider == null) {
            hit.distance = this.maxDistance;
        } else if (hit.distance < this.minDistance) {
            hit.distance = this.minDistance;
        }
        this.output = hit.distance;
        this.sensorLineRenderer.SetPosition(1, this.direction.normalized * this.output);
    }

    private void InitializeRenderer() {
        this.sensorLineRenderer = this.GetComponent<LineRenderer>();
        this.sensorLineRenderer.SetPosition(1, this.direction.normalized * this.maxDistance);
    }

    private float output;

    private Vector3 direction;

    private float minDistance;

    private float maxDistance;

    private LineRenderer sensorLineRenderer;

    private Transform parent;

}
