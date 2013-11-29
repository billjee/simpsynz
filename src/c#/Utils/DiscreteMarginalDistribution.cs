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

namespace PopulationSynthesis.Utils
{
    class DiscreteMarginalDistribution : MarginalDistribution
    {
        private Hashtable myCollection;
        private string dimension;
        public string GetDimensionName()
        {
            return dimension;
        }
        public void SetDimensionName(string dim)
        {
            dimension = dim;
        }

        public DiscreteMarginalDistribution()
        {
            myCollection = new Hashtable();
        }
        public int GetCategoryCount()
        {
            return myCollection.Count;
        }

        public bool AddValue(string category, double val)
        {
            if (!myCollection.Contains(category))
            {
                myCollection.Add(category, val);
                return true;
            }
            return true;
        }

        public override double GetValue(string category)
        {
            if(myCollection.Contains(category))
            {
                return (double)myCollection[category];
            }
            return 0.00;
        }

    }
}
