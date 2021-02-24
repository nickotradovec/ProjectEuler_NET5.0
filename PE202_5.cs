using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE202_5 : ISolve {

        const long reflections = 1000001;
        private Primes primes;
        private long C;

        public void SetData () {          

            primes = new Primes(reflections); // Can likely be reduced
            C = (reflections + 3) / 2;
        }

        public void Solve () {

            // for now, naively just store values.               
            var exclude = new HashSet<long>();
            long maxCD = 0;
            
            long i = 0;
            var e = EValue(0);
            while (e > (double)C/2) {
                if (primes.HaveCommonFactor(C, e, ref maxCD)) {exclude.Add(e);}
                i += 1;
                e = EValue(i);
            }

            i = 0;
            e = EValue(0);
            int count = 0;
            while (e > (double)C/2) {
                if (!exclude.Contains(e)) {count += 1;}
                i += 1;
                e = EValue(i);
            }

            Console.WriteLine($"Count: {2 * count}");
        } 

        public long EValue(long index) {
            return C - 1L - (index * 3);
        }
    }
}