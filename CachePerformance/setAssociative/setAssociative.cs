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
                //Console.WriteLine(cacheSize + " out of " + cacheLimit);
                //Console.Read();
                simulate();
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

        private void simulate()
        {

            double tag, offset;
            int row, passMiss;
            int totalMiss = 0;
            int set;

            for (int i = 0; i < 5; i++)
            {
                passMiss = 0;


                Console.WriteLine("\nPass: " + (i + 1));

                foreach (var address in addresses)
                {
                    tag = (address / (bytesPerBlock * rows));
                    row = (address / bytesPerBlock) % rows;
                    offset = address % bytesPerBlock;
                    set = address % ways;


                    /// maybe use one array [row * ways]
                    /// then could add row to the index to acces the  x spots where data would be stored

                    //Console.WriteLine("The address: " + address + " The tag before rounding: " + tag + " and the row: " + Math.Floor(row));

                    //hit
                    //if (cache.ContainsKey(row))
                    //{
                    //    if (cache[(int)row].tag == (int)tag && cache[(int)row].valid == true)
                    //    {
                    //        Console.WriteLine("Accessing " + address + "(tag " + tag + "): hit from row " + row);

                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Accessing " + address + "(tag " + tag + "): miss - cached to row " + row);
                    //        block b = new block(true, (int)tag);
                    //        cache[row].tag = (int)tag;
                    //        passMiss++;
                    //    }

                    //}
                    //else
                    //{

                    //    //miss
                    //    Console.WriteLine("Accessing " + address + "(tag " + tag + "): miss - cached to row " + row);
                    //    block b = new block(true, (int)tag);
                    //    cache.Add(row, b);
                    //    passMiss++;
                    //}

                }

                Console.WriteLine("pass misses: " + passMiss);
                totalMiss += passMiss;
            }
            Console.WriteLine("Total misses over 5 iterations: " + totalMiss);

            double miss = totalMiss / 5;

            double cpi = (miss * (18 + (3 * bytesPerBlock)) + (addresses.Length - miss) * 1) / addresses.Length;

            Console.WriteLine("average over 5 iterations CPI: " + cpi);

            Console.ReadLine();
        }



        public class block
        {
            public bool valid = false;

            public int tag { get; set; }



            public block(bool _valid, int _tag)
            {
                valid = _valid;
                tag = _tag;
            }
        }
    }
}
