using System;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionFloorManager : MonoBehaviour {
    [Header("Segment")]
    public GameObject floorPrefab;

    public int maxFloorSegments = 5;

    [Header("Static obstacles")]
    public GameObject staticObstaclePrefab;

    public int minStaticObstaclesPerSegment = 4;

    public int maxStaticObstaclesPerSegment = 10;

    [Range(0.0f, 180.0f)]
    public float maxObstacleRotationAngle = 180;

    public float minObstacleScale = 0.5f;

    public float maxObstacleScale = 3.0f;

    [Header("Dynamic obstacles")]
    public GameObject dynamicObstaclePrefab;

    public int minDynamicObstaclesPerSegment = 0;

    public int maxDynamicObstaclesPerSegment = 2;

    [Header("Bounds")]
    public Vector2 topLeft = Vector2.zero;

    public Vector2 bottomRight = Vector2.one;

    private void Awake() {
        segments = new Queue<GameObject>();
        segmentStaticObstacles = new Dictionary<int, List<GameObject>>();
        segmentDynamicObstacles = new Dictionary<int, List<GameObject>>();
        for (int segment = 0; segment < maxFloorSegments; segment++) {
            GameObject floorSegment = Instantiate(floorPrefab);
            floorSegment.SetActive(false);
            int segmentId = floorSegment.GetInstanceID();
            List<GameObject> segmentObstacles = new List<GameObject>();
            for (int obstacle = 0; obstacle < maxStaticObstaclesPerSegment; obstacle++) {
                GameObject staticObstacle = Instantiate(staticObstaclePrefab);
                staticObstacle.SetActive(false);
                staticObstacle.transform.SetParent(floorSegment.transform);
                segmentObstacles.Add(staticObstacle);
            }
            List<GameObject> dynamicSegmentObstacles = new List<GameObject>();
            for (int dynamicObstacle = 0; dynamicObstacle < maxDynamicObstaclesPerSegment; dynamicObstacle++)
            {
                GameObject dynamicObstacleObject = Instantiate(dynamicObstaclePrefab);
                dynamicObstacleObject.SetActive(false);
                dynamicObstacleObject.transform.SetParent(floorSegment.transform);
                dynamicSegmentObstacles.Add(dynamicObstacleObject);
            }
            segmentStaticObstacles.Add(segmentId, segmentObstacles);
            segmentDynamicObstacles.Add(segmentId, dynamicSegmentObstacles);
            segments.Enqueue(floorSegment);
        }
        random = new System.Random();
        agentsManager = GetComponent<EvolutionAgentsManager>();
    }

    public void restart()
    {
        foreach(GameObject gameObject in segments)
        {
            Destroy(gameObject);
        }
        segments = new Queue<GameObject>();
        segmentStaticObstacles = new Dictionary<int, List<GameObject>>();
        segmentDynamicObstacles = new Dictionary<int, List<GameObject>>();
        for (int segment = 0; segment < maxFloorSegments; segment++)
        {
            GameObject floorSegment = Instantiate(floorPrefab);
            floorSegment.SetActive(false);
            int segmentId = floorSegment.GetInstanceID();
            List<GameObject> segmentObstacles = new List<GameObject>();
            for (int obstacle = 0; obstacle < maxStaticObstaclesPerSegment; obstacle++)
            {
                GameObject staticObstacle = Instantiate(staticObstaclePrefab);
                staticObstacle.SetActive(false);
                staticObstacle.transform.SetParent(floorSegment.transform);
                segmentObstacles.Add(staticObstacle);
            }
            List<GameObject> dynamicSegmentObstacles = new List<GameObject>();
            for (int dynamicObstacle = 0; dynamicObstacle < maxDynamicObstaclesPerSegment; dynamicObstacle++)
            {
                GameObject dynamicObstacleObject = Instantiate(dynamicObstaclePrefab);
                dynamicObstacleObject.SetActive(false);
                dynamicObstacleObject.transform.SetParent(floorSegment.transform);
                dynamicSegmentObstacles.Add(dynamicObstacleObject);
            }
            segmentStaticObstacles.Add(segmentId, segmentObstacles);
            segmentDynamicObstacles.Add(segmentId, dynamicSegmentObstacles);
            segments.Enqueue(floorSegment);
        }
        SpawnStartSegment();
    }

    private void Start() {
        SpawnStartSegment();
    }

    private void LateUpdate() {
        if (!agentsManager.isRestarting)
        {
            foreach (GameObject agent in agentsManager.AgentsObjects)
            {
                if (agent.transform.position.x > lastSpawnedSegmentPosition.x)
                {
                    SpawnNewSegment();
                    break;
                }
            }
        }
    }

    private void SpawnStartSegment() {
        GameObject newSegment = segments.Dequeue();
        newSegment.transform.position = Vector3.zero;
        lastSpawnedSegmentPosition = Vector3.zero;
        newSegment.SetActive(true);
        segments.Enqueue(newSegment);
    }
    private void SpawnNewSegment() {
        Vector3 position = lastSpawnedSegmentPosition + OFFSET_BETWEEN_FLOOR_SEGMENTS;
        GameObject newSegment = segments.Dequeue();
        newSegment.transform.position = position;
        lastSpawnedSegmentPosition = position;
        newSegment.SetActive(true);
        segments.Enqueue(newSegment);
        InitializeObstacles(newSegment);
    }    

    private void InitializeObstacles(GameObject segment) {
        List<GameObject> obstacles = segmentStaticObstacles[segment.GetInstanceID()];
        int activeObstaclesCount = random.Next(minStaticObstaclesPerSegment, maxStaticObstaclesPerSegment);
        foreach (GameObject obstacle in obstacles) {
            if (activeObstaclesCount > 0) {
                this.RandomizeObstacle(obstacle);
                obstacle.SetActive(true);
                activeObstaclesCount--;
            } else {
                obstacle.SetActive(false);
            }
        }
        List<GameObject> dynamicObstacles = segmentDynamicObstacles[segment.GetInstanceID()];
        int activeDynamicObstaclesCount = random.Next(minDynamicObstaclesPerSegment, maxDynamicObstaclesPerSegment);
        foreach (GameObject obstacle in dynamicObstacles)
        {
            if (activeDynamicObstaclesCount > 0)
            {
                RandomizeDynamicObstacle(obstacle);
                obstacle.SetActive(true);
                activeDynamicObstaclesCount--;
            }
            else
            {
                obstacle.SetActive(false);
            }
        }
    }

    private void RandomizeObstacle(GameObject obstacle) {
        Vector3 obstaclePosition = new Vector3(
            (float)random.NextDouble() * (bottomRight.x - topLeft.x) + topLeft.x,
            0.5f,
            (float)random.NextDouble() * (topLeft.y - bottomRight.y) + bottomRight.y
        );
        obstacle.transform.localPosition = obstaclePosition;
        Vector3 obstacleScale = new Vector3(
            (float)random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale,
            1.0f,
            (float)random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale
        );
        obstacle.transform.localScale = obstacleScale;
        float randomRotationAngle = (float)random.NextDouble() * maxObstacleRotationAngle;
        obstacle.transform.localRotation = Quaternion.AngleAxis(randomRotationAngle, Vector3.up);
    }

    private void RandomizeDynamicObstacle(GameObject obstacle)
    {
        Vector3 obstaclePosition = new Vector3(
            (float)random.NextDouble() * (bottomRight.x - topLeft.x) + topLeft.x,
            0.5f,
            (float)random.NextDouble() * (topLeft.y - bottomRight.y) + bottomRight.y
        );
        obstacle.transform.localPosition = obstaclePosition;
        Vector3 obstacleScale = new Vector3(
            (float)random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale,
            1.0f,
            (float)random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale
        );
        obstacle.transform.localScale = obstacleScale;
        float randomRotationAngle = (float)random.NextDouble() * maxObstacleRotationAngle;
        obstacle.transform.localRotation = Quaternion.AngleAxis(randomRotationAngle, Vector3.up);
        Vector3 animationToPosition = new Vector3(
            (float)random.NextDouble() * (bottomRight.x - topLeft.x) + topLeft.x,
            0.5f,
            (float)random.NextDouble() * (topLeft.y - bottomRight.y) + bottomRight.y
        );
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", AnimationCurve.Linear(0, obstaclePosition.x, 5, animationToPosition.x));
        clip.SetCurve("", typeof(Transform), "localPosition.y", AnimationCurve.Linear(0, 0.5f, 5, 0.5f));
        clip.SetCurve("", typeof(Transform), "localPosition.z", AnimationCurve.Linear(0, obstaclePosition.z, 5, animationToPosition.z));
        clip.wrapMode = WrapMode.PingPong;

        Animation anim = obstacle.GetComponent<Animation>();
        anim.AddClip(clip, "bounds");
        anim.Play("bounds");
    }

    private Queue<GameObject> segments;

    private Vector3 lastSpawnedSegmentPosition = Vector3.zero;

    private Dictionary<int, List<GameObject>> segmentStaticObstacles;

    private Dictionary<int, List<GameObject>> segmentDynamicObstacles;

    private readonly Vector3 OFFSET_BETWEEN_FLOOR_SEGMENTS = new Vector3(35.0f, 0.0f, 0.0f);

    private EvolutionAgentsManager agentsManager;

    private System.Random random;
}
