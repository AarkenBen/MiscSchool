using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePerformance
{
    public class DirectMapped
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

        // number of bytes per block
        private int bytesPerBlock;

        // number of off set bits
        private double bitsOffSet;

        // number of bits required for the rows
        private double bitsRow;

        // size of cache with given information
        private double cacheSize;

        // array of addresses
        private int[] addresses;

        // represents the cache
        private double[] data;

        // represents the tag in each row
        private int[] tag;

        public DirectMapped(int _rows, int _bytesPerBlock, int[] _array)
        {
            rows = _rows;
            bytesPerBlock = _bytesPerBlock;
            addresses = _array;

            bitsOffSet = Math.Log(bytesPerBlock, 2);

            bitsRow = Math.Log(rows, 2);

            data = new double[rows];
            tag = new int[rows];

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
            cacheSize = (validBit + (addressSize - bitsOffSet - bitsRow) + (bytesPerBlock * 8)) * rows;


            if (cacheSize > cacheLimit)
                return false;
            else
                return true;
        }

        private void simulate()
        {
            
            double tag, row, offset;
            foreach(var address in addresses)
            {
                tag = (address / (bytesPerBlock * rows));
                row = (address / bytesPerBlock) % 4;
                offset = address % bytesPerBlock;

                if(data[(int)row] == address)
                {
                    //test
                }
            }
        }

    }
}
