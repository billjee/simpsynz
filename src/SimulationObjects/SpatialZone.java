
package SimulationObjects;

import Utils.*;
import java.util.ArrayList;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

public class SpatialZone
{
    private String myName;

    public void SetName(String myNm)
    {
        myName = myNm;
    }

    public String GetName()
    {
        return myName;
    }

    private String myEPFLName;

    public void SetEPFLName(String myNm)
    {
        myEPFLName = myNm;
    }

    public String GetEPFLName()
    {
        return myEPFLName;
    }

    private double surf1;

    public void SetSurfaceOne(double s1)
    {
        surf1 = s1;
    }

    public double GetSurfaceOne()
    {
        return surf1;
    }

    private double surf2;

    public void SetSurfaceTwo(double s2)
    {
        surf2 = s2;
    }

    public double GetSurfaceTwo()
    {
        return surf2;
    }

    private double surf3;

    public void SetSurfaceThree(double s3)
    {
        surf3 = s3;
    }

    public double GetSurfaceThree()
    {
        return surf3;
    }

    private double surf4;

    public void SetSurfaceFour(double s4)
    {
        surf4 = s4;
    }

    public double GetSurfaceFour()
    {
        return surf4;
    }

    //private double myPerUntChosen;
    //public void SetPercUnitsChosen(double val)
    //{
    //    myPerUntChosen = val;
    //}
    public double GetApartmentPercent()
    {
        return (double) myDwellMarginal.GetValue("3")
                / ((double)myDwellMarginal.GetValue("0")
                + (double)myDwellMarginal.GetValue("1")
                + (double) myDwellMarginal.GetValue("2")
                + (double)myDwellMarginal.GetValue("3"));
    }

    ///////////////////////////////////
    // For Household Synthesis

    //public DiscreteCondDistribution censusPersonConditionals;

    public ModelDistribution modelUnivDegConditionals;
    public ModelDistribution modelIncConditionals;
    public ModelDistribution modelCarsConditionals;
    public ModelDistribution modelDwellConditionals;

    public DiscreteMarginalDistribution myCarsMarginal;
    public DiscreteMarginalDistribution myDwellMarginal;
    public DiscreteMarginalDistribution myDwellMarginalCounts;
    public DiscreteMarginalDistribution myPersonMarginal;
    ///////////////////////////////////

    ///////////////////////////////////
    // For Person Synthesis
    public DiscreteMarginalDistribution myHhldSize2Marginal;
    public DiscreteMarginalDistribution mySexMarginal;
    public DiscreteMarginalDistribution myAgeMarginal;
    public DiscreteMarginalDistribution myEducationMarginal;

    public DiscreteCondDistribution myAgeConditional;
    public DiscreteCondDistribution mySexConditional;
    public DiscreteCondDistribution myHhldSizeConditional;
    public DiscreteCondDistribution myEduLevelConditional;

    ///////////////////////////////////

    public SpatialZone()
    {
        ///////////////////////////////////
        // For Household Synthesis

        //censusPersonConditionals = new DiscreteCondDistribution();

        modelIncConditionals = new ModelDistribution();
        modelIncConditionals.SetDimensionName("IncomeLevel");
        modelUnivDegConditionals = new ModelDistribution();
        modelUnivDegConditionals.SetDimensionName("NumWithUnivDeg");
        modelDwellConditionals = new ModelDistribution();
        modelDwellConditionals.SetDimensionName("DwellingType");
        modelCarsConditionals = new ModelDistribution();
        modelCarsConditionals.SetDimensionName("NumOfCars");

        myDwellMarginal = new DiscreteMarginalDistribution();
        myDwellMarginal.SetDimensionName("DwellingType");

        myDwellMarginalCounts = new DiscreteMarginalDistribution();
        myDwellMarginalCounts.SetDimensionName("DwellingType");

        myCarsMarginal = new DiscreteMarginalDistribution();
        myCarsMarginal.SetDimensionName("NumOfCars");

        myPersonMarginal = new DiscreteMarginalDistribution();
        myPersonMarginal.SetDimensionName("HouseholdSize");
        ///////////////////////////////////

        ///////////////////////////////////
        // For Person Synthesis
        myHhldSize2Marginal = new DiscreteMarginalDistribution();
        myHhldSize2Marginal.SetDimensionName("HouseholdSize2");

        mySexMarginal = new DiscreteMarginalDistribution();
        mySexMarginal.SetDimensionName("Sex");

        myAgeMarginal = new DiscreteMarginalDistribution();
        myAgeMarginal.SetDimensionName("MaritalStatus");

        myEducationMarginal = new DiscreteMarginalDistribution();
        myEducationMarginal.SetDimensionName("EducationLevel");

        myAgeConditional = new DiscreteCondDistribution();
        myAgeConditional.SetDimensionName("Age");

        mySexConditional = new DiscreteCondDistribution();
        mySexConditional.SetDimensionName("Sex");

        myHhldSizeConditional = new DiscreteCondDistribution();
        myHhldSizeConditional.SetDimensionName("HouseholdSize2");

        myEduLevelConditional = new DiscreteCondDistribution();
        myEduLevelConditional.SetDimensionName("EducationLevel");
    }

