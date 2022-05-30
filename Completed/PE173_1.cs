using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;


namespace ProjectEuler {
    public class PE173_1 : ISolve {

        // GLOBALS
        const int max = 1000000;

        public void SetData() {

        }

        public void Solve() {

            int rtn = 0;
            int maxT, maxT_Count;

            for( int l = 3; l < (max/4)+4; l++) {

                maxT = (int)Math.Floor((double)(l-1)/2);

                if (Math.Pow(l, 2) > max) {
                    maxT_Count = (int)Math.Floor(0.5 * (l - Math.Sqrt(Math.Pow(l, 2) - max))); 
                } else {
                    maxT_Count = maxT;
                }

                rtn += Math.Min(maxT, maxT_Count);
            }

            Console.WriteLine($"Count: {rtn}");
        }
    }
}