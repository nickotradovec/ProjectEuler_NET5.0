using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE202_4 : ISolve {

        public double sqrt3over2;
        public const double threshhold = .000001D;
        public const int minReflections = 7;
        public const int maxReflections = 200;

        public void SetData () {          
            sqrt3over2 = Math.Sqrt(3) / 2;
        }

        public void Solve() {
           
        string x1Solutions;
        int count;

            for (int i = 7; i<=maxReflections; i+=2) {

                x1Solutions = "";
                count = SolveCount(i, ref x1Solutions);
                Console.WriteLine($"Reflections: {i};\tCount: {count};\tSolutions: {x1Solutions}");
            }

        }

        public int SolveCount (int reflections, ref string x1List) {

            int evals = (reflections + 3) / 2;
            int increment = (int)Math.Ceiling(((double)evals/2));
        
            double x;
            short side = 0;   
            double phi;  
            int earlyExit;  

            int escapeCount = 0;    
            while  (increment < evals) {
                
                x = 1D;
                side = 0;
                earlyExit = 0;
                phi = Phi0((double)increment / evals);
                Evaluate(reflections, ref phi, ref x, ref side, ref earlyExit);

                if (side == 2 && x >= (0.5D - threshhold) && x <= (0.5D + threshhold) ) {
                    //Console.WriteLine($"x1: {increment}/{evals}, x[half] = {x}");
                    x1List += $"{increment}/{evals},";
                    escapeCount += 1;
                }
                         
                increment += 1;
            }  
            return escapeCount;                  
        } 

        public void Evaluate(int reflections, ref double phi, ref double x, ref short side, ref int earlyExit)
        {
            int reflectedCount = 0;
                
            while (reflectedCount < (reflections + 1)/2)
            {             
                Next(ref phi, ref x, ref side);

                if (x < threshhold || x > (1-threshhold)) {
                    earlyExit = reflectedCount;
                    return;
                }

                reflectedCount += 1;
                //Console.WriteLine($"Side: {side}\t x: {x}");
            }
        }

        public void Next(ref double phiDepart, ref double x, ref short side)
        {
            double phiDepartPrevious = phiDepart;
            double xPrevious = x;
            short sidePrevious = side;
            
            NextPoint(sidePrevious, xPrevious, phiDepartPrevious, out side, out x, out phiDepart);
        }

        public void NextPoint(short sidePrevious, double xPrevious, double phiDepartPrevious, 
                              out short side, out double x, out double phiDepart) {

            bool tryRight = false;
            
            if (phiDepartPrevious > Math.PI/2) { 
                tryRight = true; 
                side = (short)((sidePrevious + 1) % 3);

                // Departing to the right means incoming from the left
                FromLeft(xPrevious, phiDepartPrevious, out x, out phiDepart);

            } else {
                side = (short)((sidePrevious + 3 - 1) % 3);
                FromRight(xPrevious, phiDepartPrevious, out x, out phiDepart);
            }
          
            if (x < 0 || x > 1) {
                if (tryRight) { // tried right and was incorrect. try left.
                    side = (short)((sidePrevious + 3 - 1) % 3);
                    FromRight(xPrevious, phiDepartPrevious, out x, out phiDepart);
                }
                else {
                    side = (short)((sidePrevious + 1) % 3);
                    FromLeft(xPrevious, phiDepartPrevious, out x, out phiDepart);
                }
            }
        }

        public void FromLeft(double xPrevious, double phiDepartPrevious, out double x, out double phiDepart) {

            double phiEnter = phiDepartPrevious - (Math.PI/3);
            phiDepart = Math.PI - phiEnter;
            

            x = (1D - xPrevious) * (sqrt3over2 * Math.Cos(phiEnter)/Math.Sin(phiEnter) + 0.5D);
        }
        public void FromRight(double xPrevious, double phiDepartPrevious, out double x, out double phiDepart) {
            
            double phiEnter = Math.PI/3 + phiDepartPrevious;
            phiDepart = Math.PI - phiEnter;

            x = 1 - (xPrevious * (0.5D - sqrt3over2 * Math.Cos(phiEnter)/Math.Sin(phiEnter)));
        }

        public double Phi0(double x1) {
            return Math.Atan(Math.Sqrt(3)/((2/(1-x1)) - 1));
        }
    }
}