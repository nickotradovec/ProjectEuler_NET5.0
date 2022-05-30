using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE779_2 : ISolve {

        public Primes primes;
        public const int k = 1;
        public const int maxTest = 1000;
                
        public void SetData() {

            primes = new Primes(1000000);        
        }

        public void Solve() {

            double sum = 0;
            long p, a, aMax;
            double result;
            long nLast = 0;
       
            for(int n = 2; n<= maxTest; n+=1) {

                p = 1;
                a = alpha(n, ref p);
                aMax = (long)Math.Floor(Math.Log((double)n) / Math.Log((double)p));

                result = ((double)a - 1) / Math.Pow(p, k); // * ((double)1 / maxTest))

                if (result > 0 && p ==  2) { // && p ==  5
                    Console.WriteLine($"n:{n},\tn/p2:{n/Math.Pow(p,2)}, \ta:{a},\tp:{p},\tp*fK: {(p*result).ToString("0." + new string('#', 339))}"); // \tnDiff/p2:{(n-nLast)/Math.Pow(p,2)},
                    nLast = n;
                    sum += result;
                }
                
                //sum += result;
            }
            Console.WriteLine(sum/maxTest);
        }

        public double fK(long n) {

            long p = 1;
            long a = alpha(n, ref p);
            double result =  ((double)a - 1) / Math.Pow(p, k); // * ((double)1 / maxTest)
            Console.WriteLine($"n:{n},\ta:{a},\tp:{p},\tfK: {result.ToString("0." + new string('#', 339))}");

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