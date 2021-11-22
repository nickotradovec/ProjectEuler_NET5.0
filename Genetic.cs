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

            population.Add(mostFit);

            for(int i=0; i<generations; i++) {
                Console.WriteLine($"Evaluating generation: {i}, population size: {population.Count}");
                DateTime dtmStart = DateTime.Now;
                
                CreateOffspring(ref population, offspringPerParent);
                DateTime dtmOff = DateTime.Now;
                
                              
                EvaluateDuplicates(ref population);
                DateTime dtmDup = DateTime.Now;
                              
                EvaluateSurvival(ref population); 
                DateTime dtmSur = DateTime.Now; 
                
                TimeSpan tsOff = dtmOff - dtmStart;
                TimeSpan tsDup = dtmDup - dtmOff;
                TimeSpan tsSur = dtmSur - dtmDup;
                Console.WriteLine($"Generation: {i}, Population: {population.Count}, Create Offspring: {tsOff.TotalSeconds}, Duplicates: {tsDup.TotalSeconds}, Survival: {tsSur.TotalSeconds}");           
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