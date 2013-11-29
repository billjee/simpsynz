/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import java.util.HashMap;

/**
 *
 * @author XPS 1645
 */
public class DiscreteMarginalDistribution extends MarginalDistribution
{
    private HashMap<String, Object> myCollection;
    private String dimension;

    public String GetDimensionName()
    {
        return dimension;
    }

    public void SetDimensionName(String dim)
    {
        dimension = dim;
    }

    public DiscreteMarginalDistribution()
    {
        myCollection = new HashMap<String, Object>();
    }

    public int GetCategoryCount()
    {
        return myCollection.size();
    }

    public boolean AddValue(String category, double val)
    {
        if (!myCollection.containsKey(category))
        {
            myCollection.put(category, val);
            return true;
        }
        return true;
    }

    @Override
    public double GetValue(String category)
    {
        if(myCollection.containsKey(category))
        {    
            return Double.parseDouble(myCollection.get(category).toString());
        }
        return 0.00;
    }
}
