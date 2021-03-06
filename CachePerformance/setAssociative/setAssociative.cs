﻿using System;
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

        // array containing LRU bits
        private int[] lruArray;

        // array containing data
        private int[] tagArray;

        // indiciates in block is valid or not
        private bool[] valid;

        public setAssociative(int _rows, int _bytesPerBlock, int _ways, int[] _array)
        {
            rows = _rows;
            bytesPerBlock = _bytesPerBlock;
            ways = _ways;
            addresses = _array;

            bitsOffSet = Math.Log(bytesPerBlock, 2);

            bitsLRU = Math.Log(rows, 2);

            bitsRow = Math.Log(rows, 2);

            lruArray = new int[rows * ways];
            tagArray = new int[rows * ways];
            valid = new bool[rows * ways];

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
            bool itsAHit;

            for (int i = 0; i < 50; i++)
            {
                passMiss = 0;


                Console.WriteLine("\nPass: " + (i + 1));

                foreach (var address in addresses)
                {
                    itsAHit = false;

                    tag = (address / (bytesPerBlock * rows));
                    row = (address / bytesPerBlock) % rows;
                    offset = address % bytesPerBlock;
                    set = (address/bytesPerBlock) % rows;

                    //Hit
                    for(int j = 0; j < ways; j++)
                    {
                        //hit
                        if(tagArray[set + (j * rows)] == (int)tag && valid[set + (j * rows)] == true)
                        {
                            Console.WriteLine("Accessing " + address + "(tag " + tag + "): hit from set " + set);
                            itsAHit = true;
                            break;
                        }
                    }

                    //miss
                    if(!itsAHit)
                    {
                        bool noneFalse = true;

                        Console.WriteLine("Accessing " + address + "(tag " + tag + "): miss - cached to set " + set);

                        passMiss++;

                        for (int p = 0; p < ways; p++)
                        {
                            if (valid[set + (p * rows)] == false)
                            {
                                noneFalse = false;
                                tagArray[set + (p * rows)] = (int)tag;
                                valid[set + (p * rows)] = true;

                                for (int k = 0; k < ways; k++)
                                {
                                    if (k == p)
                                    {
                                        lruArray[set + (k * rows)] = rows - 1;
                                    }
                                    else
                                        lruArray[set + (k * rows)] = lruArray[set + (k * rows)] - 1;
                                }
                                break;
                            }
                        }


                        if (noneFalse)
                        {
                            int lowest = rows;
                            int wayToReplace = ways - 1;

                            for (int w = 0; w < ways; w++)
                            {

                                if (lruArray[set + (w * rows)] < lowest)
                                {
                                    lowest = lruArray[set + (w * rows)];
                                    wayToReplace = w;
                                }
                            }

                            for (int k = 0; k < ways; k++)
                            {
                                if (wayToReplace == k)
                                {
                                    lruArray[set + (k * rows)] = rows - 1;
                                }
                                else
                                    lruArray[set + (k * rows)] = lruArray[set + (k * rows)] - 1;
                            }

                            tagArray[set + (wayToReplace * rows)] = (int)tag;
                            valid[set + (wayToReplace * rows)] = true;
                        }
                        
                    }
                }

                Console.WriteLine("pass misses: " + passMiss);
                totalMiss += passMiss;
            }
            Console.WriteLine("Total misses over 5 iterations: " + totalMiss);

            double miss = totalMiss / 50;

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
