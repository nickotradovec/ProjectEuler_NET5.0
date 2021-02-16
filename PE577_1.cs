using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE577_1 : ISolve {

        private const int nMin = 3;
        private const int nMax = 12345;

        private long[] tFunction = new long[nMax+1];

        public void SetData () {
            
            // Set Triangle Number (T) Values
            int tPrevious = 0;
            for(int i=0; i<=nMax; i++) {
                tPrevious = tPrevious + i;
                tFunction[i] = tPrevious;
            }

        }

        public void Solve () {

            long sum = 0;

            for (int currentSize = nMin; currentSize<=nMax; currentSize++) {

                // Define the slope of the hexagon be WRT the triangle itself.
                // Hexagons that have three edges parallel with the triangle itself will have a 'slope' of zero and are counted as such:
                int i = 1; 
                long added0 = 0;           
                do {                
                    added0 = T(currentSize - (3 * i) + 1); 
                    sum += added0;
                    i += 1;
                } while(added0 > 0);

                // Now for all potential slopes:
                for(int x=1; x<=(currentSize/3); x++) {
                    for( int y=1; y<=(currentSize/3); y++) {

                    sum += T(currentSize - (3 * (x + y)) + 1);
                    }
                }
                Console.WriteLine(currentSize);
            }
        
            Console.WriteLine(sum);

        }

        public bool GCDOver1(int val1, int val2) {
            // Lazy GCD. Improve later
            for(int i=2; i<= Math.Min(val1, val2); i++) {
                if ((val1 % i == 0) && (val2 % i == 0)) { return true; } 
            }
            return false;
        }

        public long T(int val) {
            if (val<0){return 0;}
            return tFunction[val];
        }
    }
}