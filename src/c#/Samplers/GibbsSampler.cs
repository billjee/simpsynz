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
using SimulationObjects;
using PopulationSynthesis.Utils;

namespace Samplers
{
    class GibbsSampler : Sampler
    {
        uint agentIDCounter;
        public void SetAgentCounter(uint cntr)
        {
            agentIDCounter = cntr;
        }
        public uint GetAgentCounter()
        {
            return agentIDCounter;
        }

        private ImportanceSampler myImportantSampler;
        private MetropolisHasting myMHSampler;

        public GibbsSampler()
        {
            warmupTime = 0;
            samplingInterval = 0;
            agentIDCounter = 0;
            myImportantSampler = new ImportanceSampler();
            myMHSampler = new MetropolisHasting();
            Initialze();
        }

        public GibbsSampler(int warmup, int samplingIntv)
        {
            warmupTime = warmup;
            samplingInterval = samplingIntv;
            agentIDCounter = 0;
            Initialze();
        }

        public GibbsSampler(int warmup, int samplingIntv, int randSeed)
        {
            warmupTime = warmup;
            samplingInterval = samplingIntv;
            agentIDCounter = 0;
            Initialze(randSeed);
        }

        // the method assumes that the conditionals are full conditionals
        // The data processing has already been done
        // [BF] The method should be changed so that it can generate any
        //      kind of agents
        public List<SimulationObject> GenerateAgents(SpatialZone currZone, int numAgents,
                        SimulationObject initAgent, bool warmUpStatus,
                        List<ConditionalDistribution> mobelCond,
                        OutputFileWriter currWriter)
        {
            switch(initAgent.GetAgentType())
            {
                case AgentType.Household:
                return GenerateHousholds(currZone, numAgents,
                                    (Household)initAgent, warmUpStatus,
                                        mobelCond, currWriter);
                case AgentType.Person:
                    return GeneratePersons(currZone, numAgents,
                                    (Person)initAgent, warmUpStatus,
                                        currWriter);
            }
            return null;
        }


        private List<SimulationObject> GenerateHousholds(SpatialZone currZone, int numHousehold,
                        Household initAgent, bool warmUpStatus,
                        List<ConditionalDistribution> mobelCond,
                        OutputFileWriter currWriter)
        {
            int seltdDim = 0;
            List<ConditionalDistribution> condList = currZone.GetDataHhldCollectionsList();
            for(int i = 0; i < mobelCond.Count; i++)
            {
                condList.Add(mobelCond[i]);
            }
            var generatedAgents = new List<SimulationObject>();
            Household prevAgent = initAgent;

            ImportanceSampler currImpSampler = new ImportanceSampler();
            MetropolisHasting currMHSampler = new MetropolisHasting();
            int iter = 0;
            if(warmUpStatus == true)
            {
                iter = Constants.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Constants.SKIP_ITERATIONS * numHousehold;
            }
            Household newAgent = new Household();
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < iter; i++)
            {
                seltdDim = randGen.NextInRange(0, condList.Count - 1);

                ConditionalDistribution currDist = condList[seltdDim];

                // If the selected distribution is dwelling/cars
                // call important sampling

                /*if (currDist.GetDimensionName() == "DwellingType")
                {
                    newAgent = currImpSampler.GetNextAgent(currZone.myDwellMarginal,
                        currDist, currDist.GetDimensionName(), 
                        prevAgent, currZone);
                }
                else if (currDist.GetDimensionName() == "NumOfCars")
                {
                    newAgent = currImpSampler.GetNextAgent(currZone.myCarsMarginal,
                        currDist, currDist.GetDimensionName(),
                        prevAgent, currZone);
                }*/

                // If the selected distribution is income
                // call MH
                //                else if (((ConditionalDistribution)condList[seltdDim])
                //                                .GetDimensionName() == "IncomeLevel")
                //                {
                //                    newAgent = myMHSampler.GetNextAgent((ModelDistribution)currDist,
                //                            currDist.GetDimensionName(), prevAgent, currZone);
                //                }
                if(currDist.GetDimensionName() == "HouseholdSize")
                {
                    newAgent = (Household)currImpSampler.GetNextAgent(
                        currZone.GetHousholdSizeDist(),
                        currDist, currDist.GetDimensionName(),
                        prevAgent, currZone);
                }
                else
                {
                    var currComm = currDist.GetCommulativeValue(
                        prevAgent.GetNewJointKey(currDist.GetDimensionName())
                        , currZone);
                    newAgent = (Household)GenerateNextAgent(currComm, prevAgent,
                        currDist.GetDimensionName());
                }

                prevAgent = newAgent;
                if(warmUpStatus == false && (i % Constants.SKIP_ITERATIONS == 0))
                {
                    generatedAgents.Add(newAgent);
                    uint currIncome = IncomeConvertor.GetEuroIncome((uint)
                                        newAgent.GetIncome());
                    builder.Append(newAgent.GetZoneID()); builder.Append(',');
                    builder.Append(currZone.GetEPFLName()); builder.Append(',');
                    builder.Append((int)newAgent.GetHhldSize()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfWorkers()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfKids()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfUnivDegree()); builder.Append(',');
                    builder.Append((int)newAgent.GetIncomeLevel()); builder.Append(',');
                    builder.Append((int)newAgent.GetNumOfCars()); builder.Append(',');
                    builder.Append((int)newAgent.GetDwellingType());
                    currWriter.WriteToFile(builder.ToString());
                    builder.Clear();
                }
            }
            return generatedAgents;
        }

