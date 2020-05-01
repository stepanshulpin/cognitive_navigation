using AI.FuzzyLogic.Terms;
using AI.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class EvolutionAgentsManager : MonoBehaviour
{
    [Header("Agent")]
    public GameObject agentPrefab;

    public float agentDistance = 2.5f;

    [Header("Genetic Params")]
    public int agentsCount = 5;
    public int termParamsCount = 20;
    public double mutationProbability = 0.2;
    public int childrenSize = 4;
    public int selectParentsTournamentSize = 4;
    public double draftPart = 0.2;
    public int maxIterationCount = 100;
    public int targetFitness = 1000;

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

    public bool isRestarting { get; private set; }

    public void TargetPositionUpdated(Vector3 targetPosition)
    {
        lastTarget = targetPosition;
        foreach (EvolutionAgent agent in agents)
        {
            agent.TargetPositionUpdated(targetPosition);
        }
    }

    private GeneticAlgorithmParams setParams()
    {
        GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
        parameters.GenerationSize = agentsCount;
        parameters.ChromosomeSize = termParamsCount;
        parameters.MutationProbability = mutationProbability;
        parameters.ChildrenSize = childrenSize;
        parameters.SelectParentsTournamentSize = selectParentsTournamentSize;
        parameters.DraftPart = draftPart;
        return parameters;
    }

    private void Awake()
    {
        iteration = 0;
        agents = new List<EvolutionAgent>();
        agentsObjects = new List<GameObject>();
        int middle = agentsCount / 2 - 1;        
        geneticAlgorithm = new GeneticAlgorithm(setParams());
        List<FuzzyGene> genes = new List<FuzzyGene>();
        genes.Add(new FuzzyGene(TermType.ZShape, 0, 15, 2));
        genes.Add(new FuzzyGene(TermType.SShape, 0, 15, 2));

        genes.Add(new FuzzyGene(TermType.ZShape, 0, 10, 2));
        genes.Add(new FuzzyGene(TermType.Trapezodial, 0, 10, 4));
        genes.Add(new FuzzyGene(TermType.SShape, 0, 10, 2));

        genes.Add(new FuzzyGene(TermType.ZShape, -45, 45, 2));
        genes.Add(new FuzzyGene(TermType.Trapezodial, -45, 45, 4));
        genes.Add(new FuzzyGene(TermType.SShape, -45, 45, 2));

        geneticAlgorithm.initializeFuzzyChromosomes(genes);
        for (int agentNum = 0; agentNum < agentsCount; agentNum++)
        {
            GameObject agentObject = Instantiate(agentPrefab);
            agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, agentDistance);
            agentObject.SetActive(false);
            EvolutionAgent agent = agentObject.GetComponent<EvolutionAgent>();
            agents.Add(agent);
            agent.updateFuzzyParams(geneticAlgorithm.getFuzzyChromosome(agentNum));
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
        if (!isComplete)
        {
            tryStop();
            this.UpdateBestAgent();
        }
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
                    geneticAlgorithm.updateFitnessForChromosome(agentIndex, agent.position.x);
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

    private bool completeFitness()
    {
        ISelection selection = new EliteSelection();
        IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
        Debug.Log("Best agent fitness " + best.Fitness + " genes = " + printGenes(best));
        if (best.Fitness > targetFitness)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void restart()
    {
        //Debug.Log("Restart");
        isRestarting = true;
        for (int agentNum = 0; agentNum < agentsCount; agentNum++)
        {
            Destroy(agentsObjects[agentNum]);
        }
        iteration++;
        Debug.Log("Iteration #" + iteration);
        if (maxIterationCount == iteration || completeFitness())
        {
            completeLearning();
        }
        else
        {            
            floorManager.restart();
            agents = new List<EvolutionAgent>();
            agentsObjects = new List<GameObject>();
            geneticAlgorithm.newGenerationFuzzy();
            int middle = agentsCount / 2 - 1;
            for (int agentNum = 0; agentNum < agentsCount; agentNum++)
            {
                GameObject agentObject = Instantiate(agentPrefab);
                agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, agentDistance);
                agentObject.SetActive(false);
                EvolutionAgent agent = agentObject.GetComponent<EvolutionAgent>();
                agents.Add(agent);
                agent.updateFuzzyParams(geneticAlgorithm.getFuzzyChromosome(agentNum));
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
            isRestarting = false;
            targetManager.restart();
        }        
    }

    private void completeLearning()
    {
        ISelection selection = new EliteSelection();
        IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
        using (StreamWriter file = File.CreateText(@"D:\Учеба\Диплом\Diplom\Diplom\best_agent.txt"))
        {
            file.WriteLine("Fitness = " + best.Fitness);
            file.WriteLine("Genes = " + printGenes(best));
        }
        isComplete = true;
    }

    private String printGenes(IChromosome chromosome)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        for (int i = 0; i < chromosome.Size; i++)
        {
            stringBuilder.Append(chromosome.Genes[i].ToString("0.##")).Append(";  ");
        }
        stringBuilder.Append("]");
        return stringBuilder.ToString();
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
    private int iteration;
    private int bestAgentIndex = 0;
    private EvolutionTargetManager targetManager;
    private EvolutionFloorManager floorManager;
    private FollowingCamera followingCamera;
    private Vector3 lastTarget;
    private GeneticAlgorithm geneticAlgorithm;
    private bool isComplete = false;
}
