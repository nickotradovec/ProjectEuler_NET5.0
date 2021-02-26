using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE202_1 : ISolve {

        // alpha[n+1)] = 2(Omega[incidence plane]) - alpha[n]
        // alpha = signed incidence angle WRT to side zero which will be our reference plane.
        // Omega = angle of incidence plane (side of triangle) WRT to side zero.
        // As such, Omega[0] = 0, Omega[1] = pi/3, Omega[2] = 2pi/3

        public double sqrt3over2;

        public void SetData () {
            
            sqrt3over2 = Math.Sqrt(3) / 2;

        }

        public void Solve () {

            double alpha = Math.PI/12;
            double x = 1D;
            Side side = Side.side0;

            while (true)
            {
                Console.WriteLine($"New point at side: {side.ToString()},\t x: {x},\talpha: {alpha}");
                Next(ref alpha, ref x, ref side);
            }          
        }

        public void Next(ref double alpha, ref double x, ref Side side)
        {
            double alphaPrevious = alpha;
            double xPrevious = x;
            Side sidePrevious = side;
            double phiPrevious = Phi(alphaPrevious, sidePrevious);   
            //Console.WriteLine($"Alpha Previous: {alphaPrevious*180/Math.PI}, \tPhi: {phiPrevious*180/Math.PI}");         
            
            side = AssumeNextSide(alphaPrevious);        
            
            // may be 1 minus value, and may require 1 minus value. Need robust way to determine this.

            x = CalculateX(phiPrevious, sidePrevious, side, xPrevious);
            if (x < 0 || x > 1) { 
                // Calculate against other remaining side.
                side = RemainingSide(sidePrevious, side);
                x = CalculateX(phiPrevious, sidePrevious, side, xPrevious);
             }         

            
            alpha = (2*Omega(side) - alphaPrevious) % (2*Math.PI); 
            while (alpha < 0) {alpha += 2*Math.PI;}    
            alpha = alpha % (2*Math.PI); 
        }

        public Side AssumeNextSide(double alpha)
        {
            alpha = alpha % (2*Math.PI);
            // base on alpha, we will guess the next side as it will be the actual next side in most cases.
            // however, if we recieve an invalid value afer our guess (x<0 or x> 1) then we guessed incorrectly.
            double piOver6 = Math.PI/6;

            switch(true)
            {          
                case true when (alpha > 7*piOver6 && alpha <= 11*piOver6) : { return Side.side0; }
                case true when (alpha >= 3*piOver6 && alpha <= 7*piOver6) : { return Side.side1; }
                case true when (alpha > 11*piOver6 || alpha < 3*piOver6) : { return Side.side2; }
            }
            throw new Exception($"Uncertain next side for alpha = {alpha}");
        } 

        public double CalculateX(double phiPrevious, Side sideOrigin, Side sideNew, double xOirigin) {
     
            bool xPrior1Minus = false;
            bool xNew1Minus = false;

            switch(sideOrigin)
            {
                case Side.side0 : 
                    switch(sideNew) {
                        case Side.side1 : xPrior1Minus = true; xNew1Minus = false; break;
                        case Side.side2 : xPrior1Minus = false; xNew1Minus = true; break;
                    }
                    break;

                case Side.side1 : 
                    switch(sideNew) {
                        case Side.side0 : xPrior1Minus = false; xNew1Minus = true; break;
                        case Side.side2 : xPrior1Minus = true; xNew1Minus = false; break;
                    }
                    break;
                case Side.side2 : 
                    switch(sideNew) {
                        case Side.side0 : xPrior1Minus = true; xNew1Minus = false; break;
                        case Side.side1 : xPrior1Minus = false; xNew1Minus = true; break;
                    }
                    break;
            }

            double xUse;
            if(xPrior1Minus) {xUse = 1-xOirigin;} else {xUse = xOirigin;}
            double xVal = (xUse) / (sqrt3over2 * (Math.Cos(phiPrevious)/Math.Sin(phiPrevious)) + .5);

            double x;
            if(xNew1Minus) {x=1-xVal;} else {x=xVal;}

            return x;
        }  

        public double Phi(double alpha, Side side) {
            
            double phi = 0D;
            switch(side)
            {
                case Side.side0 : phi = Math.PI + alpha - Omega(side); break;
                case Side.side1 : phi = Omega(side) + Math.PI - alpha; break;
                case Side.side2 : phi = Math.PI + alpha - Omega(side); break;
            }
            if (phi < 0) {phi += Math.PI;}
            phi = phi % Math.PI;
            if (phi > Math.PI/2) {phi = Math.PI - phi;}
            return phi; //% (Math.PI);
        }  

        public enum Side
        {
            side0 = 0, side1 = 1, side2 = 2
        } 
        public double Omega(Side side) {
            switch(side)
            {
                case Side.side0 : return 0;
                case Side.side1 : return Math.PI/3;
                case Side.side2 : return 2*Math.PI/3;
            }
            throw new Exception($"Unable to determine Phi for side: {side.ToString()}");
        }
        public Side RemainingSide(Side sideFirst, Side sideSecond) {

            bool side0 = (sideFirst != Side.side0 && sideSecond != Side.side0);
            bool side1 = (sideFirst != Side.side1 && sideSecond != Side.side1);
            bool side2 = (sideFirst != Side.side2 && sideSecond != Side.side2);

            switch(true)
            {
                case true when side0 : return Side.side0;
                case true when side1 : return Side.side1;
                case true when side2 : return Side.side2;
            }
            throw new Exception("Error determining remaining side.");
        }
    }
}