package SimulationObjects;

import SimulationObjects.AgentAttributes.*;
import Utils.RandomNumberGen;
import Utils.Utils;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.logging.Level;
import java.util.logging.Logger;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

public class Household extends SimulationObject implements Serializable
{
    private static int idCounter = 0;

    // BF: In future it should be changed to a collection of Characteristics
    //     Iterators should be defined for them
    //     Dimension/Characteristics should be in form of structures
    private String myZoneID;
    public String GetZoneID()
    {
        return myZoneID;
    }

    public void SetZoneID(String id)
    {
        myZoneID = id;
    }

    private int myHhhldSize;
    public int GetHhldSize()
    {
        return myHhhldSize;
    }

    public void SetHhldSize(int size)
    {
        myHhhldSize = size;
    }

    private int myDwellType;
    public int GetDwellingType()
    {
        return myDwellType;
    }

    public void SetDwellingType(int dwellTyp)
    {
        myDwellType = dwellTyp;
    }

    private int myNumOfCars;
    public int GetNumOfCars()
    {
        return myNumOfCars;
    }

    public void SetNumOfCars(int numCars)
    {
        myNumOfCars = numCars;
    }

    private int myNumOfWorkers;
    public int GetNumOfWorkers()
    {
        return myNumOfWorkers;
    }

    public void SetNumOfWorkers(int numWrkr)
    {
        myNumOfWorkers = numWrkr;
    }

    private int myNumOfKids;
    public int GetNumOfKids()
    {
        return myNumOfKids;
    }

    public void SetNumOfKids(int numKids)
    {
        myNumOfKids = numKids;
    }

    private int myNumofUnivDeg;
    public int GetNumOfUnivDegree()
    {
        return myNumofUnivDeg;
    }

    public void SetNumOfUnivDegree(int numUnivDeg)
    {
        myNumofUnivDeg = numUnivDeg;
    }

    private int myIncomeLevel;
    public int GetIncomeLevel()
    {
        return myIncomeLevel;
    }

    public void SetIncomeLevel(int incomeLvl)
    {
        myIncomeLevel = incomeLvl;
    }

    private int myIncome;
    public int GetIncome()
    {
        return myIncome;
    }

    public void SetIncome(int currIncome)
    {
        myIncome = currIncome;
    }

    public Household()
    {
        myID = ++idCounter;

        myHhhldSize = HouseholdSize.TwoPersons;
        myDwellType = DwellingType.SemiAttached;
        myNumOfCars = NumOfCars.OneCar;
        myNumOfWorkers = NumOfWorkers.One;
        myNumOfKids = NumOfKids.One;
        myNumofUnivDeg = NumWithUnivDeg.None;
        myIncomeLevel = IncomeLevel.SeventyFiveToOneTwentyFive;
        myIncome = 80000;
        myType = AgentType.Household;
    }

    public Household(String currZone)
    {
        myZoneID = currZone;
        myID = ++idCounter;

        myHhhldSize = HouseholdSize.TwoPersons;
        myDwellType = DwellingType.SemiAttached;
        myNumOfCars = NumOfCars.OneCar;
        myNumOfWorkers = NumOfWorkers.One;
        myNumOfKids = NumOfKids.None;
        myNumofUnivDeg = NumWithUnivDeg.One;
        myIncomeLevel = IncomeLevel.SeventyFiveToOneTwentyFive;
        myIncome = 80000;
        myType = AgentType.Household;
    }

