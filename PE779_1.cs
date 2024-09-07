using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE779_1 : ISolve {

        public Primes primes;
        public const int k = 1;
        public const int maxTest = 6000000;
                
        public void SetData() {

            primes = new Primes(10000000);        
        }

        public void Solve() {

            double sum = 0;
            //int increment = 3;
            for(int i = 2; i<= maxTest; i+=1) {
                sum += fK(i);
            }
            Console.WriteLine(sum);
        }

        public double fK(long n) {

            long p = 1;
            long a = alpha(n, ref p);
            double result = ((double)1 / maxTest) * ((double)a - 1) / Math.Pow(p, k);
            //Console.WriteLine($"n:{n},\ta:{a},\tp:{p},\tfK: {result.ToString("0." + new string('#', 339))}");

            return result;         
        }

        public long alpha(long n, ref long p) {

            int a = 0;
            int i = 1;
            p = primes.SmallestPrime(n);
            while ( Math.Pow(p, i) <= n ) {
                if ( n % Math.Pow(p, i) == 0 ) {
                    a = i;
                }
                i++;
            }
            return a;
        }
    }
}