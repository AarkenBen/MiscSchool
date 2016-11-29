using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachePerformance
{

    public class FullyAssociative
    {

        // in all of my tests the valid column will only require one bit.
        public const int validBit = 1;

        // cache size limit
        public const int cacheLimit = 900;

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

        // number of off set bits
        private double bitsOffSet;

        // size of cache with given information
        private double cacheSize;

        // the array of address
        private int[] addresses;

        // the blocks of data
        //private Dictionary<int, block> cache;


        private int[] tags;
        private int[] data;
        private int[] lruArray;
        private bool[] valid;
        

        public FullyAssociative(int _rows, int _bytesPerBlock, int[] _array)
        {
            rows = _rows;
            bytesPerBlock = _bytesPerBlock;
            addresses = _array;

            bitsLRU = Math.Log(rows, 2);

            bitsOffSet = Math.Log(bytesPerBlock, 2);


            //cache = new Dictionary<int, block>();

            tags = new int[rows];
            data = new int[rows];
            lruArray = new int[rows];
            valid = new bool[rows];

            
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
            cacheSize = (validBit + bitsLRU + (addressSize - bitsOffSet) + (bytesPerBlock * 8)) * rows;


            if (cacheSize > cacheLimit)
                return false;
            else
                return true;
        }

        private void simulate()
        {

            double tag, offset;
            int passMiss;
            int totalMiss = 0;
            int hit = 0;
            bool hitIT = false;

            int rowToWrite = 0;
            
            for (int i = 0; i < 1; i++)
            {
                passMiss = 0;


                Console.WriteLine("\nPass: " + (i + 1));

                foreach (var address in addresses)
                {
                    hitIT = false;
                    tag = (address / (bytesPerBlock * rows));
                    //row = (address / bytesPerBlock) % rows;
                    offset = address % bytesPerBlock;

                    
                    for(int p = 0; p < data.Length; p++)
                    {
                        if (valid[p] == true)
                        {
                            int lowerBound = (address / bytesPerBlock) * bytesPerBlock;
                            Console.WriteLine("address " + address + " lower bound " + lowerBound);

                            // hit
                            if (data[p] >= lowerBound && data[p] <= lowerBound + bytesPerBlock - 1)
                            {
                                hit++;
                                hitIT = true;
                                break;
                            }
                        }

                        
                    }

                    if(hitIT == false)
                    {
                        // miss
                        Console.WriteLine("miss");
                        //figures out where to write to
                        if (valid.Contains(false))
                        {
                            for (int j = 0; j <= addresses.Length; j++)
                            {
                                if (valid[j] == false)
                                {
                                    rowToWrite = j;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            int lowest = rows;

                            for (int j = 0; j <= lruArray.Length; j++)
                            {
                                if (lruArray[j] == 0)
                                {
                                    rowToWrite = j;
                                    break;
                                }
                                else if (lruArray[j] < lowest)
                                {
                                    lowest = lruArray[j];
                                    rowToWrite = j;
                                }
                            }
                        }

                        //passMiss++;
                        data[rowToWrite] = address;
                        valid[rowToWrite] = true;

                        for (int k = 0; k < lruArray.Length; k++)
                        {
                            if (k == rowToWrite)
                            {
                                lruArray[k] = rows - 1;
                            }
                            else
                                lruArray[k] = lruArray[k] - 1;
                        }
                    }
                }
                Console.WriteLine(hit);
                passMiss = addresses.Length - hit;
                hit = 0;
                Console.WriteLine("pass misses: " + passMiss);
                totalMiss += passMiss;
            }
            Console.WriteLine("Total misses over 5 iterations: " + totalMiss);

            double miss = totalMiss / 1;

            double cpi = (miss * (20 + (1 * bytesPerBlock)) + (addresses.Length - miss) * 1) / addresses.Length;

            Console.WriteLine("average over 5 iterations CPI: " + cpi);

            Console.ReadLine();
        }



        public class block
        {
            public bool valid = false;

            public int tag { get; set; }
            
            public int LRU { get; set; }

            public int low { get; set; }

            public int high { get; set; }

            public block(bool _valid, int _tag, int _LRU, int _low, int _high)
            {
                valid = _valid;
                tag = _tag;
                LRU = _LRU;
                low = _low;
                high = _high;
            }
        }
    }
}
