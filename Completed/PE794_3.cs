using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE794_3 : ISolve {

        public const int maxSegs = 17;
           
        public Primes primes;       
        public static int commonDivisor;
            
        public static SortedList<int, SortedList<int, int>> potentialVals;
        public static int[] vals;

        public static HashSet<int>[][] candidates;
        public static Tuple<int, int>[][] ranges;

        public Solution bestSoln;
        public int minSoln;
     
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

            commonDivisor = 10; // multiplying to ensure our +-1 for range exclusion doesn't cause issues on lower values.
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
                    ranges[k][n] = new Tuple<int, int>(lowerBound, upperBound-1);
                                        
                    for(int i=0; i<vals.Length; i++) {
                        if(lowerBound <= vals[i] && vals[i] <= upperBound) {
                            candidates[k][n].Add(vals[i]);
                        }
                    }
                }
            } 

            minSoln = maxSegs * commonDivisor; // Int32.MaxValue;
        }

        public void Solve() {
                  
            Solution initSoln = new Solution(maxSegs);
            //initSoln.AssignSeq(0); // Value 0 will be last to be added as described below. WRONG YOU IDIOT. LEFT SO YOU REMEMBER.

            FindMin(initSoln);

            Console.WriteLine($"Min: {(double)minSoln/commonDivisor}");
   
        }

        private void FindMin(Solution soln) {

            if (soln.partitions <= 2) {
                if (soln.Total < minSoln) {
                    minSoln = soln.Total;
                    bestSoln = soln.Copy();
                }
                return;
            }

            var testSolns = PotentialSolns(soln);

            foreach(Solution potentialSoln in testSolns) {

                FindMin(potentialSoln);
            }
        }

        public class Solution {

            public int[] solutionValIdx; // value = vals[solutionValIdx]/commonDivisor
            public int[] solutionValRangeLower;
            public int[] solutionValRangeUpper;           
            public int[] solutionSeq; // sequence of removal. Any values > 0 have already been "removed"
            public int partitions;

            public Solution() {}

            public Solution(int solnCount) {
                
                partitions = solnCount;
                solutionValIdx = new int[solnCount];
                solutionValRangeLower = new int[solnCount];
                solutionValRangeUpper = new int[solnCount];
                solutionSeq = new int[solnCount];

                // Naive initial solution starts with the lowest value at each point. eg, the lower bound of each range
                // We know that this is the smallest possible solution and may only shift values upwards.
                for(int i=0; i<maxSegs; i++) {
                    var idx = Array.IndexOf(vals, ranges[maxSegs][i].Item1);              
                    solutionValIdx[i] = idx;

                    solutionValRangeLower[i] = ranges[maxSegs][i].Item1;
                    solutionValRangeUpper[i] = ranges[maxSegs][i].Item2; 
                }         
            }

            public Solution Copy() {
                
                var copy = new Solution();

                copy.solutionValIdx = new int[solutionValIdx.Length];
                copy.solutionValRangeLower = new int[solutionValRangeLower.Length];
                copy.solutionValRangeUpper = new int[solutionValRangeUpper.Length];
                copy.solutionSeq = new int[solutionSeq.Length];
                copy.partitions = this.partitions;
                
                for(int i=0; i<solutionValIdx.Length; i++) {
                    copy.solutionValIdx[i] = solutionValIdx[i];
                    copy.solutionValRangeLower[i] = solutionValRangeLower[i];
                    copy.solutionValRangeUpper[i] = solutionValRangeUpper[i];
                    copy.solutionSeq[i] = solutionSeq[i];
                }  

                return copy;             
            }

            public bool AssignSeq(int solnSeqIdx) { // return false if we have a range / remainder mismatch (ie, something is wrong.)
                          
                solutionSeq[solnSeqIdx] = partitions;
                partitions--;

                // Restrict covered ranges as applicable.
                int partitionIdx = 0;
                for(int i=0; i<solutionSeq.Length; i++) {
                    if( solutionSeq[i] != 0 ) {continue;}

                    solutionValRangeLower[i] = Math.Max(solutionValRangeLower[i], ranges[partitions][partitionIdx].Item1);
                    solutionValRangeUpper[i] = Math.Min(solutionValRangeUpper[i], ranges[partitions][partitionIdx].Item2);   

                    if (solutionValRangeLower[i] - 1 > solutionValRangeUpper[i]) {  // TODO: 1 off?
                        return false;
                    }

                    partitionIdx++;                 
                }
                    
                return true; // ( partitionIdx == partitions ) ;
            }
            
            public int Total
            {
                get {
                    int rtn = 0;
                    for(int i=0; i<solutionValIdx.Length; i++) {
                        rtn += vals[solutionValIdx[i]];
                    }
                    return rtn;
                }
            }
            public double[,] SolutionRaw {
                get {
                    var rtn = new double[solutionValIdx.Length,2];
                    for(int i=0; i<solutionValIdx.Length; i++) {
                        rtn[i,0] = (double)solutionSeq[i];
                        rtn[i,1] = (double)vals[solutionValIdx[i]]/commonDivisor;
                    }
                    return rtn;
                }
            }
        }

        private List<Solution> PotentialSolns(Solution solnRoot) {
           
            // First evaluate where the current solution lives in terms of the new partition boundaries.      
            var evalPartitions = solnRoot.partitions-1;           
            var partToIdx = new SortedSet<int>[evalPartitions];
        
            for(int i=0; i<evalPartitions; i++) {partToIdx[i] = new SortedSet<int>(); } 

            int pIdx = 0;              
            for(int sIdx = 0; sIdx<solnRoot.solutionSeq.Length; sIdx++) {
                if(solnRoot.solutionSeq[sIdx] != 0) { continue; }

                while ( solnRoot.solutionValRangeLower[sIdx] > ranges[evalPartitions][pIdx].Item2 ) { pIdx++; }

                partToIdx[pIdx].Add(sIdx);
            }           

            var slns = new List<Solution>();
     
            // if any partitions don't have any available values, check if we can perform a shift.
            int lastMult = -1;
            for(int i=0; i<partToIdx.Length; i++) {

                if (partToIdx[i].Count > 1) {
                    lastMult = i;

                } else if(partToIdx[i].Count == 0) { 
                    
                    // no places to "steal" and shift from. 
                    if ( lastMult < 0 ) { 
                        return slns;
                    } 

                    // shift value on the left to the right.
                    for( int shiftIdx = lastMult; shiftIdx<i; shiftIdx++) {

                        int seqPartTo = shiftIdx+1;
                        int seqToSteal = partToIdx[shiftIdx].Max;
                        var origVal = solnRoot.solutionValIdx[seqToSteal];
                        var newVal = origVal;
                        
                        do { 
                            newVal ++; 
                        } 
                        while( vals[newVal] < ranges[evalPartitions][seqPartTo].Item1 );

                        if ( vals[newVal] > solnRoot.solutionValRangeUpper[seqToSteal]) { 
                            return slns; // can't push anything into the valid range.
                        }
                       
                        solnRoot.solutionValIdx[seqToSteal] = newVal;
                        solnRoot.solutionValRangeLower[seqToSteal] = ranges[evalPartitions][seqPartTo].Item1;
                        
                        partToIdx[shiftIdx+1].Add(seqToSteal);
                        partToIdx[shiftIdx].Remove(seqToSteal);      

                    }

                    lastMult = -1;
                }                             
            }

            Solution testSoln;
            for(int i=0; i<partToIdx.Length; i++) {
                if(partToIdx[i].Count <= 1) {continue;}

                foreach( var chk in partToIdx[i] ) {

                    testSoln = solnRoot.Copy();
                    if (testSoln.AssignSeq(chk)) { slns.Add(testSoln); }
                }

            }

            // Investigate bumping any places with multiple values into the next partition. Surely not more than one partition?
            for(int i=0; i<partToIdx.Length-1; i++) {

                if (partToIdx[i].Count <= 1) { continue; }
                
                int seqToSteal = partToIdx[i].Max;
                var origVal = solnRoot.solutionValIdx[i];
                var newVal = origVal;
                    
                do { newVal ++;  } 
                while( vals[newVal] < ranges[evalPartitions][i+1].Item1 );

                if ( vals[newVal] > solnRoot.solutionValRangeUpper[seqToSteal]) {  break; }// can't push anything into the next valid range.

                solnRoot.solutionValIdx[seqToSteal] = newVal;
                solnRoot.solutionValRangeLower[seqToSteal] = ranges[evalPartitions][i+1].Item1;

                partToIdx[i+1].Add(seqToSteal);
                partToIdx[i].Remove(seqToSteal);  

                foreach( var chk in partToIdx[i+1] ) {

                    var slnAdd = solnRoot.Copy();                   
                    if (slnAdd.AssignSeq(chk)) { slns.Add(slnAdd); }              
                }                                        
            } 

            return slns;                                         
        }
    }
}