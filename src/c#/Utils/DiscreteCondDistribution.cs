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
    //*******************************************//
    //*******************************************//
    // [BF] Change the name of the ckass, so that it represents 
    //      that it deals with the discrete distributions
    //*******************************************//
    //*******************************************//

    // A DatasetCollection keep all the conditionals or counts related to
    // one dimension of the data
    // We assue that keys are ordered in following order
    // A | B, C, D, E, F
    // B | A, C, D, E, F
    // C | A, B, D, E, F
    // E | A, B, C, D, F
    // F | A, B, C, D, E
    // Each DatasetCollection will have dataset for A |... , B|..., and so on
    // Each category witin DatasetCollection will have a seperate hashtable
    sealed class DiscreteCondDistribution : ConditionalDistribution
    {
        // Hashtable represents each category of the dimension
        // Each dimension will have hashtable for values from other dimension
        private Dictionary<string,Dictionary<string,double>> Data;
        public DiscreteCondDistribution()
        {
            Data = new Dictionary<string, Dictionary<string, double>>();
            MissingDimStatus = new List<int>();
            SetDistributionType(true);
        }

        public int GetCategoryCount()
        {
            return Data.Count;
        }

        public void FlushOutData()
        {
            foreach (var currEnt in Data)
            {
                currEnt.Value.Clear();
            }
            Data.Clear();
        }

        // For A=a|B=b,C=c,D=d
        // category = a
        // key = b,c,d
        // val = P(a|b,c,d) or Count(a|b,c,d)
        public bool AddValue(string category, string key, double val)
        {
            if (category != null)
            {
                if ( Data.Count == 0)
                {
                    string[] currKeyToken = key.Split(Utils.Constants.CONDITIONAL_DELIMITER[0]);

                    foreach (string currStr in currKeyToken)
                    {
                        if( currStr.Equals(Utils.Constants.CONDITIONAL_GENERIC))
                        {
                            MissingDimStatus.Add(0);
                        }
                        else
                        {
                            MissingDimStatus.Add(1);
                        }
                    }
                }
                if((key != null) && (val >= 0.00))
                {
                    Dictionary<string, double> currCatData;
                    if(!Data.TryGetValue(category, out currCatData))
                    {
                        Data.Add(category, (currCatData = new Dictionary<string, double>()));
                    }
                    currCatData.Add(key, val);
                    return true;
                }
                return false;
            }
            return false;
        }

        // individual probability or count with the key
        public override double GetValue(string dim, 
                            string fullKey, SpatialZone curZ)
        {
            string cat = fullKey.Substring(0,
                fullKey.IndexOf(Constants.CATEGORY_DELIMITER)-1);
            string key = fullKey.Substring(
                fullKey.IndexOf(Constants.CATEGORY_DELIMITER) + 1
                , fullKey.Length - 1);
            return GetValue(dim,cat, key, null);
        }

        public override double GetValue(string dim,
                        string category, string key,
                                SpatialZone curZ)
        {
            Dictionary<string, double> currCat;
            if (Data.TryGetValue(category, out currCat))
            {
                double ret;
                if (currCat.TryGetValue(key, out ret))
                {
                    return ret;
                }
                return Constants.INVALID_UINT_VAL;
            }
            return Constants.INVALID_UINT_VAL;
        }

        // commulative probability or count for the key
        public override List<KeyValPair> GetCommulativeValue(string key,
                                            SpatialZone curZ)
        {
            var currComm = new List<KeyValPair>();
            double commCnt = 0;
            key = ProcessKey(key);
            foreach (var currEnt in Data)
            {
                KeyValPair currPair;
                double value;
                currPair.Category = currEnt.Key;
                if (currEnt.Value.TryGetValue(key, out value))
                {
                    currPair.Value = value + commCnt;
                }
                else
                {
                    currPair.Value = commCnt;
                }
                commCnt = currPair.Value;
                currComm.Add(currPair);
            }
            return currComm;
        }
    }
}
