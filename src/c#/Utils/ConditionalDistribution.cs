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
    class ConditionalDistribution
    {
        protected List<int> MissingDimStatus;

        protected string dimensionName;
        public string GetDimensionName()
        {
            return dimensionName;
        }
        public void SetDimensionName(string catName)
        {
            dimensionName = catName;
        }

        protected bool isDiscrete;
        public bool GetDistributionType()
        {
            return isDiscrete;
        }
        public void SetDistributionType(bool isDisc)
        {
            isDiscrete = isDisc;
        }

        public virtual double GetValue(string dim, 
                        string fullKey, SpatialZone curZ)
        {
            return 0;
        }
        public virtual double GetValue(string dimension,
                        string category, string key,
                                SpatialZone curZ)
        {
            return 0;
        }

        public virtual List<KeyValPair> GetCommulativeValue(string key,
                                            SpatialZone curZ)
        {
            return null;
        }
        protected string ProcessKey(string key)
        {
            string[] procdKeyTok =
                key.Split(Utils.Constants.CONDITIONAL_DELIMITER[0]);

            for (int i = 0; i < procdKeyTok.Length; i++)
            {
                if ((int)MissingDimStatus[i] == 0)
                {
                    procdKeyTok[i] =
                        Utils.Constants.CONDITIONAL_GENERIC;
                }
            }

            string procdKey = procdKeyTok[0];
            for (int i = 1; i < procdKeyTok.Length; i++)
            {
                procdKey += Utils.Constants.CONDITIONAL_DELIMITER
                          + procdKeyTok[i];
            }

            return procdKey;
        }

    }
}
