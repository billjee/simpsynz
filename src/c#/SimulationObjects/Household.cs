/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopulationSynthesis.Utils;

namespace SimulationObjects
{
    [Serializable]
    class Household : SimulationObject
    {
        private static uint idCounter = 0;

        // BF: In future it should be changed to a collection of Characteristics
        //     Iterators should be defined for them
        //     Dimension/Characteristics should be in form of structures
        private string myZoneID;
        public string GetZoneID()
        {
            return myZoneID;
        }
        public void SetZoneID(string id)
        {
            myZoneID = id;
        }
        
        private HouseholdSize myHhhldSize;
        public HouseholdSize GetHhldSize()
        {
            return myHhhldSize;
        }
        public void SetHhldSize(HouseholdSize size)
        {
            myHhhldSize = size;
        }

        private DwellingType myDwellType;
        public DwellingType GetDwellingType()
        {
            return myDwellType;
        }
        public void SetDwellingType(DwellingType dwellTyp)
        {
            myDwellType = dwellTyp;
        }
        
        private NumOfCars myNumOfCars;
        public NumOfCars GetNumOfCars()
        {
            return myNumOfCars;
        }
        public void SetNumOfCars(NumOfCars numCars)
        {
            myNumOfCars = numCars;
        }
        
        private NumOfWorkers myNumOfWorkers;
        public NumOfWorkers GetNumOfWorkers()
        {
            return myNumOfWorkers;
        }
        public void SetNumOfWorkers(NumOfWorkers numWrkr)
        {
            myNumOfWorkers = numWrkr;
        }
        
        private NumOfKids myNumOfKids;
        public NumOfKids GetNumOfKids()
        {
            return myNumOfKids;
        }
        public void SetNumOfKids(NumOfKids numKids)
        {
            myNumOfKids = numKids;
        }
        
        private NumWithUnivDeg myNumofUnivDeg;
        public NumWithUnivDeg GetNumOfUnivDegree()
        {
            return myNumofUnivDeg;
        }
        public void SetNumOfUnivDegree(NumWithUnivDeg numUnivDeg)
        {
            myNumofUnivDeg = numUnivDeg;
        }
        
        private IncomeLevel myIncomeLevel;
        public IncomeLevel GetIncomeLevel()
        {
            return myIncomeLevel;
        }
        public void SetIncomeLevel(IncomeLevel incomeLvl)
        {
            myIncomeLevel = incomeLvl;
        }

        private uint myIncome;
        public uint GetIncome()
        {
            return myIncome;
        }
        public void SetIncome(uint currIncome)
        {
            myIncome = currIncome;
        }

        public Household()
        {
            myHhhldSize = HouseholdSize.TwoPersons;
            myDwellType = DwellingType.SemiAttached;
            myNumOfCars = NumOfCars.OneCar;
            myNumOfWorkers = NumOfWorkers.One;
            myNumOfKids = NumOfKids.One;
            myNumofUnivDeg = NumWithUnivDeg.None;
            myIncomeLevel = IncomeLevel.SeventyFiveToOneTwentyFive;
            myIncome = 80000;
            Type = AgentType.Household;
        }

        public Household(string currZone)
        {
            myZoneID = currZone;
            myHhhldSize = HouseholdSize.TwoPersons;
            myDwellType = DwellingType.SemiAttached;
            myNumOfCars = NumOfCars.OneCar;
            myNumOfWorkers = NumOfWorkers.One;
            myNumOfKids = NumOfKids.None;
            myNumofUnivDeg = NumWithUnivDeg.One;
            myIncomeLevel = IncomeLevel.SeventyFiveToOneTwentyFive;
            myIncome = 80000;
            Type = AgentType.Household;
        }

