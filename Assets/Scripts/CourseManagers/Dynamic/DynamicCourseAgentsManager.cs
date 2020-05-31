using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCourseAgentsManager : MonoBehaviour {
    [Header("Agent Infinity")]
    public GameObject agentPrefab1;

    [Header("Agent Dynamic")]
    public GameObject agentPrefab2;

    public int agentsCount = 2;

    public List<GameObject> AgentsObjects {
        get {
            return this.agentsObjects;
        }
    }

    public List<InfinityCourseAgent> Agents {
        get {
            return this.agents;
        }
    }

    public void TargetPositionUpdated(Vector3 targetPosition) {
        foreach (InfinityCourseAgent agent in this.agents) {
            agent.TargetPositionUpdated(targetPosition);
        }
    }

    private void Awake() {
        this.agents = new List<InfinityCourseAgent>();
        this.agentsObjects = new List<GameObject>();
        int middle = agentsCount / 2 - 1;
        for (int agentNum = 0; agentNum < this.agentsCount; agentNum++) {
            GameObject agentObject;
            if (agentNum == 0)
            {
                agentObject = Instantiate(this.agentPrefab1);
            }
            else
            {
                agentObject = Instantiate(this.agentPrefab2);
            }
            //agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, 2.5f);
            agentObject.SetActive(false);
            if (agentNum == 0)
            {
                agents.Add(agentObject.GetComponent<InfinityCourseAgent>());
            }
            else
            {
                agents.Add(agentObject.GetComponent<DynamicAgent>());
            }
            this.agentsObjects.Add(agentObject);
        }

        this.followingCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowingCamera>();
    }

    private void Start() {
        this.bestAgentIndex = 0;
        this.agentsObjects[bestAgentIndex].SetActive(true);
        this.followingCamera.target = this.agentsObjects[bestAgentIndex].transform;
        //this.agents[bestAgentIndex].UseBestSkin();
        for (int i = 0; i < this.agentsCount; i++) {
            this.agentsObjects[i].SetActive(true);
        }
    }

    private void Update() {

    }

    private void LateUpdate() {
        this.UpdateBestAgent();
    }

    private void UpdateBestAgent() {
        int bestAgentIndex = this.FindBestAgentIndex();
        if (bestAgentIndex != this.bestAgentIndex) {
            this.agents[this.bestAgentIndex].UseRegularSkin();
            //this.agents[bestAgentIndex].UseBestSkin();
            this.bestAgentIndex = bestAgentIndex;
            this.followingCamera.target = this.AgentsObjects[this.bestAgentIndex].transform;
        }
    }

    private int FindBestAgentIndex() {
        int bestAgentIndex = 0;
        Transform bestAgent = this.agentsObjects[bestAgentIndex].transform;
        float bestAgentDistance = bestAgent.position.x;
        for (int agentIndex = 0; agentIndex < this.agentsObjects.Count; agentIndex++) {
            if (this.agentsObjects[agentIndex].activeSelf) {
                Transform agent = this.agentsObjects[agentIndex].transform;
                if (agent.position.x > bestAgentDistance) {
                    bestAgentIndex = agentIndex;
                    bestAgentDistance = agent.position.x;
                }
            }
        }
        return bestAgentIndex;
    }

    private List<GameObject> agentsObjects;

    private List<InfinityCourseAgent> agents;

    private int bestAgentIndex = 0;

    private FollowingCamera followingCamera;
}
