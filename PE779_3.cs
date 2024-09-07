using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE779_3 : ISolve {

        public Primes primes;
        public const int k = 1;
        public const long maxTest = 10000; // Int64.MaxValue;
                
        public void SetData() {

            primes = new Primes(10000000);        
        }

        public void Solve() {

            double sum = 0;
            double pSum = 0;
            long p, a;

            for (int idx = 2; idx < 3; idx++) { // primes.lstPrimes.Count-1
                
                long n = 0;
                p = primes.lstPrimes[idx];
                
                pSum = (double)1/Math.Pow(p, k);

                for( int multIdx = idx; multIdx < primes.lstPrimes.Count; multIdx++) { // Not as simple as p^2 * a single prime. Need to recursively count.
 
                    n = p * p * primes.lstPrimes[multIdx];
                    if ( n < 0 || n > maxTest) { 
                        n = maxTest; break; 
                    }
                    
                    a = alpha(n, p);              
                    pSum += ((double)a - 1) / Math.Pow(p, k);
                    Console.WriteLine($"n:{n},\tp:{p},\tfK: {pSum.ToString("0." + new string('#', 339))}");
                }

                pSum = pSum / n; // Only dividide at the end to minimize loss
                
                sum += pSum;
                if ( idx % 20 == 0 ) {Console.WriteLine($"p: {p}\tsum: {sum} ");}
            }

            Console.WriteLine(sum);
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
    }
}