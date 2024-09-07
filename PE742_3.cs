using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectEuler {
    public class PE742_3 : ISolve {

        // PROBLEM DEFINITION
        public const int sides = 1000; 

        // SOLVING PARAMETERS
        public const int baseSegmentCount = 200; // (sides/4) -1 - baseSegments equals the remainder to be calculated.
        public const int generations = 5000; // number of generations we will evaluate
        public static int children = 5;  // number of children each parent will have
        public const double maxSegmentToConsider = 1.07D; //  as function of max segment needed when sequential
        public static int maxPopulation = 2000;  
        public static int minMutations = 1; 
        public static int maxMutations = 5; 


        // GLOBALS           
        public Primes primes;       
        public static Random rnd = new Random();
        public static Segment[] baseSegments;
        public static Segment[] auxillarySegments;
                
        public void SetData() {
            primes = new Primes(sides);
            List<Segment> segments = new List<Segment>();
            segments.Add(new Segment(1,1));

            for(int y=1; y<=sides/8; y++) {
                for(int x=y+1; x<=sides/8; x++) {
                    if (primes.IsReducable(x, y)) {continue;}
                    segments.Add(new Segment(x, y));
                    segments.Add(new Segment(y, x));
                }
            }

            segments.Sort(new SegmentSort_Length()); 

            // Build base segments
            int j=0;
            baseSegments = new Segment[baseSegmentCount];
            while(j < baseSegmentCount ) {
                baseSegments[j] = new Segment(segments[j].points.Item1, segments[j].points.Item2);
                j++;
            }
            
            // build auxillary segments
            var auxSegList = new List<Segment>();
            while(segments[j].length < segments[(sides/4)].length * maxSegmentToConsider) {
                auxSegList.Add(new Segment(segments[j].points.Item1, segments[j].points.Item2));
                j++;
            }
            auxillarySegments = auxSegList.ToArray();
        }

        public void Solve() {
            
            // Create our initial parent.
            var starting = RandomPositions((sides/4)-1-baseSegmentCount, auxillarySegments.Length);
            
            // Try seeding from optimized.
            //var starting = new int[(sides/4)-1-baseSegmentCount];
            //for(int i=0; i<starting.Length; i++) { starting[i] = i; }

            var OG = new Polygon(starting);
           
            var simulate = new BreedPolygons();               
            simulate.Optimize(ref OG, generations, children);

            Console.WriteLine($"Min Area Found: {OG.Area}");
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

        public class Polygon {       
            private double calculatedArea = -1D;     
            public int[] auxillarySegmentIndices;                        
            public Polygon() {
                auxillarySegmentIndices = new int[(sides/4) - 1 - baseSegmentCount];
            } 
            public Polygon(int[] auxSegs) {
                auxillarySegmentIndices = auxSegs;              
            }

            public bool SetAuxSegment(int idx, int val) {
                if (auxillarySegmentIndices.Contains(val)) { return false; }
                
                calculatedArea = -1D; // Reset area so cached value is not taken again.
                auxillarySegmentIndices[idx] = val;
                return true;
            }

            public double Area {
                get {
                    if ( calculatedArea >= 0D) {return calculatedArea;}
                    var arc = Arc;

                    // now calculate area. This is specific to N divisible by 8.
                    int arcMaxIdx = arc.GetLength(0)-1;
                    double maxY = (double)arc[arcMaxIdx,1] + 0.5D;
                    double area = 0.5D * maxY;
                    for(int m=1; m<=arcMaxIdx; m++) {
                        area += (double)((double)(arc[m,0] - arc[m-1,0]) * ((double)maxY - (double)(arc[m,1] + arc[m-1,1])/2 ));
                    }
                    return area*4D;
                }                           
            }   

            
            private int[,] Arc {
                get {
                    var segList = new SortedList<double, Segment>((sides/4)-1);
                    for(int i=0; i<baseSegments.Length; i++) {
                        segList.Add(baseSegments[i].slope, baseSegments[i]);
                    }                   
                    for(int i=0; i<auxillarySegmentIndices.Length; i++) {
                        segList.Add(auxillarySegments[auxillarySegmentIndices[i]].slope, auxillarySegments[auxillarySegmentIndices[i]]);
                    }
                    Segment[] segs = segList.Values.ToArray();

                    //segs.Sort(new SegmentSort_Slope());

                    int[,] arc = new int[(sides/4),2];
                    int x=0; int y=0;

                    int j=0;
                    do {
                        arc[j,0] = x;
                        arc[j,1] = y;
                        if (j >= segs.Length) {break;}
                        x += segs[j].points.Item1;
                        y += segs[j].points.Item2;
                        j++;
                    } while (true);
                    return arc;
                    
                }
            }   

            public Polygon Copy() {
                Polygon rtn = new Polygon();
                for(int i=0; i<auxillarySegmentIndices.Length; i++) {
                    rtn.auxillarySegmentIndices[i] = this.auxillarySegmentIndices[i];
                }
                return rtn;
            } 

            public bool Equals(Polygon compareTo) {
                // Only need to sort auxillary segment indices since base segments are the same.
                Array.Sort(this.auxillarySegmentIndices);
                Array.Sort(compareTo.auxillarySegmentIndices);
                for(int i=0; i<this.auxillarySegmentIndices.Length; i++) {
                    if (this.auxillarySegmentIndices[i] != compareTo.auxillarySegmentIndices[i] ) {return false; }
                }
                return true;
            }
        }

        public class PolygonSort_Area : IComparer<Polygon> {
            public int Compare(Polygon a, Polygon b) {
                if (a.Area == b.Area) { return 0; }
                if (a.Area > b.Area) { return 1;}
                return -1;
            }
        }

        public class BreedPolygons : GeneticAlgorithm<Polygon> {
            // Here, we expect to have so many sides that we will assume exist on any possibly solution.
            // These would include segments such as 1,1, 2,1, etc and other shorter length sides and their inverses.
            // Next the polygon will have so many other sides that are composed of the remaining sides.
            // Thus, a polygon is really defined as a set of base segments along with auxillary segments
     
            //public BreedPolygons() { }

            public override void CreateOffspring(ref List<Polygon> pop, int children) {
                int swapsides;
                int [] auxSegs, auxPos;
                Polygon child;
                pop.Capacity = (pop.Count + 1) * children;

                int parentCount = pop.Count;
                for(int p=0; p<parentCount;p++) {

                    for(int i=0; i<children; i++) {
                        
                        child = pop[p].Copy();
                        
                        swapsides = rnd.Next(minMutations,maxMutations);
                        auxPos = RandomPositions(swapsides, (sides/4) - 1 - baseSegments.Length);
                        auxSegs = RandomPositions(swapsides, auxillarySegments.Length - 1, pop[p].auxillarySegmentIndices);                       

                        for(int j=0; j<auxSegs.Length; j++) {
                            child.SetAuxSegment(auxPos[j], auxSegs[j]);
                        }
                        pop.Add(child);                    
                    }
                }
            }

            public override void EvaluateDuplicates(ref List<Polygon> pop) {
                
                DateTime dtmStart = DateTime.Now;
                pop.Sort(new PolygonSort_Area());
                DateTime dtmSort = DateTime.Now;
                int countRemoved = 0;

                int i=1;
                while (i < pop.Count) {
                    if( pop[i].Area == pop[i-1].Area ) { // Area should be already evaluated and is fastest.
                        // Need to investigate further.
                        if (pop[i].Equals(pop[i-1])) { 
                            pop.RemoveAt(i);
                            countRemoved++;
                        }
                        else { i++; }
                    } else {
                        i++;
                    }
                }
                DateTime dtmEnd = DateTime.Now;

                TimeSpan tsSort = dtmSort - dtmStart;
                TimeSpan tsDup = dtmEnd - dtmSort;
                Console.WriteLine($"Sorting: {tsSort.TotalSeconds}, Duplicates: {tsDup.TotalSeconds}, Removed: {countRemoved}");
            }

            public override void EvaluateSurvival(ref List<Polygon> pop) {
                // For now, just consider the top maxPopulation.
                Console.WriteLine($"Min Area found: {pop[0].Area}");
                if (pop.Count < maxPopulation) {return;}
                pop.RemoveRange(maxPopulation, pop.Count - maxPopulation);
            }
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
                if (a.slope == b.slope) { throw new Exception($"DUPLICATE SLOPE FOUND! Segment a: ({a.points.Item1},{a.points.Item2}),Segment b: ({b.points.Item1},{b.points.Item2})"); }
                if (a.slope > b.slope) { return 1;}
                return -1;
            }
        }

        public static int[] RandomPositions(int count, int maxVal, int[] excludeVals = null) {
            int[] rtn = new int[count];
            int pos = rnd.Next(0,maxVal);

            for(int i=0; i<count; i++) {                    
                pos = rnd.Next(0,maxVal);
                while (rtn.Contains(pos) || (excludeVals != null && excludeVals.Contains(pos))) { pos = rnd.Next(0,maxVal); }
                rtn[i] = pos;
            }
            return rtn;
        }
    }
}