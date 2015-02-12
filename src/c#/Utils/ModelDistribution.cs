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

namespace PopulationSynthesis.Utils
{
    class ModelDistribution : ConditionalDistribution
    {
        public ModelDistribution()
        {
            SetDistributionType(false);
        }
        public override double GetValue(string dim, string fullKey, SpatialZone curZ)
        {
            string cat = fullKey.Substring(0,
                    fullKey.IndexOf(Constants.CATEGORY_DELIMITER) - 1);
            string key = fullKey.Substring(
                   fullKey.IndexOf(Constants.CATEGORY_DELIMITER) + 1
                , fullKey.Length - 1);
            return GetValue(cat, dim, key, curZ);
        }

        public override double GetValue(string dimension, 
                        string category, string key, 
                                SpatialZone curZ)
        {
            //string procdKey = ProcessKey(key);
            // [BF] For now here it will always be income or education
            if(dimension == "IncomeLevel")
            {
                return (double) ComputeIncomeProbablities(
                    category, key, curZ);
            }
            else if (dimension == "NumOfWorkers")
            {
                return (double) ComputeEducationProbablities(
                    category, key, curZ);
            }
            else if (dimension == "NumOfCars")
            {
                return (double) ComputeCarProbablities(
                    category, key, curZ);
            }
            else if (dimension == "DwellingType")
            {
                return (double)ComputeDwellingProbablities(
                    category, key, curZ);
            }
            return 0;
        }

        private List<KeyValPair> GetCommValue(string dimension
                                , string key,
                                SpatialZone curZ)
        {
            if (dimension == "IncomeLevel")
            {
                return ComputeIncomeCommulative(key, curZ);
            }
            else if (dimension == "NumWithUnivDeg")
            {
                return ComputeEducationCommulative(key, curZ);
            }
            else if (dimension == "NumOfCars")
            {
                return ComputeCarCommulative(key, curZ);
            }
            else if (dimension == "DwellingType")
            {
                return ComputeDwellingCommulative(key, curZ);
            }
            return null;
        }

        public override List<KeyValPair> GetCommulativeValue(string key,
                                            SpatialZone curZ)
        {
            return GetCommValue(GetDimensionName(), key, curZ);
        }

