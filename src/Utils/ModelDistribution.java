/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import SimulationObjects.AgentAttributes.*;
import SimulationObjects.SpatialZone;
import java.util.ArrayList;

/**
 *
 * @author XPS 1645
 */
public class ModelDistribution extends ConditionalDistribution
{
    public ModelDistribution()
    {
        SetDistributionType(false);
    }

    @Override
    public double GetValue(String dim, String fullKey, SpatialZone curZ)
    {
        String cat = fullKey.substring(0, fullKey.indexOf(Utils.CATEGORY_DELIMITER) - 1);
        String key = fullKey.substring(fullKey.indexOf(Utils.CATEGORY_DELIMITER) + 1, fullKey.length() - 1);
        return GetValue(cat, dim, key, curZ);
    }

    @Override
    public double GetValue(String dimension, String category, String key, SpatialZone curZ)
    {
        //string procdKey = ProcessKey(key);
        // [BF] For now here it will always be income or education
        if(dimension.equals("IncomeLevel"))
        {
            return ComputeIncomeProbablities(category, key, curZ);
        }
        else if (dimension.equals("NumOfWorkers"))
        {
            return ComputeEducationProbablities(category, key, curZ);
        }
        else if (dimension.equals("NumOfCars"))
        {
            return ComputeCarProbablities(category, key, curZ);
        }
        else if (dimension.equals("DwellingType"))
        {
            return ComputeDwellingProbablities(category, key, curZ);
        }
        return 0;
    }

    private ArrayList GetCommValue(String dimension, String key, SpatialZone curZ)
    {    
        if (dimension.equals("IncomeLevel"))
        {
            return ComputeIncomeCommulative(key, curZ);
        }
        else if (dimension.equals("NumWithUnivDeg"))
        {
            return ComputeEducationCommulative(key, curZ);
        }
        else if (dimension.equals("NumOfCars"))
        {
            return ComputeCarCommulative(key, curZ);
        }
        else if (dimension.equals("DwellingType"))
        {
            return ComputeDwellingCommulative(key, curZ);
        }
        return null;
    }

    @Override
    public ArrayList GetCommulativeValue(String key, SpatialZone curZ)
    {
        return GetCommValue(GetDimensionName(), key, curZ);
    }

