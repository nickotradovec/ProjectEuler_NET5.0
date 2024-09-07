using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE742_1 : ISolve {

        public const int sides = 96;
        public double minArea = 20000D; // Starting value should well exceed expected solutions.           
        public Primes primes;
        public Polygon minPolygon;        

        public void SetData() {
            primes = new Primes(sides);
        }

        public void Solve() {
            /*
            var test = new Polygon();
            test.vertices.Add(new Point(0,0));
            test.vertices.Add(new Point(2,1));
            test.vertices.Add(new Point(3,3));
            Console.WriteLine(test.Print());
            Console.WriteLine(test.N);
            Console.WriteLine(test.Area);
            */
            
            var test = new Polygon();
            test.vertices.Add(new Point(0,0));
            NextPolygonsHalf(test);           
            Console.WriteLine($"Min Polygon area: {minPolygon.Area}");
            Console.WriteLine($"Points: {minPolygon.Print()}");
            
        }

        public void NextPolygonsHalf(Polygon currentPolygon) {
            
            
            if (currentPolygon.N > sides/2) {

                currentPolygon.InferRemainder();
                double currentPolygonArea = currentPolygon.Area;

                if (currentPolygonArea < minArea ) {
                    //Console.WriteLine($"New min polygon found: {currentPolygon.Print()}");
                    minArea = currentPolygonArea;
                    minPolygon = currentPolygon.Copy();
                }
                return;
            }
            
            // If our current polygon has already exceed the min area, no point in exploring further.
            // if ( currentPolygon.Area > minArea) { return; }
            

            Polygon newTestPolygon;
            int yMax = 1;
            if (currentPolygon.vertices.Count > 5) { yMax = sides/8; }

            for(int y = 1; y <= yMax; y++) {
                for(int x = y; x < Math.Min(y/currentPolygon.Slope, (double)sides/8); x++) {
                                             
                    if (primes.IsReducable(x, y)) {continue;}

                    newTestPolygon = currentPolygon.Copy(x, y);
                    NextPolygonsHalf(newTestPolygon);
                }
            }
        }
        public void NextPolygons(Polygon currentPolygon) {
            
            double currentPolygonArea = currentPolygon.Area;
            if (currentPolygon.N >= sides) {
                if (currentPolygonArea < minArea ) {
                    //Console.WriteLine($"New min polygon found: {currentPolygon.Print()}");
                    minArea = currentPolygonArea;
                    minPolygon = currentPolygon.Copy();
                }
                return;
            }
            
            // If our current polygon has already exceed the min area, no point in exploring further.
            if ( currentPolygonArea > minArea) { return; }
            

            Polygon newTestPolygon;
            for(int y = 1; y < sides/8; y++) {
                for(int x = 1; x < Math.Min(y/currentPolygon.Slope, (double)sides/8); x++) {
                                             
                    if (primes.IsReducable(x, y)) {continue;}

                    newTestPolygon = currentPolygon.Copy(x, y);
                    NextPolygons(newTestPolygon);
                }
            }
        }

        public class Point {
            public int x;
            public int y;
            public Point(int xVal, int yVal) {
                x = xVal;
                y = yVal;
            }
        }

        public class Polygon {
            public List<Point> vertices;
            public Polygon() {
                vertices = new List<Point>(sides);
            }           
            // Assume bottom and side to be horizontal and vertical with lengths of 1/2.
            // Likely does not hold for very small N
            public double Area {
                get {
                    double maxY = (double)lastY + 0.5D;
                    double area = 0.5D * maxY;
                    for(int i=1; i<this.vertices.Count; i++) {
                        area += (double)((double)(vertices[i].x - vertices[i-1].x) * ((double)maxY - (double)(vertices[i].y + vertices[i-1].y)/2 ));
                    }
                    return area * 4D; 
                }               
            }
            public int lastX {
                get { return lastPoint.x; }
            }
            public int lastY {
                get { return lastPoint.y; }
            }
            public Point lastPoint {
                get {return vertices[vertices.Count - 1]; }
            }
            public double N {
                get { return vertices.Count * 4; }
            }
            public double Slope {
                get {
                    if (vertices.Count <= 1) { return (double)1/sides; }
                    return ( (double)(lastY - vertices[vertices.Count - 2].y) / 
                                     (lastX - vertices[vertices.Count - 2].x));
                }
            }
            public Polygon Copy(int addX = -1, int addY = -1) {
                var rtn = new Polygon();
                foreach(Point pt in this.vertices) {
                    rtn.vertices.Add(pt);
                }
                if (addX >= 0) { rtn.AddVertice(addX, addY); }
                return rtn;
            } 
            public void AddVertice(int addX, int addY) {
                vertices.Add(new Point(lastX + addX, lastY + addY));
            }
            public string Print() {
                var rtn = "";
                foreach (Point pt in this.vertices) {
                    rtn += $"({pt.x},{pt.y}),";
                }
                return rtn.TrimEnd(',');
            }  
            public void InferRemainder() {

                int center;
                if (sides % 4 == 0) {
                    center = vertices.Count - 2;
                } else {
                    center = vertices.Count - 1;
                }

                for(int i = 0; i<=center-1; i++) {
                    this.AddVertice(vertices[center-i].y - vertices[center-i-1].y,
                                    vertices[center-i].x - vertices[center-i-1].x);
                }
            }       
        }
    }
}