        // Income
        private double ComputeIncomeProbablities(string category,
                                            string procdKey,
                                            SpatialZone curZ)
        {
            var valList = GetUtilityValuesForIncome(procdKey, curZ);
            double logsum = ((double)valList[0]
                                     + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4]);
            if (int.Parse(category) ==
                (int) IncomeLevel.ThirtyOrLess)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) ==
                (int) IncomeLevel.ThirtyToSevetyFive)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) ==
                (int) IncomeLevel.SeventyFiveToOneTwentyFive)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) ==
                (int)IncomeLevel.OneTwentyFiveToTwoHundred)
            {
                return ((double)valList[3] / logsum);
            }
            else if (int.Parse(category) ==
                (int)IncomeLevel.TwohundredOrMore)
            {
                return ((double)valList[4] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeIncomeCommulative(string procdKey,
                                          SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForIncome(procdKey, curZ);
            KeyValPair currPair = new KeyValPair();
            double utilSum = (double)valList[0] + (double)valList[1]
                                     + (double)valList[2]
                                     + (double)valList[3]
                                     + (double)valList[4];
            currPair.Category = "0";//IncomeLevel.ThirtyOrLess.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//IncomeLevel.ThirtyToSevetyFive.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//IncomeLevel.SeventyFiveToOneTwentyFive.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add( currPair);

            currPair = new KeyValPair();
            currPair.Category = "3";//IncomeLevel.OneTwentyFiveToTwoHundred.ToString();
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "4";//IncomeLevel.TwohundredOrMore.ToString();
            currPair.Value = comVal + (double)valList[4] / utilSum;
            comList.Add(currPair);
            return comList;
        }
        private List<double> GetUtilityValuesForIncome(string key,
                                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(5);

            currValues.Add(1.00);
            currValues.Add(Math.Exp(-0.859 + 0.000783 * curZ.GetAverageIncome()
                            / Constants.BFRANC_TO_EURO
                         + 1.16 * Int16.Parse(curKeys[4])
                         + 1.15 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-4.57 + 0.674 * Int16.Parse(curKeys[3])
                         + 0.0012 * curZ.GetAverageIncome() / Constants.BFRANC_TO_EURO
                         + 1.87 * Int16.Parse(curKeys[4])
                         + 0.409 * Int16.Parse(curKeys[5])
                         + 2.2 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-8.11 + 1.38 * Int16.Parse(curKeys[3])
                         + 0.00157 * curZ.GetAverageIncome()
                         / Constants.BFRANC_TO_EURO
                         + 2.22 * Int16.Parse(curKeys[4])
                         + 0.415 * Int16.Parse(curKeys[5])
                         + 2.33 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-10.5 + 1.61 * Int16.Parse(curKeys[3])
                         + 0.0016 * curZ.GetAverageIncome()
                         / Constants.BFRANC_TO_EURO
                         + 3.04 * Int16.Parse(curKeys[4])
                         + 0.415 * Int16.Parse(curKeys[5])
                         + 1.64 * Int16.Parse(curKeys[1])));
            return currValues;
        }
        // Education
        private double ComputeEducationProbablities(string category,
                                            string procdKey,
                                            SpatialZone curZ)
        {
            var valList = GetUtilityValuesForEducation(procdKey, curZ);
            double logsum = ((double)valList[0]
                            + (double)valList[1]
                            + (double)valList[2]);
            if (int.Parse(category) == (int)NumWithUnivDeg.None)
            {
                return ((double)valList[0]/logsum);
            }
            else if (int.Parse(category) == (int)NumWithUnivDeg.One)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) == (int)NumWithUnivDeg.TwoOrMore)
            {
                return ((double)valList[2] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeEducationCommulative(string procdKey,
                                          SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForEducation(procdKey, curZ);
            double utilSum = (double)valList[0] + (double)valList[1]
                         + (double)valList[2];
            KeyValPair currPair = new KeyValPair();
            currPair.Category = "0";//NumWithUnivDeg.None.ToString();
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//NumWithUnivDeg.One.ToString();
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2";//NumWithUnivDeg.TwoOrMore.ToString();
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comList.Add(currPair);
            return comList;
        }

        private List<double> GetUtilityValuesForEducation(string key,
                                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            List<double> currValues = new List<double>(3);
            currValues.Add(1.00);
            currValues.Add(Math.Exp(-2.96 + 0.238 * Int16.Parse(curKeys[4])
                               + 3.34 * curZ.GetPercentHighEducated()
                               + 0.24 * Int16.Parse(curKeys[3])
                               + 0.393 * Int16.Parse(curKeys[1])));
            currValues.Add(Math.Exp(-7.19 + 0.701 * Int16.Parse(curKeys[4])
                   + 4.34 * curZ.GetPercentHighEducated()
                   + 1.09 * Int16.Parse(curKeys[3])
                   + 0.851 * Int16.Parse(curKeys[1])));
            return currValues;
        }
        // Car
        private double ComputeCarProbablities(string category,
                                            string procdKey,
                                            SpatialZone curZ)
        {
            var valList = GetUtilityValuesForCar(procdKey, curZ);
            
            double logsum = ((double)valList[0]
                            + (double)valList[1]
                            + (double)valList[2]
                            + (double)valList[3]);

            if (int.Parse(category) == (int)NumOfCars.NoCar)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) == (int)NumOfCars.OneCar)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) == (int)NumOfCars.TwoCars)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) == (int)NumOfCars.ThreeOrMore)
            {
                return ((double)valList[3] / logsum);
            }
            return 0.00;
        }
        private List<KeyValPair> ComputeCarCommulative(string procdKey,
                                          SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForCar(procdKey, curZ);
            double utilSum = (double)valList[0] + (double)valList[1]
                         + (double)valList[2] + (double)valList[3];
            KeyValPair currPair = new KeyValPair();
            currPair.Category = "0";//No Car
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//1 Car
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2"; //2 Cars
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3"; //3 or more Cars
            currPair.Value = comVal + (double)valList[3] / utilSum;
            comList.Add(currPair);

            return comList;
        }

        private List<double> GetUtilityValuesForCar(string key,
                                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(4);
            currValues.Add(1.00);
            double dwellNotApartment = 0.00;
            if (Int16.Parse(curKeys[5]) != 3)
            {
                dwellNotApartment = 0.841;
            }
            double incParam = 0.00;
            if (Int16.Parse(curKeys[3]) == 2)
            {
                incParam = 0.858;
            }
            else if (Int16.Parse(curKeys[3]) > 2)
            {
                incParam = 0.978;
            }
            double childParam = 0.00;
            if (Int16.Parse(curKeys[2]) != 0)
            {
                childParam = 0.457;
            }
            currValues.Add(Math.Exp(-2.75 + 0.504 * Int16.Parse(curKeys[4])
                               + 0.00105 * curZ.GetAverageIncome()
                               / Constants.BFRANC_TO_EURO
                               + 0.437 * Int16.Parse(curKeys[1])
                               + 0.498 * curZ.GetPercentHhldWOneCar()
                               + dwellNotApartment
                               + incParam
                               + childParam));
            incParam = 0.00;
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
                childParam = 0.800;
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
                   + 2.13 * curZ.GetPercentHhldWTwoCar()
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
        private double ComputeDwellingProbablities(string category,
                                    string procdKey,
                                    SpatialZone curZ)
        {
            var valList = GetUtilityValuesForDwelling(procdKey, curZ);

            double logsum = ((double)valList[0]
                            + (double)valList[1]
                            + (double)valList[2]
                            + (double)valList[3]);

            if (int.Parse(category) == (int)DwellingType.Separate)
            {
                return ((double)valList[0] / logsum);
            }
            else if (int.Parse(category) == (int)DwellingType.SemiAttached)
            {
                return ((double)valList[1] / logsum);
            }
            else if (int.Parse(category) == (int)DwellingType.Attached)
            {
                return ((double)valList[2] / logsum);
            }
            else if (int.Parse(category) == (int)DwellingType.Apartments)
            {
                return ((double)valList[3] / logsum);
            }
            return 0.00;
        }

        private List<KeyValPair> ComputeDwellingCommulative(string procdKey,
                                  SpatialZone curZ)
        {
            double comVal = 0.00;
            var comList = new List<KeyValPair>();
            var valList = GetUtilityValuesForDwelling(procdKey, curZ);
            double utilSum = (double)valList[0] + (double)valList[1]
                         + (double)valList[2] + (double)valList[3];
            KeyValPair currPair = new KeyValPair();
            currPair.Category = "0";//Seperate
            currPair.Value = (double)valList[0] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "1";//Semi detached
            currPair.Value = comVal + (double)valList[1] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "2"; //Attached
            currPair.Value = comVal + (double)valList[2] / utilSum;
            comVal = currPair.Value;
            comList.Add(currPair);

            currPair = new KeyValPair();
            currPair.Category = "3"; //Apartment
            currPair.Value = comVal + (double)valList[3] / utilSum;

            comList.Add(currPair);

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

        private List<double> GetUtilityValuesForDwelling(string key,
                            SpatialZone curZ)
        {
            string[] curKeys = key.Split(Constants.CONDITIONAL_DELIMITER[0]);
            var currValues = new List<double>(4);
            int hhldSz = int.Parse(curKeys[0]);
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

            currValues.Add(Math.Exp(0.423 + currWtDwell
                               - 0.279 * numCars
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
            currValues.Add(Math.Exp(0.870 + currWtDwell
                               - 0.593 * numCars
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
            currValues.Add(Math.Exp(1.20 + currWtDwell
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
}
