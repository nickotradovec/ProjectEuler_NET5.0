using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE154_6 : ISolve {

        public const int N = 200000;
        public const long multiple = 1000000000000;
        public Primes primes;
        public int[][] factorials;      
        public int[] d;
        public int[] factors;

        public void SetData () { 

            primes = new Primes(1000);
            factors = new int[]{2, 5};

            factorials = new int[N+1][];
            factorials[0] = new int[]{0,0};
            factorials[1] = new int[]{0,0};
            
            for (int i =2; i<=N; i++) {
                factorials[i] = new int[factors.Length];
                for (int j=0; j<factors.Length; j++) {
                    factorials[i][j] = CountFactorialFactorization(i, factors[j]);
                }
            }

            d = new int[factors.Length];
            primes.PrimeFactorization_ArrIdx(multiple, ref d);
            d = new int[]{d[0], d[2]};
        }

        public void Solve () {

            long count = 0;
            // This should ensure a>=b>=c
            for(int a = N; a>(int)Math.Ceiling((double)N/3); a--) {
                
                int b = Math.Min(N-a, a);
                int c = N-a-b;
                
                while (c <= b) {

                    if (factorials[N][0] - factorials[a][0] - factorials[b][0] - factorials[c][0] - d[0] >= 0 &&
                        factorials[N][1] - factorials[a][1] - factorials[b][1] - factorials[c][1] - d[1] >= 0) {
                        
                        if (a == b && b == c) {count += 1;}
                        else if (a == b || b == c) {count += 3;}
                        else {count += 6;}
                    } 

                    b--;
                    c++;
                }
            } 
            Console.WriteLine($"Final Count: {count}");
        }

        public int CountFactorialFactorization(int factorial, int primeFactor) {
            int count = 0;
            int sumcount = 0;
            int index = 1;
            do {
                count = (int)Math.Floor((double)factorial/Math.Pow(primeFactor, index));
                sumcount += count;
                index += 1;
            } while (count > 0);
            return sumcount;
        }
    }
}