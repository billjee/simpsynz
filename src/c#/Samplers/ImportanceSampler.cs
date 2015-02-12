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
using PopulationSynthesis.Utils;
using SimulationObjects;

namespace Samplers
{
    class ImportanceSampler : Sampler
    {
        RandomNumberGen myRand;
        public ImportanceSampler()
        {
            myRand = new RandomNumberGen();
        }

        public SimulationObject GetNextAgent(DiscreteMarginalDistribution f_x,
            ConditionalDistribution g_x, string dimension,
            SimulationObject prvAgent, SpatialZone currZone)
        {
            switch(prvAgent.GetAgentType())
            {
                case AgentType.Household:
                    return GetNextAgentHousehold(
                    f_x, g_x, dimension, (Household)prvAgent, currZone);
                case AgentType.Person:
                    return GetNextAgentPerson(
                    f_x, g_x, dimension, (Person)prvAgent, currZone);
                default:
                    return null;
            }
        }

        public Household GetNextAgentHousehold(DiscreteMarginalDistribution f_x, 
            ConditionalDistribution g_x, string dimension, 
            Household prvAgent, SpatialZone currZone)
        {
            KeyValPair currDimVal;
            double currProb = 0.00;
            double currRatio = 0.00;
            int cnt = 0;
            do{
                currDimVal = GenerateNextFromG_X(g_x, prvAgent,currZone);
                currProb = f_x.GetValue(currDimVal.Category);
                currRatio = currProb / currDimVal.Value;
                if (currRatio > 1.00)
                {
                    currRatio = 1.00;
                }
                if (cnt > 1000)
                {
                    currRatio = 1;
                }
                cnt++;
            } while (myRand.NextDouble() > currRatio);

            // Create the household object based on the currDimVal.category
            return (Household) prvAgent.CreateNewCopy(g_x.GetDimensionName(), Int16.Parse(currDimVal.Category));
        }

        public Person GetNextAgentPerson(DiscreteMarginalDistribution f_x,
            ConditionalDistribution g_x, string dimension,
                Person prvAgent, SpatialZone currZone)
        {
            KeyValPair currDimVal;
            double currProb = 0.00;
            double currRatio = 0.00;
            int cnt = 0;
            do
            {
                currDimVal = GenerateNextFromG_X(g_x, prvAgent, currZone);
                currProb = f_x.GetValue(currDimVal.Category);
                currRatio = currProb / currDimVal.Value;
                if (currRatio > 1.00)
                {
                    currRatio = 1.00;
                }
                if (cnt > 10000)
                {
                    currRatio = 1;
                }
                cnt++;
            } while (myRand.NextDouble() > currRatio);

            // Create the household object based on the currDimVal.category
            return (Person)prvAgent.CreateNewCopy(
                g_x.GetDimensionName(), Int16.Parse(currDimVal.Category));
        }

        private KeyValPair GenerateNextFromG_X(ConditionalDistribution curG_X, 
                                SimulationObject prvAgent, SpatialZone currZone)
        {
            List <KeyValPair> curCom = curG_X.GetCommulativeValue(
                prvAgent.GetNewJointKey(curG_X.GetDimensionName())
                    , currZone);

            double randVal = myRand.NextDoubleInRange(0, (double)
                ((KeyValPair)curCom[curCom.Count - 1]).Value);
            for (int i = 0; i < curCom.Count; i++)
            {
                if (randVal <= ((KeyValPair)curCom[i]).Value)
                {
                    KeyValPair myPair;
                    myPair.Category = ((KeyValPair)curCom[i]).Category;
                    myPair.Value = curG_X.GetValue(curG_X.GetDimensionName(),
                            myPair.Category,
                            prvAgent.GetNewJointKey(curG_X.GetDimensionName()),
                            currZone);
                    return myPair;
                }
            }
            return new KeyValPair();
        }

    }
}