        // Returns a string that gives the value of characteristics
        // except for base characteristics
        //
        // [BF] It has to be changed to dictionary iterator based
        //      once we have the characteritics in a collection
        public override string GetNewJointKey(string baseDim)
        {
            string jointKey;
            if (baseDim == "HouseholdSize")
            {
                jointKey = ((int)myNumOfWorkers).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfKids).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myIncomeLevel).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumofUnivDeg).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfCars).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myDwellType).ToString();
            }
            else if (baseDim == "DwellingType")
            {
                jointKey = ((int)myHhhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER 
                            + ((int)myNumOfWorkers).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfKids).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myIncomeLevel).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumofUnivDeg).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfCars).ToString();
            }
            else if (baseDim == "NumOfCars")
            {
                jointKey =  ((int)myHhhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER 
                            + ((int)myNumOfWorkers).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfKids).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myIncomeLevel).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumofUnivDeg).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myDwellType).ToString();
            }
            else if (baseDim == "NumOfWorkers")
            {
                jointKey = ((int)myHhhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfKids).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myIncomeLevel).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumofUnivDeg).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfCars).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myDwellType).ToString();
            }
            else if (baseDim == "NumOfKids")
            {
                jointKey = ((int)myHhhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER 
                            + ((int)myNumOfWorkers).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myIncomeLevel).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumofUnivDeg).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfCars).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myDwellType).ToString();
            }
            else if (baseDim == "NumWithUnivDeg")
            {
                jointKey = ((int)myHhhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfWorkers).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfKids).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myIncomeLevel).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfCars).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myDwellType).ToString();
            }
            else if (baseDim == "IncomeLevel")
            {
                jointKey = ((int)myHhhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfWorkers).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfKids).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumofUnivDeg).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myNumOfCars).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myDwellType).ToString();
            }
            else
            {
                jointKey = "";
            }

            return jointKey;
        }

        public override SimulationObject CreateNewCopy(string baseDim, 
            int baseDimVal)
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            Household myCopy = (Household) b.Deserialize(m);
            
            if (baseDim == "HouseholdSize")
            {
                myCopy.myHhhldSize = (HouseholdSize) baseDimVal;
            }
            else if (baseDim == "DwellingType")
            {
                myCopy.myDwellType = (DwellingType)baseDimVal;
            }
            else if (baseDim == "NumOfCars")
            {
                myCopy.myNumOfCars = (NumOfCars) baseDimVal;
            }
            else if (baseDim == "NumOfWorkers")
            {
                myCopy.myNumOfWorkers = (NumOfWorkers)baseDimVal;
            }
            else if (baseDim == "NumOfKids")
            {
                myCopy.myNumOfKids = (NumOfKids)baseDimVal;
            }
            else if (baseDim == "NumWithUnivDeg")
            {
                myCopy.myNumofUnivDeg = (NumWithUnivDeg)baseDimVal;
            }
            else if (baseDim == "IncomeLevel")
            {
                myCopy.myIncomeLevel = (IncomeLevel)baseDimVal;
            }
            else
            {
                return null;
            }
            CheckLogicalInconsistencies(myCopy);
            return myCopy;
        }

        public Household CreateNewCopy(uint income)
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            Household myCopy = (Household)b.Deserialize(m);

            myCopy.myIncome = income;
            myCopy.myIncomeLevel = IncomeConvertor.ConvertValueToLevel(income);
            CheckLogicalInconsistencies(myCopy);
            return myCopy;
        }

        private bool CheckLogicalInconsistencies(Household currHhld)
        {
            int totPer = (int)currHhld.GetHhldSize();
            int totWrk = (int)currHhld.GetNumOfWorkers();
            int totUnv = (int)currHhld.GetNumOfUnivDegree();
            int totKid = (int)currHhld.GetNumOfKids();
           
            if (totPer == 0)
            {
                totPer = 1;
            }

            if (totUnv > totPer)
            {
                currHhld.SetNumOfUnivDegree((NumWithUnivDeg)totPer);
                currHhld.SetNumOfWorkers((NumOfWorkers) totPer);
                currHhld.SetNumOfKids(NumOfKids.None);
                return true;
            }
            if (totWrk > totPer)
            {
                currHhld.SetNumOfWorkers((NumOfWorkers)totPer);
                currHhld.SetNumOfUnivDegree((NumWithUnivDeg)totPer);
                currHhld.SetNumOfKids(NumOfKids.None);
                return true;
            }
            if (totKid >= totPer)
            {
                currHhld.SetNumOfWorkers((NumOfWorkers)totPer);
                currHhld.SetNumOfUnivDegree((NumWithUnivDeg)totPer);
                currHhld.SetNumOfKids(NumOfKids.None);
                return true;
            }

            // number of univ degrees
            if (totKid > 0)
            {
                if ((totWrk < totUnv) 
                    && (totPer < (totWrk - totUnv)+totKid+totWrk))
                {
                    currHhld.SetNumOfUnivDegree((NumWithUnivDeg)(totWrk));
                }
            }

            // number of kids
            if (totWrk == 0)
            {
                currHhld.SetNumOfKids(NumOfKids.None);
            }

            if ((totPer - totWrk == 0 && totKid > 0 )
               || (totPer - totUnv == 0 && totKid > 0))
            {
                currHhld.SetNumOfKids(NumOfKids.None);
            } else if (totKid > totPer - totWrk)
            {
                currHhld.SetNumOfKids(NumOfKids.None);
            }
            return true;
        }
    }

    public static class IncomeConvertor
    {
        private static RandomNumberGen randGen = new RandomNumberGen();
        public static uint ConvertLevelToValue(IncomeLevel currHhldIncLvl)
        {
            uint income = 0;
            if (currHhldIncLvl == IncomeLevel.ThirtyOrLess)
            {
                if (randGen.NextDouble() > 0.3)
                {
                    income = (uint)Math.Round(
                        randGen.NextDoubleInRange(10000, 30000));
                }
                else
                {
                    income = (uint)Math.Round(
                        randGen.NextDoubleInRange(1000, 10000));
                }
            }
            else if (currHhldIncLvl == IncomeLevel.ThirtyToSevetyFive)
            {
                income = (uint)Math.Round(randGen.NextDoubleInRange(30000, 75000), 0);
            }
            else if (currHhldIncLvl == IncomeLevel.SeventyFiveToOneTwentyFive)
            {
                income = (uint)Math.Round(randGen.NextDoubleInRange(75000, 125000), 0);
            }
            else if (currHhldIncLvl == IncomeLevel.OneTwentyFiveToTwoHundred)
            {
                income = (uint)Math.Round(randGen.NextDoubleInRange(125000, 200000), 0);
            }
            else
            {
                if (randGen.NextDouble() > 0.3)
                {
                    income = (uint)Math.Round(randGen.NextDoubleInRange(200000, 2000000), 0);
                }
                else
                {
                    income = (uint)Math.Round(randGen.NextDoubleInRange(2000000, 10000000), 0);
                }
            }
            return income;
        }

        public static IncomeLevel ConvertValueToLevel(uint currHhldInc)
        {
            if (currHhldInc <= 30000)
            {
                return IncomeLevel.ThirtyOrLess;
            }
            else if (currHhldInc > 30000 && currHhldInc <= 75000)
            {
                return IncomeLevel.ThirtyToSevetyFive;
            }
            else if (currHhldInc > 75000 && currHhldInc <= 125000)
            {
                return IncomeLevel.SeventyFiveToOneTwentyFive;
            }
            else if (currHhldInc > 125000 && currHhldInc <= 200000)
            {
                return IncomeLevel.OneTwentyFiveToTwoHundred;
            }
            else
            {
                return IncomeLevel.TwohundredOrMore;
            }
        }
        public static uint GetEuroIncome(uint currHhhldInc)
        {
            return (uint) Math.Round(currHhhldInc / Constants.BFRANC_TO_EURO,0);
        }

        public static int GetEuroIncome(IncomeLevel currHhldIncLvl)
        {
            int income = 0;
            if (currHhldIncLvl == IncomeLevel.ThirtyOrLess)
            {
                income = (int)Math.Round(randGen.NextDoubleInRange(20000, 30000) /
                    Constants.BFRANC_TO_EURO, 0);
            }
            else if (currHhldIncLvl == IncomeLevel.ThirtyToSevetyFive)
            {
                income = (int)Math.Round(randGen.NextDoubleInRange(30000, 75000) /
                    Constants.BFRANC_TO_EURO, 0);
            }
            else if (currHhldIncLvl == IncomeLevel.SeventyFiveToOneTwentyFive)
            {
                income = (int)Math.Round(randGen.NextDoubleInRange(75000, 125000) /
                    Constants.BFRANC_TO_EURO, 0);
            }
            else if (currHhldIncLvl == IncomeLevel.OneTwentyFiveToTwoHundred)
            {
                income = (int)Math.Round(randGen.NextDoubleInRange(125000, 200000) /
                    Constants.BFRANC_TO_EURO, 0);
            }
            else
            {
                income = (int)Math.Round(randGen.NextDoubleInRange(200000, 1000000) /
                    Constants.BFRANC_TO_EURO, 0);
            }
            return income;
        }

    }
}
