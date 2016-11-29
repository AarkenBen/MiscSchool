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

        // the Keys coorilate to the rows and the value holds the tag, valid, and data.
        Dictionary<int, block> cache;

        public DirectMapped(int _rows, int _bytesPerBlock, int[] _array)
        {
            rows = _rows;
            bytesPerBlock = _bytesPerBlock;
            addresses = _array;

            bitsOffSet = Math.Log(bytesPerBlock, 2);

            bitsRow = Math.Log(rows, 2);

            data = new double[bytesPerBlock];

            cache = new Dictionary<int, block>();

            if (withinAvailableRange())
            {

                //Console.WriteLine(cacheSize + " out of " + cacheLimit);
                //Console.Read();
                simulate();
            }
            else
            {
                Console.WriteLine(cacheSize + "  is over the limit of " + cacheLimit);
                
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

            double tag, offset;
            int row, passMiss;
            int totalMiss = 0;

            for (int i = 0; i < 5; i++)
            {
                passMiss = 0;
                

                Console.WriteLine("\nPass: " + (i + 1));

                foreach (var address in addresses)
                {
                    tag = (address / (bytesPerBlock * rows));
                    row = (address / bytesPerBlock) % rows;
                    offset = address % bytesPerBlock;


                    //Console.WriteLine("The address: " + address + " The tag before rounding: " + tag + " and the row: " + Math.Floor(row));

                    //hit
                    if (cache.ContainsKey(row))
                    {
                        if (cache[(int)row].tag == (int)tag && cache[(int)row].valid == true)
                        {
                            Console.WriteLine("Accessing " + address + "(tag " + tag + "): hit from row " + row);

                        }
                        else
                        {
                            Console.WriteLine("Accessing " + address + "(tag " + tag + "): miss - cached to row " + row);
                            block b = new block(true, (int)tag);
                            cache[row].tag = (int)tag;
                            passMiss++;
                        }

                    }
                    else
                    {

                        //miss
                        Console.WriteLine("Accessing " + address + "(tag " + tag + "): miss - cached to row " + row);
                        block b = new block(true, (int)tag);
                        cache.Add(row, b);
                        passMiss++;
                    }
                    
                }

                Console.WriteLine("pass misses: " + passMiss);
                totalMiss += passMiss;
            }
            Console.WriteLine("Total misses over 5 iterations: " + totalMiss);

            double miss = totalMiss / 5;

            double cpi = (miss * ( 20 + (1 * bytesPerBlock)) + (addresses.Length - miss) * 1) / addresses.Length;

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
