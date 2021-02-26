using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE86_1 : ISolve {     

        public const int maxSize = 100;
        public HashSet<PythagoreanTriple> triples;
        public Primes primes;

        public void SetData () { 
            
            primes = new Primes((long)Math.Pow(maxSize, 2));
            triples = new HashSet<PythagoreanTriple>(new PythComp());
            // https://www.chilimath.com/lessons/geometry-lessons/generating-pythagorean-triples/
            long a, b;

            // Generate all triples where short leg < 100, and long leg (which will become sum of other two legs) < 2*MaxSize
            for(long n = 1; 2*n<=(maxSize*2); n++) {
                for(long m = n+1; Math.Pow(m,2) - Math.Pow(n,2) <= maxSize; m++) {

                    a = (int)(Math.Pow(m,2) - Math.Pow(n,2));
                    b = 2*n*m;
                    if (a > (maxSize*2) || b > (maxSize*2)) {break;}
                    if (primes.Reduce(ref a, ref b)) {continue;}

                    if (!triples.Add(new PythagoreanTriple(a, b))) {
                        throw new Exception("Attempted to add duplicate.");
                    };
                }
            }
        }

        public void Solve () {

            long count = 0; 
            long tripleCount;
            long effA, effB;          

            foreach (PythagoreanTriple triple in triples) {
                
                tripleCount = 0;
                int i = 1;
                do {
                    effA = i*triple.a;
                    effB = i*triple.b; // b should be largest.

                    // now we need to get a combination of all possible 'folds' to count.
                    // This requires x > y,z while still x < maxSize

                    //i ++;
                } while (true);

                Console.WriteLine($"Pythagorean Triple: [{triple.a},{triple.b},{triple.c}], \tCount: {tripleCount}");
                count += tripleCount;
            }

            Console.WriteLine(count);
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
    }
}