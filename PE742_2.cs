using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE742_2 : ISolve {

        public const int sides = 1000;     
        public Primes primes;       
        public List<Segment> segments = new List<Segment>();
        double minArea;
        
        public void SetData() {
            primes = new Primes(sides);

            for(int y=1; y<=sides/8; y++) {
                for(int x=y+1; x<=sides/8; x++) {
                    if (primes.IsReducable(x, y)) {continue;}
                    segments.Add(new Segment(x, y));
                }
            }

            minArea = 10000000000D;            
        }

        public void Solve() {
            
            // grab segments in order of ascending length until we have our necessary count
            var minSegments = new List<Segment>((sides/4)-1);
            
            segments.Sort(new SegmentSort_Length());
            int i=0;
            while(i < (sides/8) - 1) {
                minSegments.Add(segments[i]);
                i++;
            }
            minSegments.Add(new Segment(1, 1));
            Reflect(ref minSegments);

            int testQuantity = 20; // this means highest 10 positions will be substituted with lowest 20 available values
            var tests = ChooseFour(testQuantity*2);// ChooseThree(testQuantity*2);

            List<Segment> testSegs;
            int minSwapIndex = sides/8 - 1 - 4;
            int[,] arc;
            double area;

            foreach(int[] test in tests) {
                if ( test == null) {break;}
                
                testSegs = minSegments.ConvertAll(seg => new Segment(seg.points.Item1, seg.points.Item2));
                for(int j=0; j<test.Length; j++) {
                    testSegs[minSwapIndex + j] = segments[minSwapIndex+test[j]];
                }

                Reflect(ref testSegs);
                testSegs.Sort(new SegmentSort_Slope());
                arc = Arc(testSegs);
                area = Area(arc);
                if (area < minArea) {
                    Console.WriteLine($"Min area: {area} found at vals: ({test[0]},{test[1]},{test[2]})");
                    minArea = area;
                }
            }


            Console.WriteLine(minArea);               
        }

        public int[,] Arc(List<Segment> segments) {

            // form useful points for the arc.
            int[,] arc = new int[(sides/4),2];
            int x=0; int y=0;

            int j=0;
            do {
                arc[j,0] = x;
                arc[j,1] = y;
                if (j >= segments.Count) {break;}
                x += segments[j].points.Item1;
                y += segments[j].points.Item2;
                j++;
            } while (true);
            return arc;
        }

        public void Reflect(ref List<Segment> segments) {

            int quantity = sides/8;
            Segment reflect;
            var blnAdd = (segments.Count <= quantity);

            for(int k=1; k<quantity; k++) {
                reflect = segments[quantity - 1 - k];
                if (blnAdd) {
                    segments.Add(new Segment(reflect.points.Item2, reflect.points.Item1));
                } else {
                    segments[quantity+k-1] = new Segment(reflect.points.Item2, reflect.points.Item1);
                }
            }
        }

        public double Area(int[,] arc) {

            // now calculate area. This is specific to N divisible by 8.
            int arcMaxIdx = arc.GetLength(0)-1;
            double maxY = (double)arc[arcMaxIdx,1] + 0.5D;
            double area = 0.5D * maxY;
            for(int m=1; m<=arcMaxIdx; m++) {
                area += (double)((double)(arc[m,0] - arc[m-1,0]) * ((double)maxY - (double)(arc[m,1] + arc[m-1,1])/2 ));
            }
            return area*4D;
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

        public class SegmentSort_Length : IComparer<Segment> {
            public int Compare(Segment a, Segment b) {
                if (Math.Round(a.length, 12) > Math.Round(b.length, 12)) { return 1;}
                if (Math.Round(a.length, 12) == Math.Round(b.length, 12)) { return 0;}
                return -1;
            }
        }
        public class SegmentSort_Slope : IComparer<Segment> {
            public int Compare(Segment a, Segment b) {
                if (a.slope > b.slope) { return 1;}
                return -1;
            }
        }

        public int[][] ChooseThree(int quantity) {
            var combinations = new int[quantity*(quantity-1)*(quantity-2)][];
            int idx = 0;
            for(int i=0; i<quantity; i++) {
                for(int j=i+1; j<quantity; j++) {
                    for(int k=j+1; k<quantity; k++) {                   
                        combinations[idx] = new int[]{i,j,k};
                        idx++;
                    }
                }
            }
            return combinations;
        }

        public int[][] ChooseFour(int quantity) {
            var combinations = new int[quantity*(quantity-1)*(quantity-2)*(quantity-3)][];
            int idx = 0;
            for(int i=0; i<quantity; i++) {
                for(int j=i+1; j<quantity; j++) {
                    for(int k=j+1; k<quantity; k++) {  
                        for(int l=k+1; l<quantity; l++) {                      
                            combinations[idx] = new int[]{i,j,k,l};
                            idx++;
                        }
                    }
                }
            }
            return combinations;
        }
    }
}