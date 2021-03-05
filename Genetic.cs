using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {

    public abstract class GeneticAlgorithm<T> {
        public List<T> population = new List<T>();
        //public event EventHandler generationEvaluated;

        public void Optimize(ref T mostFit, int generations, int offspringPerParent) {
            // do stuff
            population.Add(mostFit);

            for(int i=0; i<generations; i++) {
                Console.WriteLine($"Evaluating generation: {i}, population size: {population.Count}");

                CreateOffspring(ref population, offspringPerParent);
                EvaluateDuplicates(ref population);
                EvaluateSurvival(ref population);              
            }

            mostFit = population[0];
        }
        /// <summary>
        /// This function should create offspring for each original member of the population
        /// It should do some by implementing some random variation and "mating" that with the parent
        /// </summary>
        public abstract void CreateOffspring(ref List<T> pop, int children);
        /// <summary>
        /// This function should remove any duplicates, however that is evaluated. Should implement ICompare
        /// </summary>
        /// <param name="pop"></param>
        public abstract void EvaluateDuplicates(ref List<T> pop);
        /// <summary>
        /// This function should kill off those members which are too far from our "benchmark"
        /// </summary>
        public abstract void EvaluateSurvival(ref List<T> pop);
    }
}