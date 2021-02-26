using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE202_2 : ISolve {

        public double sqrt3over2;
        public const int increments = 10000;
        const int reflections = 47;
        public const double threshhold = .002D;

        public void SetData () {
            
            sqrt3over2 = Math.Sqrt(3) / 2;

        }


        public void Solve () {

            double phi = 0.454427276386134D;
            double x = 1D;
            short side = (short)0;
            Evaluate(ref phi, ref x, ref side);

        }

   /*   
        public void Solve () {    // This solve is an auxillary function to narrow down an exact value
            int maxPrecision = 14;
            int precision = 3;
            double startPhi0 = .8679 * (Math.PI/6);
            double x;
            short side;

            while (precision < maxPrecision) {
                
                double error = 1;
                double delta = Math.Pow(10, (-1) * (precision+1));
                double phi;
                int iMin = 0;
                double minError = 1;

                for(int i=0; i<= 20; i++) {
                    
                    x = 1D;
                    side = (short)0;  
                    double phi0 = startPhi0 + ((i-10)*delta);   
                    phi = phi0;          
                    Evaluate(ref phi, ref x, ref side);
                    if (side == 0) { error = 1-x; } else if (side == 1) { error=x; }

                    if (error < minError) { minError = error; iMin = i; }
                }

                startPhi0 = startPhi0 + ((iMin-10)*delta); 

                precision += 1;
            }

            Console.WriteLine(startPhi0);     
            
        } 
   */  
/*
        public void Solve () {

            double phiIncrement = (Math.PI/6)/increments;
            int increment = 1;

            double x;
            short side;
            double phi = phiIncrement;
            
            while  (phi < Math.PI/6) {
                
                x = 1D;
                side = (short)0;
                Evaluate(ref phi, ref x, ref side);

                if ((side == 0 && x > (1-threshhold) || (side == 1 && x < threshhold))) {
                    Console.WriteLine($"Increment: {increment}\tSide: {side}\t Position: {x}");
                }
                
                increment += 1;
                phi = increment * phiIncrement;
            }                    
        } 

*/
        public void Evaluate(ref double phi, ref double x, ref short side)
        {
            int reflectedCount = 0;
                
            while (reflectedCount <= reflections)
            {             
                Next(ref phi, ref x, ref side);
                reflectedCount += 1;
                Console.WriteLine($"Side: {side}\t x: {x}");
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
    }
}