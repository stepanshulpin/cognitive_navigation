using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.GeneticAlgorithm;
using NUnit.Framework;
using UnityEngine;

namespace Tests.AI.GeneticAlgorithmTest
{
    class GeneticAlgorithmFuzzyChromosomeTests
    {

        [Test]
        public void TestGeneticAlgorithmFunctionMax()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 10;
            parameters.ChromosomeSize = 3;
            parameters.MutationProbability = 0.2;
            parameters.DraftPart = 0.2;
            parameters.ChildrenSize = 8;
            parameters.SelectParentsTournamentSize = 5;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            List<FuzzyGene> genes = new List<FuzzyGene>();
            genes.Add(new FuzzyGene(3, 5, 1));
            genes.Add(new FuzzyGene(0, 3, 2));
            geneticAlgorithm.initializeFuzzyChromosomes(genes);

            int iterCount = 100;
            for (int iter = 0; iter < iterCount; iter++)
            {
                if (iter % 10 == 0)
                {
                    Debug.Log("Iter = " + iter);
                }
                calculateFitness2(geneticAlgorithm.GetCurrentGeneration());
                if (stop2(geneticAlgorithm.GetCurrentGeneration()))
                {
                    break;
                }
                if (iter == iterCount - 1)
                {
                    break;
                }
                geneticAlgorithm.newGenerationFuzzy();
            }
            ISelection selection = new EliteSelection();
            IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
            double res = function2(best);
            Debug.Log("Fitness = " + res);
            Debug.Log("Values = " + best.Genes[0] + " | " + best.Genes[1] + " | " + best.Genes[2]);
            Assert.IsTrue(Math.Abs(51 - res) < 0.1);
        }

        private bool stop2(Generation generation)
        {
            foreach (IChromosome chromosome in generation.Individuals)
            {
                if (chromosome.Fitness > 50.9)
                {
                    return true;
                }
            }
            return false;
        }

        private void calculateFitness2(Generation generation)
        {
            foreach (IChromosome chromosome in generation.Individuals)
            {
                double res = function2(chromosome);
                chromosome.Fitness = res;
            }
        }

        private double function2(IChromosome chromosome)
        {
            double x = chromosome.Genes[0];
            double y = chromosome.Genes[1];
            double z = chromosome.Genes[2];
            return -x * x - 5 * y * y - 3 * z * z + x * y - 2 * x * z + 2 * y * z + 11 * x + 2 * y + 18 * z + 10;
        }

    }
}
