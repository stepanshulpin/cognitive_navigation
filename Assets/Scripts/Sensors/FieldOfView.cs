using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask dynamicObstacleMask;

    public LayerMask staticObstacleMask;

    public float meshResolution;

    public int edgeResolveIterations;

    public float edgeDistanceThreshold;

    public float maskCutawayDistance = 0.1f;

    public MeshFilter viewMeshFilter;

    [HideInInspector]
    public List<Transform> visibleDynamicObstacles = new List<Transform>();

    public struct ViewCastInfo {
        public bool isHitted;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool isHitted, Vector3 point, float distance, float angle) {
            this.isHitted = isHitted;
            this.point = point;
            this.distance = distance;
            this.angle = angle;
        }
    }

    public struct EdgeInfo {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB) {
            this.pointA = pointA;
            this.pointB = pointB;
        }
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal) {
        if (!isGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Start() {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("TrackDynamicObstaclesWithDelay", 0.2f);
    }
    private void LateUpdate() {
        DrawFieldOfView();
    }

    private IEnumerator TrackDynamicObstaclesWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindDynamicObstacles();
        }
    }

    private void FindDynamicObstacles() {
        visibleDynamicObstacles.Clear();
        Collider[] dynamicObstaclesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, dynamicObstacleMask);
        foreach (Collider dynamicObstacle in dynamicObstaclesInViewRadius) {
            Transform dynamicObstacleTransform = dynamicObstacle.transform;
            Vector3 directionToObstacle = (dynamicObstacleTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToObstacle) < viewAngle / 2) {
                float distanceToObstacle = Vector3.Distance(transform.position, dynamicObstacleTransform.position);
                if (!Physics.Raycast(transform.position, directionToObstacle, distanceToObstacle, staticObstacleMask)) {
                    visibleDynamicObstacles.Add(dynamicObstacleTransform);
                }
            }
        }
    }

    private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
            if (newViewCast.isHitted == minViewCast.isHitted && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    private ViewCastInfo ViewCast(float globalAngle) {
        Vector3 direction = this.DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, viewRadius, staticObstacleMask)) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, this.transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    private void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.isHitted != newViewCast.isHitted || (oldViewCast.isHitted && newViewCast.isHitted && edgeDstThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * maskCutawayDistance;
            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private Mesh viewMesh;
}
