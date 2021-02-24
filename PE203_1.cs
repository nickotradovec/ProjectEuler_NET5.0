using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE203_1 : ISolve {

        public const int rows = 51;
        public Primes primes;
        public int[][] factorials;    
        public int maxPrimeIndex;
        public HashSet<long> added;

        public void SetData () { 

            primes = new Primes(rows);
            maxPrimeIndex = primes.lstPrimes.Count - 1;

            factorials = new int[rows+1][];
            factorials[0] = new int[]{};
            factorials[1] = new int[]{};

            int[] newVals; 
            for (int i =2; i<=rows; i++) {
                newVals = factorials[i-1].ToArray();               
                primes.PrimeFactorization_ArrIdx(i, ref newVals);
                factorials[i] = newVals;
            }

            added = new HashSet<long>();
            added.Add(1);
        }

        public void Solve () {

            long count = 1;
            for(int row=2; row<rows; row++) {

                // This should ensure a>=b>=c
                for(int a = row; a>=(int)Math.Floor((double)row/2); a--) {
                
                   int b = row-a;

                    long val = 0;
                    if(IsSquareFree(row, a, b, out val) && !added.Contains(val)) {
                        count += val;
                        added.Add(val);
                    }
                    
                } 
            Console.WriteLine($"Final Count: {count}");
            }           
        }

        public bool IsSquareFree(int row, int a, int b, out long value) {

            value = 1;
            int power;
            for(int i=0; i<factorials[row].Length; i++) {
                power = factorials[row][i] - factorials[a].GV(i) - factorials[b].GV(i);
                if(power >= 2)  {
                    return false;
                } else if (power == 1){
                    value *= primes.lstPrimes[i];
                }                        
            }
            return true;
        }

    }
}