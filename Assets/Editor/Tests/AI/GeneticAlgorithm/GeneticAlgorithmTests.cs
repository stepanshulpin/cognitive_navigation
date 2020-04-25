using AI.GeneticAlgorithm;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tests.AI.GeneticAlgorithmTest
{
    class GeneticAlgorithmTests
    {

        [Test]
        public void TestGeneticAlgorithmSimpleTask()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 100;
            parameters.ChromosomeSize = 4;
            parameters.MinGenValue = 1;
            parameters.MaxGenValue = 30;
            parameters.MutationProbability = 0.2;
            parameters.ChildrenSize = 80;
            parameters.SelectParentsTournamentSize = 20;
            parameters.DraftPart = 0.4;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initialize();

            int iterCount = 100;
            for (int iter = 0; iter < iterCount; iter++)
            {
                if (iter % 100 == 0)
                {
                    Debug.Log("Iter = " + iter);
                }
                calculateFitness(geneticAlgorithm.GetCurrentGeneration());
                if (stop(geneticAlgorithm.GetCurrentGeneration()))
                {
                    break;
                }
                if (iter == iterCount - 1)
                {
                    break;
                }
                geneticAlgorithm.newGeneration();
            }
            ISelection selection = new EliteSelection();
            IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
            double res = function(best);
            Debug.Log("Fitness = " + res);
            Debug.Log("Values = " + best.Genes[0] + " | " + best.Genes[1] + " | " + best.Genes[2]);
            Assert.IsTrue(Math.Abs(30 - res) < 0.1);
        }

        [Test]
        public void TestGeneticAlgorithmFunctionMax()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 100;
            parameters.ChromosomeSize = 3;
            parameters.MinGenValue = -5;
            parameters.MaxGenValue = 15;
            parameters.MutationProbability = 0.2;
            parameters.DraftPart = 0.2;
            parameters.ChildrenSize = 80;
            parameters.SelectParentsTournamentSize = 20;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initialize();

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
                geneticAlgorithm.newGeneration();
            }
            ISelection selection = new EliteSelection();
            IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
            double res = function2(best);
            Debug.Log("Fitness = " + res);
            Debug.Log("Values = " + best.Genes[0] + " | " + best.Genes[1] + " | " + best.Genes[2]);
            Assert.IsTrue(Math.Abs(51 - res) < 0.1);
        }

        [Test]
        public void TestLinearRecombination()
        {
            double[] genes1 = new double[2] { 3, 3 };
            double[] genes2 = new double[2] { 1, 1 };
            double alpha = 0.5;
            IChromosome parent1 = new NumericChromosome(genes1);
            IChromosome parent2 = new NumericChromosome(genes2);
            LinearRecombination recombination = new LinearRecombination(null);
            IChromosome chromosome = recombination.calc(parent1, parent2, alpha);
            Assert.AreEqual(2, chromosome.Genes[0]);
        }

        [Test]
        public void TestTornamentSelectionSaveSize()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 5;
            parameters.ChromosomeSize = 4;
            parameters.MinGenValue = 1;
            parameters.MaxGenValue = 30;
            parameters.SelectParentsTournamentSize = 4;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initialize();
            calculateFitness(geneticAlgorithm.GetCurrentGeneration());
            geneticAlgorithm.tournamentSelection();
            Assert.AreEqual(5, geneticAlgorithm.GetCurrentGeneration().Individuals.Count);
        }

        [Test]
        public void TestToList()
        {
            List<double> list1 = new List<double>();
            list1.Add(5.0);
            list1.Add(6.0);

            List<double> list2 = list1.ToList();
            list1.RemoveAt(0);
            Assert.AreNotEqual(list1.Count, list2.Count);
        }

        private bool stop(Generation generation)
        {
            foreach (IChromosome chromosome in generation.Individuals)
            {
                if (chromosome.Fitness > -0.1)
                {
                    return true;
                }
            }
            return false;
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

        private void calculateFitness(Generation generation)
        {
            foreach (IChromosome chromosome in generation.Individuals)
            {
                double res = function(chromosome);
                chromosome.Fitness = -Math.Abs(30 - res);

            }
        }
        private void calculateFitness2(Generation generation)
        {
            foreach (IChromosome chromosome in generation.Individuals)
            {
                double res = function2(chromosome);
                chromosome.Fitness = res;
            }
        }


        private double function(IChromosome chromosome)
        {
            double[] genes = chromosome.Genes;
            return genes[0] + 2 * genes[1] + 3 * genes[2] + 4 * genes[3];
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
