using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE204_1 : ISolve {

        public const int hamType = 100;
        public const int maxVal = 1000000000;

        public Primes primes;       

        public void SetData () { 

            primes = new Primes(maxVal);
        }

        public void Solve () {

            bool[] vals = new bool[maxVal+1];

            int idx = 0;
            while (primes.lstPrimes[idx] <= hamType) {idx++;}

            long val, prime;
            for(int i=idx; i<primes.lstPrimes.Count; i++) {
                prime = primes.lstPrimes[i];
                val = prime;
                while (val <= maxVal) {
                    vals[val] = true;
                    val += prime;
                }
            }

            long count = 0;
            for(int i=1; i<=maxVal; i++) {
                if (!vals[i]) {count++;}
            }
            Console.WriteLine(count);
        
        }
    }
}