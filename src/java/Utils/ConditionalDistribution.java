/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package Utils;

import SimulationObjects.SpatialZone;
import java.util.ArrayList;

/**
 *
 * @author XPS 1645
 */
public class ConditionalDistribution
{
    protected ArrayList<Integer> missingDimStatus;

    protected String dimensionName;

    public String GetDimensionName()
    {
        return dimensionName;
    }

    public void SetDimensionName(String catName)
    {
        dimensionName = catName;
    }

    protected boolean isDiscrete;

    public boolean GetDistributionType()
    {
        return isDiscrete;
    }

    public void SetDistributionType(boolean isDisc)
    {
        isDiscrete = isDisc;
    }

    public double GetValue(String dim, String fullKey, SpatialZone curZ)
    {
        return 0;
    }

    public double GetValue(String dimension, String category, String key, SpatialZone curZ)
    {
        return 0;
    }

    public ArrayList GetCommulativeValue(String key, SpatialZone curZ)
    {
        return null;
    }

    protected String ProcessKey(String key)
    {
        String[] procdKeyTok = key.split(Utils.CONDITIONAL_DELIMITER);

        for (int i = 0; i < procdKeyTok.length; i++)
        {
            if ((int)missingDimStatus.get(i) == 0)
            {
                procdKeyTok[i] = Utils.CONDITIONAL_GENERIC;
            }
        }

        String procdKey = procdKeyTok[0];
        for (int i = 1; i < procdKeyTok.length; i++)
        {
            procdKey += Utils.CONDITIONAL_DELIMITER + procdKeyTok[i];
        }

        return procdKey;
    }
}
