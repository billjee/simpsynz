/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopulationSynthesis.Utils
{
    class RandomNumberGen
    {
        private Random myRandGen;
        public RandomNumberGen()
        {
            myRandGen = new Random();
        }
        public RandomNumberGen(int seed)
        {
            myRandGen = new Random(seed);
        }
        // Returns a value from 0 and 2,147,483,646
        public int Next()
        {
            return myRandGen.Next();
        }
        public int NextInRange(int strt, int end)
        {
            return (myRandGen.Next() % ((end - strt) + 1 )) + strt;
        }
        public double NextDouble()
        {
            return myRandGen.NextDouble();
        }
        public double NextDoubleInRange(double strt, double end)
        {
            return (myRandGen.NextDouble() * (end - strt)) + strt;
        }
        public ArrayList GetNNumbersInRange(int strt, int end, int n)
        {
            ArrayList currList = new ArrayList();
            if (strt >= end)
            {
                return currList;
            }
            for (int i = 0; i < n; i++)
            {
                currList.Add(NextInRange(strt, end));
            }
            return currList;
        }

        // Get normal (Gaussian) random sample with mean 0 
        // and standard deviation 1
        public double GetNextStdNormal()
        {
            // Use Box-Muller algorithm
            double u1 = NextDouble();

            double u2 = NextDouble();

            double r = Math.Sqrt(-2.0 * Math.Log(u1));

            double theta = 2.0 * Math.PI * u2;

            return r * Math.Sin(theta);

        }

        // Get normal (Gaussian) random sample with 
        // specified mean and standard deviation
        public double GetNextNormal(double mean, 
            double standardDeviation)
        {
            if (standardDeviation <= 0.0)
            {
                string msg = string.Format("Shape must be positive. Received {0}.", standardDeviation);
                throw new ArgumentOutOfRangeException(msg);
            }

            return mean + standardDeviation * GetNextStdNormal();
        }
    }
}
