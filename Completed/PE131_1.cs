using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE131_1 : ISolve {     

        // There are some prime values, p, for which there exists a positive integer, n, such that the expression n^3 + p*n^2 is a perfect cube

        public const int max = 1000000;
        public Primes mPrm;

        public List<long> cubes = new List<long>(max);

        public void SetData () {            
            mPrm = new Primes((long)Math.Pow(max, (double)3/2));

            for(int i=1; i<Math.Pow(Int64.MaxValue, (double)1/3); i++) { cubes.Add((long)Math.Pow(i, 3)); }
        }

        public void Solve () {
         
            int pIdx = 0;
            long prime = mPrm.lstPrimes[pIdx];
            
            var lstAnswers = new List<Tuple<long, long, long>>(); // prime, n, c^3
            long n, cCubedMin;

            while (prime <= max) {
                
                for(int i=0; i<max; i++) {
                    if ( cubes[i] <= prime ) {continue;}
                    if ( cubes[i] > max ) {break;}

                    n = cubes[i] - prime;
                    cCubedMin = GetNumericValue(c3Min(mPrm.PrimeFactorization_SD(n)));

                    if ( cubes.Contains(cubes[i] * cCubedMin) ) {
                        lstAnswers.Add(new Tuple<long, long, long>(prime, n, cCubedMin));
                        break;
                    }
                }
                
                pIdx += 1;
                prime = mPrm.lstPrimes[pIdx];
            }

            Console.WriteLine(lstAnswers.Count);
        } 

        public SortedDictionary<int, int> c3Min(SortedDictionary<int, int> nFactorization) {

            var cMin = new SortedDictionary<int, int>(nFactorization);
            foreach( KeyValuePair<int, int> factor in nFactorization) {
                cMin[factor.Key] = (int)Math.Ceiling((double)nFactorization[factor.Key] * 2 / 3) * 3;
            }
            return cMin;
        }

        public long GetNumericValue(SortedDictionary<int, int> pf) {
            long val = 1;
            foreach( KeyValuePair<int, int> factor in pf) {
                val *= (long)Math.Pow(factor.Key, factor.Value);
            }
            return val;
        }
    }
}