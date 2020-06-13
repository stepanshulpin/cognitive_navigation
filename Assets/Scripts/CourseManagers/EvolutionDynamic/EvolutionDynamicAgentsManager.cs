using AI.FuzzyLogic.Terms;
using AI.GeneticAlgorithm;
using Assets.Scripts.Agents.Dynamic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class EvolutionDynamicAgentsManager : MonoBehaviour
{
    [Header("Agent")]
    public GameObject agentPrefab;

    public float agentDistance = 2.5f;

    [Header("Genetic Params")]
    public int agentsCount = 5;
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

    public List<EvolutionDynamicAgent> Agents
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
        foreach (EvolutionDynamicAgent agent in agents)
        {
            agent.TargetPositionUpdated(targetPosition);
        }
    }

    private GeneticAlgorithmParams setParams()
    {
        GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
        parameters.GenerationSize = agentsCount;
        parameters.MutationProbability = mutationProbability;
        parameters.ChildrenSize = childrenSize;
        parameters.SelectParentsTournamentSize = selectParentsTournamentSize;
        parameters.DraftPart = draftPart;
        return parameters;
    }
    public class FuzzyParams
    {
        public string close = "close";
        public string medium = "medium";
        public string far = "far";
        public string fastNeg = "fastNeg";
        public string mediumNeg = "mediumNeg";
        public string slowNeg = "slowNeg";
        public string slowPos = "slowPos";
        public string mediumPos = "mediumPos";
        public string fastPos = "fastPos";

        public double sensorsMin = 0;
        public double sensorsMax = 15;
        public double speedMin = -10;
        public double speedMax = 10;
    }

    private List<FuzzyGene.GeneParams> createGeneParams() 
    {
        List<FuzzyGene.GeneParams> geneParams = new List<FuzzyGene.GeneParams>();
        FuzzyParams fuzzyParams = new FuzzyParams();
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.close, fuzzyParams.sensorsMin, fuzzyParams.sensorsMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.medium, fuzzyParams.sensorsMin, fuzzyParams.sensorsMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.far, fuzzyParams.sensorsMin, fuzzyParams.sensorsMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.fastNeg, fuzzyParams.speedMin, fuzzyParams.speedMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.mediumNeg, fuzzyParams.speedMin, fuzzyParams.speedMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.slowNeg, fuzzyParams.speedMin, fuzzyParams.speedMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.slowPos, fuzzyParams.speedMin, fuzzyParams.speedMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.mediumPos, fuzzyParams.speedMin, fuzzyParams.speedMax));
        geneParams.Add(new FuzzyGene.GeneParams(fuzzyParams.fastPos, fuzzyParams.speedMin, fuzzyParams.speedMax));
        return geneParams;
    }

    private void Awake()
    {
        logger = new Utils.Logger();
        logger.setFileName(@"D:\Учеба\Диплом\Diplom\Diplom\evolution");
        iteration = 0;
        agents = new List<EvolutionDynamicAgent>();
        agentsObjects = new List<GameObject>();
        int middle = agentsCount / 2 - 1;        
        geneticAlgorithm = new GeneticAlgorithm(setParams());
        geneticAlgorithm.initializeFuzzyChromosomes(createGeneParams());
        floorManager = GetComponent<EvolutionDynamicFloorManager>();
        for (int agentNum = 0; agentNum < agentsCount; agentNum++)
        {
            GameObject agentObject = Instantiate(agentPrefab);
            agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, agentDistance);
            agentObject.SetActive(false);
            EvolutionDynamicAgent agent = agentObject.GetComponent<EvolutionDynamicAgent>();
            agents.Add(agent);
            agent.UpdateBounds(floorManager.topLeft.y, floorManager.bottomRight.y);
            agent.updateFuzzyParams(geneticAlgorithm.getFuzzyChromosome(agentNum));
            agentsObjects.Add(agentObject);
        }
        targetManager = GetComponent<EvolutionDynamicTargetManager>();
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
        logger.printToConsole("Best agent fitness " + best.Fitness.ToString("0.##") + " genes = " + printGenes((FuzzyChromosome)best));
        double averageFitness = geneticAlgorithm.GetCurrentGeneration().CalculateFitness() / geneticAlgorithm.GetCurrentGeneration().Individuals.Count;
        logger.printToConsole("Average fitness " + averageFitness.ToString("0.##"));
        logger.printToFile(best.Fitness.ToString("0.##") + "\t" + averageFitness.ToString("0.##"));
        if (averageFitness > targetFitness)
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
        logger.printToConsole("Iteration #" + iteration);
        if (maxIterationCount == iteration || completeFitness())
        {
            completeLearning();
        }
        else
        {            
            floorManager.restart();
            agents = new List<EvolutionDynamicAgent>();
            agentsObjects = new List<GameObject>();
            double mutationAttenuation = ((double)(maxIterationCount - iteration)) / maxIterationCount;
            geneticAlgorithm.newGenerationFuzzy(mutationAttenuation / 3);
            int middle = agentsCount / 2 - 1;
            for (int agentNum = 0; agentNum < agentsCount; agentNum++)
            {
                GameObject agentObject = Instantiate(agentPrefab);
                agentObject.transform.position += (agentNum - middle) * new Vector3(0, 0, agentDistance);
                agentObject.SetActive(false);
                EvolutionDynamicAgent agent = agentObject.GetComponent<EvolutionDynamicAgent>();
                agents.Add(agent);
                agent.UpdateBounds(floorManager.topLeft.y, floorManager.bottomRight.y);
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
            file.WriteLine("Fitness = " + best.Fitness.ToString("0.##"));
            file.WriteLine("Genes = " + printGenes((FuzzyChromosome)best));
        }
        isComplete = true;
    }

    private String printGenes(FuzzyChromosome chromosome)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (FuzzyGene gene in chromosome.FuzzyGenes)
        {
            stringBuilder.Append(gene.TermType).Append(": ");
            stringBuilder.Append("[");
            for (int i = 0; i < gene.Size; i++)
            {
                stringBuilder.Append(gene.Values[i].ToString("0.##")).Append(";  ");
            }
            stringBuilder.Append("] ");
        }

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
    private List<EvolutionDynamicAgent> agents;
    private int iteration;
    private int bestAgentIndex = 0;
    private EvolutionDynamicTargetManager targetManager;
    private EvolutionDynamicFloorManager floorManager;
    private FollowingCamera followingCamera;
    private Vector3 lastTarget;
    private GeneticAlgorithm geneticAlgorithm;
    private bool isComplete = false;
    private Utils.Logger logger;
}
