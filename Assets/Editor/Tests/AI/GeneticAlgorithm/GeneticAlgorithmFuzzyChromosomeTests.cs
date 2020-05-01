using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI.FuzzyLogic.Terms;
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
            parameters.GenerationSize = 20;
            parameters.MutationProbability = 0.2;
            parameters.DraftPart = 0.2;
            parameters.ChildrenSize = 20;
            parameters.SelectParentsTournamentSize = 10;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initializeFuzzyChromosomes(createGeneParams());

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
                double coef = ((double)(iterCount - iter)) / iterCount;
                geneticAlgorithm.newGenerationFuzzy(coef);
            }
            ISelection selection = new EliteSelection();
            IChromosome best = selection.Select(1, geneticAlgorithm.GetCurrentGeneration())[0];
            double res = function2(best);
            Debug.Log("Fitness = " + res);
            Debug.Log(printGenes((FuzzyChromosome)best));
            Assert.IsTrue(Math.Abs(51 - res) < 0.1);
        }

        [Test]
        public void CanInit()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 20;
            parameters.MutationProbability = 0.2;
            parameters.DraftPart = 0.2;
            parameters.ChildrenSize = 8;
            parameters.SelectParentsTournamentSize = 5;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initializeFuzzyChromosomes(createGeneParams());
        }

        private List<FuzzyGene.GeneParams> createGeneParams()
        {
            List<FuzzyGene.GeneParams> geneParams = new List<FuzzyGene.GeneParams>();
            geneParams.Add(new FuzzyGene.GeneParams("test", 0, 5));
            return geneParams;
        }

        [Test]
        public void CanIterate()
        {
            GeneticAlgorithmParams parameters = new GeneticAlgorithmParams();
            parameters.GenerationSize = 20;
            parameters.MutationProbability = 0.2;
            parameters.DraftPart = 0.2;
            parameters.ChildrenSize = 8;
            parameters.SelectParentsTournamentSize = 5;
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(parameters);
            geneticAlgorithm.initializeFuzzyChromosomes(createGeneParams());

            geneticAlgorithm.newGenerationFuzzy(0.9);
        }

        [Test]
        public void TestRecombinationFuzzy()
        {
            FuzzyChromosome chromosome1 = new FuzzyChromosome();
            FuzzyGene gene = new FuzzyGene(TermType.SShape, "test1", -5, 5);
            double[] values = new double[2] { 1, 2 };
            gene.UpdateValues(values);
            chromosome1.AddFuzzyGene(gene);
            FuzzyGene gene2 = new FuzzyGene(TermType.Trapezodial, "test2", -3, 3);
            double[] values2 = new double[4] { 1, 2, 3, 4 };
            gene2.UpdateValues(values2);
            chromosome1.AddFuzzyGene(gene2);

            FuzzyChromosome chromosome2 = new FuzzyChromosome();
            FuzzyGene gene3 = new FuzzyGene(TermType.ZShape, "test3", -5, 5);
            double[] values3 = new double[2] { 3, 2 };
            gene3.UpdateValues(values3);
            chromosome2.AddFuzzyGene(gene3);
            FuzzyGene gene4 = new FuzzyGene(TermType.Trapezodial, "test4", -3, 3);
            double[] values4 = new double[4] { 6, 7, 8, 9 };
            gene4.UpdateValues(values4);
            chromosome2.AddFuzzyGene(gene4);

            LinearRecombination recombination = new LinearRecombination(null);
            IChromosome newChromosome = recombination.calc(chromosome1, chromosome2, 0.5);

            Debug.Log(printGenes((FuzzyChromosome)newChromosome));
            Debug.Log(printGenes(chromosome1));
            Debug.Log(printGenes(chromosome2));
            Assert.AreEqual(4.5, newChromosome.Genes[3]);
        }

        [Test]
        public void TestMaxIteration()
        {
            int maxIter = 100;
            int iter = 17;
            double coef = ((double) (maxIter - iter)) / maxIter;
            Debug.Log(coef);
            Assert.IsTrue(Math.Abs(coef - 0.83) < 0.001);
        }

        [Test]
        public void TestTermBoundsMutation()
        {
            FuzzyChromosome chromosome1 = new FuzzyChromosome();
            FuzzyGene gene = new FuzzyGene(TermType.SShape, "test1", -5, 5);
            double[] values = new double[2] { 1, 2 };
            gene.UpdateValues(values);
            chromosome1.AddFuzzyGene(gene);
            FuzzyGene gene2 = new FuzzyGene(TermType.Trapezodial, "test2", -3, 3);
            double[] values2 = new double[4] { 1, 2, 3, 4 };
            gene2.UpdateValues(values2);
            chromosome1.AddFuzzyGene(gene2);

            IMutation mutation = new TermBoundsMutation();
            mutation.Mutate(chromosome1, 1);

            Debug.Log(printGenes(chromosome1));
            Assert.AreNotEqual(1, chromosome1.Genes[0]);
        }

        [Test]
        public void TestTermShapeMutation()
        {
            FuzzyChromosome chromosome1 = new FuzzyChromosome();
            FuzzyGene gene = new FuzzyGene(TermType.SShape, "test1", -5, 5);
            double[] values = new double[2] { 1, 2 };
            gene.UpdateValues(values);
            chromosome1.AddFuzzyGene(gene);
            FuzzyGene gene2 = new FuzzyGene(TermType.Trapezodial, "test2", -3, 3);
            double[] values2 = new double[4] { 1, 2, 3, 4 };
            gene2.UpdateValues(values2);
            chromosome1.AddFuzzyGene(gene2);

            IMutation mutation = new TermShapeMutation();
            mutation.Mutate(chromosome1, 1);

            Debug.Log(printGenes(chromosome1));
            Assert.AreNotEqual(TermType.SShape, chromosome1.FuzzyGenes[0].TermType);
        }

        [Test]
        public void CanMutateGene()
        {
            TermShapeMutation mutation = new TermShapeMutation();
            FuzzyGene gene = new FuzzyGene(TermType.SShape, "test1", -5, 5);
            double[] values = new double[2] { 1, 2 };
            gene.UpdateValues(values);
            mutation.mutate(gene);
            Debug.Log(printGene(gene));
            Assert.AreNotEqual(TermType.SShape, gene.TermType);
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
            if (chromosome.Genes.Length != 3)
            {
                return -1000;
            }
            double x = chromosome.Genes[0];
            double y = chromosome.Genes[1];
            double z = chromosome.Genes[2];
            return -x * x - 5 * y * y - 3 * z * z + x * y - 2 * x * z + 2 * y * z + 11 * x + 2 * y + 18 * z + 10;
        }

        private String printGene(FuzzyGene gene)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(gene.TermType).Append(": ");
            stringBuilder.Append("[");
            for (int i = 0; i < gene.Size; i++)
            {
                stringBuilder.Append(gene.Values[i].ToString("0.##")).Append(";  ");
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        private String printGenes(FuzzyChromosome chromosome)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(FuzzyGene gene in chromosome.FuzzyGenes)
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
    }
}