    // Returns a string that gives the value of characteristics
    // except for base characteristics
    //
    // [BF] It has to be changed to dictionary iterator based
    //      once we have the characteritics in a collection
    @Override
    public String GetNewJointKey(String baseDim)
    {
        String jointKey;
        if (baseDim.equals("HouseholdSize"))
        {
            jointKey = String.valueOf(myNumOfWorkers)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfKids)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myIncomeLevel)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumofUnivDeg)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfCars)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myDwellType);
        }
        else if (baseDim.equals("DwellingType"))
        {    
            jointKey = String.valueOf(myHhhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfWorkers)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfKids)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myIncomeLevel)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumofUnivDeg)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfCars);
        }
        else if (baseDim.equals("NumOfCars"))
        {    
            jointKey =  String.valueOf(myHhhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfWorkers)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfKids)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myIncomeLevel)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumofUnivDeg)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myDwellType);
        }
        else if (baseDim.equals("NumOfWorkers"))
        {    
            jointKey = String.valueOf(myHhhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfKids)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myIncomeLevel)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumofUnivDeg)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfCars)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myDwellType);
        }
        else if (baseDim.equals("NumOfKids"))
        {
            jointKey = String.valueOf(myHhhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfWorkers)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myIncomeLevel)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumofUnivDeg)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfCars)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myDwellType);
        }
        else if (baseDim.equals("NumWithUnivDeg"))
        {
            jointKey = String.valueOf(myHhhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfWorkers)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfKids)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myIncomeLevel)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfCars)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myDwellType);
        }
        else if (baseDim.equals("IncomeLevel"))
        { 
            jointKey = String.valueOf(myHhhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfWorkers)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfKids)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumofUnivDeg)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myNumOfCars)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myDwellType);
        }
        else
        {
            jointKey = "";
        }

        return jointKey;
    }
    
    @Override
    public SimulationObject CreateNewCopy(String baseDim, int baseDimVal)
    {
        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        ObjectOutputStream oos;
        try {
            oos = new ObjectOutputStream(bos);
            oos.writeObject(this);
            oos.flush();
            oos.close();
            bos.close();
            byte [] byteData = bos.toByteArray();

            ByteArrayInputStream bais = new ByteArrayInputStream(byteData);
            try
            {
               Household myCopy = (Household) new ObjectInputStream(bais).readObject();
               if (baseDim.equals("HouseholdSize"))
               {
                    myCopy.myHhhldSize = baseDimVal;
               }
               else if (baseDim.equals("DwellingType"))
               {
                    myCopy.myDwellType = baseDimVal;
               }
               else if (baseDim.equals("NumOfCars"))
               {
                    myCopy.myNumOfCars = baseDimVal;
               }
               else if (baseDim.equals("NumOfWorkers"))
               {
                    myCopy.myNumOfWorkers = baseDimVal;
               }
               else if (baseDim.equals("NumOfKids"))
               {
                    myCopy.myNumOfKids = baseDimVal;
               }
               else if (baseDim.equals("NumWithUnivDeg"))
               {
                    myCopy.myNumofUnivDeg = baseDimVal;
               }
               else if (baseDim.equals("IncomeLevel"))
               {
                    myCopy.myIncomeLevel = baseDimVal;
               }
               else
               {
                    return null;
               }
               CheckLogicalInconsistencies(myCopy);
               return myCopy;
            } catch (ClassNotFoundException ex) {
                Logger.getLogger(Household.class.getName()).log(Level.SEVERE, null, ex);
            }

        } catch (IOException ex) {
            Logger.getLogger(Household.class.getName()).log(Level.SEVERE, null, ex);
        }
        return null;
    }

    public Household CreateNewCopy(int income)
    {
        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        ObjectOutputStream oos;
        try {
            oos = new ObjectOutputStream(bos);
            oos.writeObject(this);
            oos.flush();
            oos.close();
            bos.close();
            byte [] byteData = bos.toByteArray();

            ByteArrayInputStream bais = new ByteArrayInputStream(byteData);
            try
            {
                Household myCopy = (Household) new ObjectInputStream(bais).readObject();

                myCopy.myIncome = income;
                myCopy.myIncomeLevel = IncomeConvertor.ConvertValueToLevel(income);
                CheckLogicalInconsistencies(myCopy);
                return myCopy;
            
            } catch (ClassNotFoundException ex) {
                Logger.getLogger(Household.class.getName()).log(Level.SEVERE, null, ex);
            }

        } catch (IOException ex) {
            Logger.getLogger(Household.class.getName()).log(Level.SEVERE, null, ex);
        }
        return null;
    }

        private boolean CheckLogicalInconsistencies(Household currHhld)
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
                currHhld.SetNumOfUnivDegree(totPer);
                currHhld.SetNumOfWorkers(totPer);
                currHhld.SetNumOfKids(NumOfKids.None);
                return true;
            }
            if (totWrk > totPer)
            {
                currHhld.SetNumOfWorkers(totPer);
                currHhld.SetNumOfUnivDegree(totPer);
                currHhld.SetNumOfKids(NumOfKids.None);
                return true;
            }
            if (totKid >= totPer)
            {
                currHhld.SetNumOfWorkers(totPer);
                currHhld.SetNumOfUnivDegree(totPer);
                currHhld.SetNumOfKids(NumOfKids.None);
                return true;
            }

            // number of univ degrees
            if (totKid > 0)
            {
                if ((totWrk < totUnv)
                    && (totPer < (totWrk - totUnv)+totKid+totWrk))
                {
                    currHhld.SetNumOfUnivDegree(totWrk);
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
        public static class IncomeConvertor
    {
        private static RandomNumberGen randGen = new RandomNumberGen();
        public static int ConvertLevelToValue(int currHhldIncLvl)
        {
            int income = 0;
            if (currHhldIncLvl == IncomeLevel.ThirtyOrLess)
            {
                if (randGen.NextDouble() > 0.3)
                {
                    income = (int)Math.round(
                        randGen.NextDoubleInRange(10000, 30000));
                }
                else
                {
                    income = (int)Math.round(
                        randGen.NextDoubleInRange(1000, 10000));
                }
            }
            else if (currHhldIncLvl == IncomeLevel.ThirtyToSevetyFive)
            {
                income = (int) Math.round(randGen.NextDoubleInRange(30000, 75000));
            }
            else if (currHhldIncLvl == IncomeLevel.SeventyFiveToOneTwentyFive)
            {
                income = (int)Math.round(randGen.NextDoubleInRange(75000, 125000));
            }
            else if (currHhldIncLvl == IncomeLevel.OneTwentyFiveToTwoHundred)
            {
                income = (int)Math.round(randGen.NextDoubleInRange(125000, 200000));
            }
            else
            {
                if (randGen.NextDouble() > 0.3)
                {
                    income = (int)Math.round(randGen.NextDoubleInRange(200000, 2000000));
                }
                else
                {
                    income = (int)Math.round(randGen.NextDoubleInRange(2000000, 10000000));
                }
            }
            return income;
        }

        public static int ConvertValueToLevel(int currHhldInc)
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
        public static int GetEuroIncome(int currHhhldInc)
        {
            return (int) Math.round(currHhhldInc / Utils.BFRANC_TO_EURO);
        }

        public static int GetEuroIncomeCont(int currHhldIncLvl)
        {
            int income = 0;
            if (currHhldIncLvl == IncomeLevel.ThirtyOrLess)
            {
                income = (int)Math.round(randGen.NextDoubleInRange(20000, 30000) /
                    Utils.BFRANC_TO_EURO);
            }
            else if (currHhldIncLvl == IncomeLevel.ThirtyToSevetyFive)
            {
                income = (int)Math.round(randGen.NextDoubleInRange(30000, 75000) /
                    Utils.BFRANC_TO_EURO);
            }
            else if (currHhldIncLvl == IncomeLevel.SeventyFiveToOneTwentyFive)
            {
                income = (int)Math.round(randGen.NextDoubleInRange(75000, 125000) /
                    Utils.BFRANC_TO_EURO);
            }
            else if (currHhldIncLvl == IncomeLevel.OneTwentyFiveToTwoHundred)
            {
                income = (int)Math.round(randGen.NextDoubleInRange(125000, 200000) /
                    Utils.BFRANC_TO_EURO);
            }
            else
            {
                income = (int)Math.round(randGen.NextDoubleInRange(200000, 1000000) /
                    Utils.BFRANC_TO_EURO);
            }
            return income;
        }
    }
}
