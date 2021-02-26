using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE154_5 : ISolve {

        public const int N = 200000;
        public const long multiple = 1000000000000;
        public Primes primes;
        public int[][] factorials;      
        public int[] d;     
        public int maxPrimeEvaluate;  
        public int maxPrimeIndex;

        public void SetData () { 

            // Get Primes to be used
            maxPrimeEvaluate = (int)Math.Ceiling((double)N/3);           
            primes = new Primes(maxPrimeEvaluate);
            maxPrimeIndex = primes.lstPrimes.Count - 1;

            // Set factorization of our divisor D
            d = new int[3]; // Only prime factors are 2 and 5
            primes.PrimeFactorization_ArrIdx(multiple, ref d);

            // Create factorial array
            factorials = new int[N+1][];
            factorials[0] = new int[]{};
            factorials[1] = new int[]{};
            
            int[] newVals; 
            for (int i =2; i<=N; i++) {
                newVals = factorials[i-1].ToArray();               
                primes.PrimeFactorization_ArrIdx(i, ref newVals, maxPrimeEvaluate);
                factorials[i] = newVals;
            }

            d = new int[3];
            primes.PrimeFactorization_ArrIdx(multiple, ref d, maxPrimeEvaluate);

            var fact200k = factorials[N];
        }

        public void Solve () {

            int count = 0;
            int[] factNOverDfactB;
            int badIndex;
            // This should ensure a>=b>=c // b[start] = (int)Math.Ceiling((double)N/3)
            for( int b = (int)Math.Ceiling((double)N/3); b<= N/2; b++) {

                factNOverDfactB = new int[maxPrimeIndex];
                for(int i=0; i<maxPrimeIndex; i++) {
                    factNOverDfactB[i] = factorials[N][i] - d.GV(i) - factorials[b][i];
                }

                for( int a = b; a+b <= N; a++) {
                    badIndex = -1;
                    int c = N - a - b;
                    if (c < 0) {continue;}

                    if (IsDivisible(factNOverDfactB, a, c, out badIndex)) {
                        if (a == b && b == c) {count += 1;}
                        else if (a == b || b == c) {count += 3;}
                        else {count += 6;}
                    } else if ( badIndex != 0 && badIndex != 2) { 
                        Console.WriteLine($"Non-divisible index at: {badIndex}");
                    }
                }
                Console.WriteLine($"b={b}, count={count}");
            }

        }

        public bool IsDivisible(int[] factNOverDfactB, int a, int c, out int badIndex) {
            badIndex = -1;
            /*
            int maxEvaluate = factorials[c].Length;
            for(int i=0; i<maxEvaluate; i++) {
                if (factNOverDfactB[i] - factorials[a][i] - factorials[c][i] < 0) { badIndex = i; return false; }
            }
            */
            int maxEvaluate = factorials[a].Length;
            for(int i=0; i<maxEvaluate-1; i++) {
                if (factNOverDfactB[i] - factorials[a][i] - factorials[c].GV(i) < 0) { badIndex = i; return false; }
            }      
            return true;
        }
    }
}