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
using System.IO;
using PopulationSynthesis.Utils;
using Samplers;

namespace SimulationObjects
{
    class World : SimulationObject
    {
        private DiscreteCondDistribution mobelWrkrsConditionals;
        private DiscreteCondDistribution mobelKidsConditionals;
        private DiscreteCondDistribution mobelPersConditionals;

        private InputDataReader CensusDwellFileReader;
        private InputDataReader CensusCarFileReader;
        private InputDataReader CensusPersonFileReader;
        private InputDataReader censusUnivDegFileReader;

        private InputDataReader CensusAgeFileReader;
        private InputDataReader CensusSexFileReader;
        private InputDataReader CensusHhldSizeFileReader;
        private InputDataReader CensusEduLevelFileReader;


        private OutputFileWritter agentsOutputFile;

        private Hashtable myZonalCollection;
        private ArrayList myHhldsPool;
        private ArrayList myPersonPool;

        private static uint idCounter = 0;

        private Hashtable zonalControlTotals;
        private GibbsSampler myGibbsSampler;

        public World()
        {
            myID = ++idCounter;
            myHhldsPool = new ArrayList();
            myPersonPool = new ArrayList();
            myZonalCollection = new Hashtable();

            myGibbsSampler = new GibbsSampler();
        }

        public void Initialize(bool createPool, AgentType currType)
        {
            InitializeInputData(currType);
            if(createPool == true)
            {
                LoadZones(currType);
                LoadZonalData(currType);
            }
            else
            {
                if(currType != AgentType.Person)
                {
                    using (InputDataReader currReader = new InputDataReader(Constants.DATA_DIR
                                + "Household\\CensusHhldCountByDwell.csv"))
                    {
                        zonalControlTotals = new Hashtable();
                        currReader.FillControlTotalsByDwellType(zonalControlTotals);
                    }
                }
            }
        }

        private void InitializeInputData(AgentType currType)
        {
            if(currType == AgentType.Household)
            {
                mobelWrkrsConditionals = new DiscreteCondDistribution();
                mobelKidsConditionals = new DiscreteCondDistribution();
                mobelPersConditionals = new DiscreteCondDistribution();
            }
        }

        public void LoadZonalData(AgentType currType)
        {
            if(currType == AgentType.Household)
            {
                LoadMobelData();
                LoadMarginalsForCars();
                LoadMarginalsForDwellings();
                LoadMarginalsForPersons();
            }
            else if(currType == AgentType.Person)
            {
                foreach(DictionaryEntry ent in myZonalCollection)
                {
                    OpenCensusFiles(currType);
                    SpatialZone currZone = (SpatialZone)ent.Value;
                    LoadPesronCensusData(currZone);
                    CloseCensusFiles(currType);
                }
                LoadMarginalsForHhldSize2();
                LoadMarginalsForAge();
                LoadMarginalsForSex();
                LoadMarginalsForEducation();
            }
            GC.KeepAlive(myZonalCollection);
        }

        void LoadPesronCensusData(SpatialZone currZone)
        {
            currZone.myAgeConditional.FlushOutData();
            CensusAgeFileReader.
              FillCollection2(currZone.myAgeConditional);
            currZone.myAgeConditional.SetDimensionName("Age");

            currZone.mySexConditional.FlushOutData();
            CensusSexFileReader.
              FillCollection2(currZone.mySexConditional);
            currZone.mySexConditional.SetDimensionName("Sex");

            currZone.myHhldSizeConditional.FlushOutData();
            CensusHhldSizeFileReader.
              FillCollection2(currZone.myHhldSizeConditional);
            currZone.myHhldSizeConditional.SetDimensionName("HouseholdSize2");

            currZone.myEduLevelConditional.FlushOutData();
            CensusEduLevelFileReader.
              FillCollection2(currZone.myEduLevelConditional);
            currZone.myEduLevelConditional.SetDimensionName("EducationLevel");
        }

        void LoadMobelData()
        {
            using (var mobelWrkrsFileReader = new InputDataReader(
                Constants.DATA_DIR + "Household\\MobelNbWrkr.csv"))
            {
                mobelWrkrsConditionals.FlushOutData();
                mobelWrkrsFileReader.FillCollection2(mobelWrkrsConditionals);
                mobelWrkrsConditionals.SetDimensionName("NumOfWorkers");
            }

            using (var mobelKidsFileReader = new InputDataReader(
                Constants.DATA_DIR + "Household\\MobelNbKids.csv"))
            {
                mobelKidsConditionals.FlushOutData();
                mobelKidsFileReader.FillCollection2(mobelKidsConditionals);
                mobelKidsConditionals.SetDimensionName("NumOfKids");
            }


            using (var mobelPersFileReader = new InputDataReader(
                Constants.DATA_DIR + "Household\\MobelNbPers.csv"))
            {
                mobelPersConditionals.FlushOutData();
                mobelPersFileReader.FillCollection2(mobelPersConditionals);
                mobelPersConditionals.SetDimensionName("HouseholdSize");
            }
        }

        private void OpenCensusFiles(AgentType currType)
        {
            if(currType == AgentType.Household)
            {
                CensusPersonFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Household\\CensusNumOfPers.csv");
                CensusDwellFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Household\\CensusDwellingType.csv");
                CensusCarFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Household\\CensusNumOfCars.csv");

                CensusPersonFileReader.GetConditionalList();
                CensusDwellFileReader.GetConditionalList();
                CensusCarFileReader.GetConditionalList();
            }
            else if(currType == AgentType.Person)
            {
                CensusAgeFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Person\\ConditionalExperiments"
                    + "\\NoSexConditional\\CensusAge.csv");

                CensusSexFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Person\\ConditionalExperiments"
                    + "\\NoSexConditional\\CensusSex.csv");

                CensusHhldSizeFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Person\\ConditionalExperiments"
                    + "\\NoSexConditional\\CensusHouseholdSize2.csv");

