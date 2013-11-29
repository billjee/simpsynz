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
using SimulationObjects;

namespace PopulationSynthesis.Utils
{
    class InputDataReader : FileManager
    {
        TextReader myFileReader;
        ArrayList conditionalNames;

        public InputDataReader()
        {
            conditionalNames = new ArrayList();
        }
        public override void OpenFile(string fileName)
        {
            myFileReader = new StreamReader(fileName);
        }
        public override void CloseFile()
        {
            myFileReader.Close();
        }


        // Should be called before fillCollection1
        public void GetConditionalList()
        {
            string strTok = myFileReader.ReadLine();
            string[] strs = strTok.Split(',');
            for (int i = 1; i < strs.Length; i++)
            {
                conditionalNames.Add(strs[i]);
            }
        }

        // For zone by zone information (conditionals as columns)
        public string FillCollection1(DiscreteCondDistribution currColl)
        {
            string strTok = myFileReader.ReadLine();
            if (strTok != null)
            {
                string[] strColl = strTok.Split(',');
                for (int i = 0; i < conditionalNames.Count; i++)
                {
                    string currCond = (string) conditionalNames[i];
                    string[] currDimVal = currCond.Split(
                        Utils.Constants.CATEGORY_DELIMITER[0]);

                    currColl.AddValue(currDimVal[0], currDimVal[1], 
                                        uint.Parse(strColl[i+1]));
                }
                return strColl[0];
            }
            return "";
        }

        // For 1 values for all zones (conditionals as rows)
        public void FillCollection2(DiscreteCondDistribution currColl)
        {
            string strTok = myFileReader.ReadLine();
            while ((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                int j = strToken[0].IndexOf(
                        Utils.Constants.CATEGORY_DELIMITER);
                string catName = strToken[0].Substring(0, j);
                string condName = strToken[0].Substring(j + 1,
                         strToken[0].Length - (catName.Length + 1));

                /*string[] strToken = strTok.Split(',');
                //int j =  strToken[0].IndexOf(
                //        Utils.Constants.CATEGORY_DELIMITER);
                string catName = strTok.Substring(0,1);
                string condName = strTok.Substring(2, strTok.LastIndexOf(',')-2);*/

                currColl.AddValue(catName, condName,
                        double.Parse(strToken[1]));
            }
        }

        public bool LoadZonalPopulationPool(ArrayList currPopPool)
        {
            string strTok;
            int i = 0;
            while ((strTok = myFileReader.ReadLine()) != null
                    && i < Constants.POOL_COUNT)
            {
                currPopPool.Add(strTok);
                i++;
            }
            if (i == Constants.POOL_COUNT || i > 0)
            {
                return true;
            }

            return false;
        }

        public bool LoadZonalPopulationPoolByType(ArrayList currPopPool, string type)
        {
            string strTok;
            int i = 0;
            while ((strTok = myFileReader.ReadLine()) != null
                    && i < Constants.POOL_COUNT)
            {
                string [] xStrs = strTok.Split(',');
                if( xStrs[9] == type)
                {
                    strTok = xStrs[3] + "," + xStrs[4] + "," +
                            xStrs[5] + "," + xStrs[6] + "," +
                            xStrs[7] + "," + xStrs[8] + "," +
                            xStrs[9];
                    currPopPool.Add(strTok);
                }
                i++;
            }
            if (i == Constants.POOL_COUNT || i > 0)
            {
                return true;
            }

            return false;
        }

        public void FillControlTotals(Hashtable currTable)
        {
            string strTok;
            myFileReader.ReadLine();
            while ((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if (!currTable.ContainsKey(strToken[0]))
                {
                    currTable.Add(strToken[0], Int32.Parse(strToken[2]));
                }
            }
        }

        public void FillControlTotalsByDwellType(Hashtable currTable)
        {
            string strTok;
            myFileReader.ReadLine();
            while ((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if (!currTable.ContainsKey(strToken[0]))
                {
                    currTable.Add(strToken[0], strToken);
                }
            }
        }

        public void FillZonalData(Hashtable currTable)
        {
            string strTok;
            myFileReader.ReadLine();
            while ((strTok = myFileReader.ReadLine()) != null)
            {
                string[] strToken = strTok.Split(',');
                if (!currTable.ContainsKey(strToken[0]))
                {
                    SpatialZone curZ = new SpatialZone();
                    curZ.SetName(strToken[0]);
                    curZ.SetEPFLName(strToken[1]);
                    //curZ.SetHhldControlTotal(strToken[0],uint.Parse(strToken[2]));
                    curZ.SetAverageIncome(Double.Parse(strToken[2])
                        *Constants.BFRANC_TO_EURO);
                    curZ.SetPercentHighEducated(Double.Parse(strToken[3]));
                    curZ.SetSurfaceOne(double.Parse(strToken[4]));
                    curZ.SetSurfaceOne(double.Parse(strToken[5]));
                    curZ.SetSurfaceOne(double.Parse(strToken[6]));
                    curZ.SetSurfaceOne(double.Parse(strToken[7]));
                    //curZ.SetPercUnitsChosen(Double.Parse(strToken[4]));
                    currTable.Add(strToken[0], curZ);

                }
            }
        }
    }
}
