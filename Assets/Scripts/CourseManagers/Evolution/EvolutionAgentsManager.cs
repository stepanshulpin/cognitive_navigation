using System;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionAgentsManager : MonoBehaviour
{
    [Header("Agent")]
    public GameObject agentPrefab;

    public float agentDistance = 2.5f;
    public int agentsCount = 5;

    public List<GameObject> AgentsObjects
    {
        get
        {
            return agentsObjects;
        }
    }

    public List<EvolutionAgent> Agents
    {
        get
        {
            return agents;
        }
    }

    public void TargetPositionUpdated(Vector3 targetPosition)
    {
        lastTarget = targetPosition;
        foreach (EvolutionAgent agent in agents)
        {
            agent.TargetPositionUpdated(targetPosition);
        }
    }

    private void Awake()
    {
        agents = new List<EvolutionAgent>();
        agentsObjects = new List<GameObject>();
        int middle = agentsCount / 2 - 1;
        for (int agentNum = 0; agentNum < agentsCount; agentNum++)
        {
            GameObject agentObject = Instantiate(agentPrefab);
            agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, agentDistance);
            agentObject.SetActive(false);
            agents.Add(agentObject.GetComponent<EvolutionAgent>());
            agentsObjects.Add(agentObject);
        }
        targetManager = GetComponent<EvolutionTargetManager>();
        floorManager = GetComponent<EvolutionFloorManager>();
        followingCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowingCamera>();
    }

    private void Start()
    {
        bestAgentIndex = 0;
        agentsObjects[bestAgentIndex].SetActive(true);
        followingCamera.target = agentsObjects[bestAgentIndex].transform;
        agents[bestAgentIndex].UseBestSkin();
        for (int i = 0; i < agentsCount; i++)
        {
            agentsObjects[i].SetActive(true);
        }
    }

    private void Update()
    {

    }

    private void LateUpdate()
    {
        tryStop();
        this.UpdateBestAgent();
    }

    private void tryStop()
    {
        int activeCount = 0;
        for (int agentIndex = 0; agentIndex < agentsObjects.Count; agentIndex++)
        {
            if (agentsObjects[agentIndex].activeSelf)
            {
                if (Agents[agentIndex].IsCrashed())
                {
                    Transform agent = agentsObjects[agentIndex].transform;
                    agentsObjects[agentIndex].SetActive(false);
                    Debug.Log("Agent crashed " + agentIndex + " position is " + agent.position.x);

                } else
                {
                    activeCount++;
                }
            }
        }
        if (activeCount == 0)
        {
            restart();
        }
    }

    private void restart()
    {
        for (int agentNum = 0; agentNum < agentsCount; agentNum++)
        {
            Destroy(agentsObjects[agentNum]);
        }
        Debug.Log("Restart");
        floorManager.restart();
        agents = new List<EvolutionAgent>();
        agentsObjects = new List<GameObject>();
        int middle = agentsCount / 2 - 1;
        for (int agentNum = 0; agentNum < agentsCount; agentNum++)
        {
            GameObject agentObject = Instantiate(agentPrefab);
            agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, agentDistance);
            //agentObject.transform.position += new Vector3(followingCamera.target.position.x, 0, 0);
            agentObject.SetActive(false);
            agents.Add(agentObject.GetComponent<EvolutionAgent>());
            agentsObjects.Add(agentObject);
        }
        bestAgentIndex = 0;
        agentsObjects[bestAgentIndex].SetActive(true);
        followingCamera.target = agentsObjects[bestAgentIndex].transform;
        agents[bestAgentIndex].UseBestSkin();
        for (int i = 0; i < agentsCount; i++)
        {
            agentsObjects[i].SetActive(true);
        }
        targetManager.restart();
    }

    private void UpdateBestAgent()
    {
        int bestAgentIndex = FindBestAgentIndex();
        if (bestAgentIndex != this.bestAgentIndex)
        {
            agents[this.bestAgentIndex].UseRegularSkin();
            agents[bestAgentIndex].UseBestSkin();
            this.bestAgentIndex = bestAgentIndex;
            followingCamera.target = AgentsObjects[this.bestAgentIndex].transform;
        }
    }

    private int FindBestAgentIndex()
    {
        int bestAgentIndex = 0;
        Transform bestAgent = agentsObjects[bestAgentIndex].transform;
        float bestAgentDistance = bestAgent.position.x;
        for (int agentIndex = 0; agentIndex < agentsObjects.Count; agentIndex++)
        {
            if (agentsObjects[agentIndex].activeSelf)
            {
                Transform agent = agentsObjects[agentIndex].transform;
                if (agent.position.x > bestAgentDistance)
                {
                    bestAgentIndex = agentIndex;
                    bestAgentDistance = agent.position.x;
                }
            }
        }
        return bestAgentIndex;
    }

    private List<GameObject> agentsObjects;

    private List<EvolutionAgent> agents;

    private int bestAgentIndex = 0;
    private EvolutionTargetManager targetManager;
    private EvolutionFloorManager floorManager;
    private FollowingCamera followingCamera;
    private Vector3 lastTarget;
}
