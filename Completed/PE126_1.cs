using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace ProjectEuler {
    public class PE126_1 : ISolve {    

        Int32 maxTest = 25000; 
        Int16 testVal = 1000;
        Int16[] vals;

        public void SetData () {
            vals = new Int16[maxTest+1];
        }

        public void Solve () {
         
            for(int s1=1; s1<=maxTest; s1++) {
                for(int s2=s1; 2*s1*s2<=maxTest; s2++) {
                    for(int s3=s2; 2*((s1*s2) + (s2*s3) + (s1*s3))<=maxTest; s3++) {
                        EvaluateLayers(s1, s2, s3);
                    }
                }
            }

            int answer = 0;
            int i=0;
            while (answer == 0 && i<maxTest) {
                if (vals[i] == testVal) {
                    answer = i;
                }
                i++;
            }
            Console.WriteLine(answer);
        }
        
        private void EvaluateLayers(int s1, int s2, int s3) {

            Int64 u0 = 2*((s1*s2) + (s2*s3) + (s1*s3));
            if (u0 > maxTest) {return;}

            Int64 p0 = 4*(s1+s2+s3);

            Int64 n = 1;
            Int64 layerCount;

            while (true) {
                layerCount = u0 + ((n-1)*(p0)) + ((n-1) * (n-2) * 4);    
                if (layerCount < 0 || layerCount > maxTest) {break;}    
                vals[layerCount] ++;     
                n++;
            }
        }
    }
}