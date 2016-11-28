using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePerformance
{
    public class test
    {

        public static void main(string[] args)
        {
            //// create a set associative cache
            //setAssociative sA = new setAssociative();

            //// create a direct mapped cache
            //DirectMapped dM = new DirectMapped();

            // create a fully associative cache
            FullyAssociative fA = new FullyAssociative(8,8);



        }

   }
}
