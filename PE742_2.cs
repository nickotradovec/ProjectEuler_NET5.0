using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE742_2 : ISolve {

        public const int sides = 96;     
        public Primes primes;       
        public List<Segment> segments = new List<Segment>();
        
        public void SetData() {
            primes = new Primes(sides);

            for(int y=1; y<=sides/8; y++) {
                for(int x=y+1; x<=sides/8; x++) {
                    if (primes.IsReducable(x, y)) {continue;}
                    segments.Add(new Segment(x, y));
                }
            }

            
        }

        public void Solve() {
            
            // grab segments in order of ascending length until we have our necessary count
            var used = new List<Segment>(sides/8);
            used.Add(new Segment(1, 1));

            segments.Sort(new SetmentSort_Length());
            int i=0;
            while(i < (sides/8) - 1) {
                used.Add(segments[i]);
                i++;
            }

            // sort according to slope as that will be how they are connected.
            used.Sort(new SetmentSort_Slope());

            // form useful points for the arc.
            int[,] arc = new int[(sides/4),2];
            int x=0; int y=0;

            int j=0;
            do {
                arc[j,0] = x;
                arc[j,1] = y;
                x += used[j].points.Item1;
                y += used[j].points.Item2;
                j++;
            } while (j < used.Count);
            arc[used.Count,0] = x;
            arc[used.Count,1] = y;

            // reflecting along the diagonal,
            for(int k=1; k<used.Count; k++) {
                x += used[used.Count-k-1].points.Item2; // x and y should be reversed for hte reflection here.
                y += used[used.Count-k-1].points.Item1;
                arc[used.Count+k,0] = x;
                arc[used.Count+k,1] = y;
            }

            // now calculate area. This is specific to N divisible by 8.
            int arcMaxIdx = arc.GetLength(0)-1;
            double maxY = (double)arc[arcMaxIdx,1] + 0.5D;
            double area = 0.5D * maxY;
            for(int m=1; m<=arcMaxIdx; m++) {
                area += (double)((double)(arc[m,0] - arc[m-1,0]) * ((double)maxY - (double)(arc[m,1] + arc[m-1,1])/2 ));
            }
            area *= 4;
            Console.WriteLine($"Last point at: ({arc[arcMaxIdx,0]},{arc[arcMaxIdx,1]})");
            Console.WriteLine(area);                 
        }

        public class Segment {
            public double slope;
            public double length;
            public Tuple<int, int> points;
            public Segment(int x, int y) {
                points = new Tuple<int, int>(x, y);
                length = Math.Sqrt(Math.Pow((double)x, 2) + Math.Pow((double)y, 2));
                slope = (double)y/x;
            }            
        }

        public class SetmentSort_Length : IComparer<Segment> {
            public int Compare(Segment a, Segment b) {
                if (Math.Round(a.length, 12) > Math.Round(b.length, 12)) { return 1;}
                if (Math.Round(a.length, 12) == Math.Round(b.length, 12)) { return 0;}
                return -1;
            }
        }
        public class SetmentSort_Slope : IComparer<Segment> {
            public int Compare(Segment a, Segment b) {
                if (a.slope > b.slope) { return 1;}
                return -1;
            }
        }
    }
}