        private List<SimulationObject> GeneratePersons(SpatialZone currZone, int numPerson,
                            Person initAgent, bool warmUpStatus,
                            OutputFileWriter currWriter)
        {
            int seltdDim = 0;
            List<ConditionalDistribution> condList = currZone.GetPersonDataCollectionsList();
            var generatedAgents = new List<SimulationObject>();
            Person prevAgent = initAgent;
            ImportanceSampler currImpSampler = new ImportanceSampler();

            int iter = 0;
            if(warmUpStatus == true)
            {
                iter = Constants.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Constants.SKIP_ITERATIONS * numPerson;
            }
            Person newAgent = new Person();
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < iter; i++)
            {
                seltdDim = randGen.NextInRange(0, condList.Count - 1);

                DiscreteCondDistribution currDist =
                    (DiscreteCondDistribution)condList[seltdDim];

                /*if (currDist.GetDimensionName() == "HouseholdSize2")
                {
                    newAgent = (Person) currImpSampler.GetNextAgent(
                                currZone.myHhldSize2Marginal,
                                currDist, currDist.GetDimensionName(),
                                (SimulationObject) prevAgent, currZone);
                }
                else if (currDist.GetDimensionName() == "Age")
                {
                    newAgent = (Person)currImpSampler.GetNextAgent(
                                currZone.myAgeMarginal,
                                currDist, currDist.GetDimensionName(),
                                (SimulationObject)prevAgent, currZone);
                }
                else*/
                if(currDist.GetDimensionName() == "Sex")
                {
                    newAgent = (Person)currImpSampler.GetNextAgent(
                                currZone.mySexMarginal,
                                currDist, currDist.GetDimensionName(),
                                (SimulationObject)prevAgent, currZone);
                }
                /*else if (currDist.GetDimensionName() == "EducationLevel")
                {
                    newAgent = (Person)currImpSampler.GetNextAgent(
                                currZone.myEducationMarginal,
                                currDist, currDist.GetDimensionName(),
                                (SimulationObject)prevAgent, currZone);
                }*/
                else
                {
                    List<KeyValPair> currComm = currDist.GetCommulativeValue(
                         prevAgent.GetNewJointKey(currDist.GetDimensionName())
                            , currZone);
                    newAgent = (Person)GenerateNextAgent(currComm,
                            (SimulationObject)prevAgent,
                            currDist.GetDimensionName());
                }

                prevAgent = newAgent;
                if(warmUpStatus == false && (i % Constants.SKIP_ITERATIONS == 0))
                {
                    //generatedAgents.Add(newAgent);
                    builder.Append((int)newAgent.GetAge()); builder.Append(',');
                    builder.Append(newAgent.GetZoneID()); builder.Append(',');
                    builder.Append((int)newAgent.GetSex()); builder.Append(',');
                    builder.Append((int)newAgent.GetHhldSize()); builder.Append(',');
                    builder.Append((int)newAgent.GetEducationLevel());
                    currWriter.WriteToFile(builder.ToString());
                    builder.Clear();
                }
            }
            return generatedAgents;
        }

        // Should generate a deep copy of self
        private SimulationObject GenerateNextAgent(List<KeyValPair> curCom,
            SimulationObject prvAgnt, string genDim)
        {
            double currMax = (double)
                ((KeyValPair)curCom[curCom.Count - 1]).Value;
            if(currMax != 0.00)
            {
                double randVal = randGen.NextDoubleInRange(0, currMax);
                for(int i = 0; i < curCom.Count; i++)
                {
                    if(randVal <= ((KeyValPair)curCom[i]).Value)
                    {
                        return prvAgnt.CreateNewCopy(genDim, i);
                    }
                }

            }
            else
            {
                return prvAgnt.CreateNewCopy(genDim,
                    randGen.NextInRange(0, (curCom.Count - 1)));
            }
            return null;
        }
    }
}
