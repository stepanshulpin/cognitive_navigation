﻿using Utils;

namespace AI.GeneticAlgorithm
{
    public class NumericChromosome : IChromosome
    {
        public double[] Genes
        {
            get
            {
                return genes;
            }
        }

        public int Size
        {
            get
            {
                return genes.Length;
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

        public NumericChromosome()
        {
            fitness = 0.0f;
        }

        public NumericChromosome(int size)
        {
            fitness = 0.0f;
            genes = new double[size];
        }

        public NumericChromosome(double[] genes)
        {
            this.genes = genes;
            fitness = 0.0f;
        }        

        public void UpdateGenes(double[] genes)
        {
            //TODO: check
            genes.CopyTo(this.genes, 0);
        }

        public IChromosome Clone()
        {
            NumericChromosome res = new NumericChromosome(genes.Length);
            genes.CopyTo(res.genes, 0);
            return res;
        }

        private double fitness;
        private double[] genes;
    }
}
