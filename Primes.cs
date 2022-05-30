using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProjectEuler
{
    public class Primes
    {
        public Int64 maxList;
        public HashSet<long> hstPrimes;
        public List<long> lstPrimes;

        //public Boolean blnCachePrimeFactorization = false;
        //private Dictionary<Int64, List<Int64>> cachePrimeFactorization = new Dictionary<long, List<long>>();
        public Primes(long maxComplete) {
            maxList = maxComplete;
            int approxPrimeCount = (int)Math.Ceiling((double)maxList / Math.Log(maxList));
            hstPrimes = new HashSet<long>(approxPrimeCount);
            lstPrimes = new List<long>(approxPrimeCount);
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

        public long SmallestPrime(long number) {

            int idx = 0;

            while ( true ) {

                if ( number % lstPrimes[idx] == 0 ) {
                    return lstPrimes[idx];                  
                }
                idx ++;
            } 
            // return number;
        }

        // *** Returns prime factorization in arry indexed by prime number seq.
        // if passed in with values already partially populated, effectively multiplies values
        public void PrimeFactorization_ArrIdx(long number, ref int[] values, int maxPrimeEvaluate = -1) {
         
            int idx = 0;
            while ( number > 1 ) {
                
                if (idx >= values.Length) { Array.Resize(ref values, values.Length+1); }
                if (number % lstPrimes[idx] == 0) {
                    values[idx] += 1;
                    number /= lstPrimes[idx];             
                } else { 
                    idx ++; 
                    // in some cases, we may not care about primes greater than a specified value.
                    if (maxPrimeEvaluate > 0 && ( idx >= lstPrimes.Count || lstPrimes[idx] > maxPrimeEvaluate)) {break;}
                }
            } 
        }

        public bool Reduce(ref long int1, ref long int2) {
            bool reduced = false;
            int i = 0;
            while ( lstPrimes[i] <= Math.Min(int1, int2) ) {
                if (int1 % lstPrimes[i] == 0 && int2 % lstPrimes[i] == 0) {
                    int1 /= lstPrimes[i];
                    int2 /= lstPrimes[i];
                    reduced = true;
                } else { i++; }              
            }
            return reduced;
        }

        public bool IsReducable(long int1, long int2) {
            int i = 0;
            while ( lstPrimes[i] <= Math.Min(int1, int2) ) {
                if (int1 % lstPrimes[i] == 0 && int2 % lstPrimes[i] == 0) {
                    return true;
                } else { i++; }              
            }
            return false;
        }

        public long GCD(long int1, long int2) {
            long gcd = 1;
            int i = 0;
            while ( lstPrimes[i] < Math.Min(int1, int2) ) {
                if (int1 % lstPrimes[i] == 0 && int2 % lstPrimes[i] == 0) {
                    int1 /= lstPrimes[i];
                    int2 /= lstPrimes[i];
                    gcd *= lstPrimes[i];
                } else { i++; }              
            }
            return gcd;
        }

        public SortedDictionary<int, int> PrimeFactorization_SD(long number) {
            
            var lstFactorization = new SortedDictionary<int, int>();
            int idx = 0;
            long current = number;
            long priorFactor = -1;

            while ( current > 1 ) {

                if ( current % lstPrimes[idx] == 0 ) {
                    if (lstPrimes[idx] == priorFactor) {
                        lstFactorization[(int)lstPrimes[idx]] += 1;
                    } else {
                        lstFactorization.Add((int)lstPrimes[idx], 1);
                        priorFactor = lstPrimes[idx];
                    }
                    current /= lstPrimes[idx];
                } else {             
                    priorFactor = lstPrimes[idx];
                    idx ++;
                }
            } 
            return lstFactorization;
        }

        public SortedDictionary<int, int> PrimeFactorization_SD(long number, int maxPrimeFactor, out bool factorComplete) {
            
            var lstFactorization = new SortedDictionary<int, int>();
            int idx = 0;
            long current = number;
            long priorFactor = -1;

            while ( current > 1 ) {

                if( lstPrimes[idx] > maxPrimeFactor) {
                    factorComplete = false;
                    return lstFactorization;

                }else if ( current % lstPrimes[idx] == 0 ) {
                    if (lstPrimes[idx] == priorFactor) {
                        lstFactorization[(int)lstPrimes[idx]] += 1;
                    } else {
                        lstFactorization.Add((int)lstPrimes[idx], 1);
                        priorFactor = lstPrimes[idx];
                    }
                    current /= lstPrimes[idx];
                } else {             
                    priorFactor = lstPrimes[idx];
                    idx ++;
                }
            } 

            factorComplete = true;
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

        public bool HaveCommonFactor(long val1, long val2, ref long minCommonFactor) {

            if (val1  < val2) {throw new Exception("val1 should be greater than val2");}
            if (val1 % val2 == 0) {minCommonFactor = val2; return true;}

            var factorization = PrimeFactorization(val2);

            foreach(var factor in factorization) {
                if (val1 % factor == 0) { minCommonFactor = factor; return true; }
            }

            return false;
        }
    }
}