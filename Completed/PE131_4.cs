using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE131_4 : ISolve {     

        // There are some prime values, p, for which there exists a positive integer, n, such that the expression n^3 + p*n^2 is a perfect cube

        public const int max = 1000000;
        public int maxN;
        public Primes mPrm;

        public void SetData () {            

            maxN = (int)(max*10) + 1;;
            mPrm = new Primes(maxN); 

        }

        public void Solve () {

            int answer = 0;
            int lastPrime = 1;
            int x=2;
            bool factorSuccessful;

            while(lastPrime < max) {
              
                //var xFact = mPrm.PrimeFactorization_SD(x);

                factorSuccessful = false;
                var xFact = mPrm.PrimeFactorization_SD(x, (int)Math.Ceiling(Math.Sqrt(lastPrime))+10, out factorSuccessful);

                if (factorSuccessful) {

                    // empirically, we know that x > n > (x - p)
                    // because we don't yet have p, we can use the prior prime.
                    var divisors = Divisors(GetMaxDivisor(xFact), x-lastPrime, x);
         
                    long pTest;
                    foreach (var div in divisors) {
                        pTest = x3OverN2MinusN(xFact, div);

                        if (pTest >= lastPrime && mPrm.IsPrime(pTest)) {
                            Console.WriteLine($"x={x},\t n={NumericalValue(div)},\t prime={pTest},\t xFact:{PrintFactors(xFact)},\t nFact:{PrintFactors(div)}");
                            lastPrime = (int)pTest;
                            if(pTest <= max) { answer ++; }                     
                            break;
                        }
                    }
                }
                x++;
            }

            Console.WriteLine($"{answer}");
            
        } 

        private SortedDictionary<int, int> GetMaxDivisor(SortedDictionary<int, int> xFact) {
                                
            SortedDictionary<int, int> n3MaxDivisor = new SortedDictionary<int, int>();

            foreach(KeyValuePair<int, int> div in xFact) {
                n3MaxDivisor.Add(div.Key, (int)Math.Floor((decimal)3*div.Value/2));
            }

            return n3MaxDivisor;
        }

        private List<SortedDictionary<int, int>> Divisors(SortedDictionary<int, int> maxDivisor, int boundL, int boundU) {

            int divisorsCount = 1;
            foreach(int divExp in maxDivisor.Values) {
                divisorsCount *= (divExp + 1);
            }

            List<SortedDictionary<int, int>> divisors = new List<SortedDictionary<int, int>>(divisorsCount);
            GetDivisors(maxDivisor, ref divisors, 0, boundL, boundU);

            return divisors;
        }

        private void GetDivisors(SortedDictionary<int, int> currentVal,  ref List<SortedDictionary<int, int>> divisors, int position, int boundL, int boundU) {

            SortedDictionary<int, int> div;
            long numVal;

            for(int i=0; i<=currentVal.ElementAt(position).Value; i++) {

                div = new SortedDictionary<int, int>();
                foreach(KeyValuePair<int, int> fct in currentVal) {div.Add(fct.Key, fct.Value); }
                div[currentVal.ElementAt(position).Key] -= i;
                numVal = NumericalValue(div);

                if(numVal < boundL) {
                    break;
                }else if(position + 1 < currentVal.Count) {
                    GetDivisors(div, ref divisors, position+1, boundL, boundU);
                } else if(numVal <= boundU) {
                    divisors.Add(div);
                }
            }          
        }

        private long x3OverN2MinusN(SortedDictionary<int, int> x, SortedDictionary<int, int> n) {

            long n3OverX2 = 1;
            foreach(KeyValuePair<int, int> factor in x) {
                n3OverX2 *= (long)Math.Pow(factor.Key, ((factor.Value*3) - (n[factor.Key]*2)));
            }

            return n3OverX2 - NumericalValue(n);
        }

        private long NumericalValue(SortedDictionary<int, int> val) {
            long rtn = 1;
            foreach (var factor in val) { rtn *= (long)Math.Pow(factor.Key, factor.Value); }
            return rtn;
        }

        private string PrintFactors(SortedDictionary<int, int> factorization) {

            string rtn = "";
            foreach(KeyValuePair<int, int> factor in factorization) {
                if(factor.Value > 0) {
                    rtn = $"{rtn}({factor.Key.ToString()}^{factor.Value.ToString()})";
                }
            }
            return rtn;
        }

        private bool AllPowers3(SortedDictionary<int, int> num) {
            foreach(var val in num.Values) {
                if (val % 3 != 0) { return false; }
            }
            return true;
        }
    }
}