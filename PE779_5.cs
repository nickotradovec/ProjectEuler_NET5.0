using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE779_5 : ISolve {

        public Primes primes;
        public const int k = 1;
        public const long maxTest = 1000000000; // Int64.MaxValue;
        //public SortedList<long, double> testRecord;
                
        public void SetData() {

            primes = new Primes(20000000);    
            //testRecord = new SortedList<long, double>(1000);

        }

        public void Solve() {

            double sum = 0;
            long p;
            double toAdd;

            for (int i = 0; i<400; i++) { //  // idx<primes.lstPrimes.Count
                
                p = primes.lstPrimes[i];

                //toAdd = (((double)p/(p-1))-(double)1-((double)1/p)) / ((double)p * pMult);

                /*toAdd = 0;
                for(int e=2; e<100; e++) {
                    toAdd += (double)1/Math.Pow(p, e); // (double)1/Math.Pow(p, e)
                }
                toAdd /= ((double)p * pMult); */

                toAdd = 0;

                int count = 1; // 0
                while (primes.lstPrimes[count+i] < Math.Pow(p, 2)) {
                    count ++;
                }

                toAdd = (((double)p/(p-1))-(double)1-((double)1/p)) * ((double)count / ((Math.Pow(p, 2))*(p-1)));

                Console.WriteLine($"p:{p},\tf:{toAdd}");
                sum += toAdd;
            }

            Console.WriteLine(sum);
        }
    }  
}