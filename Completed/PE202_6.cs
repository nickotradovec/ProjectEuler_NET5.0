using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE202_6 : ISolve {

        const long reflections = 12017639147;
        private Primes primes;
        private long C;
        private long eMin;
        private long eMax;
        private long count;
        HashSet<long> evaluatedFactors;
        private List<long> cFactorization;       

        public void SetData () {          

            C = (reflections + 3) / 2;
            primes = new Primes((long)Math.Ceiling(Math.Max((double)Math.Sqrt(C), 10000D)));
            cFactorization = primes.PrimeFactorization(C);

            eMax = EMax();     
            eMin = EMin(eMax);

            // We'll start considering all values, then subtract off invalid values.
            count = ((eMax-eMin) / 3) + 1;

            evaluatedFactors = new HashSet<long>();
        }

        public void Solve () {
                 
            foreach(long factor in cFactorization) {
                EvaluateFactor(factor);
            }

            Console.WriteLine($"Count: {count * 2}");
        } 

        public void EvaluateFactor(long factor) {

            if (FactorsContainValue(factor)) {return;}
            long testVal = (factor * (long)Math.Floor(((double)eMin - 1) / factor)) + factor;
            if (testVal > eMax) {return;}

            // add back factors doubly removed
            int i = 1;
            while (testVal < eMax) {
                if ( !FactorsContainValue(testVal)) {
                    count -= 1;
                }
                testVal += (3 * factor);
                i ++;
            }
            evaluatedFactors.Add(factor);
        }

        public bool FactorsContainValue(long value) {
            foreach (long factor in evaluatedFactors) {
                if (value % factor == 0) { return true; }
            }
            return false;
        }

        public long EMax() {
            if ((reflections - 1) % 3 == 0) { return C - 1;}
            if ((reflections + 1) % 3 == 0) { return C - 2;}
            throw new Exception("0 reflections!");
        }

        public long EMin(long eMax) {
            return eMax - (3L * (long)Math.Floor(((double)C-3)/6));
        }
    }
}