using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE154_1 : ISolve {

        public const int N = 20;
        public const long modVal = 100;
        public void SetData () { }

        public void Solve () {

            int n = 1;
            var coeffs = new Dictionary<CoefficientKeys, long>(3, new CoefComp());
            coeffs.Add(new CoefficientKeys(1,0,0), 1);
            coeffs.Add(new CoefficientKeys(0,1,0), 1);
            coeffs.Add(new CoefficientKeys(0,0,1), 1);
            //coeffs.Add(new int[]{0,0,1}, 1);
            
            while (n < N) { 
                n += 1;
                NextExpansion(n, ref coeffs);             

                //Console.WriteLine($"n = {n}");
                //WriteCoeffs(coeffs);
            }

            WriteCoeffs(coeffs);

            int divisibleCount = 0;
            foreach(KeyValuePair<CoefficientKeys, long> kvp in coeffs) {
                if (kvp.Value % modVal == 0) {divisibleCount += 1;}
            }
            Console.WriteLine($"At N={N}, {divisibleCount} terms are divisible by {modVal}");
         
        } 

        public void WriteCoeffs(Dictionary<CoefficientKeys, long> coeffs) {
            
            foreach (KeyValuePair<CoefficientKeys, long> kvp in coeffs) {
                //Console.Write($"({kvp.Key.exp[0]},{kvp.Key.exp[1]},{kvp.Key.exp[2]}):{kvp.Value};");
                Console.Write($"{kvp.Value},");
            }
            Console.WriteLine();
        }

        public void NextExpansion(int newExpansion, ref Dictionary<CoefficientKeys, long> coeffs) {

            var newCoeffs = new Dictionary<CoefficientKeys, long>(expansionQuantity(newExpansion), new CoefComp());
                                           
            foreach( KeyValuePair<CoefficientKeys, long> kvp in coeffs) { 
             
                for ( int i=0; i<=2; i++) {

                    CoefficientKeys newKey = new CoefficientKeys(kvp.Key.exp[0], kvp.Key.exp[1], kvp.Key.exp[2]);
                    newKey.IncrementByIndex(i, 1);

                    long newCoeffsVal;
                    if (newCoeffs.TryGetValue(newKey, out newCoeffsVal)) {
                        newCoeffs[newKey] = (newCoeffsVal + kvp.Value);
                    } else {
                        newCoeffs.Add(newKey, kvp.Value);
                    }
                }              
            }
            coeffs = newCoeffs;
        }

        public int expansionQuantity(int n) {
            return (n + 1) * (n + 2) / 2;
        } 

        public class CoefficientKeys {
            public int[] exp;

            public CoefficientKeys(int x, int y, int z) {
                exp = new int[]{x, y, z};
            } 
            public void IncrementByIndex(int index, int incrementVal) {
                exp[index] += incrementVal;
            }     
        }
        public class CoefComp : IEqualityComparer<CoefficientKeys> {
            public bool Equals(CoefficientKeys a, CoefficientKeys b) 
            {
               return a.exp[0] == b.exp[0] && a.exp[1] == b.exp[1] && a.exp[2] == b.exp[2];
            }
            public int GetHashCode(CoefficientKeys a)
            {
                return (N * N * a.exp[0]) + (N * a.exp[1] + a.exp[0]);
            }
        }
    }
}