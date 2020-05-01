using AI.FuzzyLogic.Terms;
using System;
using System.Collections.Generic;

namespace AI.GeneticAlgorithm
{
    public class FuzzyChromosome : IChromosome
    {
        public double[] Genes
        {
            get
            {
                List<double> genes = new List<double>();
                fuzzyGenes.ForEach(gene => genes.AddRange(gene.Values));
                return genes.ToArray();
            }
        }

        public int Size
        {
            get
            {
                int size = 0;
                fuzzyGenes.ForEach(gene => size += gene.Size);
                return size;
            }
        }

        public double Fitness
        {
            get
            {
                return fitness;
            }
            set
            {
                fitness = value;
            }
        }

        public List<FuzzyGene> FuzzyGenes { get => fuzzyGenes; }

        public FuzzyChromosome()
        {
            fuzzyGenes = new List<FuzzyGene>();
        }

        public void UpdateGenes(double[] genes)
        {
            int prevIndex = 0;
            foreach(FuzzyGene gene in fuzzyGenes)
            {
                double[] values = new double[gene.Size];
                Array.Copy(genes, prevIndex, values, 0, gene.Size);
                gene.UpdateValues(values);
                prevIndex += gene.Size;
            }
        }

        public bool hasBounds()
        {
            return true;
        }

        public IChromosome Clone()
        {
            FuzzyChromosome res = new FuzzyChromosome();
            foreach(FuzzyGene gene in fuzzyGenes)
            {
                res.AddFuzzyGene(gene.Clone());
            }
            return res;
        }

        public void AddFuzzyGene(FuzzyGene fuzzyGene)
        {
            fuzzyGenes.Add(fuzzyGene);
        }

        public Tuple<double, double> getRanges(int geneIndex)
        {
            int prevIndex = 0;
            foreach (FuzzyGene gene in fuzzyGenes)
            {
                if (geneIndex >= prevIndex && geneIndex < gene.Size + prevIndex)
                {
                    return Tuple.Create(gene.MinValue, gene.MaxValue);
                }
                prevIndex += gene.Size;
            }
            throw new Exception("fuzzy Genes must contain index");
        }

        private double fitness;
        private List<FuzzyGene> fuzzyGenes;
    }
}
