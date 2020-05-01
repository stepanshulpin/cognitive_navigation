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
    class FuzzyChromosomeTests
    {

        [Test]
        public void CanCreateFuzzyGene()
        {
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            Assert.AreEqual(5, gene.MaxValue);
        }

        [Test]
        public void TestFuzzeGenValuesNotNull()
        {
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            Assert.IsNotNull(gene.Values);
        }

        [Test]
        public void CanCreateFuzzyGeneWithDefaultValues()
        {
            double[] values = new double[3] { 1, 2, 3 };
            FuzzyGene gene = new FuzzyGene(values, -5, 5, 3);
            Assert.AreEqual(2, gene.Values[1]);
        }

        [Test]
        public void CanUpdateFuzzyGeneValues()
        {
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            double[] values = new double[3] { 1, 2, 3 };
            gene.UpdateValues(values);
            Assert.AreEqual(2, gene.Values[1]);
        }
        [Test]
        public void CannotUpdateFuzzyGeneValuesWithWrongSize()
        {
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            double[] values = new double[4] { 1, 2, 3, 4 };
            Assert.Throws<Exception>(() => gene.UpdateValues(values));
        }

        [Test]
        public void CanCloneFuzzyGene()
        {
            double[] values = new double[3] { 1, 2, 3 };
            FuzzyGene gene = new FuzzyGene(values, -5, 5, 3);
            FuzzyGene newGene = gene.Clone();
            double[] newValues = new double[3] { 4, 5, 6 };
            newGene.UpdateValues(newValues);
            Assert.AreNotEqual(gene.Values[2], newGene.Values[2]);
        }

        [Test]
        public void CanCloneFuzzyGeneAndUpdate()
        {
            double[] values = new double[3] { 1, 2, 3 };
            FuzzyGene gene = new FuzzyGene(values, -5, 5, 3);
            FuzzyGene newGene = gene.Clone();
            double[] newValues = new double[3] { 4, 5, 6 };
            gene.UpdateValues(newValues);
            Assert.AreNotEqual(gene.Values[2], newGene.Values[2]);
        }

        [Test]
        public void CanCreateFuzzyChromosome()
        {
            FuzzyChromosome chromosome = new FuzzyChromosome();
            Assert.IsNotNull(chromosome.FuzzyGenes);
        }

        [Test]
        public void CanAddFuzzyGeneToFuzzyChromosome()
        {
            FuzzyChromosome chromosome = new FuzzyChromosome();
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            chromosome.AddFuzzyGene(gene);
            Assert.AreEqual(-5, chromosome.FuzzyGenes[0].MinValue);
        }

        private FuzzyChromosome createChromosome()
        {
            FuzzyChromosome chromosome = new FuzzyChromosome();
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            double[] values = new double[3] { 1, 2, 3 };
            gene.UpdateValues(values);
            chromosome.AddFuzzyGene(gene);
            FuzzyGene gene2 = new FuzzyGene(-3, 3, 5);
            double[] values2 = new double[5] { 1, 2, 3, 4, 5 };
            gene2.UpdateValues(values2);
            chromosome.AddFuzzyGene(gene2);
            return chromosome;
        }

        [Test]
        public void CanGetFuzzyChromosomeSize()
        {
            IChromosome chromosome = createChromosome();
            Assert.AreEqual(8, chromosome.Size);
        }

        [Test]
        public void CanGetRangesForFirstGeneValue()
        {
            IChromosome chromosome = createChromosome();
            Assert.AreEqual(-5, chromosome.getRanges(0).Item1);
        }

        [Test]
        public void CanGetRangesForLastFirstGeneValue()
        {
            IChromosome chromosome = createChromosome();
            Assert.AreEqual(5, chromosome.getRanges(2).Item2);
        }

        [Test]
        public void CanGetRangesForFirstSecondGeneValue()
        {
            IChromosome chromosome = createChromosome();
            Assert.AreEqual(3, chromosome.getRanges(3).Item2);
        }

        [Test]
        public void CanGetRangesForLastSecondGeneValue()
        {
            IChromosome chromosome = createChromosome();
            Assert.AreEqual(-3, chromosome.getRanges(7).Item1);
        }

        [Test]
        public void CannnotGetRangesIndexOutOfBound()
        {
            IChromosome chromosome = createChromosome();
            Assert.Throws<Exception>(() => chromosome.getRanges(8));
        }

        private FuzzyChromosome createChromosomeWithThreeGenes()
        {
            FuzzyChromosome chromosome = new FuzzyChromosome();
            FuzzyGene gene = new FuzzyGene(-5, 5, 3);
            double[] values = new double[3] { 1, 2, 3 };
            gene.UpdateValues(values);
            chromosome.AddFuzzyGene(gene);
            FuzzyGene gene2 = new FuzzyGene(-3, 3, 5);
            double[] values2 = new double[5] { 1, 2, 3, 4, 5 };
            gene2.UpdateValues(values2);
            chromosome.AddFuzzyGene(gene2);
            FuzzyGene gene3 = new FuzzyGene(-4, 4, 4);
            double[] values3 = new double[4] { 1, 2, 3, 4 };
            gene3.UpdateValues(values3);
            chromosome.AddFuzzyGene(gene3);
            return chromosome;
        }

        [Test]
        public void CanGetRangesForThirdGeneValue()
        {
            IChromosome chromosome = createChromosomeWithThreeGenes();
            Assert.AreEqual(4, chromosome.getRanges(9).Item2);
        }

        [Test]
        public void CanGetGenes()
        {
            IChromosome chromosome = createChromosomeWithThreeGenes();
            Assert.AreEqual(4, chromosome.Genes[6]);
        }

        [Test]
        public void CanGetGenesTwo()
        {
            IChromosome chromosome = createChromosomeWithThreeGenes();
            Assert.AreEqual(2, chromosome.Genes[9]);
        }

        [Test]
        public void CanCloneChromosome()
        {
            IChromosome chromosome = createChromosomeWithThreeGenes();
            IChromosome newChromosome = chromosome.Clone();
            Assert.AreEqual(2, newChromosome.Genes[9]);
        }

        [Test]
        public void CanUpdateChromosome()
        {
            IChromosome chromosome = createChromosome();
            double[] newGenes = new double[8] { 8, 7, 6, 5, 4, 3, 2, 1 };
            chromosome.UpdateGenes(newGenes);
            Assert.AreEqual(5, chromosome.Genes[3]);
        }

        [Test]
        public void CanUpdateChromosomeTwo()
        {
            FuzzyChromosome chromosome = createChromosome();
            double[] newGenes = new double[8] { 8, 7, 6, 5, 4, 3, 2, 1 };
            chromosome.UpdateGenes(newGenes);
            Assert.AreEqual(8, chromosome.FuzzyGenes[0].Values[0]);
        }

        [Test]
        public void CanCloneAndUpdate()
        {
            FuzzyChromosome chromosome = createChromosome();
            double[] newGenes = new double[8] { 8, 7, 6, 5, 4, 3, 2, 1 };
            FuzzyChromosome newChromosome = (FuzzyChromosome)chromosome.Clone();
            chromosome.UpdateGenes(newGenes);
            Assert.AreNotEqual(newChromosome.Genes[3], chromosome.Genes[3]);
        }

        [Test]
        public void CanCloneAndUpdateChromosomeWithThreeGenes()
        {
            FuzzyChromosome chromosome = createChromosomeWithThreeGenes();
            double[] newGenes = new double[12] {12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            FuzzyChromosome newChromosome = (FuzzyChromosome)chromosome.Clone();
            newChromosome.UpdateGenes(newGenes);
            Assert.AreEqual(9, newChromosome.Genes[3]);
        }

        [Test]
        public void DoesnotUpdatePrevChromosome()
        {
            FuzzyChromosome chromosome = createChromosomeWithThreeGenes();
            double[] newGenes = new double[12] { 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            FuzzyChromosome newChromosome = (FuzzyChromosome)chromosome.Clone();
            newChromosome.UpdateGenes(newGenes);
            Assert.AreNotEqual(9, chromosome.Genes[3]);
        }

        [Test]
        public void CanMutateFuzzy()
        {
            IChromosome chromosome = createChromosome();
            IMutation mutation = new TermBoundsMutation();
            mutation.Mutate(chromosome, 1);
            Debug.Log(printGenes(chromosome));
            Assert.AreNotEqual(1, chromosome.Genes[0]);
        }

        [Test]
        public void CanMutateFuzzyBound()
        {
            IChromosome chromosome = createChromosome();
            IMutation mutation = new TermBoundsMutation();
            mutation.Mutate(chromosome, 1);
            Debug.Log(printGenes(chromosome));
            Assert.IsTrue(chromosome.Genes[5] <= 3 && chromosome.Genes[5] >= -3);
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

    }
}
