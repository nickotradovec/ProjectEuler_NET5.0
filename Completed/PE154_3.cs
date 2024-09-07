using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    // From 154_2 but written with arrays instead of dictionaries for performance.
    public class PE154_3 : ISolve {

        public const int N = 140000;
        public const int N_MaxEvaluate = 140000;
        public const long multiple = 10000000000; //1000000000000;
        public Primes primes;
        public int[][] factorials;      
        public int maxPrimeIndex;
        public int[] nFactorialMinusDivisor;

        public void SetData () { 
            
            primes = new Primes(N);
            maxPrimeIndex = primes.lstPrimes.Count;
            factorials = new int[N+1][];
                      
            factorials[0] = new int[0];
            factorials[1] = new int[0];
            
            int[] newVals;          

            for (int i =2; i<=N; i++) {
                newVals = factorials[i-1].ToArray();               
                primes.PrimeFactorization_ArrIdx(i, ref newVals, N_MaxEvaluate);
                factorials[i] = newVals;
            }

            int[] divisor = new int[3];       
            primes.PrimeFactorization_ArrIdx(multiple, ref divisor);

            nFactorialMinusDivisor = new int[maxPrimeIndex];
            for(int i=0; i<maxPrimeIndex; i++) {
                nFactorialMinusDivisor[i] = factorials[N].GV(i) - divisor.GV(i);
            }
            
            //Console.WriteLine(maxFactorialDivisor(nFactorialMinusDivisor));
        }

        public void Solve () {
            // for enforcing sequencing, we wnat i >= j >= k

            // Find max that can be supported (iMax) where divisibility is possible.
            int nMax = maxFactorialDivisor(nFactorialMinusDivisor);
            Console.WriteLine($"NMax: {nMax}");

            int k;
            int termsCounted = 0;
            int divisibleCount = 0;

            for(int i=N; i>=Math.Floor((double)N/3); i-- ) {

                if (i % 1000 == 0 ) {Console.WriteLine($"Evaluating: i={i}, DivisibleCount:{divisibleCount}");}

                for(int j=N-i; j >= Math.Ceiling((double)(N-i)/2); j-- ) {
                    
                    k = N - i - j;
                    if (k > i || k > j || k<0 || k > j || j > i)  { continue; }
                    
                    if (EvaluatedDivisible(i, j, k)) {
                        if ( i == j && i == k) { divisibleCount += 1; }
                        else if ( i == j || j == k || k == i) { divisibleCount += 3; }
                        else { divisibleCount += 6; }
                    }
                    termsCounted += 1;                  
                }
            }

            Console.WriteLine($"Terms to count: {termsCounted}");
            Console.WriteLine($"Terms divisible: {divisibleCount}");
        }

        public int maxFactorialDivisor(int[] divisor) {
            
            for(int i=N; i>0; i--) {
                bool div = true;
                int j=0;
                while (div == true && j<factorials[i].Length) {
                    if (divisor.GV(j) - factorials[i][j] < 0) {div = false; break;}
                    j++;
                }
                if (div) { return i; }
            }
            return -1;
        }

        // This will be the full evaluation of evaluateing N!/(x!y!z!) % divisor
        public bool EvaluatedDivisible(int num1, int num2, int num3) {

            for(int i=0; i<=factorials[num1].Length; i++) {
                if (nFactorialMinusDivisor.GV(i) - factorials[num1].GV(i) - factorials[num2].GV(i) - factorials[num3].GV(i) < 0) {return false;}
            }
            return true;
        }  
    }
    static class Extension
    {
        public static int GV(this int[] array, int index)
        {
            if (index >= array.Length) {return 0;}
            return array[index];
        }
    } 
}