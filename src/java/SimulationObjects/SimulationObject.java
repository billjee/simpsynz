package SimulationObjects;

import java.io.Serializable;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

public class SimulationObject implements Serializable
{
    protected int myID;

    public int GetID()
    {
        return myID;
    }

    public void SetID(int id)
    {
        myID = id;
    }

    protected int myType;
    public int GetAgentType()
    {
        return myType;
    }

    public void SetAgentType(int curTyp)
    {
        myType = curTyp;
    }

    public String GetNewJointKey(String baseDim)
    {
        return "";
    }

    public SimulationObject CreateNewCopy(String baseDim, int baseDimVal)
    {
        return null;
    }
}
