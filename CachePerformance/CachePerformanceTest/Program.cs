using CachePerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            //homework
            int[] array = { 4, 8, 20, 24, 28, 36, 44, 20, 24, 28, 36, 40, 44, 68, 72, 92, 96, 100, 104, 108, 112, 100, 112, 116, 120, 128, 140 };

            //test
            //int[] array = { 16, 20, 24, 28, 32, 36, 60, 64, 56, 60, 64, 68, 56, 60, 64, 72, 76, 92, 96, 100, 104, 108, 112, 120, 124, 128, 144, 148 };


            // create a set associative cache
            //setAssociative sA = new setAssociative(1,8,10, array);


            // create a direct mapped cache
            //DirectMapped dm = new DirectMapped(1,64, array);

            //// create a fully associative cache
            FullyAssociative fA = new FullyAssociative(2,32, array);



        }
    }
}
