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
    class DiscreteCondDistribution : ConditionalDistribution
    {
        // Hashtable represents each category of the dimension
        // Each dimension will have hashtable for values from other dimension
        private Hashtable myCollection;
        public DiscreteCondDistribution()
        {
            myCollection = new Hashtable();
            missingDimStatus = new ArrayList();
            SetDistributionType(true);
        }

        public int GetCategoryCount()
        {
            return myCollection.Count;
        }

        public void FlushOutData()
        {
            foreach (DictionaryEntry currEnt in myCollection)
            {
                ((Hashtable)currEnt.Value).Clear();
            }
            myCollection.Clear();
        }

        // For A=a|B=b,C=c,D=d
        // category = a
        // key = b,c,d
        // val = P(a|b,c,d) or Count(a|b,c,d)
        public bool AddValue(string category, string key, double val)
        {
            if (category != null)
            {
                if ( myCollection.Count == 0)
                {
                    string[] currKeyToken = key.Split(Utils.Constants.CONDITIONAL_DELIMITER[0]);

                    foreach (string currStr in currKeyToken)
                    {
                        if( currStr.Equals(Utils.Constants.CONDITIONAL_GENERIC))
                        {
                            missingDimStatus.Add(0);
                        }
                        else
                        {
                            missingDimStatus.Add(1);
                        }
                    }
                }
                if (!myCollection.Contains(category))
                {
                    myCollection.Add(category, new Hashtable());
                }
                if ((key != null) && (val >= 0.00))
                {
                    Hashtable currCatData = (Hashtable) 
                                    myCollection[category];
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
            if (myCollection.Contains(category))
            {
                Hashtable currCat = (Hashtable)myCollection[category];
                if (currCat.Contains(key))
                {
                    return (double)currCat[key];
                }
                return Constants.INVALID_UINT_VAL;
            }
            return Constants.INVALID_UINT_VAL;
        }

        // commulative probability or count for the key
        public override ArrayList GetCommulativeValue(string key,
                                            SpatialZone curZ)
        {
            ArrayList currComm = new ArrayList();
            double commCnt = 0;

            key = ProcessKey(key);

            foreach (DictionaryEntry currEnt in myCollection)
            {
                KeyValPair currPair = new KeyValPair();
                currPair.category = (string) currEnt.Key;
                if (((Hashtable)currEnt.Value).Contains(key))
                {
                    currPair.value =
                    ((double)((Hashtable)currEnt.Value)[key])
                    + commCnt;

                }
                else
                {
                    currPair.value = commCnt;
                }
                commCnt = currPair.value;
                currComm.Add(currPair);
            }
            return currComm;
        }
    }
}
