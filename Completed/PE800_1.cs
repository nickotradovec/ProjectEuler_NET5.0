using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE800_1 : ISolve {

        public const int maxValBase = 800;
        public Primes primes;
                
        public void SetData() {

            primes = new Primes(10*maxValBase);        
        }

        public void Solve() {

            BigInteger max = BigInteger.Pow(maxValBase, maxValBase);
            long count = 0;
            long pCount = 0;

            int pIdx = 0;
            int qIdx = 0;

            int p, q;
            int qMax;
            
            while(pIdx + 1 < primes.lstPrimes.Count) {  

                pCount = 0; 
                qMax = 0;

                p = (int)primes.lstPrimes[pIdx];
                if ( p > maxValBase ) { 
                    break; 
                }
                

                qIdx = pIdx + 1;                
                while ( qIdx < primes.lstPrimes.Count ) { 
                    q = (int)primes.lstPrimes[qIdx];

                    if ( BigInteger.Multiply(BigInteger.Pow((BigInteger)q, p), BigInteger.Pow((BigInteger)p, q)) > max) { 
                        break; 
                    } 
                    
                    qMax = q;
                    pCount += 1;
                    qIdx++;
                }
                
                if (pCount > 0) { 
                    //Console.WriteLine($"pIdx: {pIdx},\tp:{p},\tqMax:{qMax},\tpCount:{pCount},\tC/QP:{(double)maxValBase/qMax/p}");                  
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