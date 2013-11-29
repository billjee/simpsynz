/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import SimulationObjects.SpatialZone;
import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author XPS 1645
 */
public class InputDataReader extends FileManager
{
        BufferedReader myFileReader;
        ArrayList conditionalNames;

        public InputDataReader()
        {
            conditionalNames = new ArrayList();
        }
        @Override
        public void OpenFile(String fileName)
        {
            FileReader fstream = null;
            try {
                fstream = new FileReader(fileName);
                myFileReader = new BufferedReader(fstream);
            } catch (IOException ex) {
                Logger.getLogger(OutputFileWritter.class.getName()).log(Level.SEVERE, null, ex);
            }
        }

    @Override
        public void CloseFile() throws IOException
        {
            myFileReader.close();
        }


        // Should be called before fillCollection1
        public void GetConditionalList() throws IOException
        {
            String strTok = myFileReader.readLine();
            String[] strs = strTok.split(",");
            for (int i = 1; i < strs.length; i++)
            {
                conditionalNames.add(strs[i]);
            }
        }

        // For zone by zone information (conditionals as columns)
        public String FillCollection1(DiscreteCondDistribution currColl) throws IOException
        {
            String strTok = myFileReader.readLine();
            if (strTok != null)
            {
                String[] strColl = strTok.split(",");
                for (int i = 0; i < conditionalNames.size(); i++)
                {
                    String currCond = (String) conditionalNames.get(i);
                    String[] currDimVal = currCond.split(Utils.CATEGORY_DELIMITER);

                    currColl.AddValue(currDimVal[0], currDimVal[1],
                                        Integer.parseInt(strColl[i+1]));
                }
                return strColl[0];
            }
            return "";
        }

        // For 1 values for all zones (conditionals as rows)
        public void FillCollection2(DiscreteCondDistribution currColl) throws IOException
        {
            String strTok = myFileReader.readLine();
            while ((strTok = myFileReader.readLine()) != null)
            {
                String[] strToken = strTok.split(",");
                int j = strToken[0].indexOf(
                        Utils.CATEGORY_DELIMITER);
                String catName = strToken[0].substring(0, j);
                String condName = strToken[0].substring(j + 1,
                         strToken[0].length() - (catName.length() + 1));

                /*string[] strToken = strTok.Split(',');
                //int j =  strToken[0].IndexOf(
                //        Utils.Constants.CATEGORY_DELIMITER);
                string catName = strTok.Substring(0,1);
                string condName = strTok.Substring(2, strTok.LastIndexOf(',')-2);*/

                currColl.AddValue(catName, condName,
                        Double.parseDouble(strToken[1]));
            }
        }

        public boolean LoadZonalPopulationPool(ArrayList currPopPool) throws IOException
        {
            String strTok;
            int i = 0;
            while ((strTok = myFileReader.readLine()) != null
                    && i < Utils.POOL_COUNT)
            {
                currPopPool.add(strTok);
                i++;
            }
            if (i == Utils.POOL_COUNT || i > 0)
            {
                return true;
            }

            return false;
        }

        public boolean LoadZonalPopulationPoolByType(ArrayList currPopPool, String type) throws IOException
        {
            String strTok;
            int i = 0;
            while ((strTok = myFileReader.readLine()) != null
                    && i < Utils.POOL_COUNT)
            {
                String [] xStrs = strTok.split(",");
                if( xStrs[9].equals(type))
                {
                    strTok = xStrs[3] + "," + xStrs[4] + "," +
                            xStrs[5] + "," + xStrs[6] + "," +
                            xStrs[7] + "," + xStrs[8] + "," +
                            xStrs[9];
                    currPopPool.add(strTok);
                }
                i++;
            }
            if (i == Utils.POOL_COUNT || i > 0)
            {
                return true;
            }

            return false;
        }

        public void FillControlTotals(HashMap<String, Object> currTable) throws IOException
        {
            String strTok;
            myFileReader.readLine();
            while ((strTok = myFileReader.readLine()) != null)
            {
                String[] strToken = strTok.split(",");
                if (!currTable.containsKey(strToken[0]))
                {
                    currTable.put(strToken[0], Integer.parseInt(strToken[2]));
                }
            }
        }

        public void FillControlTotalsByDwellType(HashMap<String, Object> currTable) throws IOException
        {
            String strTok;
            myFileReader.readLine();
            while ((strTok = myFileReader.readLine()) != null)
            {
                String[] strToken = strTok.split(",");
                if (!currTable.containsKey(strToken[0]))
                {
                    currTable.put(strToken[0], strToken);
                }
            }
        }

        public void FillZonalData(HashMap<String, Object> currTable) throws IOException
        {
            String strTok;
            myFileReader.readLine();
            while ((strTok = myFileReader.readLine()) != null)
            {
                String[] strToken = strTok.split(",");
                if (!currTable.containsKey(strToken[0]))
                {
                    SpatialZone curZ = new SpatialZone();
                    curZ.SetName(strToken[0]);
                    curZ.SetEPFLName(strToken[1]);
                    //curZ.SetHhldControlTotal(strToken[0],uint.Parse(strToken[2]));
                    curZ.SetAverageIncome(Double.parseDouble(strToken[2])
                        *Utils.BFRANC_TO_EURO);
                    curZ.SetPercentHighEducated(Double.parseDouble(strToken[3]));
                    curZ.SetSurfaceOne(Double.parseDouble(strToken[4]));
                    curZ.SetSurfaceOne(Double.parseDouble(strToken[5]));
                    curZ.SetSurfaceOne(Double.parseDouble(strToken[6]));
                    curZ.SetSurfaceOne(Double.parseDouble(strToken[7]));
                    //curZ.SetPercUnitsChosen(Double.Parse(strToken[4]));
                    currTable.put(strToken[0], curZ);

                }
            }
        }
}
