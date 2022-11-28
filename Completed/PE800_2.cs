using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE800_2 : ISolve {

        public const int maxValBase = 800800;
        public Primes primes;
                
        public void SetData() {

            primes = new Primes(20*maxValBase);        
        }

        public void Solve() {

            //BigInteger max = BigInteger.Pow(maxValBase, maxValBase);
            long count = 0;
            long pCount = 0;

            int pIdx = 0;
            int qIdx = primes.lstPrimes.Count-1;
            int p, q;
            int qMax;
            
            while(pIdx + 1 < primes.lstPrimes.Count) {  

                pCount = 0; 
                qMax = 0;

                p = (int)primes.lstPrimes[pIdx];
                if ( p > maxValBase ) { 
                    break; 
                }
                                         
                while ( maxValBase - (Math.Pow((double)p, (double)primes.lstPrimes[qIdx]/maxValBase) * Math.Pow((double)primes.lstPrimes[qIdx], (double)p/maxValBase)) < 0)  { 
                    qIdx--;
                }

                qIdx += 1;
                qMax = (int)primes.lstPrimes[qIdx];
                pCount = qIdx - pIdx - 1;
                
                if (pCount > 0) { 
                    //Console.WriteLine($"pIdx: {pIdx},\tp:{p},\tqMax:{qMax},\tpCount:{pCount}");                  
                } else {
                    break;
                }
                
                count += pCount;
                pIdx++;
            }

            Console.WriteLine(count);
        }

    }
}