                CensusEduLevelFileReader = new InputDataReader(
                    Constants.DATA_DIR + "\\Person\\ConditionalExperiments"
                    + "\\NoSexConditional\\CensusEducationLevel.csv");
                CensusEduLevelFileReader.GetConditionalList();
            }
        }

        void CloseCensusFiles(AgentType currType)
        {
            if(currType == AgentType.Household)
            {

                CensusDwellFileReader.Dispose();
                CensusCarFileReader.Dispose();
                CensusPersonFileReader.Dispose();
            }
            else if(currType == AgentType.Person)
            {
                CensusAgeFileReader.Dispose();
                CensusSexFileReader.Dispose();
                CensusHhldSizeFileReader.Dispose();
                CensusEduLevelFileReader.Dispose();
            }
        }

        public void CreateHoseholdPopulationPool(string fileName)
        {
            using (var agentsOutputFile = new OutputFileWritter(fileName))
            {
                uint agentsCreated = 1;
                uint counter = 0;
                ArrayList mobelCond = new ArrayList();
                mobelCond.Add((ConditionalDistribution)mobelWrkrsConditionals);
                mobelCond.Add((ConditionalDistribution)mobelKidsConditionals);
                mobelCond.Add((ConditionalDistribution)mobelPersConditionals);

                foreach(DictionaryEntry entry in myZonalCollection)
                {
                    SpatialZone currZone = (SpatialZone)entry.Value;
                    // warmup time
                    myGibbsSampler.GenerateAgents(currZone,
                                    Constants.WARMUP_ITERATIONS,
                                    new Household(currZone.GetName()), true,
                                    mobelCond,
                                    agentsOutputFile);
                    myHhldsPool.Clear();
                    myGibbsSampler.SetAgentCounter(agentsCreated + counter);
                    // actual generation
                    myHhldsPool = myGibbsSampler.GenerateAgents(currZone,
                                    Constants.POOL_COUNT,
                                    new Household(currZone.GetName()), false,
                                    mobelCond,
                                    agentsOutputFile);
                    agentsCreated += (uint)myHhldsPool.Count;
                }
            }
        }

        public void CreatePersonPopulationPool(string fileName)
        {
            using (var agentsOutputFile = new OutputFileWritter(fileName))
            {
                uint agentsCreated = 1;
                uint counter = 0;
                ArrayList mobelCond = new ArrayList();

                foreach(DictionaryEntry entry in myZonalCollection)
                {
                    SpatialZone currZone = (SpatialZone)entry.Value;
                    if(currZone.GetName() != "1004")
                    {
                        continue;
                    }
                    // warmup time
                    myGibbsSampler.GenerateAgents(currZone,
                                    Constants.WARMUP_ITERATIONS,
                                    new Person(currZone.GetName()), true,
                                    mobelCond,
                                    agentsOutputFile);
                    myPersonPool.Clear();
                    myGibbsSampler.SetAgentCounter(agentsCreated + counter);
                    // actual generation
                    myPersonPool = myGibbsSampler.GenerateAgents(currZone,
                                    Constants.POOL_COUNT,
                                    new Person(currZone.GetName()), false,
                                    mobelCond,
                                    agentsOutputFile);
                    agentsCreated += (uint)myPersonPool.Count;
                }
            }
        }

        public void LoadZones(AgentType CurrType)
        {
            if(CurrType == AgentType.Household)
            {
                using (var currReader = new InputDataReader(
                    Constants.DATA_DIR + "Household\\CensusZonalData.csv"))
                {
                    currReader.FillZonalData(myZonalCollection);
                }
            }
            else if(CurrType == AgentType.Person)
            {
                CreateSpatialZones();
            }
        }

        private void CreateSpatialZones()
        {
            SpatialZone currZone = new SpatialZone();
            currZone.SetName("1004");
            myZonalCollection.Add("1004", currZone);

            currZone = new SpatialZone();
            currZone.SetName("1007");
            myZonalCollection.Add("1007", currZone);

            currZone = new SpatialZone();
            currZone.SetName("1018");
            myZonalCollection.Add("1018", currZone);

            currZone = new SpatialZone();
            currZone.SetName("1020");
            myZonalCollection.Add("1020", currZone);
        }

        public void CreatePopulationByDwellingType(int seed, string poolfileName,
                string fileName)
        {
            ArrayList hhldPool = GetHouseholdPoolForClonning(poolfileName);

            using (InputDataReader currReader = new InputDataReader(poolfileName))
            using (OutputFileWritter currOutputFile = new OutputFileWritter(fileName))
            {
                ArrayList currPool = new ArrayList();
                RandomNumberGen currRandGen = new RandomNumberGen(seed);
                currOutputFile.WriteToFile("HhldID,SectorID,HhldSize,NbOfWorkers,"
                            + "NbofKids,NbofUnivDegree,IncLvl,NumbCars,"
                            + "DwellTyp(PopSynt),EPFL_SectorID,BuildingID");
                int totSingle = 0;
                int totSemi = 0;
                int totDb = 0;
                int toApt = 0;

                while(currReader.LoadZonalPopulationPool(currPool) == true)
                {
                    string[] currStrTok = ((string)currPool[0]).Split(',');
                    int indx = 0;

                    string currKey = "";
                    if(zonalControlTotals.ContainsKey(currStrTok[1]))
                    {
                        // for each type of dwelling
                        for(int i = 0; i < 4; i++)
                        {
                            ArrayList currDwellHhld = new ArrayList();
                            foreach(string currHhld in currPool)
                            {
                                string[] currHhldVal = currHhld.Split(',');
                                if(currHhldVal[(currHhldVal.Length - 1)] == i.ToString())
                                {
                                    currDwellHhld.Add(currHhld);
                                }
                            }
                            currKey = (string)currStrTok[1];
                            string[] contTotStr = (string[])zonalControlTotals[currKey];

                            // number of dwellings of certain type
                            indx = int.Parse(contTotStr[i + 2]);
                            string ZnID = contTotStr[0];
                            string ZnEFPLID = contTotStr[1];
                            string bldId = "0" + (i + 1).ToString();

                            if(indx > currDwellHhld.Count)
                            {
                                indx = indx - currDwellHhld.Count;
                                for(int x = 0; x < currDwellHhld.Count; x++)
                                {
                                    string[] hhldValues = ((string)
                                            currDwellHhld[x]).Split(',');
                                    string currHhldStr = hhldValues[0] + "," + ZnID.Substring(0, 5)
                                                + "," + hhldValues[3] + "," + hhldValues[4]
                                                + "," + hhldValues[5] + "," + hhldValues[6]
                                                + "," + hhldValues[7] + "," + hhldValues[8]
                                                + "," + hhldValues[9] + ","
                                                + contTotStr[1] + "," + contTotStr[1] + bldId;
                                    currOutputFile.WriteToFile(currHhldStr);
                                    //Console.WriteLine(currHhldStr);
                                    if(i == 0) totSingle++;
                                    else if(i == 1) totSemi++;
                                    else if(i == 2) totDb++;
                                    else toApt++;
                                }
                                ArrayList currRandList = currRandGen.GetNNumbersInRange(0,
                                    hhldPool.Count - 1, indx);
                                for(int j = 0; j < currRandList.Count; j++)
                                {
                                    string[] hhldValues = ((string)
                                        hhldPool[(int)currRandList[j]]).Split(',');
                                    string currHhldStr = ZnID + i.ToString() + j.ToString()
                                            + "," + ZnID.Substring(0, 5)
                                            + "," + hhldValues[0] + "," + hhldValues[1]
                                            + "," + hhldValues[2] + "," + hhldValues[3]
                                            + "," + hhldValues[4] + "," + hhldValues[5]
                                            + "," + hhldValues[6] + ","
                                            + ZnEFPLID + "," + ZnEFPLID + bldId;
                                    currOutputFile.WriteToFile(currHhldStr);
                                    //Console.WriteLine(currHhldStr);
                                    if(i == 0) totSingle++;
                                    else if(i == 1) totSemi++;
                                    else if(i == 2) totDb++;
                                    else toApt++;
                                }
                            }
                            else
                            {
                                ArrayList currRandList = currRandGen.GetNNumbersInRange(0,
                                    currDwellHhld.Count - 1, indx);
                                for(int j = 0; j < currRandList.Count; j++)
                                {
                                    string[] hhldValues = ((string)
                                        currDwellHhld[(int)currRandList[j]]).Split(',');
                                    string currHhldStr = hhldValues[0] + "," + ZnID.Substring(0, 5)
                                            + "," + hhldValues[3] + "," + hhldValues[4]
                                            + "," + hhldValues[5] + "," + hhldValues[6]
                                            + "," + hhldValues[7] + "," + hhldValues[8]
                                            + "," + hhldValues[9] + ","
                                            + contTotStr[1] + "," + contTotStr[1] + bldId;
                                    currOutputFile.WriteToFile(currHhldStr);
                                    //Console.WriteLine(currHhldStr);
                                    if(i == 0) totSingle++;
                                    else if(i == 1) totSemi++;
                                    else if(i == 2) totDb++;
                                    else toApt++;
                                }
                            }
                        }
                    }
                    currPool.Clear();
                }
                Console.WriteLine("Total Detached:\t" + totSingle.ToString()
                              + "\nTotal SemiDetached:\t" + totSemi.ToString()
                              + "\nTotal Attached:\t" + totDb.ToString()
                              + "\nTotal Apartment:\t" + toApt.ToString());
            }
        }

        public ArrayList GetHouseholdPoolForClonning(string fileName)
        {
            ArrayList currArrayList = new ArrayList();
            ArrayList currPool = new ArrayList();
            using (var currReader = new InputDataReader(Constants.DATA_DIR + "Household\\SyntheticHhld.csv"))
            {
                RandomNumberGen currRand = new RandomNumberGen();
                while(currReader.LoadZonalPopulationPoolByType
                    (currPool, "3") == true)
                {
                    if(currArrayList.Count > 60000)
                    {
                        return currArrayList;
                    }

                    if(currPool.Count > 0)
                    {
                        int numB = (int)Math.Ceiling((currPool.Count * 0.1));
                        ArrayList curDrw = currRand.GetNNumbersInRange(
                                                0, currPool.Count - 1, (numB));
                        if(curDrw.Count > 0)
                        {
                            for(int i = 0; i < numB; i++)
                            {
                                currArrayList.Add(currPool[(int)curDrw[i]]);
                            }
                        }
                    }
                    currPool.Clear();
                }
            }
            return currArrayList;
        }

        public void CreateHouseholdPopulation()
        {
            using (var currReader = new InputDataReader(
                Constants.DATA_DIR + "SyntheticHhld_withourImpSamp.csv"))
            {
                ArrayList currPool = new ArrayList();
                RandomNumberGen currRandGen = new RandomNumberGen();
                using (var currOutputFile = new OutputFileWritter(Constants.DATA_DIR + "PopulationRealization20k.csv"))
                {
                    currOutputFile.WriteToFile("HhldID,SectorID,HhldSize,NbOfWorkers,"
                                + "NbofKids,NbofUnivDegree,IncLvl,NumbCars,"
                                + "DwellTyp(PopSynt),EPFL_SectorID,BuildingID");
                    while(currReader.LoadZonalPopulationPool(currPool) == true)
                    {
                        string[] currStrTok = ((string)currPool[0]).Split(',');
                        int indx = 0;
                        string currKey = "";
                        if(zonalControlTotals.ContainsKey(currStrTok[1]))
                        {
                            currKey = (string)currStrTok[1];
                            indx = (int)zonalControlTotals[currKey];
                            ArrayList currRandList = currRandGen.GetNNumbersInRange(0,
                                Constants.POOL_COUNT - 1, indx);
                            for(int i = 0; i < currRandList.Count; i++)
                            {
                                string[] hhldValues = ((string)
                                    currPool[(int)currRandList[i]]).Split(',');
                                int bld = Int16.Parse(hhldValues[9]) + 1;
                                currOutputFile.WriteToFile(hhldValues[0] + "," + hhldValues[1]
                                        + "," + hhldValues[3] + "," + hhldValues[4]
                                        + "," + hhldValues[5] + "," + hhldValues[6]
                                        + "," + hhldValues[7] + "," + hhldValues[8]
                                        + "," + hhldValues[9] + ","
                                        + hhldValues[2] + "," + hhldValues[2]
                                        + "0" + bld);
                            }
                        }
                        currPool.Clear();
                    }
                }
            }
        }

        internal struct ZonalStat
        {
            public string ZoneName;
            public double Count;
            public double Sum;
        }

        public void ComputeCommuneLevelStatisticsIncome(string poplFile)
        {
            StreamReader currReader = new StreamReader(poplFile);

            Hashtable currIncome = new Hashtable();

            StreamWriter currOutputFile = new
                StreamWriter(Constants.DATA_DIR + "CommuneStatisticsIncome.csv");
            string currHhld;
            currReader.ReadLine();
            while(!currReader.EndOfStream)
            {
                currHhld = currReader.ReadLine();
                string[] currHhldTok = currHhld.Split(',');
                string currsector = currHhldTok[1].Substring(0, 5);
                if(currIncome.Contains(currsector))
                {
                    ZonalStat currStat = (ZonalStat)currIncome[currsector];
                    currStat.Count++;
                    currStat.Sum += Int32.Parse(currHhldTok[6]);
                    currIncome[currsector] = currStat;
                    int cntUn = int.Parse(currHhldTok[5]);
                    int cntTot = int.Parse(currHhldTok[2]);

                }
                else
                {
                    ZonalStat currStat = new ZonalStat();
                    currStat.ZoneName = currsector;
                    currStat.Sum = Int32.Parse(currHhldTok[6]);
                    currStat.Count = 1;
                    currIncome.Add(currsector, currStat);

                }
            }

            currOutputFile.WriteLine("Commune,Income");
            foreach(DictionaryEntry ent in currIncome)
            {
                ZonalStat curSt = (ZonalStat)ent.Value;
                currOutputFile.WriteLine(curSt.ZoneName + "," +
                    curSt.Sum / curSt.Count);
            }
            currReader.Close();
            currOutputFile.Close();
        }

        public void DiscretizeIncomeAgain(string poplFile)
        {
            using (var currReader = new StreamReader(poplFile))
            {
                Hashtable currIncome = new Hashtable();

                using (var currOutputFile = new
                    StreamWriter(Constants.DATA_DIR + "BrusselsPopulation_DiscIncome.csv"))
                {
                    string currHhld;
                    currOutputFile.WriteLine(currReader.ReadLine() + ",IncLvl");
                    while(!currReader.EndOfStream)
                    {
                        currHhld = currReader.ReadLine();
                        string[] currHhldTok = currHhld.Split(',');
                        Int32 currIncomeVal = Int32.Parse(currHhldTok[6]);

                        if(currIncomeVal < 745)
                        {
                            currHhld += ",0";
                        }
                        else if(currIncomeVal >= 745 && currIncomeVal < 1860)
                        {
                            currHhld += ",1";
                        }
                        else if(currIncomeVal >= 1860 && currIncomeVal < 3100)
                        {
                            currHhld += ",2";
                        }
                        else if(currIncomeVal >= 3100 && currIncomeVal < 4959)
                        {
                            currHhld += ",3";
                        }
                        else if(currIncomeVal >= 4959)
                        {
                            currHhld += ",4";
                        }
                        currOutputFile.WriteLine(currHhld);
                    }

                }
            }
        }

        public void ComputeCommuneLevelStatisticsDiscInc(string poplFile, int category)
        {
            using (var currReader = new StreamReader(poplFile))
            {
                Hashtable currEdu = new Hashtable();

                using (var currOutputFile = new
                    StreamWriter(Constants.DATA_DIR + "CommuneStatistics4IncLvl.csv"))
                {
                    string currHhld;
                    currReader.ReadLine();
                    while(!currReader.EndOfStream)
                    {
                        currHhld = currReader.ReadLine();
                        string[] currHhldTok = currHhld.Split(',');
                        string currsector = currHhldTok[1].Substring(0, 5);
                        int cntUn = int.Parse(currHhldTok[11]);
                        if(currEdu.Contains(currsector) && cntUn == category)
                        {
                            ZonalStat currStat = (ZonalStat)currEdu[currsector];
                            currStat.Count++;
                            currEdu[currsector] = currStat;
                        }
                        else if(cntUn == category)
                        {
                            ZonalStat currStat = new ZonalStat();
                            currStat.ZoneName = currsector;
                            currStat.Count = 1;
                            currEdu.Add(currsector, currStat);

                        }
                    }

                    currOutputFile.WriteLine("Commune,IncLvl4");
                    foreach(DictionaryEntry ent in currEdu)
                    {
                        ZonalStat curSt = (ZonalStat)ent.Value;
                        currOutputFile.WriteLine(curSt.ZoneName + "," +
                            curSt.Count);
                    }
                }
            }
        }

        public void ComputeCommuneLevelStatisticsEdu(string poplFile, int category)
        {
            using (var currReader = new StreamReader(poplFile))
            {
                Hashtable currEdu = new Hashtable();

                using (var currOutputFile = new
                    StreamWriter(Constants.DATA_DIR + "CommuneStatistics2High.csv"))
                {
                    string currHhld;
                    currReader.ReadLine();
                    while(!currReader.EndOfStream)
                    {
                        currHhld = currReader.ReadLine();
                        string[] currHhldTok = currHhld.Split(',');
                        string currsector = currHhldTok[1].Substring(0, 5);
                        int cntUn = int.Parse(currHhldTok[5]);
                        if(currEdu.Contains(currsector) && cntUn == category)
                        {
                            ZonalStat currStat = (ZonalStat)currEdu[currsector];
                            currStat.Count++;
                            currEdu[currsector] = currStat;
                        }
                        else if(cntUn == category)
                        {
                            ZonalStat currStat = new ZonalStat();
                            currStat.ZoneName = currsector;
                            currStat.Count = 1;
                            currEdu.Add(currsector, currStat);

                        }
                    }

                    currOutputFile.WriteLine("Commune,Edu2");
                    foreach(DictionaryEntry ent in currEdu)
                    {
                        ZonalStat curSt = (ZonalStat)ent.Value;
                        currOutputFile.WriteLine(curSt.ZoneName + "," +
                            curSt.Count);
                    }
                }
            }
        }

        public void ComputeCommuneLevelStatisticsPeople(string poplFile, int category)
        {
            using (var currReader = new StreamReader(poplFile))
            {
                Hashtable currEdu = new Hashtable();

                using (var currOutputFile = new
                    StreamWriter(Constants.DATA_DIR + "CommuneStatistics5Per.csv"))
                {
                    string currHhld;
                    currReader.ReadLine();
                    while(!currReader.EndOfStream)
                    {
                        currHhld = currReader.ReadLine();
                        string[] currHhldTok = currHhld.Split(',');
                        string currsector = currHhldTok[1].Substring(0, 5);
                        int cntUn = int.Parse(currHhldTok[2]);
                        if(currEdu.Contains(currsector) && cntUn == category)
                        {
                            ZonalStat currStat = (ZonalStat)currEdu[currsector];
                            currStat.Count++;
                            currEdu[currsector] = currStat;
                        }
                        else if(cntUn == category)
                        {
                            ZonalStat currStat = new ZonalStat();
                            currStat.ZoneName = currsector;
                            currStat.Count = 1;
                            currEdu.Add(currsector, currStat);

                        }
                    }

                    currOutputFile.WriteLine("Commune,People5");
                    foreach(DictionaryEntry ent in currEdu)
                    {
                        ZonalStat curSt = (ZonalStat)ent.Value;
                        currOutputFile.WriteLine(curSt.ZoneName + "," +
                            curSt.Count);
                    }
                }
            }
        }

        public void ComputeCommuneLevelStatisticsPeople(string poplFile,
                    string fileNam, string comList)
        {
            using (StreamReader currReader = new StreamReader(poplFile))
            using (StreamReader currComuneList = new StreamReader(comList))
            using (StreamWriter currOutputFile = new StreamWriter(fileNam))
            {
                Hashtable currPerM = new Hashtable();
                Hashtable currPerF = new Hashtable();
                Hashtable currPerTwo = new Hashtable();
                Hashtable currPerThree = new Hashtable();
                Hashtable currPerFour = new Hashtable();
                Hashtable currPerFive = new Hashtable();
                string currHhld;
                currReader.ReadLine();
                while(!currReader.EndOfStream)
                {
                    currHhld = currReader.ReadLine();
                    string[] currHhldTok = currHhld.Split(',');
                    string currsector = currHhldTok[1].Substring(0, 5);
                    int cntUn = int.Parse(currHhldTok[2]);
                    if(currPerM.Contains(currsector) && cntUn == 0)
                    {
                        ZonalStat currStat = (ZonalStat)currPerM[currsector];
                        currStat.Count++;
                        currPerM[currsector] = currStat;
                    }
                    else if(cntUn == 0)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currPerM.Add(currsector, currStat);

                    }
                    if(currPerF.Contains(currsector) && cntUn == 1)
                    {
                        ZonalStat currStat = (ZonalStat)currPerF[currsector];
                        currStat.Count++;
                        currPerF[currsector] = currStat;
                    }
                    else if(cntUn == 1)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currPerF.Add(currsector, currStat);

                    }
                    if(currPerTwo.Contains(currsector) && cntUn == 2)
                    {
                        ZonalStat currStat = (ZonalStat)currPerTwo[currsector];
                        currStat.Count++;
                        currPerTwo[currsector] = currStat;
                    }
                    else if(cntUn == 2)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currPerTwo.Add(currsector, currStat);
                    }
                    if(currPerThree.Contains(currsector) && cntUn == 3)
                    {
                        ZonalStat currStat = (ZonalStat)currPerThree[currsector];
                        currStat.Count++;
                        currPerThree[currsector] = currStat;
                    }
                    else if(cntUn == 3)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currPerThree.Add(currsector, currStat);

                    }
                    if(currPerFour.Contains(currsector) && cntUn == 4)
                    {
                        ZonalStat currStat = (ZonalStat)currPerFour[currsector];
                        currStat.Count++;
                        currPerFour[currsector] = currStat;
                    }
                    else if(cntUn == 4)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currPerFour.Add(currsector, currStat);

                    }
                    if(currPerFive.Contains(currsector) && cntUn == 5)
                    {
                        ZonalStat currStat = (ZonalStat)currPerFive[currsector];
                        currStat.Count++;
                        currPerFive[currsector] = currStat;
                    }
                    else if(cntUn == 5)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currPerFive.Add(currsector, currStat);

                    }
                }
                currOutputFile.WriteLine("Commune,Male,Female,Per2,Per3,Per4,Per5");
                string str = currComuneList.ReadLine();

                while(!currComuneList.EndOfStream)
                {
                    str = currComuneList.ReadLine();
                    string strConcat = str;
                    if(currPerM.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currPerM[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }

                    if(currPerF.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currPerF[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }

                    if(currPerTwo.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currPerTwo[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }

                    if(currPerThree.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currPerThree[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }
                    if(currPerFour.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currPerFour[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }
                    if(currPerFive.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currPerFive[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }
                    currOutputFile.WriteLine(strConcat);
                }
            }
        }

        public void ComputeCommuneLevelStatisticsCars(string poplFile,
                            string fileNam, string comList)
        {
            using (StreamReader currReader = new StreamReader(poplFile))
            using (StreamReader currComuneList = new StreamReader(comList))
            using (StreamWriter currOutputFile = new StreamWriter(fileNam))
            {
                Hashtable currCarZero = new Hashtable();
                Hashtable currCarOne = new Hashtable();
                Hashtable currCarTwo = new Hashtable();
                Hashtable currCarThree = new Hashtable();


                string currHhld;
                currReader.ReadLine();
                while(!currReader.EndOfStream)
                {
                    currHhld = currReader.ReadLine();
                    string[] currHhldTok = currHhld.Split(',');
                    string currsector = currHhldTok[1].Substring(0, 5);
                    int cntUn = int.Parse(currHhldTok[7]);
                    if(currCarZero.Contains(currsector) && cntUn == 0)
                    {
                        ZonalStat currStat = (ZonalStat)currCarZero[currsector];
                        currStat.Count++;
                        currCarZero[currsector] = currStat;
                    }
                    else if(cntUn == 0)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currCarZero.Add(currsector, currStat);

                    }
                    if(currCarOne.Contains(currsector) && cntUn == 1)
                    {
                        ZonalStat currStat = (ZonalStat)currCarOne[currsector];
                        currStat.Count++;
                        currCarOne[currsector] = currStat;
                    }
                    else if(cntUn == 1)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currCarOne.Add(currsector, currStat);

                    }
                    if(currCarTwo.Contains(currsector) && cntUn == 2)
                    {
                        ZonalStat currStat = (ZonalStat)currCarTwo[currsector];
                        currStat.Count++;
                        currCarTwo[currsector] = currStat;
                    }
                    else if(cntUn == 2)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currCarTwo.Add(currsector, currStat);
                    }
                    if(currCarThree.Contains(currsector) && cntUn == 3)
                    {
                        ZonalStat currStat = (ZonalStat)currCarThree[currsector];
                        currStat.Count++;
                        currCarThree[currsector] = currStat;
                    }
                    else if(cntUn == 3)
                    {
                        ZonalStat currStat = new ZonalStat();
                        currStat.ZoneName = currsector;
                        currStat.Count = 1;
                        currCarThree.Add(currsector, currStat);

                    }
                }
                currOutputFile.WriteLine("Commune,Car0,Car1,Car2,Car3");
                string str = currComuneList.ReadLine();

                while(!currComuneList.EndOfStream)
                {
                    str = currComuneList.ReadLine();
                    string strConcat = str;
                    if(currCarZero.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currCarZero[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }

                    if(currCarOne.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currCarOne[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }

                    if(currCarTwo.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currCarTwo[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }

                    if(currCarThree.Contains(str))
                    {
                        ZonalStat curSt = (ZonalStat)currCarThree[str];
                        strConcat += "," + curSt.Count.ToString();
                    }
                    else
                    {
                        strConcat += ",0";
                    }
                    currOutputFile.WriteLine(strConcat);
                }
            }
        }

        public List<Dictionary<string, ZonalStat>> ComputeCommuneMCStatsCars(int runNum, int seed,
                            string poolFileName, bool delRealizations)
        {
            string poplFile = Constants.DATA_DIR +
                            "Household\\PopulationRealization" + runNum.ToString() + ".csv";
            CreatePopulationByDwellingType(seed, poolFileName, poplFile);
            StreamReader currReader = new StreamReader(poplFile);
            var currCarZero = new Dictionary<string, ZonalStat>();
            var currCarOne = new Dictionary<string, ZonalStat>();
            var currCarTwo = new Dictionary<string, ZonalStat>();
            var currCarThree = new Dictionary<string, ZonalStat>();
            string currHhld;
            currReader.ReadLine();
            while(!currReader.EndOfStream)
            {
                currHhld = currReader.ReadLine();
                string[] currHhldTok = currHhld.Split(',');
                string currsector = currHhldTok[1].Substring(0, 5);
                int cntUn = int.Parse(currHhldTok[7]);
                if(currCarZero.ContainsKey(currsector) && cntUn == 0)
                {
                    var currStat = currCarZero[currsector];
                    currStat.Count++;
                    currCarZero[currsector] = currStat;
                }
                else if(cntUn == 0)
                {
                    var currStat = new ZonalStat();
                    currStat.ZoneName = currsector;
                    currStat.Count = 1;
                    currCarZero.Add(currsector, currStat);

                }
                if(currCarOne.ContainsKey(currsector) && cntUn == 1)
                {
                    ZonalStat currStat = currCarOne[currsector];
                    currStat.Count++;
                    currCarOne[currsector] = currStat;
                }
                else if(cntUn == 1)
                {
                    ZonalStat currStat = new ZonalStat();
                    currStat.ZoneName = currsector;
                    currStat.Count = 1;
                    currCarOne.Add(currsector, currStat);

                }
                if(currCarTwo.ContainsKey(currsector) && cntUn == 2)
                {
                    ZonalStat currStat = (ZonalStat)currCarTwo[currsector];
                    currStat.Count++;
                    currCarTwo[currsector] = currStat;
                }
                else if(cntUn == 2)
                {
                    ZonalStat currStat = new ZonalStat();
                    currStat.ZoneName = currsector;
                    currStat.Count = 1;
                    currCarTwo.Add(currsector, currStat);
                }
                if(currCarThree.ContainsKey(currsector) && cntUn == 3)
                {
                    ZonalStat currStat = (ZonalStat)currCarThree[currsector];
                    currStat.Count++;
                    currCarThree[currsector] = currStat;
                }
                else if(cntUn == 3)
                {
                    ZonalStat currStat = new ZonalStat();
                    currStat.ZoneName = currsector;
                    currStat.Count = 1;
                    currCarThree.Add(currsector, currStat);

                }
            }

            var currArrayList = new List<Dictionary<string, ZonalStat>>();
            currArrayList.Add(currCarZero);
            currArrayList.Add(currCarOne);
            currArrayList.Add(currCarTwo);
            currArrayList.Add(currCarThree);
            currReader.Close();
            if(delRealizations == true)
            {
                File.Delete(poplFile);
            }
            return currArrayList;
        }

        public void WriteMCStatsToFile(string CommuneList, List<Dictionary<string, World.ZonalStat>> RunsOutput,
                                int category)
        {
            using (StreamReader currComuneList = new StreamReader(CommuneList))
            using (StreamWriter currOutputFile = new
                StreamWriter(Constants.DATA_DIR +
                "CommuneStatistics_Cars" + category + ".csv"))
            {
                //currOutputFile.WriteLine("Commune,Car0,Car1,Car2,Car3");
                string str = currComuneList.ReadLine();
                StringBuilder strConcat = new StringBuilder();
                while(!currComuneList.EndOfStream)
                {
                    str = currComuneList.ReadLine();
                    strConcat.Append(str);
                    for(int j = 0; j < RunsOutput.Count; j++)
                    {
                        ZonalStat curSt;
                        Dictionary<string, ZonalStat> currStat = RunsOutput[j];
                        if(currStat.TryGetValue(str, out curSt))
                        {
                            strConcat.Append(",");
                            strConcat.Append(curSt.Count);
                        }
                        else
                        {
                            strConcat.Append(",0");
                        }
                    }
                    currOutputFile.WriteLine(strConcat);
                    strConcat.Clear();
                }
            }
        }

        public void CreatePersonPopulation(string popPoolFileNm,
                            string outFileNm, int cnt)
        {
            using (TextReader currReader = new StreamReader(popPoolFileNm))
            {
                currReader.ReadLine();
                RandomNumberGen currRandGen = new RandomNumberGen();
                using (TextWriter currOutputFile = new StreamWriter(outFileNm))
                {
                    currOutputFile.WriteLine("ID,Age,Sex,HhldSize,Edu_Lvl");
                    string currInStr;
                    int currCnt = 0;
                    while((currInStr = currReader.ReadLine()) != null)
                    {
                        string[] currStrTok = currInStr.Split(',');

                        if(currRandGen.NextDouble() < 0.5 && currCnt < cnt)
                        {
                            currCnt++;
                            currOutputFile.WriteLine(currStrTok[0]
                                                    + "," + currStrTok[2]
                                                    + "," + currStrTok[3]
                                                    + "," + currStrTok[4]
                                                    + "," + currStrTok[5]);
                        }
                    }
                }
            }
        }

        public void ComputeSectorLevelStatistics(string poplFile, int sectIndx,
                                int dimIndx, int catCnt)
        {
            StreamReader currReader = new StreamReader(poplFile);

            Hashtable currDimension = new Hashtable();

            StreamWriter currOutputFile = new
                StreamWriter(Constants.DATA_DIR
                            + "SectorStatistics" + dimIndx + ".csv");
            string currHhld;
            currReader.ReadLine();
            for(int i = 1001; i < 5946; i++)
            {
                currDimension.Add(i.ToString(), new Hashtable());
            }
            while(!currReader.EndOfStream)
            {
                currHhld = currReader.ReadLine();
                string[] currHhldTok = currHhld.Split(',');
                string currsector = currHhldTok[sectIndx];
                /*if (currDimension.Contains(currsector))
                {*/
                Hashtable currData = (Hashtable)currDimension[currsector];
                if(currData.Contains(currHhldTok[dimIndx]))
                {
                    KeyValPair currStat = (KeyValPair)
                        currData[currHhldTok[dimIndx]];
                    currStat.value++;
                    currData[currHhldTok[dimIndx]] = currStat;
                    currDimension[currsector] = currData;
                }
                else
                {
                    KeyValPair currStat = new KeyValPair();
                    currStat.category = currHhldTok[dimIndx];
                    currStat.value = 1;
                    Hashtable currCat = (Hashtable)
                            currDimension[currsector];
                    currCat.Add(currStat.category, currStat);
                    currDimension[currsector] = currCat;
                }
                /*}
                else
                {
                    Hashtable currCat = new Hashtable();
                    KeyValPair currStat = new KeyValPair();
                    currStat.category = currHhldTok[dimIndx];
                    currStat.value = 1;
                    currCat.Add(currStat.category, currStat);
                    currDimension.Add(currsector, currCat);
                }*/
            }
            string firstRow = "Sector";
            for(int i = 0; i < catCnt; i++)
            {
                firstRow += "," + i.ToString();
            }
            currOutputFile.WriteLine(firstRow);
            foreach(DictionaryEntry ent in currDimension)
            {
                Hashtable catEnt = (Hashtable)ent.Value;
                string curString = (string)ent.Key;
                for(int i = 0; i < catCnt; i++)
                {
                    if(catEnt.Contains(i.ToString()))
                    {
                        KeyValPair curSt = (KeyValPair)catEnt[i.ToString()];
                        curString += "," +
                            ((int)curSt.value).ToString();
                    }
                    else
                    {
                        curString += ",0";
                    }
                }
                currOutputFile.WriteLine(curString);
            }
            currReader.Close();
            currOutputFile.Close();
        }

        // [BF] make it proper
        private void LoadMarginalsForDwellings()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Household\\CensusDwellingType.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                double n_sep = Double.Parse(strToken[1]);
                double n_sem = Double.Parse(strToken[2]);
                double n_att = Double.Parse(strToken[3]);
                double n_app = Double.Parse(strToken[4]);
                double sumD = n_sep + n_sem + n_att + n_app;
                SpatialZone currZone = (SpatialZone)myZonalCollection[strToken[0]];

                if(sumD == 0)
                {
                    currZone.myDwellMarginal.AddValue(
                        "0", 0.25);
                    currZone.myDwellMarginal.AddValue(
                        "1", 0.25);
                    currZone.myDwellMarginal.AddValue(
                        "2", 0.25);
                    currZone.myDwellMarginal.AddValue(
                        "3", 0.25);
                }
                else
                {
                    currZone.myDwellMarginal.AddValue(
                        "0", n_sep / sumD);
                    currZone.myDwellMarginalCounts.AddValue(
                        "0", n_sep);
                    currZone.myDwellMarginal.AddValue(
                        "1", n_sem / sumD);
                    currZone.myDwellMarginalCounts.AddValue(
                        "1", n_sem);
                    currZone.myDwellMarginal.AddValue(
                        "2", n_att / sumD);
                    currZone.myDwellMarginalCounts.AddValue(
                        "2", n_att);
                    currZone.myDwellMarginal.AddValue(
                        "3", n_app / sumD);
                    currZone.myDwellMarginalCounts.AddValue(
                        "3", n_app);
                }
            }
            myFileReader.Close();
        }

        private void LoadMarginalsForCars()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Household\\CensusNumOfCars.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                double n_zero = Double.Parse(strToken[1]);
                double n_one = Double.Parse(strToken[2]);
                double n_two = Double.Parse(strToken[3]);
                double n_three = Double.Parse(strToken[4]);
                double sumD = n_zero + n_one + n_two + n_three;
                SpatialZone currZone = (SpatialZone)myZonalCollection[strToken[0]];
                if(sumD == 0.00)
                {
                    currZone.myCarsMarginal.AddValue(
                        "0", 0.4);
                    currZone.myCarsMarginal.AddValue(
                        "1", 0.3);
                    currZone.myCarsMarginal.AddValue(
                        "2", 0.15);
                    currZone.myCarsMarginal.AddValue(
                        "3", 0.15);
                }
                else
                {
                    currZone.myCarsMarginal.AddValue(
                        "0", n_zero / sumD);
                    currZone.myCarsMarginal.AddValue(
                        "1", n_one / sumD);
                    currZone.myCarsMarginal.AddValue(
                        "2", n_two / sumD);
                    currZone.myCarsMarginal.AddValue(
                        "3", n_three / sumD);
                }
            }
            myFileReader.Close();
        }

        private void LoadMarginalsForPersons()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Household\\CensusNumOfPers.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                double n_zero = Double.Parse(strToken[1]);
                double n_one = Double.Parse(strToken[2]);
                double n_two = Double.Parse(strToken[3]);
                double n_three = Double.Parse(strToken[4]);
                double n_four = Double.Parse(strToken[5]);
                double n_five = Double.Parse(strToken[6]);

                double sumD =
                    n_zero + n_one + n_two + n_three + n_four + n_five;
                SpatialZone currZone =
                    (SpatialZone)myZonalCollection[strToken[0]];
                if(sumD == 0.00)
                {
                    currZone.myPersonMarginal.AddValue(
                        "0", 0.4);
                    currZone.myPersonMarginal.AddValue(
                        "1", 0.4);
                    currZone.myPersonMarginal.AddValue(
                        "2", 0.2);
                    currZone.myPersonMarginal.AddValue(
                        "3", 0.0);
                    currZone.myPersonMarginal.AddValue(
                        "4", 0.0);
                    currZone.myPersonMarginal.AddValue(
                        "5", 0.0);
                }
                else
                {
                    currZone.myPersonMarginal.AddValue(
                        "0", n_zero / sumD);
                    currZone.myPersonMarginal.AddValue(
                        "1", n_one / sumD);
                    currZone.myPersonMarginal.AddValue(
                        "2", n_two / sumD);
                    currZone.myPersonMarginal.AddValue(
                        "3", n_three / sumD);
                    currZone.myPersonMarginal.AddValue(
                        "4", n_four / sumD);
                    currZone.myPersonMarginal.AddValue(
                        "5", n_five / sumD);
                }
            }
            myFileReader.Close();
        }

        private void LoadMarginalsForHhldSize2()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Person\\CensusHhldSize2Marginal.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if(strToken[0] != "1004")
                {
                    continue;
                }
                double n_zero = Double.Parse(strToken[1]);
                double n_one = Double.Parse(strToken[2]);
                double n_two = Double.Parse(strToken[3]);
                double n_three = Double.Parse(strToken[4]);
                double n_four = Double.Parse(strToken[5]);
                double n_five = Double.Parse(strToken[6]);

                double sumD =
                    n_zero + n_one + n_two + n_three + n_four + n_five;
                SpatialZone currZone =
                    (SpatialZone)myZonalCollection[strToken[0]];
                if(sumD == 0.00)
                {
                    /*currZone.myHhldSize2Marginal.AddValue(
                        "0", 0.15);
                    currZone.myHhldSize2Marginal.AddValue(
                        "1", 0.3);
                    currZone.myHhldSize2Marginal.AddValue(
                        "2", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "3", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "4", 0.05);
                    currZone.myHhldSize2Marginal.AddValue(
                        "5", 0.1);*/
                }
                else
                {
                    currZone.myHhldSize2Marginal.AddValue(
                        "0", n_zero);
                    currZone.myHhldSize2Marginal.AddValue(
                        "1", n_one);
                    currZone.myHhldSize2Marginal.AddValue(
                        "2", n_two);
                    currZone.myHhldSize2Marginal.AddValue(
                        "3", n_three);
                    currZone.myHhldSize2Marginal.AddValue(
                        "4", n_four);
                    currZone.myHhldSize2Marginal.AddValue(
                        "5", n_five);
                }
            }
            myFileReader.Close();
        }

        private void LoadMarginalsForAge()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Person\\CensusAgeMarginal.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if(strToken[0] != "1004")
                {
                    continue;
                }
                double n_zero = Double.Parse(strToken[1]);
                double n_one = Double.Parse(strToken[2]);
                double n_two = Double.Parse(strToken[3]);
                double n_three = Double.Parse(strToken[4]);
                double n_four = Double.Parse(strToken[5]);
                double n_five = Double.Parse(strToken[6]);
                double n_six = Double.Parse(strToken[7]);
                double n_seven = Double.Parse(strToken[8]);

                double sumD =
                    n_zero + n_one + n_two + n_three + n_four + n_five
                    + n_six + n_seven;
                SpatialZone currZone =
                    (SpatialZone)myZonalCollection[strToken[0]];
                if(sumD == 0.00)
                {
                    /*currZone.myHhldSize2Marginal.AddValue(
                        "0", 0.15);
                    currZone.myHhldSize2Marginal.AddValue(
                        "1", 0.3);
                    currZone.myHhldSize2Marginal.AddValue(
                        "2", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "3", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "4", 0.05);
                    currZone.myHhldSize2Marginal.AddValue(
                        "5", 0.1);*/
                }
                else
                {
                    currZone.myAgeMarginal.AddValue(
                        "0", n_zero);
                    currZone.myAgeMarginal.AddValue(
                        "1", n_one);
                    currZone.myAgeMarginal.AddValue(
                        "2", n_two);
                    currZone.myAgeMarginal.AddValue(
                        "3", n_three);
                    currZone.myAgeMarginal.AddValue(
                        "4", n_four);
                    currZone.myAgeMarginal.AddValue(
                        "5", n_five);
                    currZone.myAgeMarginal.AddValue(
                        "6", n_six);
                    currZone.myAgeMarginal.AddValue(
                        "7", n_seven);
                }
            }
            myFileReader.Close();
        }

        private void LoadMarginalsForSex()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Person\\CensusSexMarginal.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if(strToken[0] != "1004")
                {
                    continue;
                }
                double n_zero = Double.Parse(strToken[1]);
                double n_one = Double.Parse(strToken[2]);

                double sumD = n_zero + n_one;
                SpatialZone currZone =
                    (SpatialZone)myZonalCollection[strToken[0]];
                if(sumD == 0.00)
                {
                    /*currZone.myHhldSize2Marginal.AddValue(
                        "0", 0.15);
                    currZone.myHhldSize2Marginal.AddValue(
                        "1", 0.3);
                    currZone.myHhldSize2Marginal.AddValue(
                        "2", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "3", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "4", 0.05);
                    currZone.myHhldSize2Marginal.AddValue(
                        "5", 0.1);*/
                }
                else
                {
                    currZone.mySexMarginal.AddValue(
                        "0", n_zero);
                    currZone.mySexMarginal.AddValue(
                        "1", n_one);
                }
            }
            myFileReader.Close();
        }

        private void LoadMarginalsForEducation()
        {
            TextReader myFileReader = new StreamReader(Constants.DATA_DIR +
            "Person\\CensusEducationMarginal.csv");
            string strTok;
            myFileReader.ReadLine();
            while((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if(strToken[0] != "1004")
                {
                    continue;
                }
                double n_zero = Double.Parse(strToken[1]);
                double n_one = Double.Parse(strToken[2]);
                double n_two = Double.Parse(strToken[3]);
                double n_three = Double.Parse(strToken[4]);

                double sumD = n_zero + n_one;
                SpatialZone currZone =
                    (SpatialZone)myZonalCollection[strToken[0]];
                if(sumD == 0.00)
                {
                    /*currZone.myHhldSize2Marginal.AddValue(
                        "0", 0.15);
                    currZone.myHhldSize2Marginal.AddValue(
                        "1", 0.3);
                    currZone.myHhldSize2Marginal.AddValue(
                        "2", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "3", 0.2);
                    currZone.myHhldSize2Marginal.AddValue(
                        "4", 0.05);
                    currZone.myHhldSize2Marginal.AddValue(
                        "5", 0.1);*/
                }
                else
                {
                    currZone.myEducationMarginal.AddValue(
                        "0", n_zero);
                    currZone.myEducationMarginal.AddValue(
                        "1", n_one);
                    currZone.myEducationMarginal.AddValue(
                        "2", n_two);
                    currZone.myEducationMarginal.AddValue(
                        "3", n_three);
                }
            }
            myFileReader.Close();
        }
    }
}
