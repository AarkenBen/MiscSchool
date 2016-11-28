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
            int[] array = {4, 8, 20, 24, 28, 36, 44, 20, 24, 28, 36, 40, 44, 68, 72, 92, 96, 100, 104, 108, 112, 100, 112, 116, 120, 128, 140};
            // create a set associative cache
            //setAssociative sA = new setAssociative(4,4,4,array);


            //// create a direct mapped cache
            //DirectMapped dM = new DirectMapped(8,8,array);

            //// create a fully associative cache
            //FullyAssociative fA = new FullyAssociative(8, 8,array);



        }
    }
}
