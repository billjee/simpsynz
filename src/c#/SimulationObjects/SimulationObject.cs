/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimulationObjects
{
    [Serializable]
    class SimulationObject
    {
        protected uint myID;
        public uint GetID()
        {
            return myID;
        }

        public void SetID(uint id)
        {
            myID = id;
        }

        protected AgentType myType;
        public AgentType GetAgentType()
        {
            return myType;
        }
        public void SetAgentType(AgentType curTyp)
        {
            myType = curTyp;
        }
        public virtual string GetNewJointKey(string baseDim)
        {
            return "";
        }
        public virtual SimulationObject CreateNewCopy(string baseDim,
            int baseDimVal)
        {
            return null;
        }
    }
}
