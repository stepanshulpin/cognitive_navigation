﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCourseFloorManager : MonoBehaviour
{
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

    private void Awake()
    {
        segments = new Queue<GameObject>();
        segmentStaticObstacles = new Dictionary<int, List<GameObject>>();
        segmentDynamicObstacles = new Dictionary<int, List<GameObject>>();
        for (int segment = 0; segment < this.maxFloorSegments; segment++)
        {
            GameObject floorSegment = Instantiate(this.floorPrefab);
            floorSegment.SetActive(false);
            int segmentId = floorSegment.GetInstanceID();
            List<GameObject> segmentObstacles = new List<GameObject>();
            for (int obstacle = 0; obstacle < maxStaticObstaclesPerSegment; obstacle++)
            {
                GameObject staticObstacle = Instantiate(this.staticObstaclePrefab);
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
        agentsManager = GetComponent<DynamicCourseAgentsManager>();
    }

    private void Start()
    {
        this.SpawnStartSegment();
    }

    private void LateUpdate()
    {
        foreach (GameObject agent in this.agentsManager.AgentsObjects)
        {
            if (agent.transform.position.x > this.lastSpawnedSegmentPosition.x)
            {
                this.SpawnNewSegment();
                break;
            }
        }
    }

    private void SpawnStartSegment()
    {
        GameObject newSegment = segments.Dequeue();
        newSegment.transform.position = Vector3.zero;
        this.lastSpawnedSegmentPosition = Vector3.zero;
        newSegment.SetActive(true);
        this.segments.Enqueue(newSegment);
    }
    private void SpawnNewSegment()
    {
        Vector3 position = this.lastSpawnedSegmentPosition + OFFSET_BETWEEN_FLOOR_SEGMENTS;
        GameObject newSegment = segments.Dequeue();
        newSegment.transform.position = position;
        this.lastSpawnedSegmentPosition = position;
        newSegment.SetActive(true);
        this.segments.Enqueue(newSegment);
        this.InitializeObstacles(newSegment);
    }

    private void InitializeObstacles(GameObject segment)
    {
        List<GameObject> obstacles = this.segmentStaticObstacles[segment.GetInstanceID()];
        int activeObstaclesCount = this.random.Next(this.minStaticObstaclesPerSegment, this.maxStaticObstaclesPerSegment);
        foreach (GameObject obstacle in obstacles)
        {
            if (activeObstaclesCount > 0)
            {
                RandomizeObstacle(obstacle);
                obstacle.SetActive(true);
                activeObstaclesCount--;
            }
            else
            {
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

    private void RandomizeDynamicObstacle(GameObject obstacle)
    {
        Vector3 obstaclePosition = new Vector3(
            (float)this.random.NextDouble() * (bottomRight.x - topLeft.x) + topLeft.x,
            0.5f,
            (float)this.random.NextDouble() * (topLeft.y - bottomRight.y) + bottomRight.y
        );
        obstacle.transform.localPosition = obstaclePosition;
        Vector3 obstacleScale = new Vector3(
            (float)this.random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale,
            1.0f,
            (float)this.random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale
        );
        obstacle.transform.localScale = obstacleScale;
        float randomRotationAngle = (float)this.random.NextDouble() * this.maxObstacleRotationAngle;
        obstacle.transform.localRotation = Quaternion.AngleAxis(randomRotationAngle, Vector3.up);
        Vector3 animationToPosition = new Vector3(
            (float)this.random.NextDouble() * (bottomRight.x - topLeft.x) + topLeft.x,
            0.5f,
            (float)this.random.NextDouble() * (topLeft.y - bottomRight.y) + bottomRight.y
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

    private void RandomizeObstacle(GameObject obstacle)
    {
        Vector3 obstaclePosition = new Vector3(
            (float)this.random.NextDouble() * (bottomRight.x - topLeft.x) + topLeft.x,
            0.5f,
            (float)this.random.NextDouble() * (topLeft.y - bottomRight.y) + bottomRight.y
        );
        obstacle.transform.localPosition = obstaclePosition;
        Vector3 obstacleScale = new Vector3(
            (float)this.random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale,
            1.0f,
            (float)this.random.NextDouble() * (maxObstacleScale - minObstacleScale) + minObstacleScale
        );
        obstacle.transform.localScale = obstacleScale;
        float randomRotationAngle = (float)this.random.NextDouble() * this.maxObstacleRotationAngle;
        obstacle.transform.localRotation = Quaternion.AngleAxis(randomRotationAngle, Vector3.up);
    }

    private Queue<GameObject> segments;

    private Vector3 lastSpawnedSegmentPosition = Vector3.zero;

    private Dictionary<int, List<GameObject>> segmentStaticObstacles;

    private Dictionary<int, List<GameObject>> segmentDynamicObstacles;

    private readonly Vector3 OFFSET_BETWEEN_FLOOR_SEGMENTS = new Vector3(35.0f, 0.0f, 0.0f);

    private DynamicCourseAgentsManager agentsManager;

    private System.Random random;
}