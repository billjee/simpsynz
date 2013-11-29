/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import java.io.*;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

//    // This should substitute as the only Key Value pair
//    public struct KeyValPair1
//    {
//        public string category;
//        public uint value;
//    }
public class ConditionalGenerator
{
    ConditionalDataReader myDataReader;
    OutputFileWritter myDataWriter;
    ArrayList myDimenionNames;
    HashMap<String, Object> myCondCollection;

    public ConditionalGenerator()
    {
        myDataReader = new ConditionalDataReader();
        myDataWriter = new OutputFileWritter();
        myCondCollection = new HashMap();
        myDimenionNames = new ArrayList();
    }

    public boolean GenerateConditionals(String inputDataFile, String inputDescFile ) throws IOException
    {
        myDataReader.OpenFile(inputDataFile);
	String currRow = null;
        currRow = myDataReader.GetNextRow();
        if (currRow != null)
        {
            SetDimensions(inputDescFile);
        }
        else
        {
            return false;
        }
        currRow = myDataReader.GetNextRow();

        for (int i = 0; i < myDimenionNames.size(); i++)
        {
            CreateCategoryCombinations(i);
        }
        while(currRow != null )
        {
            WriteNextConditional(currRow);
            currRow = myDataReader.GetNextRow();
        }

        for (Map.Entry<String, Object> currTable : myCondCollection.entrySet())
        {
            myDataWriter.OpenFile(Utils.DATA_DIR + "Census" + currTable.getKey() + ".csv");
            myDataWriter.WriteToFile("Conditional,Count");

            if(currTable.getValue() instanceof HashMap)
            {
                HashMap<String, Object> curr = (HashMap)currTable.getValue();
                for (Map.Entry<String, Object> currPair : curr.entrySet())
                {
                    myDataWriter.WriteToFile(currPair.getKey() + "," + currPair.getValue().toString());
                }
            }
            myDataWriter.CloseFile();
        }
	
        myDataReader.CloseFile();
        return true;
    }

    private void SetDimensions(String inputDescFile ) throws IOException
    {
        ConditionalDataReader descReader = new ConditionalDataReader();
        descReader.OpenFile(inputDescFile);
        String dimStr = descReader.GetNextRow();
        dimStr.trim();
        while (dimStr != null)
        {
            String[] currTok = dimStr.split(Utils.COLUMN_DELIMETER);
            myCondCollection.put(currTok[0], new HashMap<String, Object>());
            ArrayList curCats = new ArrayList();
                
            for (String curCat: currTok)
            {
                if (curCat != null)
                {
                    curCats.add(curCat);
                }
            }
                
            myDimenionNames.add(curCats);
            dimStr = descReader.GetNextRow();
        }
            
        descReader.CloseFile();
    }

    private void CreateCategoryCombinations(int idx)
    {
        int dimCnt = 1;
        String curDimNm = myDimenionNames.get(idx).toString();

        for(Iterator<ArrayList> curDim = myDimenionNames.iterator(); curDim.hasNext();)
        {
            ArrayList next = curDim.next();
            dimCnt *= ((next).size()-1);
        }

       String[] combStr = new String[dimCnt];
       int offset = 0;
       for(int i = 1; i< ((ArrayList)myDimenionNames.get(idx)).size(); i++)
       {
           for (int j = 0; j < dimCnt / (((ArrayList)myDimenionNames.get(idx)).size()-1); j++)
           {
               combStr[j + offset] = ((ArrayList)myDimenionNames.get(idx)).get(i).toString() + Utils.CATEGORY_DELIMITER;
           }
                
           offset += dimCnt / (((ArrayList)myDimenionNames.get(idx)).size() - 1);
       }
     
       offset = dimCnt /(((ArrayList)myDimenionNames.get(idx)).size() - 1);

       for (int i = 0; i < myDimenionNames.size(); i++)
       {
           if(i != idx)
           {
               AppendDimensions(combStr, i, offset);
               offset /= (((ArrayList)myDimenionNames.get(i)).size() - 1);
           }
       }

       HashMap<String, Object> currDimColl = (HashMap<String, Object>) myCondCollection.get(myDimenionNames.get(idx).toString());
            
       for (String curStr: combStr)
       {
           KeyValPair currKey = new KeyValPair();
           currKey.category = curStr.substring(0,curStr.length()-1);
           currKey.value = 0;
           currDimColl.put(currKey.category, (Object)currKey);
       }
    }
        
    private void AppendDimensions(String[] stringCol, int currDim, int offset)
    {
        ArrayList currDimL = (ArrayList)myDimenionNames.get(currDim);
        int DimOff = offset / (currDimL.size() - 1);
        int repeat = stringCol.length / offset;
        int cursor = 0;
        for (int i = 0; i < repeat; i++)
        {
            for (int j = 1; j < currDimL.size(); j++)
            {
                for (int k = 0; k < DimOff; k++)
                {
                    stringCol[cursor] += currDimL.get(j).toString() + Utils.CONDITIONAL_DELIMITER;
                    cursor++;
                }
            }
        }
    }
    
    private void WriteNextConditional(String currStr)
    {
        String[] currVal = currStr.split(Utils.COLUMN_DELIMETER);
			
        for(int i = 0; i < currVal.length; i++)
        {
            HashMap<String, Object> currDimColl = (HashMap<String, Object>) myCondCollection.get(myDimenionNames.get(i).toString());
            String currCondNm = currVal[i] + Utils.CATEGORY_DELIMITER;
            for (int j = 0; j < currVal.length; j++)
            {
                if (i != j)
                {
                    currCondNm = currCondNm + currVal[j] + Utils.CONDITIONAL_DELIMITER;
                }
            }
            currCondNm = currCondNm.substring(0, currCondNm.length() - 1);
            if (currDimColl.containsKey(currCondNm))
            {
                KeyValPair mycurVal = (KeyValPair) currDimColl.get(currCondNm);
                mycurVal.value++;
                currDimColl.put(currCondNm, mycurVal);
            }
            else
            {
                KeyValPair curPair = new KeyValPair();
                curPair.value = 1;
                curPair.category = currCondNm;
                //currDimColl.Add(currCondNm, curPair);
            }
        }
    }
}
