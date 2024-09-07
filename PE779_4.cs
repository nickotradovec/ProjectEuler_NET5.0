using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE779_4 : ISolve {

        public Primes primes;
        public const int k = 1;
        public const long maxTest = 200000000; // Int64.MaxValue;
        //public SortedList<long, double> testRecord;
                
        public void SetData() {

            primes = new Primes(200000000);    
            //testRecord = new SortedList<long, double>(1000);

        }

        public void Solve() {

            double sum = 0;
            double pSum = 0;
            //long p, a;
            long maxSquare = (long)Math.Floor(Math.Sqrt(maxTest));

            for (int idx = 0; primes.lstPrimes[idx] < maxSquare; idx++) { //  // idx<primes.lstPrimes.Count
                
                pSum = fk(new List<int[]> {new int[]{idx, 2}});
                pSum = pSum / maxTest;
                
                sum += pSum;
                //if ( idx % 20 == 0 ) {Console.WriteLine($"p: {p}\tsum: {sum} ");}

                //foreach( KeyValuePair<long, double> wrt in testRecord) {
                //    Console.WriteLine($"n:{wrt.Key}\tsum:{wrt.Value}");
                //}
            }

            Console.WriteLine(sum);
        }

        public double fk(List<int[]> factorization) {

            // if (exceedsMax(factorization)) { return 0; }
            //double sum = ((double)alpha(factorization) - 1) / (Math.Pow(primes.lstPrimes[factorization[0][0]], k));
            double sum = ((double)factorization[0][1] - 1) / primes.lstPrimes[factorization[0][0]];
            
            //testRecord.Add(val(factorization), sum);
            //Console.WriteLine($"p:{primes.lstPrimes[factorization[0][0]]}\tn:{val(factorization)}\tsum:{sum}");
            
            // we want to add an exponent to the current value and then we wnat to add a new factor (each prime with power 1)
            long maxMultiplier = maxMult(factorization);
            if (primes.lstPrimes[factorization[factorization.Count-1][0]] > maxMultiplier) {return sum;}

            var sameExp = copyFactorization(factorization);
            sameExp[sameExp.Count-1][1] += 1;
            sum += fk(sameExp);

            List<int[]> newFct;
            for( int i = factorization[factorization.Count-1][0] + 1; i < primes.lstPrimes.Count && primes.lstPrimes[i] <= maxMultiplier; i++) {              
                newFct = copyFactorization(factorization);
                newFct.Add(new int[]{i, 1});
                sum += fk(newFct);
            }

            return sum;
        }


        // can likely be simplified
        public long alpha(List<int[]> nFactorization) {
            
            long val = 1;
            foreach (int[] fct in nFactorization) {
                val *= (long)Math.Pow(primes.lstPrimes[fct[0]], fct[1]);
            }
            return alpha(val, primes.lstPrimes[nFactorization[0][0]]);
        }

        public long alpha(long n, long p) {

            int a = 0;
            int i = 1;
            while ( Math.Pow(p, i) > 0 && Math.Pow(p, i) <= n ) {
                if ( n % Math.Pow(p, i) == 0 ) { a = i; }
                i++;
            }
            return a;
        }

        public bool exceedsMax(List<int[]> factorization) {
            long val = 1;
            foreach (int[] fct in factorization) {
                val *= (long)Math.Pow(primes.lstPrimes[fct[0]], fct[1]);
                if ( val < 0 || val > maxTest ) { return true; }
            }
            return false;
        }

        public long maxMult(List<int[]> factorization) {
            long val = 1;
            foreach (int[] fct in factorization) {
                val *= (long)Math.Pow(primes.lstPrimes[fct[0]], fct[1]);
                if ( val < 0 || val > maxTest ) { return 1; }
            }
            return (long)Math.Floor((double)maxTest/val);
        }

        public List<int[]> copyFactorization(List<int[]> factorization) {

            var copy = new List<int[]>(factorization.Count);
            foreach ( int[] fct in factorization ) {
                copy.Add(new int[]{fct[0], fct[1]});
            }
            return copy;
        }

        public long val(List<int[]> factorization) {
            long val = 1;
            foreach (int[] fct in factorization) {
                val *= (long)Math.Pow(primes.lstPrimes[fct[0]], fct[1]);
            }
            return val;
        }
    }  
}