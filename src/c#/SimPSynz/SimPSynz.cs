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
//using IPF;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            AgentType curTyp = new AgentType();
            curTyp = AgentType.Person;
            CreateUsingSimulation(curTyp);
            //CreateConditionalsFromSample();
            //CreateUsingIPF(curTyp);

            ////////////////////////////////
            ////////////////////////////////
            World myWorld = new World();
            for (int i = 0; i < 20; i++)
            {
                myWorld.CreatePersonPopulation(Constants.DATA_DIR
                    + "\\Person\\ConditionalExperiments"
                    + "\\NoSexConditional\\AgeHhldSizeEdu"
                    + "\\SyntheticPerson_AgeHhldSizeEdu.csv",
                    Constants.DATA_DIR
                    + "\\Person\\ConditionalExperiments"
                    + "\\NoSexConditional\\AgeHhldSizeEdu"
                    + "\\SyntheticPersonRealization_AgeHhldSizeEdu" + i + ".csv", 28533);
            }
        }

/*        private static void CreateUsingIPF(AgentType currType)
        {

            IPFWorld currIPFWorld = new IPFWorld();
            //currIPFWold.Initialize();
            //currIPFWorld.CreatePopulationFromContTable1(Constants.DATA_DIR
            //    +"IPF\\ContTabOut_20Per_CH1004.csv", Constants.DATA_DIR
            //    +"IPF\\SyntheticPopulation_20Per_CH1004.csv");
            currIPFWorld.ClonePopulationFromSample(Constants.DATA_DIR
                + "IPF\\ContTabOut_01Per_r1.csv"
                , Constants.DATA_DIR
                + "IPF\\CH1004_01Per_Census2000.csv"
                , Constants.DATA_DIR
                + "IPF\\Output\\SyntheticPopulation_01Per_CH1004_r1.csv");
        }
*/
        private static void CreateUsingSimulation(AgentType currType)
        {
            if (currType == AgentType.Household)
            {
                CreateHhldViaSimulation();
            }
            else if (currType == AgentType.Person)
            {
                CreatePersonViaSimulation();
            }
        }

        private static void CreatePersonViaSimulation()
        {
            World currWorld = new World();
            currWorld.Initialize(true,AgentType.Person);
            currWorld.CreatePersonPopulationPool(
                Constants.DATA_DIR + "\\Person\\ConditionalExperiments"
                + "\\NoSexConditional\\AgeHhldSizeEdu"
                + "\\SyntheticPerson_AgeHhldSizeEdu.csv");
        }

        private static void CreateHhldViaSimulation()
        {
            World currWorld = new World();
            
            currWorld.Initialize(false,AgentType.Household);
            //currWorld.CreateHoseholdPopulationPool(Constants.DATA_DIR 
            //    + "Household\\SyntheticHhld.csv");

            //////////////////////////////////////////////
            //// Realization of a population
            /*for (int i = 0; i < 1; i++)
            {
                currWorld.CreatePopulationByDwellingType((int)DateTime.Now.Ticks,
                                    Constants.DATA_DIR + "Household\\SyntheticHhld.csv",
                                    Constants.DATA_DIR + "Household\\PopRealization" + i.ToString() + ".csv");
            }*/

            //// Commune Level Statistics
            //for (int i = 0; i < 1; i++)
            //{
            //    currWorld.ComputeCommuneLevelStatisticsPeople(Constants.DATA_DIR
            //            + "PopRealization" + i + ".csv",
            //            Constants.DATA_DIR + "ComunePerStats_NoIS" + i + ".csv",
            //            Constants.DATA_DIR + "CommuneList.csv");
            //    currWorld.ComputeCommuneLevelStatisticsCars(Constants.DATA_DIR
            //            + "PopRealization" + i + ".csv",
            //            Constants.DATA_DIR + "ComuneCarStats_NoIS" + i + ".csv",
            //            Constants.DATA_DIR + "CommuneList.csv");
            //}

            /*int[] currDimCat = { 6,3,3,3,5,4,4};
            for (int i = 0; i < 1; i++)
            {
                for (int j = 2; j < 8; j++)
                {
                    currWorld.ComputeSectorLevelStatistics(Constants.DATA_DIR
                            + "PopRealization" + i + ".csv", 9 , j,currDimCat[j-2]);
                }
            }*/

            //////////////////////////////////////////////

            ///////////////////////
            var runsListZero = new List<Dictionary<string, World.ZonalStat>>();
            var runsListOne = new List<Dictionary<string, World.ZonalStat>>();
            var runsListTwo = new List<Dictionary<string, World.ZonalStat>>();
            var runsListThree = new List<Dictionary<string, World.ZonalStat>>();

            for (int i = 0; i < 50; i++)
            {
                var CurrTotals = currWorld.ComputeCommuneMCStatsCars(
                                                i, (int) DateTime.Now.Ticks,
                                                Constants.DATA_DIR
                                     + "Household\\SyntheticHhld.csv", false);
                runsListZero.Add(CurrTotals[0]);
                runsListOne.Add(CurrTotals[1]);
                runsListTwo.Add(CurrTotals[2]);
                runsListThree.Add(CurrTotals[3]);
            }

            currWorld.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListZero, 0);
            currWorld.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListOne, 1);
            currWorld.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListTwo, 2);
            currWorld.WriteMCStatsToFile(Constants.DATA_DIR +
                        "Household\\CommuneList.csv", runsListThree, 3);
            //////////////////////////////


            //ConditionalGenerator myCondGen = new ConditionalGenerator();
            //    myCondGen.GenerateConditionals(
            //    Constants.DATA_DIR + "MobelDataset.csv",
            //    Constants.DATA_DIR + "MobelDimensions.csv");
        }

        private static void CreateConditionalsFromSample()
        {
            ConditionalGenerator currGen 
                = new ConditionalGenerator();
            currGen.GenerateConditionals(
                Constants.DATA_DIR + "Person\\ConditionalExperiments"
                + "\\CH1004_Population_Census2000.csv",
                Constants.DATA_DIR + "Person\\ConditionalExperiments"
                +"\\CensusDimDesc.csv");
        }
    }
}
