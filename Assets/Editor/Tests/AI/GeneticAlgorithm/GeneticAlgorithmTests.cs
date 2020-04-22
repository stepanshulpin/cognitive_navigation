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
            parameters.GenerationSize = 50;
            parameters.ChromosomeSize = 4;
            parameters.MinGenValue = 1;
            parameters.MaxGenValue = 30;
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
                List<IChromosome> children = new List<IChromosome>();
                for (int i = 0; i < 100; i++)
                {
                    List<IChromosome> parents = geneticAlgorithm.tournamentSelection(20);
                    children.Add(geneticAlgorithm.linearRecombination(parents));
                }
                List<IChromosome> previous = geneticAlgorithm.GetCurrentGeneration().Individuals;
                foreach(IChromosome chromosome in previous)
                {
                    geneticAlgorithm.termBoundsMutation(chromosome, 0.0);
                }
                foreach (IChromosome chromosome in children)
                {
                    geneticAlgorithm.termBoundsMutation(chromosome, 0.5);
                }
                Generation newGeneration = geneticAlgorithm.eliteDraft(geneticAlgorithm.GetCurrentGeneration(), children, 0.2);
                geneticAlgorithm.registerNewGeneration(newGeneration);
            }
            ISelection selection = new EliteSelection();
            IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
            double res = function(best);
            Debug.Log("Fitness = " + res);
            Debug.Log("Values = " + best.Genes[0] + " | " + best.Genes[1] + " | " + best.Genes[2]);
            Assert.IsTrue(Math.Abs(30 - res) < 0.1);
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
        public void TestEqualsAfterMutation()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 5;
            parameters.ChromosomeSize = 4;
            parameters.MinGenValue = 1;
            parameters.MaxGenValue = 30;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initialize();
            IChromosome chromosome = geneticAlgorithm.GetCurrentGeneration().Individuals[0];
            geneticAlgorithm.termBoundsMutation(chromosome, 1);
            Assert.AreEqual(chromosome.Genes[0], geneticAlgorithm.GetCurrentGeneration().Individuals[0].Genes[0]);
        }

        [Test]
        public void TestTornamentSelectionSaveSize()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 5;
            parameters.ChromosomeSize = 4;
            parameters.MinGenValue = 1;
            parameters.MaxGenValue = 30;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initialize();
            calculateFitness(geneticAlgorithm.GetCurrentGeneration());
            geneticAlgorithm.tournamentSelection(4);
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
                if(chromosome.Fitness > -0.1)
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
            double y = chromosome.Genes[0];
            double z = chromosome.Genes[0];
            return -x * x - 5 * y * y - 3 * z * z + x * y - 2 * x * z + 2 * y * z + 11 * x + 2 * y + 18 * z + 10;
        }


    }
}
