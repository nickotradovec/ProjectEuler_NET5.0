using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE808_2 : ISolve {

        private const int maxCount = 50;
        public Primes prm;

        public void SetData() {

            prm = new Primes(40000000); // set empirically to prevent out of bounds

        }

        public void Solve() {

            int currentCount = 0;
            long sum = 0;

            int digitCount = 2;
            int lb = 3; int ub = -1;

            while (digitCount < 19) // int64 may only hold up to 19 digits
            {
                var hstVals = GetVals(digitCount, lb, out ub);
                CountReversed(hstVals, ref currentCount, ref sum);
                if (currentCount >= maxCount) { break; }

                lb = ub;
                digitCount++;
            }

            Console.WriteLine($"DigitCount: {digitCount}, CurrentCount: {currentCount}, Sum: {sum}");

        }

        public HashSet<string> GetVals(int digitCount, int lb, out int ub)
        {
            var hstRtn = new HashSet<string>();
            var val = (long)(Math.Pow(prm.lstPrimes[lb], 2));
            string vals = val.ToString();
            do
            {
                hstRtn.Add(vals);
                val = (long)(Math.Pow(prm.lstPrimes[lb], 2));
                vals = val.ToString();
                lb++;

            } while (vals.Length <= digitCount);

            ub = lb - 1; // overshot by 1.
            return hstRtn;
        }

        public static void CountReversed(HashSet<string> vals, ref int currentCount, ref long sum)
        {
            var rvsd = new SortedSet<long>(); // hashset is not ordered.
            foreach (var strVal in vals)
            {
                if( !IsPalindrome(strVal) && vals.Contains(Reverse(strVal)))
                {
                    rvsd.Add(Int64.Parse(strVal));
                }
            }

            foreach(var val in rvsd)
            {
                currentCount++;
                sum += val;
                //Console.WriteLine(val);
                if (currentCount >= maxCount) { return; }
            }
        }
        public static string Reverse(string str)
        {
            var chr = str.ToCharArray();
            var chrRvs = new char[chr.Length];
            for(int i = chr.Length - 1; i>= 0; i--)
            {
                chrRvs[chr.Length - i - 1] = chr[i];       
            }
            return new string(chrRvs);
        }

        public static bool IsPalindrome(string str)
        {
            var chr = str.ToCharArray();
            for (int i = 0; i < str.Length/2; i++)
            {
                if (chr[i] != chr[chr.Length - 1- i]) { return false; }
            }
            return true;
        }

    }
}