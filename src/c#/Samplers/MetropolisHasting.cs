/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopulationSynthesis.Utils;
using SimulationObjects;

namespace Samplers
{
    public struct KeyValDoublePair
    {
        public double category;
        public double val;
    }
    // [BF] Rightnow it is only fixed for a log normal distribution for
    // income only
    class StateGenerator
    {
        double mean;
        public double GetMean()
        {
            return mean;
        }
        double stdev;
        RandomNumberGen myRandGen;

        // Note that the parameters are given as log-normal ones
        // Internally they are converted into normal
        public void SetParameters(double meu, double sigma)
        {
            double meuSqr = Math.Pow(meu,2);
            double sigSqr = Math.Pow(sigma,2);

            mean = Math.Log(meuSqr/Math.Pow((meuSqr+sigSqr),0.5));
            stdev = Math.Pow(Math.Log(Math.Pow((sigma/meu),2)+1),0.5);

            myRandGen = new RandomNumberGen();

        }
        
        public KeyValDoublePair GetNextState()
        {
            double nxtVal = myRandGen.GetNextNormal(mean, stdev);
            double f_X_Val = GetTransitionProbablity(nxtVal);
            KeyValDoublePair currPair = new KeyValDoublePair();
            currPair.category = nxtVal;
            currPair.val = f_X_Val;
            return currPair;
        }

        public double GetTransitionProbablity(double nxtVal)
        {
            return 1 / (2.50662827 * stdev)
                        * (Math.Exp(-Math.Pow(nxtVal - mean, 2)
                        / (2 * Math.Pow(stdev,2))));
        }
    }

    class MetropolisHasting : Sampler
    {
        public MetropolisHasting()
        {
            randGen = new RandomNumberGen();
        }

        public Household GetNextAgent(ModelDistribution g_x, 
            string dimension, Household prvAgent, SpatialZone currZone)
        {
            StateGenerator currStateGen = new StateGenerator();
            currStateGen.SetParameters(currZone.GetAverageIncome(),
                                            currZone.GetAverageIncome() * 2);
            double q_previous;
            double q_current = 0.00;
            double expIncome = Math.Log(prvAgent.GetIncome());
            KeyValDoublePair currPair = new KeyValDoublePair();
            //start with mean value
            currPair.category = currStateGen.GetMean();
            currPair.val = currStateGen.GetTransitionProbablity(currPair.category);

            for (int i = 0; i < Constants.WARMUP_ITERATIONS; i++)
            {
                q_previous = currStateGen.GetTransitionProbablity(
                               Math.Log((double)prvAgent.GetIncome()));
                q_current = currPair.val;
                IncomeLevel prevLvl = IncomeConvertor.ConvertValueToLevel(
                                        (uint) Math.Exp(expIncome));
                IncomeLevel currLvl = IncomeConvertor.ConvertValueToLevel(
                                        (uint) Math.Exp(currPair.category));
                double b_prev = g_x.GetValue(dimension, prevLvl.ToString(),
                    prvAgent.GetNewJointKey(dimension), currZone);
                double b_curr = g_x.GetValue(dimension, currLvl.ToString(),
                    prvAgent.GetNewJointKey(dimension), currZone);
                if (b_prev == 0.00)
                    b_prev = 0.0000001;
                if (b_curr == 0.00)
                    b_curr = 0.0000001;
                if (q_current == 0.00)
                    q_current = 0.0000001;
                double comVal = (b_curr * q_previous) / (b_prev * q_current);
                if (comVal == 0.00)
                    comVal = 0.0000001;
                if (randGen.NextDouble() < comVal)
                {
                    expIncome = currPair.category;
                }
                currPair = currStateGen.GetNextState();
            }
            return prvAgent.CreateNewCopy((uint)Math.Exp(expIncome));
        }
    }
}
