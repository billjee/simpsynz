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
    sealed class Person : SimulationObject
    {
        private static uint idCounter = 0;

        private HouseholdSize2 HouseholdSize;
        public HouseholdSize2 GetHhldSize()
        {
            return HouseholdSize;
        }
        public void SetHhldSize(HouseholdSize2 size)
        {
            HouseholdSize = size;
        }
        private Age Age;
        public Age GetAge()
        {
            return Age;
        }
        public void SetAge(Age curAge)
        {
            Age = curAge;
        }
        private Sex Sex;
        public Sex GetSex()
        {
            return Sex;
        }
        public void SetSex(Sex curSex)
        {
            Sex = curSex;
        }
        private EducationLevel EducationLevel;
        public EducationLevel GetEducationLevel()
        {
            return EducationLevel;
        }
        public void SetEducationLevel(EducationLevel eduLvl)
        {
            EducationLevel = eduLvl;
        }

        private string ZoneID;
        public string GetZoneID()
        {
            return ZoneID;
        }
        public void SetZoneID(string id)
        {
            ZoneID = id;
        }

        public Person()
        {
            Sex = Sex.Female;
            Age = Age.ThirtyFiveToFortyFour;
            EducationLevel = EducationLevel.primary;
            HouseholdSize = HouseholdSize2.TwoPersons;
            Type = AgentType.Person;
        }

        public Person(string currZone)
        {
            ZoneID = currZone;
            Sex = Sex.Female;
            Age = Age.ThirtyFiveToFortyFour;
            EducationLevel = EducationLevel.primary;
            HouseholdSize = HouseholdSize2.TwoPersons;
            Type = AgentType.Person;
        }

        private Person(Person original)
        {
            Type = AgentType.Person;
            //copy the values
            ZoneID = original.ZoneID;
            Age = original.Age;
            Sex = original.Sex;
            HouseholdSize = original.HouseholdSize;
            EducationLevel = original.EducationLevel;
        }

        public override string GetNewJointKey(string baseDim)
        {
            string jointKey;
            if(baseDim == "Age")
            {
                jointKey = ((int)Sex).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)HouseholdSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)EducationLevel).ToString();
            }
            else if(baseDim == "Sex")
            {
                jointKey = ((int)Age).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)HouseholdSize).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)EducationLevel).ToString();
            }
            else if(baseDim == "HouseholdSize2")
            {
                jointKey = ((int)Age).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)Sex).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)EducationLevel).ToString();
            }
            else if(baseDim == "EducationLevel")
            {
                jointKey = ((int)Age).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)Sex).ToString()
                            + Constants.CONDITIONAL_DELIMITER
                            + ((int)HouseholdSize).ToString();
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
            Person copy = new Person(this);
            // apply the new change
            switch(baseDim)
            {
                case "Age":
                    copy.Age = (Age)baseDimVal;
                    break;
                case "Sex":
                    copy.Sex = (Sex)baseDimVal;
                    break;
                case "HouseholdSize2":
                    copy.HouseholdSize = (HouseholdSize2)baseDimVal;
                    break;
                case "EducationLevel":
                    copy.EducationLevel = (EducationLevel)baseDimVal;
                    break;
                default:
                    return null;
            }
            return copy;
        }
    }
}
