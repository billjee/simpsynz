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
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PopulationSynthesis.Utils;

namespace SimulationObjects
{
    [Serializable]
    class Person : SimulationObject
    {
        private static uint idCounter = 0;

        private HouseholdSize2 myHhldSize;
        public HouseholdSize2 GetHhldSize()
        {
            return myHhldSize;
        }
        public void SetHhldSize(HouseholdSize2 size)
        {
            myHhldSize = size;
        }
        private Age myAge;
        public Age GetAge()
        {
            return myAge;
        }
        public void SetAge(Age curAge)
        {
            myAge = curAge;
        }
        private Sex mySex;
        public Sex GetSex()
        {
            return mySex;
        }
        public void SetSex(Sex curSex)
        {
            mySex = curSex;
        }
        private EducationLevel myEducLevel;
        public EducationLevel GetEducationLevel()
        {
            return myEducLevel;
        }
        public void SetEducationLevel(EducationLevel eduLvl)
        {
            myEducLevel = eduLvl;
        }

        private string myZoneID;
        public string GetZoneID()
        {
            return myZoneID;
        }
        public void SetZoneID(string id)
        {
            myZoneID = id;
        }

        public Person()
        {
            myID = ++idCounter;
            mySex = Sex.Female;
            myAge = Age.ThirtyFiveToFortyFour;
            myEducLevel = EducationLevel.primary;
            myHhldSize = HouseholdSize2.TwoPersons;
            myType = AgentType.Person;
        }

        public Person(string currZone)
        {
            myZoneID = currZone;
            myID = ++idCounter;
            mySex = Sex.Female;
            myAge = Age.ThirtyFiveToFortyFour;
            myEducLevel = EducationLevel.primary;
            myHhldSize = HouseholdSize2.TwoPersons;
            myType = AgentType.Person;
        }

        public override string GetNewJointKey(string baseDim)
        {
            string jointKey;
            if (baseDim == "Age")
            {
                jointKey = ((int)mySex).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myHhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myEducLevel).ToString();
            }
            else if (baseDim == "Sex")
            {
                jointKey = ((int)myAge).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myHhldSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myEducLevel).ToString();
            }
            else if (baseDim == "HouseholdSize2")
            {
                jointKey = ((int)myAge).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)mySex).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myEducLevel).ToString();
            }
            else if (baseDim == "EducationLevel")
            {
                jointKey = ((int)myAge).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)mySex).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)myHhldSize).ToString();
            }
            else
            {
                jointKey = "";
            }

            return jointKey;
        }

        public override SimulationObject CreateNewCopy(string baseDim,
            int baseDimVal)
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(m, this);
            m.Position = 0;
            Person myCopy = (Person)b.Deserialize(m);

            if (baseDim == "Age")
            {
                myCopy.myAge = (Age)baseDimVal;
            }
            else if (baseDim == "Sex")
            {
                myCopy.mySex = (Sex)baseDimVal;
            }
            else if (baseDim == "HouseholdSize2")
            {
                myCopy.myHhldSize = (HouseholdSize2)baseDimVal;
            }
            else if (baseDim == "EducationLevel")
            {
                myCopy.myEducLevel = (EducationLevel)baseDimVal;
            }
            else
            {
                return null;
            }
            return myCopy;
        }
    }
}
