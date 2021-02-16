using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler
{
    public class Primes
    {
        public Int64 maxList;
        public HashSet<long> hstPrimes = new HashSet<long>();
        public List<long> lstPrimes = new List<long>();

        //public Boolean blnCachePrimeFactorization = false;
        //private Dictionary<Int64, List<Int64>> cachePrimeFactorization = new Dictionary<long, List<long>>();
        public Primes(long maxComplete) {
            maxList = maxComplete;
            SetBySeive();
        }

        private void SetBySeive() {
            Boolean[] seive = new Boolean[maxList + 1]; // because of defaulting, true wil mean not prime
            for (long i=2; i<maxList; i++) {
                
                // if the number itself is not prime, we will have already gone through the divisors
                if (seive[i]) { continue; }

                long notPrime = 2*i;
                while ( notPrime <= maxList ) {
                    seive[notPrime] = true;
                    notPrime += i;
                }
            }
            // Set to our hashset and list
            for ( long i=2; i<=maxList; i++) {
                if ( ! seive[i] ) {
                    hstPrimes.Add(i);
                    lstPrimes.Add(i);
                }
            }
        }

        public Boolean IsPrime(long number) {
            if ( number <= maxList ) {
                return hstPrimes.Contains(number);

            } else if ( number < maxTest ) {
                int idx = 0;
                long test = lstPrimes[idx];
                do {
                    test = lstPrimes[idx];
                    if ( number % test == 0 ) { return false; }
                } while( test <= Math.Ceiling(Math.Sqrt(number)) );
                return true;
            }
            throw new Exception("Can not test number larger than the max initialized squared!");
        }

        public List<Int64> PrimeFactorization(long number) {

            var lstFactorization = new List<long>();
            int idx = 0;
            long current = number;

            while ( current > 1 ) {

                if ( current % lstPrimes[idx] == 0 ) {
                    lstFactorization.Add(lstPrimes[idx]);
                    current /= lstPrimes[idx];
                } else {
                    idx ++;
                }
            } 
            return lstFactorization;
        }

        public long DivisorCount(long number) {

            long divisorCount = 1;
            var grouped = Standard.GroupList<long>(PrimeFactorization(number));
            foreach (var kvp in grouped) {
                divisorCount *= (kvp.Value + 1);
            }
            return divisorCount;
        }

        public long DivisorCount(Dictionary<long, int> grouped) {

            long divisorCount = 1;
            foreach (var kvp in grouped) {
                divisorCount *= (kvp.Value + 1);
            }
            return divisorCount;
        }

        public List<long> Divisors(long number, bool proper) {

            var rtn = new List<long>{1};
            var grouped = Standard.GroupList<long>(PrimeFactorization(number));        
            var components = new List<List<long>>();
     
            foreach ( var kvp in grouped ) {
                var toAdd = new List<long>();
                for (int i=0; i<=kvp.Value; i++) {
                    toAdd.Add((long)Math.Pow(kvp.Key, i));
                }
                components.Add(toAdd);
            }

            for (int i = 0; i<components.Count; i++) {
                rtn = Standard.CrossMultiply(components[i], rtn);
            }

            rtn.Sort();
            if (proper) { rtn.RemoveAt(rtn.Count - 1); }
            return rtn;
        }

        public long maxTest {
            get {
                return (long)Math.Pow(maxList, 2);
            }
        }
    }
}