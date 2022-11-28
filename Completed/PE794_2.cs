using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE794_2 : ISolve {

        public const int maxSegs = 11;
           
        public Primes primes;       
        public int commonDivisor;
            
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

            minSoln = commonDivisor * maxSegs;
        }

        public void Solve() {
                  
            Solution initSoln = new Solution(maxSegs);
            initSoln.AssignSeq(0); // Value 0 will be last to be added as described below.

            FindMin(initSoln);

            Console.WriteLine($"Min: {(double)minSoln/commonDivisor}");
   
        }

        private void FindMin(Solution soln) {

            if (soln.partitions <= 2) {
                if (soln.SolutionValue() < minSoln) {
                    minSoln = soln.SolutionValue();
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

            public bool Shift(int solnValIdxIdx, int solnValIdx) {

                // "Shfit" the value and recompute ranges. If fails range check, return false
                return false;

            }

            public bool AssignSeq(int solnSeqIdx) { // return false if we have a range / remainder mismatch (ie, something is wrong.)
                          
                solutionSeq[solnSeqIdx] = partitions;
                partitions--;
                
                /*
                solutionValRangeLower[solnSeqIdx] = Math.Max(solutionValRangeLower[solnSeqIdx], ranges[partitions][partIdx].Item1 + 1);
                solutionValRangeUpper[solnSeqIdx] = Math.Min(solutionValRangeUpper[solnSeqIdx], ranges[partitions][partIdx].Item2); 

                if (solutionValRangeLower[solnSeqIdx] > solutionValRangeUpper[solnSeqIdx]) { 
                    return false;
                }
                */
                
                // Restrict covered ranges as applicable.
                int partitionIdx = 0;
                for(int i=0; i<partitions; i++) {
                    if( solutionSeq[i] != 0 ) {continue;}

                    // we know our values are in increasing order and so are our partitions.
                    //while ( partitionIdx < partitions && ranges[partitions][partitionIdx].Item1 < solutionValRangeLower[i] ) { partitionIdx++; }

                    solutionValRangeLower[i] = Math.Max(solutionValRangeLower[i], ranges[partitions][partitionIdx].Item1);
                    solutionValRangeUpper[i] = Math.Min(solutionValRangeUpper[i], ranges[partitions][partitionIdx].Item2);   

                    if (solutionValRangeLower[i] > solutionValRangeUpper[i]) { 
                        return false;
                    }

                    partitionIdx++;                 
                }
                    
                return (partitionIdx == partitions-1 ) ;
            }
            
            public int SolutionValue() {
                int rtn = 0;
                for(int i=0; i<solutionValIdx.Length; i++) {
                    rtn += vals[solutionValIdx[i]];
                }
                return rtn;
            }
        }

        private List<Solution> PotentialSolns(Solution solnRoot) {
           
            // First evaluate where the current solution lives in terms of the new partition boundaries.      
            var evalPartitions = solnRoot.partitions-1;           
            var partToIdx = new SortedSet<int>[evalPartitions];
        
            for(int i=0; i<evalPartitions; i++) {partToIdx[i] = new SortedSet<int>(); } 

            /*
            int vIdx = 0;
            for(int pIdx = 0; pIdx<solnRoot.partitions; pIdx++) {
             
                while( vIdx < maxSegs && vals[solnRoot.solutionValIdx[vIdx]] <= ranges[evalPartitions][pIdx].Item2 ) {
                 
                    if(solnRoot.solutionSeq[vIdx] == 0) {

                        if ( vals[solnRoot.solutionValIdx[vIdx]] == ranges[evalPartitions][pIdx].Item2 ) {
                            // could be used for either partition. A negative will denote this and make this easier to identify.  


                            
                            // TODO : Needs to work with already defined trivial pulldown/pullups.     
                            // VALUES MAY ALREADY BE DECIDED HERE
                            partToIdx[pIdx].Add((-1)*vIdx);
                            partToIdx[pIdx+1].Add((-1)*vIdx);
                            vIdx++;   
                            break;                            
                    
                        } else {
                            partToIdx[pIdx].Add(vIdx);
                        }                  
                    }
                
                    vIdx++;
                }
            }
        
            for(int pIdx = 0; pIdx<evalPartitions; pIdx++) {
                for(int sIdx = 0; sIdx<solnRoot.solutionSeq.Length; sIdx++) {

                    if(solnRoot.solutionSeq[sIdx] != 0) { continue; }

                    // since we know we can never shift left, we are really just interested in if the lower bound is in the range of the current partition
                    //if( solnRoot.solutionValRangeLower[sIdx] == ranges[evalPartitions][pIdx].Item1 ) {
                        // the value straddles two partitions.
                        //partToIdx[pIdx].Add((-1)*sIdx);
                        //partToIdx[pIdx+1].Add((-1)*sIdx);

                    if (solnRoot.solutionValRangeLower[sIdx] > ranges[evalPartitions][pIdx].Item1 && solnRoot.solutionValRangeLower[sIdx] <= ranges[evalPartitions][pIdx].Item2) {
                        partToIdx[pIdx].Add(sIdx);
                    }

                }           
            }
            */

            int pIdx = 0;              
            for(int sIdx = 0; sIdx<solnRoot.solutionSeq.Length; sIdx++) {
                if(solnRoot.solutionSeq[sIdx] != 0) { continue; }

                while ( solnRoot.solutionValRangeLower[sIdx] > ranges[evalPartitions][pIdx].Item2 ) { pIdx++; }

                partToIdx[pIdx].Add(sIdx);
            }           

            /*
            no benifit in minimization to having a value just to the "left" of a value            
            // first need to determine a "side" for any values straddling a partition line          
            for(int pLow=0; pLow<evalPartitions-1; pLow++) {
                for(int j=0; j<partToIdx[pLow].Count; j++) {
                    
                    if(partToIdx[pLow][j] < 0) {

                        if (partToIdx[pLow].Count == 1 && partToIdx[pLow].Count > 1) {
                            // TODO: need to validate that this is a legitimate solution or let this happen later
                            partToIdx[pLow][j] = Math.Abs(partToIdx[pLow][j]);
                            partToIdx[pLow+1].Remove(ppartToIdx[pLow][j]);
                        
                        } else if (partToIdx[pLow].Count > 1 && partToIdx[pLow].Count == 1)  {
                            partToIdx[pLow].Remove(j);
                            partToIdx[pLow+1][j] = Math.Abs(partToIdx[pLow+1].IndexOf(partToIdx[pLow][j]));   

                        } else {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
            */

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

                        int seqToSteal = partToIdx[shiftIdx].Max;
                        var origVal = solnRoot.solutionValIdx[seqToSteal];
                        var newVal = origVal;
                        
                        do { 
                            newVal ++; 
                        } 
                        while( vals[newVal] < ranges[evalPartitions][i - (i-shiftIdx)].Item1 );

                        // check if still in range now. if not, return solns
                        if ( vals[newVal] > solnRoot.solutionValRangeUpper[seqToSteal] + 1 ) { return slns; }
                       
                        solnRoot.solutionValIdx[seqToSteal] = newVal;
                        solnRoot.solutionValRangeLower[seqToSteal] = ranges[evalPartitions][i - (i-shiftIdx) + 1].Item1;
                        
                        partToIdx[shiftIdx+1].Add(seqToSteal);
                        partToIdx[shiftIdx].Remove(seqToSteal);      

                    }

                    lastMult = -1;

                    /*
                    if ( i > 0 && partToIdx[i-1].Count > 1 && solnRoot.solutionValRangeUpper[partToIdx[i-1][partToIdx[i-1].Count-1]] <= ranges[evalPartitions][i].Item2 ) { 
                        var newVal = solnRoot.solutionValIdx[partToIdx[i-1][partToIdx[i-1].Count-1]];
                        do { 
                            newVal ++; 
                        } 
                        while( vals[newVal] < ranges[evalPartitions][i].Item1 );
                       
                        solnRoot.solutionValIdx[partToIdx[i-1][partToIdx[i-1].Count-1]] = newVal;
                        solnRoot.solutionValRangeLower[partToIdx[i-1][partToIdx[i-1].Count-1]] = ranges[evalPartitions][i].Item1;
                        
                        partToIdx[i].Add(partToIdx[i-1][partToIdx[i-1].Count-1]);
                        partToIdx[i-1].RemoveAt(partToIdx[i-1].Count-1);                  

                    } else if (i<evalPartitions-1 && partToIdx[i+1].Count > 1) {
                        // can we ever shift left? don't think so.
                        return slns; //throw new NotImplementedException()
                    
                    } else {
                        return slns;
                    */    
                }                             
            }

            Solution testSoln;
            for(int i=0; i<partToIdx.Length; i++) {
                if(partToIdx[i].Count <= 1) {continue;}

                foreach( var chk in partToIdx[i] ) {

                    testSoln = solnRoot.Copy();
                    if (testSoln.AssignSeq(Math.Abs(chk))) { slns.Add(testSoln); }

                }

                /* for( int j=0; j<partToIdx[i].Count; j++) {
                    testSoln = solnRoot.Copy();
                    if (testSoln.AssignSeq(Math.Abs(partToIdx[i][j]))) { slns.Add(testSoln); }
                } */
            }

            // now go through and create solutions

            return slns;
        }
    }
}