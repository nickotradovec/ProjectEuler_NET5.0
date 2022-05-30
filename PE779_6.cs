using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE779_6 : ISolve {

        // https://en.wikipedia.org/wiki/List_of_mathematical_series

        public Primes primes;
        public const int k = 1;
        public const long maxTest = 1000000000; // Int64.MaxValue;
        //public SortedList<long, double> testRecord;
                
        public void SetData() {

            primes = new Primes(20000000);    
            //testRecord = new SortedList<long, double>(1000);

        }

        public void Solve() {

            double sum = 0;
            long p;
            double toAdd;

            for (int i = 0; i<400; i++) { //  // idx<primes.lstPrimes.Count
                
                p = primes.lstPrimes[i];
                toAdd = 0;

                

                Console.WriteLine($"p:{p},\tf:{toAdd}");
                sum += toAdd;
            }

            Console.WriteLine(sum);
        }       
    }  

    public class PrimeFactor {
        public long maxIndex;
        public List<long>[] factorization;
        public PrimeFactor(long maxVal) {
            maxIndex = maxVal+1;
            factorization = new List<long>[maxIndex];

            InitializePrimeFacotrs();
            BuildPrimeFactors();
        }

        private void BuildPrimeFactors() {

            for(int i=2; i<maxIndex; i++) {
                
            }

        }

        private void InitializePrimeFacotrs() {
            for(int i=0; i<maxIndex; i++) {
                factorization[i] = new List<long>();
            }
        }
    }
}