    private double averageIncome;

    public double GetAverageIncome()
    {
        return averageIncome;
    }

    public void SetAverageIncome(double avgInc)
    {
        averageIncome = avgInc;
    }

    private double percentHighEducated;

    public double GetPercentHighEducated()
    {
        return percentHighEducated;
    }

    public void SetPercentHighEducated(double perHighEdu)
    {
        percentHighEducated = perHighEdu;
    }

    private KeyValPair hhldControlTotal;
        
    public void SetHhldControlTotal(String key, int val)
    {
        hhldControlTotal.category = key;
        hhldControlTotal.value = val;
    }

    public ArrayList GetDataHhldCollectionsList()
    {
        ArrayList currColl = new ArrayList();

        currColl.add((ConditionalDistribution)modelIncConditionals);
        currColl.add((ConditionalDistribution)modelUnivDegConditionals);
        currColl.add((ConditionalDistribution)modelDwellConditionals);
        currColl.add((ConditionalDistribution)modelCarsConditionals);

        return currColl;
    }

    public ArrayList GetPersonDataCollectionsList()
    {
        ArrayList currColl = new ArrayList();
        currColl.add((ConditionalDistribution)myAgeConditional);
        currColl.add((ConditionalDistribution)mySexConditional);
        currColl.add((ConditionalDistribution)myHhldSizeConditional);
        currColl.add((ConditionalDistribution)myEduLevelConditional);
        return currColl;
    }

    public DiscreteMarginalDistribution GetHousholdSizeDist()
    {
        return myPersonMarginal;
    }

    public int GetNumHhldWOneCar()
    {
        return (int) myCarsMarginal.GetValue("1");
    }

    public int GetNumHhldWTwoCar()
    {
        return (int) myCarsMarginal.GetValue("2");
    }

    public int GetNumHhldWThreeCar()
    {
        return (int) myCarsMarginal.GetValue("3");
    }

    public double GetPercentHhldWOneCar()
    {
        double sum = myCarsMarginal.GetValue("1")
                    + myCarsMarginal.GetValue("2")
                    + myCarsMarginal.GetValue("3");

        if (sum > 0.00)
        {
            return myCarsMarginal.GetValue("1") / sum;
        }
        return 0.00;
    }

    public double GetPercentHhldWTwoCar()
    {
        double sum = myCarsMarginal.GetValue("1")
                    + myCarsMarginal.GetValue("2")
                    + myCarsMarginal.GetValue("3");

        if (sum > 0.00)
        {
            return myCarsMarginal.GetValue("2") / sum;
        }    
        return 0.00;
    }

    public double GetPercentHhldWThreeCar()
    {
        double sum = myCarsMarginal.GetValue("1")
                    + myCarsMarginal.GetValue("2")
                    + myCarsMarginal.GetValue("3");

        if (sum > 0.00)
        {
            return myCarsMarginal.GetValue("3") / sum;
        }
        return 0.00;
    }

    public DiscreteMarginalDistribution GetCarMarginal()
    {
        return myCarsMarginal;
    }

    public DiscreteMarginalDistribution GetDwellingMarginals()
    {
        return myDwellMarginal;
    }

    public DiscreteMarginalDistribution GetDwellingMarginalsByCount()
    {
        return myDwellMarginalCounts;
    }

    public DiscreteMarginalDistribution GetPersonHhldSizeMarginal()
    {
        return myHhldSize2Marginal;
    }

    public DiscreteMarginalDistribution GetPersonSexMarginal()
    {
        return mySexMarginal;
    }

    public DiscreteMarginalDistribution GetPersonAgeMarginal()
    {
        return myAgeMarginal;
    }

    public DiscreteMarginalDistribution GetPersonEduMarginal()
    {
        return myEducationMarginal;
    }
}
