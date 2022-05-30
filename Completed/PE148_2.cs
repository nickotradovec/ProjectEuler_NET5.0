using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using ProjectEuler;


namespace ProjectEuler {
    public class PE148_2 : ISolve {

        // GLOBALS
        const long rowsInBase7 = 33531600616;
        long[] T;

        public void SetData() {
            T = new long[]{0, 1, 3, 6, 10, 15, 21, 28};
        }

        public void Solve() {
    
            int[] number = rowsInBase7.ToString().Select(o=> Convert.ToInt32(o) - 48 ).ToArray();

            long answer = 0;
            long coefficient = 1;

            for(int i=0; i<number.Length; i++) {

                answer += coefficient * T[number[i]] * (long)Math.Pow(T[7], number.Length - i - 1);
                coefficient *= (number[i] + 1);

            }          

            Console.WriteLine($"Total Entries: {answer}");
        }   

    }
}