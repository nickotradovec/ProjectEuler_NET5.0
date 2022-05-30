using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectEuler {
    public static class Standard {
        public static Dictionary<T, Int32> GroupList<T> (List<T> list) {

            Dictionary<T, Int32> rtn = new Dictionary<T, int> ();
            foreach (T val in list) {
                if (rtn.ContainsKey (val)) {
                    rtn[val]++;
                } else {
                    rtn.Add (val, 1);
                }
            }
            return rtn;
        }

        public static List<long> CrossMultiply(List<long> list1, List<long> list2) {

            var rtn = new List<long>();
            foreach ( var item1 in list1 ) {
                foreach ( var item2 in list2 ) {
                    rtn.Add( item1 * item2 );
                }
            }
            return rtn;
        }

        public static long Sum(List<long> list) {

            long rtn = 0;
            foreach ( var item in list ) { rtn += item; }
            return rtn;
        }

        public static string ArrayAsString(int[] current) {
            string str = "";
            foreach( int w in current) {
                str += $"{w},";
            }
            if (str.Length > 0) {str = str.Remove(str.Length-1);}
            return str;
        }
        
    }   
}