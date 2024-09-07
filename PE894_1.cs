using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace ProjectEuler {
    public class PE894_1 : ISolve {

        public void SetData() {

        }

        public void Solve() {

            double b = Math.PI/10;
            double s = 0.9;
            double d;
            double error;

            while (true)
            {
                ScalingFactorFromB(b, ref s);
                d = GetDistance(b, s);

                error = EvaluateDiscrepancy(b, s, d);
            }
            
            Console.WriteLine("sdlkfjs dflskj");
        }   

        
        public void ScalingFactorFromB(double b, ref double s) // Equation 6 - E(s,b)
        {
            // Use Newton's method to get s from b. s0 should be initial guess
            double error = 1000f;

            while (error > .0001)
            {
                s = 0.9f;

                error = Equation6Discrepancy(b, s);
            }
        } 

        public double Equation6Discrepancy(double b, double s)
        {
            double p1 = (s+1) * 2f * ( Math.Cos( TwoPiMinusSevenBoverEight(b) ) + ( Math.Pow(s, 8) * Math.Cos(b) ));
            double p2 = (Math.Pow(s, 13) - Math.Pow(s, 15)) * Math.Sqrt( Math.Pow(s, 2) + 1f - (s * 2f * Math.Cos( TwoPiPlusBoverEight(b) )) );
            return p1 - p2;
        }

        public double GetDistance(double b, double s) // Equation 1 - E(s, b, d)
        {
            return (s+1) / Math.Sqrt( Math.Pow(s, 2) + 1f - (s * 2f * Math.Cos( TwoPiPlusBoverEight(b) )));
        }

        public double EvaluateDiscrepancy(double b, double s, double d) // Equation 5
        {
            return Math.Pow(s, 13) - Math.Pow(s, 15) - (d * 2f * (Math.Cos( TwoPiMinusSevenBoverEight(b) ) + (Math.Pow(s, 8) * Math.Cos(b))));
        }

        public static double TwoPiMinusSevenBoverEight(double b)
        {
            return ((Math.PI * 2) - (b * 7)) / 8;
        }

        public static double TwoPiPlusBoverEight(double b)
        {
            return ((Math.PI * 2) + b ) / 8;
        }
    }
}