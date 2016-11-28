using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePerformance
{
    public class setAssociative
    {

        // in all of my tests the valid column will only require one bit.
        public const int validBit = 1;

        // cache size limit
        public const int cacheLimit = 800;

        // size of the address
        public const int addressSize = 16;

        // size of the instructions
        public const int instructionSize = 32;

        // number of rows in cache
        private int rows;

        // number of LRU bits
        private double bitsLRU;

        // number of bytes per block
        private int bytesPerBlock;

        // number of bits required for the rows
        private double bitsRow;

        // number of off set bits
        private double bitsOffSet;

        // size of cache with given information
        private double cacheSize;

        //number of ways
        private int ways;

        // array of addresses
        private int[] addresses;

        public setAssociative(int _rows, int _bytesPerBlock, int _ways, int[] _array)
        {
            rows = _rows;
            bytesPerBlock = _bytesPerBlock;
            ways = _ways;
            addresses = _array;

            bitsOffSet = Math.Log(bytesPerBlock, 2);

            bitsLRU = Math.Log(rows, 2);

            bitsRow = Math.Log(rows, 2);

            if (withinAvailableRange())
            {

                Console.WriteLine(cacheSize + " out of " + cacheLimit);
                Console.Read();
            }
            else
            {
                Console.WriteLine(cacheSize + "  is over the limit of " + cacheLimit);
                Console.Read();
            }
        }


        /// <summary>
        /// Deteremine if the supplied parameters total bits will fit into cache
        /// </summary>
        /// <returns></returns>
        private bool withinAvailableRange()
        {
            cacheSize = (((validBit + bitsLRU + (addressSize - bitsOffSet - bitsRow) + (bytesPerBlock * 8)) * rows) * ways);


            if (cacheSize > cacheLimit)
                return false;
            else
                return true;
        }
    }
}
