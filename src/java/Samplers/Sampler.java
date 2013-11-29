package Samplers;

import Utils.RandomNumberGen;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: parent class of all the samplers
 * comments:
 */
public class Sampler
{
    protected int warmupTime;
        public int GetWarmupTime()
        {
            return warmupTime;
        }
        public void SetWarmupTime( int val )
        {
            warmupTime = val;
        }

        protected int samplingInterval;
        public int GetSamplingInterval()
        {
            return samplingInterval;
        }
        public void SetSamplingInterval(int val)
        {
            samplingInterval = val;
        }

        protected RandomNumberGen randGen;

        public void Initialze()
        {
            randGen = new RandomNumberGen();
        }

        public void Initialze(int seed)
        {
            randGen = new RandomNumberGen(seed);
        }
}
