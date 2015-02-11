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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopulationSynthesis.Utils
{
    //    // This should substitute as the only Key Value pair
    //    public struct KeyValPair1
    //    {
    //        public string category;
    //        public uint value;
    //    }
    class ConditionalGenerator
    {
        ArrayList myDimenionNames;
        Hashtable myCondCollection;

        public ConditionalGenerator()
        {
            myCondCollection = new Hashtable();
            myDimenionNames = new ArrayList();
        }

        public bool GenerateConditionals(string inputDataFile, string inputDescFile)
        {
            using (var reader = new ConditionalDataReader(inputDataFile))
            {
                string currRow = null;
                currRow = reader.GetNextRow();
                if(currRow != null)
                {
                    SetDimensions(inputDescFile);
                }
                else
                {
                    return false;
                }
                currRow = reader.GetNextRow();

                for(int i = 0; i < myDimenionNames.Count; i++)
                {
                    CreateCategoryCombinations(i);
                }
                while(currRow != null)
                {
                    WriteNextConditional(currRow);
                    currRow = reader.GetNextRow();
                }

                foreach(DictionaryEntry currTable in myCondCollection)
                {
                    using (var writer = new OutputFileWritter(
                        Constants.DATA_DIR + "Census" + currTable.Key + ".csv"))
                    {
                        writer.WriteToFile("Conditional,Count");
                        foreach(DictionaryEntry currPair in
                                    (Hashtable)currTable.Value)
                        {
                            writer.WriteToFile(
                                ((KeyValPair)currPair.Value).category
                                + "," + ((KeyValPair)currPair.Value)
                                            .value.ToString());
                        }
                    }
                }
            }
            return true;
        }

        private void SetDimensions(string inputDescFile)
        {
            using (var descReader = new ConditionalDataReader(inputDescFile))
            {
                string dimStr = descReader.GetNextRow();
                dimStr.TrimEnd();
                while(dimStr != null)
                {
                    string[] currTok = dimStr.Split(
                        Constants.COLUMN_DELIMETER[0]);
                    myCondCollection.Add(currTok[0], new Hashtable());
                    ArrayList curCats = new ArrayList();
                    foreach(string curCat in currTok)
                    {
                        if(curCat != null && curCat != "")
                        {
                            curCats.Add(curCat);
                        }
                    }
                    myDimenionNames.Add(curCats);
                    dimStr = descReader.GetNextRow();
                }
            }
        }

        private void CreateCategoryCombinations(int idx)
        {
            int dimCnt = 1;
            string curDimNm = (string)((ArrayList)myDimenionNames[idx])[0];
            foreach(ArrayList curDim in myDimenionNames)
            {
                dimCnt *= ((curDim).Count - 1);
            }

            string[] combStr = new string[dimCnt];
            int offset = 0;
            for(int i = 1; i < ((ArrayList)myDimenionNames[idx]).Count;
                i++)
            {
                for(int j = 0; j <
                    dimCnt / (((ArrayList)myDimenionNames[idx]).Count - 1)
                    ; j++)
                {
                    combStr[j + offset] =
                        (string)((ArrayList)myDimenionNames[idx])[i]
                        + Constants.CATEGORY_DELIMITER;
                }
                offset += dimCnt /
                    (((ArrayList)myDimenionNames[idx]).Count - 1);
            }

            offset = dimCnt /
                    (((ArrayList)myDimenionNames[idx]).Count - 1);

            for(int i = 0; i < myDimenionNames.Count; i++)
            {
                if(i != idx)
                {

                    AppendDimensions(combStr, i, offset);
                    offset /= (((ArrayList)myDimenionNames[i]).Count - 1);
                }
            }

            Hashtable currDimColl = (Hashtable)
                    myCondCollection[(string)
                    ((ArrayList)myDimenionNames[idx])[0]];

            foreach(string curStr in combStr)
            {
                KeyValPair currKey = new KeyValPair();
                currKey.category = curStr.Substring(0, curStr.Length - 1);
                currKey.value = 0;
                currDimColl.Add(currKey.category, currKey);
            }
        }

        private void AppendDimensions(string[] stringCol,
            int currDim, int offset)
        {
            ArrayList currDimL = (ArrayList)myDimenionNames[currDim];
            int DimOff = offset / (currDimL.Count - 1);
            int repeat = stringCol.Length / offset;
            int cursor = 0;
            for(int i = 0; i < repeat; i++)
            {
                for(int j = 1; j < currDimL.Count; j++)
                {
                    for(int k = 0; k < DimOff; k++)
                    {
                        stringCol[cursor] += currDimL[j]
                            + Constants.CONDITIONAL_DELIMITER;
                        cursor++;
                    }
                }
            }
        }

        private void WriteNextConditional(string currStr)
        {
            string[] currVal = currStr.Split(Constants.COLUMN_DELIMETER[0]);

            for(int i = 0; i < currVal.Length; i++)
            {
                Hashtable currDimColl = (Hashtable)
                    myCondCollection[(string)((ArrayList)myDimenionNames[i])[0]];
                string currCondNm = currVal[i] +
                    Constants.CATEGORY_DELIMITER;
                for(int j = 0; j < currVal.Length; j++)
                {
                    if(i != j)
                    {
                        currCondNm = currCondNm + currVal[j]
                            + Constants.CONDITIONAL_DELIMITER;
                    }
                }
                currCondNm =
                    currCondNm.Substring(0, currCondNm.Length - 1);
                if(currDimColl.Contains(currCondNm))
                {
                    KeyValPair mycurVal =
                        (KeyValPair)currDimColl[currCondNm];
                    mycurVal.value++;
                    currDimColl[currCondNm] = mycurVal;
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
}

