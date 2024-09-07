using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using ProjectEuler;


namespace ProjectEuler {
    public class PE148_1 : ISolve {

        // GLOBALS
        const int rows = 775;

        public void SetData() {}

        public void Solve() {

            var pt = new PascalTriangle();   
            int totalEntries = 1;
            int totalEntriesDivBy7 = 1;
            int entriesDivBy7;


            int[] current;
            for (int l=1; l<rows; l++) {

                current = pt.NextRow();
              
                totalEntries += current.Length;
                entriesDivBy7 = DivBy7(current);
                totalEntriesDivBy7 += current.Length-entriesDivBy7;
                Console.WriteLine($"{pt.line},\t count: {current.Length-entriesDivBy7},\t {Standard.ArrayAsString(current)}");
            }


            Console.WriteLine($"Total Entries: {totalEntries},\t DivBy7: {totalEntriesDivBy7},\t NotDivBy7: {totalEntries-totalEntriesDivBy7}");
        }

        public static int DivBy7(int[] arr) {
            int count = 0;
            for (int i=1; i < Math.Floor((double)arr.Length/2); i++) { // know to be symmetric and that value at index 0 = 1
                if ( arr[i] == 0) { count += 2; } // % 7 
            }

            if (arr.Length % 2 == 1) {
                if ( arr[(arr.Length-1)/2] == 0) { count += 1; } // % 7 
            }

            return count;
        }

        private class PascalTriangle
        {
            public int line;
            public int[] row;

            public PascalTriangle() {
                row = new int[]{1};  // TODO: REVERT TO 1
                line = 1; 
            }

            public int[] NextRow() {
          
                int[] newRow = new int[row.Length+1];
                
                newRow[0] = row[0];
                newRow[newRow.Length-1] = row[row.Length-1];
                
                for(int i = 1; i<row.Length; i++) {
                    newRow[i] = (row[i] + row[i-1]) % 7;
                }

                row = newRow;
                line ++;
                return row;
            }
        }      
    }
}