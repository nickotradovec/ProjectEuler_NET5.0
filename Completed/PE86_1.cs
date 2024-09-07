using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE86_1 : ISolve {     

        public const int threshHoldCount = 1000000;
        public const int maxEvaluation = 2000;
     
        public Dictionary<long, int> counts;
        public HashSet<PythagoreanTriple> triples;
        public Primes primes;

        public void SetData () { 

            primes = new Primes(maxEvaluation*12);
            triples = new HashSet<PythagoreanTriple>(new PythComp());
            // https://www.chilimath.com/lessons/geometry-lessons/generating-pythagorean-triples/
            long a, b;

            // Generate all triples where short leg < 100, and long leg (which will become sum of other two legs) < 2*MaxSize
            for(long n = 1; n<=2*maxEvaluation; n++) {
                for(long m = n+1; Math.Pow(m,2) - Math.Pow(n,2) <= 2*maxEvaluation; m++) {

                    a = (int)(Math.Pow(m,2) - Math.Pow(n,2));
                    b = 2*n*m;
                    
                    if (primes.Reduce(ref a, ref b)) {continue;}

                    if (!primes.Reduce(ref a, ref b) && a <= (maxEvaluation*2) && b <= (maxEvaluation*2)) {
                        triples.Add(new PythagoreanTriple(a, b));
                    }
                    
                }
            }

            counts = new Dictionary<long, int>(maxEvaluation+1);
        }

        public void Solve () {
           
            foreach (PythagoreanTriple triple in triples) {
                
                EvaluateTriples(triple);
            }

            long count = 0;
            int m = 0;  
            while ( count < threshHoldCount ) {
                m++;
                if (counts.ContainsKey(m)) { count += counts[m]; }                    
            }

            Console.WriteLine(m);
        } 

        public class PythagoreanTriple {
            public long a;
            public long b;

            public PythagoreanTriple(long leg1, long leg2) {
                a = Math.Min(leg1, leg2);
                b = Math.Max(leg1, leg2);
            } 
            
            public long c {
                get { return (long)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)); }
            } 
        }
        
        public class PythComp : IEqualityComparer<PythagoreanTriple> {
            public bool Equals(PythagoreanTriple triple1, PythagoreanTriple triple2) 
            {
                return (triple1.a == triple2.a && triple1.b == triple2.b);
            }
            public int GetHashCode(PythagoreanTriple trp)
            {
                return (int)Math.Sqrt(Math.Pow(trp.a, 2) + Math.Pow(trp.b, 2));
            }
        }  

        private void EvaluateTriples(PythagoreanTriple t) {

            // int count = 0;
            // longest side will be t.a = s3
            // s1 > t.b-t.a
            // s2 = t.b-s1
            // s2 <= s3 = t.a

            // Split along long side. Both portions of hte long side must be less than max
            for(int mult = 1; mult*(t.b-t.a) <= maxEvaluation; mult++) {

                for(long s1=mult*(t.b-t.a); 2*s1 <= mult*t.b; s1++) {
           
                    if( s1 <= maxEvaluation && (mult*t.b)-s1 <= maxEvaluation && mult*t.a <= maxEvaluation ) { 
                        
                        AddOrUpdateCount(mult*t.a, 1);
                        //count++; 
                        
                    }
                }
            }
        
            // Split along short side (t.a), t.b will be the long side.
            for(int mult = 1; mult*t.b <= maxEvaluation; mult++) {

                for(long s1 = 1; 2*s1 <= mult*t.a; s1 ++) {
           
                    if( s1 <= maxEvaluation && mult*(t.a-s1) <= maxEvaluation && mult*t.b <= maxEvaluation ) { 
                        
                        AddOrUpdateCount(mult*t.b, 1);

                        //count++; 
                    }
                }
            }

            //return count;           
        } 

        private void AddOrUpdateCount(long index, int count) {

            if (!counts.ContainsKey(index)) {
                counts.Add(index, count);
            } else {
                counts[index] += count;
            }

        }
    }
}