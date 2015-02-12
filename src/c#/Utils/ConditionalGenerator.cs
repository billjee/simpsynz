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
        List<List<string>> DimenionNames;
        Dictionary<string, Dictionary<string, KeyValPair>> CondCollection;

        public ConditionalGenerator()
        {
            CondCollection = new Dictionary<string, Dictionary<string, KeyValPair>>();
            DimenionNames = new List<List<string>>();
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

                for(int i = 0; i < DimenionNames.Count; i++)
                {
                    CreateCategoryCombinations(i);
                }
                while(currRow != null)
                {
                    WriteNextConditional(currRow);
                    currRow = reader.GetNextRow();
                }

                foreach(var currTable in CondCollection)
                {
                    using (var writer = new OutputFileWriter(
                        Constants.DATA_DIR + "Census" + currTable.Key + ".csv"))
                    {
                        writer.WriteToFile("Conditional,Count");
                        foreach(var currPair in currTable.Value)
                        {
                            writer.WriteToFile(
                                ((KeyValPair)currPair.Value).Category
                                + "," + ((KeyValPair)currPair.Value)
                                            .Value.ToString());
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
                    CondCollection.Add(currTok[0], new Dictionary<string, KeyValPair>());
                    var curCats = new List<string>();
                    foreach(string curCat in currTok)
                    {
                        if(curCat != null && curCat != "")
                        {
                            curCats.Add(curCat);
                        }
                    }
                    DimenionNames.Add(curCats);
                    dimStr = descReader.GetNextRow();
                }
            }
        }

        private void CreateCategoryCombinations(int idx)
        {
            int dimCnt = 1;
            string curDimNm = DimenionNames[idx][0];
            foreach(var curDim in DimenionNames)
            {
                dimCnt *= ((curDim).Count - 1);
            }

            string[] combStr = new string[dimCnt];
            int offset = 0;
            for(int i = 1; i < DimenionNames[idx].Count;
                i++)
            {
                for(int j = 0; j <
                    dimCnt / (DimenionNames[idx].Count - 1)
                    ; j++)
                {
                    combStr[j + offset] =
                        DimenionNames[idx][i]
                        + Constants.CATEGORY_DELIMITER;
                }
                offset += dimCnt /
                    (DimenionNames[idx].Count - 1);
            }

            offset = dimCnt /
                    (DimenionNames[idx].Count - 1);

            for(int i = 0; i < DimenionNames.Count; i++)
            {
                if(i != idx)
                {
                    AppendDimensions(combStr, i, offset);
                    offset /= (DimenionNames[i].Count - 1);
                }
            }

            var currDimColl = CondCollection[DimenionNames[idx][0]];

            foreach(string curStr in combStr)
            {
                KeyValPair currKey = new KeyValPair();
                currKey.Category = curStr.Substring(0, curStr.Length - 1);
                currKey.Value = 0;
                currDimColl.Add(currKey.Category, currKey);
            }
        }

        private void AppendDimensions(string[] stringCol,
            int currDim, int offset)
        {
            var currDimL = DimenionNames[currDim];
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
                var currDimColl = CondCollection[DimenionNames[i][0]];
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
                currCondNm = currCondNm.Substring(0, currCondNm.Length - 1);
                KeyValPair currentValue;
                if(currDimColl.TryGetValue(currCondNm, out currentValue))
                {
                    currentValue.Value++;
                    currDimColl[currCondNm] = currentValue;
                }
                else
                {
                    KeyValPair curPair = new KeyValPair();
                    curPair.Value = 1;
                    curPair.Category = currCondNm;
                    currDimColl.Add(currCondNm, curPair);
                }
            }

        }
    }
}

