using System;

namespace ProjectEuler
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dtmStart = DateTime.Now;
            ISolve problem = new PE894_1();
            problem.SetData();

            TimeSpan tspInitLength = DateTime.Now - dtmStart;
            Console.WriteLine($"Initial data set. Time elapsed: {tspInitLength.TotalSeconds}");

            problem.Solve();

            TimeSpan tspLength = DateTime.Now - dtmStart;
            Console.WriteLine($"Done. Total elapsed time: {tspLength.TotalSeconds} seconds");
        }
    }

    public interface ISolve {

        void SetData();
        void Solve();
    }
}
