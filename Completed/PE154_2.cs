using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE154_2 : ISolve {

        public const int N = 200000; //10; // 
        public const long multiple = 120; // 1000000000000;
        public Primes primes;
        public SortedDictionary<int,int>[] factorials;
        public SortedDictionary<int,int> divisor;

        public void SetData () { 
            
            primes = new Primes(N);
            
            factorials = new SortedDictionary<int,int>[N+1];
            factorials[0] = new SortedDictionary<int,int>{};
            factorials[1] = new SortedDictionary<int,int>{};

            for (int i =2; i<=N; i++) {
                factorials[i] = Multiply(factorials[i-1], i);
            }

            divisor = primes.PrimeFactorization_SD(multiple);
        }

        public void Solve () {
            // for enforcing sequencing, we wnat i >= j >= k
            int k;
            int termsCounted = 0;
            int divisibleCount = 0;

            for(int i=N; i>=Math.Floor((double)N/3); i-- ) {
                for(int j=(N-i); j >= 0; j-- ) {
                    
                    k = N - i - j;
                    if (k > i || k > j) { continue; }
                    
                    if (IsDivisible(Divide(factorials[N], Multiply(factorials[i], factorials[j], factorials[k])), divisor)) {
                        divisibleCount += 3;
                    }
                    termsCounted += 1;
                }
            }

            Console.WriteLine($"Terms to count: {termsCounted}");
        }

        public bool IsDivisible(    SortedDictionary<int, int> numerator,
                                    SortedDictionary<int, int> divisor) {

            // divisor cannot contain                            
            foreach(KeyValuePair<int, int> kvp in numerator) {
                if ( !divisor.ContainsKey(kvp.Key) || divisor[kvp.Key] > kvp.Value) {return false;}
            }
            return true;
        }

        public SortedDictionary<int, int> Multiply( SortedDictionary<int, int> num1, 
                                                    SortedDictionary<int, int> num2,
                                                    SortedDictionary<int, int> num3) {
            return Multiply(num3, Multiply(num2, num1));
        }

        public SortedDictionary<int, int> Multiply( SortedDictionary<int, int> num1, 
                                                    SortedDictionary<int, int> num2) {
            // num2 should be largest to minimize redim
            foreach (KeyValuePair<int, int> kvp in num1) {
                if (num2.ContainsKey(kvp.Key)) {
                    num2[kvp.Key] += kvp.Value;
                } else {
                    num2.Add(kvp.Key, kvp.Value);
                }
            }
            return num2;
        }

        public SortedDictionary<int, int> Multiply( SortedDictionary<int, int> num1, 
                                                    long num2) 
        {
            return Multiply(primes.PrimeFactorization_SD(num2), num1);
        }

        public SortedDictionary<int, int> Divide(   SortedDictionary<int, int> num1, 
                                                    SortedDictionary<int, int> num2) {
            foreach (KeyValuePair<int, int> kvp in num1) {
                if (num2.ContainsKey(kvp.Key)) {
                    num2[kvp.Key] -= kvp.Value;
                } else {
                    num2.Add(kvp.Key, (-1) * kvp.Value);
                }
            }
            return num2;
        }
    }
}