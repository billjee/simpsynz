
package Samplers;

import SimulationObjects.*;
import SimulationObjects.AgentAttributes.*;
import SimulationObjects.Household.*;
import Utils.*;
import java.io.IOException;
import java.util.ArrayList;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

public class GibbsSampler extends Sampler
{
        int agentIDCounter;
        public void SetAgentCounter(int cntr)
        {
            agentIDCounter = cntr;
        }
        public int GetAgentCounter()
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
        public ArrayList GenerateAgents(SpatialZone currZone, int numAgents,
                        SimulationObject initAgent, boolean warmUpStatus,
                        ArrayList mobelCond,
                        OutputFileWritter currWriter) throws IOException
        {
            if (initAgent.GetAgentType() == AgentType.Household)
            {
                return GenerateHousholds(currZone, numAgents,
                                    (Household) initAgent, warmUpStatus,
                                        mobelCond, currWriter);
            }
            if (initAgent.GetAgentType() == AgentType.Person)
            {
                return GeneratePersons(currZone, numAgents,
                                    (Person) initAgent, warmUpStatus,
                                        currWriter);
            }
            return null;
        }


        private ArrayList GenerateHousholds(SpatialZone currZone, int numHousehold,
                        Household initAgent, boolean warmUpStatus,
                        ArrayList mobelCond,
                        OutputFileWritter currWriter) throws IOException
        {
            int seltdDim = 0;
            ArrayList condList = currZone.GetDataHhldCollectionsList();
            condList.add(mobelCond.get(0));
            condList.add(mobelCond.get(1));
            condList.add(mobelCond.get(2));
            ArrayList generatedAgents = new ArrayList();
            Household prevAgent = initAgent;

            ImportanceSampler currImpSampler = new ImportanceSampler();
            MetropolisHasting currMHSampler = new MetropolisHasting();
            int iter = 0;
            if (warmUpStatus == true)
            {
                iter = Utils.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Utils.SKIP_ITERATIONS * numHousehold;
            }
            Household newAgent = new Household();
            for (int i = 0; i < iter; i++)
            {
                seltdDim = randGen.NextInRange(0, condList.size() - 1);

                ConditionalDistribution currDist =
                    (ConditionalDistribution)condList.get(seltdDim);

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
                if (currDist.GetDimensionName().equals("HouseholdSize"))
                {
                    newAgent = (Household)currImpSampler.GetNextAgent(
                        currZone.GetHousholdSizeDist(),
                        currDist, currDist.GetDimensionName(),
                        prevAgent, currZone);
                }
                else
                {
                    ArrayList currComm = currDist.GetCommulativeValue(
                        prevAgent.GetNewJointKey(currDist.GetDimensionName())
                        , currZone);
                    newAgent = (Household)GenerateNextAgent(currComm, prevAgent,
                        currDist.GetDimensionName());
                }

                prevAgent = newAgent;
                if (warmUpStatus == false && (i % Utils.SKIP_ITERATIONS == 0))
                {
                    newAgent.SetID(agentIDCounter);
                    agentIDCounter++;
                    generatedAgents.add(newAgent);
                    int currIncome = IncomeConvertor.GetEuroIncome((int)
                                        newAgent.GetIncome());

                    String currStrAgent = newAgent.GetID()
                        + "," + newAgent.GetZoneID()
                        + "," + currZone.GetEPFLName()
                        + "," + String.valueOf((int)newAgent.GetHhldSize())
                        + "," + String.valueOf((int)newAgent.GetNumOfWorkers())
                        + "," + String.valueOf((int)newAgent.GetNumOfKids())
                        + "," + String.valueOf((int)newAgent.GetNumOfUnivDegree())
                        + "," + String.valueOf((int)newAgent.GetIncomeLevel())
                        + "," + String.valueOf((int)newAgent.GetNumOfCars())
                        + "," + String.valueOf((int)newAgent.GetDwellingType());
                    currWriter.WriteToFile(currStrAgent);
                    //Console.WriteLine(currStrAgent);
                }
            }
            return generatedAgents;
        }

        private ArrayList GeneratePersons(SpatialZone currZone, int numPerson,
                            Person initAgent, boolean warmUpStatus,
                            OutputFileWritter currWriter) throws IOException
        {
            int seltdDim = 0;
            ArrayList condList = currZone.GetPersonDataCollectionsList();
            ArrayList generatedAgents = new ArrayList();
            Person prevAgent = initAgent;
            ImportanceSampler currImpSampler = new ImportanceSampler();

            int iter = 0;
            if (warmUpStatus == true)
            {
                iter = Utils.WARMUP_ITERATIONS;
            }
            else
            {
                iter = Utils.SKIP_ITERATIONS * numPerson;
            }
            Person newAgent = new Person();
            for (int i = 0; i < iter; i++)
            {
                seltdDim = randGen.NextInRange(0, condList.size() - 1);

                DiscreteCondDistribution currDist =
                    (DiscreteCondDistribution)condList.get(seltdDim);

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
                else*/ if (currDist.GetDimensionName().equals("Sex"))
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
                    ArrayList currComm = currDist.GetCommulativeValue(
                         prevAgent.GetNewJointKey(currDist.GetDimensionName())
                            , currZone);
                    newAgent = (Person)GenerateNextAgent(currComm,
                            (SimulationObject)prevAgent,
                            currDist.GetDimensionName());
                }

                prevAgent = newAgent;
                if (warmUpStatus == false && (i % Utils.SKIP_ITERATIONS == 0))
                {
                    newAgent.SetID(agentIDCounter);
                    agentIDCounter++;
                    generatedAgents.add(newAgent);

                    String currStrAgent = newAgent.GetID()
                        + "," + newAgent.GetZoneID()
                        + "," + String.valueOf((int)newAgent.GetAge())
                        + "," + String.valueOf((int)newAgent.GetSex())
                        + "," + String.valueOf((int)newAgent.GetHhldSize())
                        + "," + String.valueOf((int)newAgent.GetEducationLevel());

                    currWriter.WriteToFile(currStrAgent);
                    //Console.WriteLine(currStrAgent);
                }
            }
            return generatedAgents;
        }

        // Should generate a deep copy of self
        private SimulationObject GenerateNextAgent(ArrayList curCom,
            SimulationObject prvAgnt, String genDim)
        {
            double currMax = (double)
                ((KeyValPair)curCom.get(curCom.size() - 1)).value;
            if (currMax != 0.00)
            {
                double randVal = randGen.NextDoubleInRange(0, currMax);
                for (int i = 0; i < curCom.size(); i++)
                {
                    if (randVal <= ((KeyValPair)curCom.get(1)).value)
                    {
                        return prvAgnt.CreateNewCopy(genDim, i);
                    }
                }

            }
            else
            {
                return prvAgnt.CreateNewCopy(genDim,
                    randGen.NextInRange(0, (curCom.size() - 1)));
            }
            return null;
        }
}