    // Income
    private double ComputeIncomeProbablities(String category, String procdKey, SpatialZone curZ)
    {
        ArrayList valList = GetUtilityValuesForIncome(procdKey, curZ);
        double logsum = (Double.parseDouble(valList.get(0).toString()) + Double.parseDouble(valList.get(1).toString())
                                     + Double.parseDouble(valList.get(2).toString())
                                     + Double.parseDouble(valList.get(3).toString())
                                     + Double.parseDouble(valList.get(4).toString()));
        if (Integer.parseInt(category) == (int) IncomeLevel.ThirtyOrLess)
        {
            return (Double.parseDouble(valList.get(0).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int) IncomeLevel.ThirtyToSevetyFive)
        {    
            return (Double.parseDouble(valList.get(1).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int) IncomeLevel.SeventyFiveToOneTwentyFive)
        {
            return (Double.parseDouble(valList.get(2).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)IncomeLevel.OneTwentyFiveToTwoHundred)
        {
            return (Double.parseDouble(valList.get(3).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)IncomeLevel.TwohundredOrMore)
        {
            return (Double.parseDouble(valList.get(4).toString()) / logsum);
        }
        return 0.00;
    }
    
    private ArrayList ComputeIncomeCommulative(String procdKey, SpatialZone curZ)
    {
        double comVal = 0.00;
        ArrayList comList = new ArrayList();
        ArrayList valList = GetUtilityValuesForIncome(procdKey, curZ);
        KeyValPair currPair = new KeyValPair();
        double utilSum = Double.parseDouble(valList.get(0).toString()) + Double.parseDouble(valList.get(1).toString())
                                     + Double.parseDouble(valList.get(2).toString())
                                     + Double.parseDouble(valList.get(3).toString())
                                     + Double.parseDouble(valList.get(4).toString());
        currPair.category = "0";//IncomeLevel.ThirtyOrLess.ToString();
        currPair.value = Double.parseDouble(valList.get(0).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "1";//IncomeLevel.ThirtyToSevetyFive.ToString();
        currPair.value = comVal + Double.parseDouble(valList.get(1).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "2";//IncomeLevel.SeventyFiveToOneTwentyFive.ToString();
        currPair.value = comVal + Double.parseDouble(valList.get(2).toString()) / utilSum;
        comVal = currPair.value;
        comList.add( currPair);

        currPair = new KeyValPair();
        currPair.category = "3";//IncomeLevel.OneTwentyFiveToTwoHundred.ToString();
        currPair.value = comVal + Double.parseDouble(valList.get(3).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "4";//IncomeLevel.TwohundredOrMore.ToString();
        currPair.value = comVal + Double.parseDouble(valList.get(4).toString()) / utilSum;
        comList.add(currPair);
        return comList;
    }

    private ArrayList GetUtilityValuesForIncome(String key, SpatialZone curZ)
    {
        String[] curKeys = key.split(Utils.CONDITIONAL_DELIMITER);
        ArrayList currValues = new ArrayList(5);

        currValues.add(1.00);
        currValues.add(Math.exp(-0.859 + 0.000783 * curZ.GetAverageIncome() / Utils.BFRANC_TO_EURO
                         + 1.16 * Integer.parseInt(curKeys[4])
                         + 1.15 * Integer.parseInt(curKeys[1])));

        currValues.add(Math.exp(-4.57 + 0.674 * Integer.parseInt(curKeys[3])
                         + 0.0012 * curZ.GetAverageIncome() / Utils.BFRANC_TO_EURO
                         + 1.87 * Integer.parseInt(curKeys[4])
                         + 0.409 * Integer.parseInt(curKeys[5])
                         + 2.2 * Integer.parseInt(curKeys[1])));

        currValues.add(Math.exp(-8.11 + 1.38 * Integer.parseInt(curKeys[3])
                         + 0.00157 * curZ.GetAverageIncome()
                         / Utils.BFRANC_TO_EURO
                         + 2.22 * Integer.parseInt(curKeys[4])
                         + 0.415 * Integer.parseInt(curKeys[5])
                         + 2.33 * Integer.parseInt(curKeys[1])));

        currValues.add(Math.exp(-10.5 + 1.61 * Integer.parseInt(curKeys[3])
                         + 0.0016 * curZ.GetAverageIncome()
                         / Utils.BFRANC_TO_EURO
                         + 3.04 * Integer.parseInt(curKeys[4])
                         + 0.415 * Integer.parseInt(curKeys[5])
                         + 1.64 * Integer.parseInt(curKeys[1])));

       return currValues;
    }

    // Education
    private double ComputeEducationProbablities(String category, String procdKey, SpatialZone curZ)
    {
        ArrayList valList = GetUtilityValuesForEducation(procdKey, curZ);
        double logsum = (Double.parseDouble(valList.get(0).toString())
                            + Double.parseDouble(valList.get(1).toString())
                            + Double.parseDouble(valList.get(2).toString()));

        if (Integer.parseInt(category) == (int)NumWithUnivDeg.None)
        {
            return (Double.parseDouble(valList.get(0).toString())/logsum);
        }
        else if (Integer.parseInt(category) == (int)NumWithUnivDeg.One)
        {
            return (Double.parseDouble(valList.get(1).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)NumWithUnivDeg.TwoOrMore)
        {
            return (Double.parseDouble(valList.get(2).toString()) / logsum);
        }
        return 0.00;
    }

    private ArrayList ComputeEducationCommulative(String procdKey, SpatialZone curZ)
    {
        double comVal = 0.00;
        ArrayList comList = new ArrayList();
        ArrayList valList = GetUtilityValuesForEducation(procdKey, curZ);
        double utilSum = Double.parseDouble(valList.get(0).toString()) + Double.parseDouble(valList.get(1).toString())
                         + Double.parseDouble(valList.get(2).toString());

        KeyValPair currPair = new KeyValPair();
        currPair.category = "0";//NumWithUnivDeg.None.ToString();
        currPair.value = Double.parseDouble(valList.get(0).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "1";//NumWithUnivDeg.One.ToString();
        currPair.value = comVal + Double.parseDouble(valList.get(1).toString())/ utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "2";//NumWithUnivDeg.TwoOrMore.ToString();
        currPair.value = comVal + Double.parseDouble(valList.get(2).toString()) / utilSum;
        comList.add(currPair);
        return comList;
    }

    private ArrayList GetUtilityValuesForEducation(String key, SpatialZone curZ)
    {
        String[] curKeys = key.split(Utils.CONDITIONAL_DELIMITER);
        ArrayList currValues = new ArrayList(3);
        currValues.add(1.00);
        currValues.add(Math.exp(-2.96 + 0.238 * Integer.parseInt(curKeys[4])
                               + 3.34 * curZ.GetPercentHighEducated()
                               + 0.24 * Integer.parseInt(curKeys[3])
                               + 0.393 * Integer.parseInt(curKeys[1])));
            
        currValues.add(Math.exp(-7.19 + 0.701 * Integer.parseInt(curKeys[4])
                   + 4.34 * curZ.GetPercentHighEducated()
                   + 1.09 * Integer.parseInt(curKeys[3])
                   + 0.851 * Integer.parseInt(curKeys[1])));

        return currValues;
    }

    // Car
    private double ComputeCarProbablities(String category, String procdKey, SpatialZone curZ)
    {
        ArrayList valList = GetUtilityValuesForCar(procdKey, curZ);

        double logsum = (Double.parseDouble(valList.get(0).toString())
                            + Double.parseDouble(valList.get(1).toString())
                            + Double.parseDouble(valList.get(2).toString())
                            + Double.parseDouble(valList.get(3).toString()));

        if (Integer.parseInt(category) == (int)NumOfCars.NoCar)
        {
            return (Double.parseDouble(valList.get(0).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)NumOfCars.OneCar)
        {
            return (Double.parseDouble(valList.get(1).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)NumOfCars.TwoCars)
        {
            return (Double.parseDouble(valList.get(2).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)NumOfCars.ThreeOrMore)
        {
            return (Double.parseDouble(valList.get(3).toString()) / logsum);
        }
        return 0.00;
    }
        
    private ArrayList ComputeCarCommulative(String procdKey, SpatialZone curZ)
    {
        double comVal = 0.00;
        ArrayList comList = new ArrayList();
        ArrayList valList = GetUtilityValuesForCar(procdKey, curZ);
        double utilSum = Double.parseDouble(valList.get(0).toString()) + Double.parseDouble(valList.get(1).toString())
                         + Double.parseDouble(valList.get(2).toString()) + Double.parseDouble(valList.get(3).toString());

        KeyValPair currPair = new KeyValPair();
        currPair.category = "0";//No Car
        currPair.value = Double.parseDouble(valList.get(0).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "1";//1 Car
        currPair.value = comVal + Double.parseDouble(valList.get(1).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "2"; //2 Cars
        currPair.value = comVal + Double.parseDouble(valList.get(2).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "3"; //3 or more Cars
        currPair.value = comVal + Double.parseDouble(valList.get(3).toString()) / utilSum;
        comList.add(currPair);

        return comList;
    }
    
    private ArrayList GetUtilityValuesForCar(String key, SpatialZone curZ)
    {
        String[] curKeys = key.split(Utils.CONDITIONAL_DELIMITER);
        ArrayList currValues = new ArrayList(4);
        currValues.add(1.00);
        double dwellNotApartment = 0.00;
        if (Integer.parseInt(curKeys[5]) != 3)
        {
            dwellNotApartment = 0.841;
        }
        double incParam = 0.00;
        if (Integer.parseInt(curKeys[3]) == 2)
        {
            incParam = 0.858;
        }
        else if (Integer.parseInt(curKeys[3]) > 2)
        {
            incParam = 0.978;
        }
        double childParam = 0.00;
        if (Integer.parseInt(curKeys[2]) != 0)
        {
            childParam = 0.457;
        }
        currValues.add(Math.exp(-2.75 + 0.504 * Integer.parseInt(curKeys[4])
                               + 0.00105 * curZ.GetAverageIncome()
                               / Utils.BFRANC_TO_EURO
                               + 0.437 * Integer.parseInt(curKeys[1])
                               + 0.498 * curZ.GetPercentHhldWOneCar()
                               + dwellNotApartment
                               + incParam
                               + childParam));
        incParam = 0.00;
        if (Integer.parseInt(curKeys[3]) == 2)
        {
            incParam = 1.87;
        }
        else if (Integer.parseInt(curKeys[3]) > 2)
        {
            incParam = 2.43;
        }
        childParam = 0.00;
        if (Integer.parseInt(curKeys[2]) != 0)
        {
            childParam = 0.800;
        }
        dwellNotApartment = 0.00;
        if (Integer.parseInt(curKeys[5]) != 3)
        {
            dwellNotApartment = 1.86;
        }
        currValues.add(Math.exp(-7.02 + 0.933 * Integer.parseInt(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Utils.BFRANC_TO_EURO
                   + 1.24 * Integer.parseInt(curKeys[1])
                   + 2.13 * curZ.GetPercentHhldWTwoCar()
                   + dwellNotApartment
                   + incParam
                   + childParam));
        incParam = 0.00;
        if (Integer.parseInt(curKeys[3]) == 2)
        {
            incParam = 1.42;
        }
        else if (Integer.parseInt(curKeys[3]) > 2)
        {
            incParam = 3.24;
        }

        dwellNotApartment = 0.00;
        if (Integer.parseInt(curKeys[5]) != 3)
        {
            dwellNotApartment = 2.66;
        }
        currValues.add(Math.exp(-10.1 + 1.07 * Integer.parseInt(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Utils.BFRANC_TO_EURO
                   + 1.60 * Integer.parseInt(curKeys[1])
                   + 14.1 * curZ.GetPercentHhldWThreeCar()
                   + dwellNotApartment
                   + incParam));
        return currValues;
    }

        /*private ArrayList GetUtilityValuesForCar(string key,
                                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            ArrayList currValues = new ArrayList(4);
            currValues.Add(1.00);
            double dwellNotApartment = 0.00;
            if( Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 0.841;
            }
            double incParam = 0.00;
            if(Int16.Parse(curKeys[3])==2)
            {
                incParam = 0.858;
            }
            else if (Int16.Parse(curKeys[3])>2)
            {
                incParam = 0.978;
            }
            double childParam = 0.00;
            if(Int16.Parse(curKeys[2])!=0)
            {
                childParam = 0.457;
            }
            currValues.Add(Math.Exp(-2.75 + 0.504 * Int16.Parse(curKeys[4])
                               + 0.00105 * curZ.GetAverageIncome()
                               / Constants.BFRANC_TO_EURO
                               + 0.437 * Int16.Parse(curKeys[1])
                               + 0.00105 * curZ.GetNumHhldWOneCar()
                               + dwellNotApartment
                               + incParam
                               + childParam));
            incParam=0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 1.87;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 2.43;
            }
            childParam = 0.00;
            if (Int16.Parse(curKeys[2]) != 0)
            {
                childParam = 0.801;
            }
            dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 1.86;
            }
            currValues.Add(Math.Exp(-7.02 + 0.933 * Int16.Parse(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Constants.BFRANC_TO_EURO
                   + 1.24 * Int16.Parse(curKeys[1])
                   + 0.0045 * curZ.GetNumHhldWTwoCar()
                   + dwellNotApartment
                   + incParam
                   + childParam));
            incParam = 0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 1.42;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 3.24;
            }

            dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 2.66;
            }
            currValues.Add(Math.Exp(-10.1 + 1.07 * Int16.Parse(curKeys[4])
                   + 0.00126 * curZ.GetAverageIncome()
                   / Constants.BFRANC_TO_EURO
                   + 1.60 * Int16.Parse(curKeys[1])
                   + 0.0298 * curZ.GetNumHhldWThreeCar()
                   + dwellNotApartment
                   + incParam));

            return currValues;

        }*/


    // Dwelling
    private double ComputeDwellingProbablities(String category, String procdKey, SpatialZone curZ)
    {
        ArrayList valList = GetUtilityValuesForDwelling(procdKey, curZ);

        double logsum = (Double.parseDouble(valList.get(0).toString())
                            + Double.parseDouble(valList.get(1).toString())
                            + Double.parseDouble(valList.get(2).toString())
                            + Double.parseDouble(valList.get(3).toString()));

        if (Integer.parseInt(category) == (int)DwellingType.Separate)
        {
            return (Double.parseDouble(valList.get(0).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)DwellingType.SemiAttached)
        {
            return (Double.parseDouble(valList.get(1).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)DwellingType.Attached)
        {
            return (Double.parseDouble(valList.get(2).toString()) / logsum);
        }
        else if (Integer.parseInt(category) == (int)DwellingType.Apartments)
        {
            return (Double.parseDouble(valList.get(3).toString()) / logsum);
        }
        return 0.00;
    }
    
    private ArrayList ComputeDwellingCommulative(String procdKey, SpatialZone curZ)
    {
        double comVal = 0.00;
        ArrayList comList = new ArrayList();
        ArrayList valList = GetUtilityValuesForDwelling(procdKey, curZ);
        double utilSum = Double.parseDouble(valList.get(0).toString()) + Double.parseDouble(valList.get(1).toString())
                         + Double.parseDouble(valList.get(2).toString() + Double.parseDouble(valList.get(3).toString()));
        KeyValPair currPair = new KeyValPair();
        currPair.category = "0";//Seperate
        currPair.value = Double.parseDouble(valList.get(0).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "1";//Semi detached
        currPair.value = comVal + Double.parseDouble(valList.get(1).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "2"; //Attached
        currPair.value = comVal + Double.parseDouble(valList.get(2).toString()) / utilSum;
        comVal = currPair.value;
        comList.add(currPair);

        currPair = new KeyValPair();
        currPair.category = "3"; //Apartment
        currPair.value = comVal + Double.parseDouble(valList.get(3).toString()) / utilSum;
        comList.add(currPair);

        return comList;
    }

        //private ArrayList GetUtilityValuesForDwelling(string key,
        //                            SpatialZone curZ)
        //{
        //    string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
        //    ArrayList currValues = new ArrayList(4);
        //    currValues.Add(1.00);

        //    double childParam = 0.00;
        //    if (Int16.Parse(curKeys[2]) != 0)
        //    {
        //        childParam = 0.484;
        //    }
        //    currValues.Add(Math.Exp(1.52 - 0.000549 * curZ.GetAverageIncome()
        //                       / Constants.BFRANC_TO_EURO
        //                       - 0.572 * Int16.Parse(curKeys[5])
        //                       + childParam));
        //    double incParam=0.00;
        //    if (Int16.Parse(curKeys[3]) > 2 )
        //    {
        //        incParam = 0.664;
        //    }

        //    if (Int16.Parse(curKeys[2]) != 0)
        //    {
        //        childParam = 0.430;
        //    }
        //    currValues.Add(Math.Exp(5.95 - 0.00208 * curZ.GetAverageIncome()
        //                       / Constants.BFRANC_TO_EURO
        //                       + 0.266 * Int16.Parse(curKeys[4])
        //                       - 1.16 * Int16.Parse(curKeys[5])
        //                       + incParam + childParam));
        //    if (Int16.Parse(curKeys[3]) == 2)
        //    {
        //        incParam = -0.681;
        //    }

        //    if (Int16.Parse(curKeys[2]) != 0)
        //    {
        //        childParam = -0.642;
        //    }
        //    currValues.Add(Math.Exp(4.52 - 0.00233 * curZ.GetAverageIncome()
        //                        / Constants.BFRANC_TO_EURO
        //                       + 0.811 * Int16.Parse(curKeys[4])
        //                       - 1.89 * Int16.Parse(curKeys[5])
        //                       + 5.77 * curZ.GetApartmentPercent()
        //                       + incParam + childParam));
        //    return currValues;
        //}


    private ArrayList GetUtilityValuesForDwelling(String key, SpatialZone curZ)
    {    
        String[] curKeys = key.split(Utils.CONDITIONAL_DELIMITER);
        ArrayList currValues = new ArrayList(4);
        int hhldSz = Integer.parseInt(curKeys[0]);
        double b_surf = 0.00;
        if (hhldSz == 2)
        {
            b_surf = 0.0146;
        }
        else if( hhldSz == 3)
        {
            b_surf = 0.0194;
        }
        else if (hhldSz > 3)
        {
            b_surf = 0.0249;
        }

        double currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("0");
        if (currWtDwell == 0)
        {
            currWtDwell = 0.0000001;
        }
        else
        {
            currWtDwell = Math.log(currWtDwell);
        }
        currValues.add(Math.exp(currWtDwell + b_surf * curZ.GetSurfaceOne()));

        currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("1");
        if (currWtDwell == 0)
        {
            currWtDwell = 0.0000001;
        }
        else
        {
            currWtDwell = Math.log(currWtDwell);
        }
        int numCars = Integer.parseInt(curKeys[5]);
        int incLvl = Integer.parseInt(curKeys[3]);

        currValues.add(Math.exp(0.423 + currWtDwell
                               - 0.279 * numCars
                               + b_surf * curZ.GetSurfaceTwo()));

        currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("2");
        if (currWtDwell == 0)
        {
            currWtDwell = 0.0000001;
        }
        else
        {
            currWtDwell = Math.log(currWtDwell);
        }
        currValues.add(Math.exp(0.870 + currWtDwell
                               - 0.593 * numCars
                               + b_surf * curZ.GetSurfaceThree()));

        currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("3");
        if (currWtDwell == 0)
        {
            currWtDwell = 0.0000001;
        }
        else
        {
            currWtDwell = Math.log(currWtDwell);
        }
        currValues.add(Math.exp(1.20 + currWtDwell
                               - 0.9482 * numCars
                               + b_surf * curZ.GetSurfaceFour()));
        return currValues;
    }

        /*private ArrayList GetUtilityValuesForDwelling(string key,
                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            ArrayList currValues = new ArrayList(4);
            int hhldSz = int.Parse(curKeys[0]);
            double b_surf = 0.00;
            if (hhldSz == 2)
            {
                b_surf = 0.0122;
            }
            else if( hhldSz == 3)
            {
                b_surf = 0.0168;
            }
            else if (hhldSz > 4)
            {
                b_surf = 0.0234;
            }
            double currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("0");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(currWtDwell
                               + b_surf * curZ.GetSurfaceOne()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("1");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            int numCars = int.Parse(curKeys[5]);
            int incLvl = int.Parse(curKeys[3]);
            double b_inc = 0.00;
            if (incLvl > 2)
            {
                b_inc = 0.584;
            }
            currValues.Add(Math.Exp(0.377 + currWtDwell
                               - 0.19 * numCars
                               - b_inc
                               + b_surf * curZ.GetSurfaceTwo()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("2");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            currValues.Add(Math.Exp(0.838 + currWtDwell
                               - 0.591 * numCars
                               + b_surf * curZ.GetSurfaceThree()));

            currWtDwell = curZ.GetDwellingMarginalsByCount().GetValue("3");
            if (currWtDwell == 0)
            {
                currWtDwell = 0.0000001;
            }
            else
            {
                currWtDwell = Math.Log(currWtDwell);
            }
            b_inc = 0.00;
            if (incLvl == 2)
            {
                b_inc = 0.392;
            }
            else if (incLvl > 2)
            {
                b_inc = 0.721;
            }
            currValues.Add(Math.Exp(1.17 + currWtDwell
                               - 0.792 * numCars
                               - b_inc
                               + b_surf * curZ.GetSurfaceFour()));
            return currValues;
        }*/
}
