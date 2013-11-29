
package Samplers;

import SimulationObjects.*;
import SimulationObjects.AgentAttributes.*;
import SimulationObjects.Household.*;
import Utils.*;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */
public class MetropolisHasting extends Sampler
{
    public static class KeyValDoublePair
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
            double meuSqr = Math.pow(meu,2);
            double sigSqr = Math.pow(sigma,2);

            mean = Math.log(meuSqr/Math.pow((meuSqr+sigSqr),0.5));
            stdev = Math.pow(Math.log(Math.pow((sigma/meu),2)+1),0.5);

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
                        * (Math.exp(-Math.pow(nxtVal - mean, 2)
                        / (2 * Math.pow(stdev,2))));
        }
    }

        public MetropolisHasting()
        {
            randGen = new RandomNumberGen();
        }

        public Household GetNextAgent(ModelDistribution g_x,
            String dimension, Household prvAgent, SpatialZone currZone)
        {
            StateGenerator currStateGen = new StateGenerator();
            currStateGen.SetParameters(currZone.GetAverageIncome(),
                                            currZone.GetAverageIncome() * 2);
            double q_previous;
            double q_current = 0.00;
            double expIncome = Math.log(prvAgent.GetIncome());
            KeyValDoublePair currPair = new KeyValDoublePair();
            //start with mean value
            currPair.category = currStateGen.GetMean();
            currPair.val = currStateGen.GetTransitionProbablity(currPair.category);

            for (int i = 0; i < Utils.WARMUP_ITERATIONS; i++)
            {
                q_previous = currStateGen.GetTransitionProbablity(
                               Math.log((double)prvAgent.GetIncome()));
                q_current = currPair.val;
                int prevLvl = IncomeConvertor.ConvertValueToLevel(
                                        (int) Math.exp(expIncome));
                int currLvl = IncomeConvertor.ConvertValueToLevel(
                                        (int) Math.exp(currPair.category));
                double b_prev = g_x.GetValue(dimension, String.valueOf(prevLvl),
                    prvAgent.GetNewJointKey(dimension), currZone);
                double b_curr = g_x.GetValue(dimension, String.valueOf(currLvl),
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
            return prvAgent.CreateNewCopy((int)Math.exp(expIncome));
        }
}
