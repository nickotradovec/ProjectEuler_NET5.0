using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE794_1 : ISolve {

        public const int maxSegs = 4;
           
        public Primes primes;       
        public int commonDivisor;
            
        SortedList<int, SortedList<int, int>> potentialVals;
        public int[] vals;

        public HashSet<int>[][] candidates;
        public Tuple<int, int>[][] ranges;
                
        public void SetData() {
            
            primes = new Primes(maxSegs);
            
            var commonFactorization = new SortedDictionary<int, int>();

            // TODO: Really these could be more efficient.
            for (int i=maxSegs; i > 1; i--) {
                var PF = primes.PrimeFactorization_SD((long)i);

                foreach( KeyValuePair<int, int> kvp in PF) {
                    if (commonFactorization.ContainsKey(kvp.Key)) {
                        commonFactorization[kvp.Key] = Math.Max(kvp.Value, commonFactorization[kvp.Key]);
                    } else {
                        commonFactorization.Add(kvp.Key, kvp.Value);                       
                    }
                }
            }

            commonDivisor = 1;
            foreach( KeyValuePair<int, int> kvp in commonFactorization) {
                commonDivisor *= (int)Math.Pow(kvp.Key, kvp.Value);
            }
            Console.WriteLine($"Common Divisor: {commonDivisor}");

            potentialVals = new SortedList<int, SortedList<int, int>>();
            SortedList<int, int> slAdd;
            for(int i = 1; i<= maxSegs; i++) {
                for(int j=0; j<i; j++) {
               
                    if (potentialVals.ContainsKey(j*commonDivisor/i)) {
                        potentialVals[j*commonDivisor/i].Add(i, j);

                    } else {
                        slAdd = new SortedList<int, int>();
                        slAdd.Add(i, j);
                        potentialVals.Add(j*commonDivisor/i, slAdd);                   
                    }                  
                }               
            }   

            vals = new int[potentialVals.Count];
            int idx = 0;
            foreach( var pv in potentialVals) { 
                vals[idx] = pv.Key; 
                idx++;
            }

            candidates = new HashSet<int>[maxSegs+1][];
            ranges = new Tuple<int, int>[maxSegs+1][];
            
            int lowerBound, upperBound;           
            for(int k=1; k<=maxSegs; k++) {
                candidates[k] = new HashSet<int>[k];
                ranges[k] = new Tuple<int, int>[k];

                for(int n=0; n<k; n++) {
                    candidates[k][n] = new HashSet<int>();

                    lowerBound = (n)*commonDivisor/k;
                    upperBound = (n+1)*commonDivisor/k;
                    ranges[k][n] = new Tuple<int, int>(lowerBound, upperBound);
                                        
                    for(int i=0; i<vals.Length; i++) {
                        if(lowerBound <= vals[i] && vals[i] <= upperBound) {
                            candidates[k][n].Add(vals[i]);
                        }
                    }
                }
            } 
        }

        public void Solve() {
       
            // Naive solution starts with the lowest value at each point. eg, the lower bound of each range
            var testSolution = new int[maxSegs];
            for(int i=0; i<maxSegs; i++) {
                var idx = Array.IndexOf(vals, ranges[maxSegs][i].Item1);              
                testSolution[i] = idx;
            }

            int minSoln = maxSegs * commonDivisor;
            var testSolutionSeq = new int[maxSegs];
            var testPartitions = maxSegs;


            testPartitions -= 1;
            TestSolution(ref testSolution, ref testSolutionSeq, testPartitions, ref minSoln);

            Console.WriteLine($"Min: {(double)minSoln/commonDivisor}");
   
        }

        // solutionVals will point to the index of the value we're interested in vals
        // this should simplify shifting values later on if necessary.
        private void TestSolution(ref int[] solutionVals, ref int[] solutionSeq, int testPartitions, ref int minVal) {

            List<int>[] rangeToAvailable, solutionValsToRange;
            AvailableInRange(solutionVals, solutionSeq, testPartitions, out rangeToAvailable, out solutionValsToRange);

            // if we're down to testPartitions = 1, we're done as all values work.

            for(int i=0; i<testPartitions; i++) {
                //if ( rangeToAvailable[i].Count == 0 ) // no solutions available
                
                if ( rangeToAvailable[i].Count > 1 ) { // This is where we need to "remove" a value. Improve selection criteria?
                
                    // for now, just remove the first item.
                    solutionSeq[rangeToAvailable[i][0]] = testPartitions;
                    testPartitions --;
                    TestSolution(ref solutionVals, ref solutionSeq, testPartitions, ref minVal);
                    break;
                }
            }

        }


        // solutionVals will point to the index of the value we're interested in vals
        // this should simplify shifting values later on if necessary.

        public void AvailableInRange(   int[] solutionVals, int[] solutionSeq, int testPartitions,
                                        out List<int>[] rangeToAvailable, out List<int>[] solutionValsToRange) {

            rangeToAvailable = new List<int>[testPartitions]; 
            solutionValsToRange = new List<int>[solutionSeq.Length]; // easier to handle values that straddle a range limit?

            int s = 0;
            for(int rangeIdx = 0; rangeIdx<testPartitions; rangeIdx++) {
                rangeToAvailable[rangeIdx] = new List<int>();

                while(s < solutionSeq.Length && vals[solutionVals[s]] <= ranges[testPartitions][rangeIdx].Item2) {

                    if( solutionSeq[s] == 0 ) {
                        if(vals[solutionVals[s]] == ranges[testPartitions][rangeIdx].Item2) {
                            rangeToAvailable[rangeIdx].Add(s);
                            solutionValsToRange[s] = new List<int>(2);
                            solutionValsToRange[s].Add(rangeIdx);
                            break;

                        } else {
                            rangeToAvailable[rangeIdx].Add(s);
                            solutionValsToRange[s] = new List<int>(1);
                            solutionValsToRange[s].Add(rangeIdx);
                            s++;
                        } 

                    } else {
                        s++;
                    }              
                }
            }
        }
    }
}