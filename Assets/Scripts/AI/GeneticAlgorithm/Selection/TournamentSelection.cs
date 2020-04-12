using System;
using System.Collections.Generic;
using System.Linq;

namespace AI.GeneticAlgorithm {
    public class TournamentSelection<T> : ISelection<T> {
        public TournamentSelection(int tournamentSize, Random random) : this(tournamentSize, true, random) {
        }

        public TournamentSelection(int tournamentSize, bool doesWinnerCompeteFurther, Random random) {
            this.tournamentSize = tournamentSize;
            this.doesWinnerCompeteFurther = doesWinnerCompeteFurther;
            this.random = random;
        }

        public List<IGenome<T>> Select(int count, Generation<T> generation) {
            List<IGenome<T>> individuals = generation.Individuals.ToList();
            int[] participants;
            List<IGenome<T>> selected = new List<IGenome<T>>();
            while (selected.Count < count) {
                participants = Enumerable.Repeat(0, this.tournamentSize).Select(
                    individual => this.random.Next(individuals.Count)
                ).ToArray();
                IGenome<T> winner = individuals.Where(
                    (individual, index) => participants.Contains(index)
                ).OrderByDescending(individual => individual.Fitness).First();
                selected.Add(winner);
                if (!this.doesWinnerCompeteFurther) {
                    individuals.Remove(winner);
                }
            }
            return selected;
        }

        private int tournamentSize;

        private bool doesWinnerCompeteFurther;

        private Random random;
    }
}
