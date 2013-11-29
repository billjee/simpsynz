package SimulationObjects;

import SimulationObjects.AgentAttributes.*;
import Utils.Utils;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.logging.Level;
import java.util.logging.Logger;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

public class Person extends SimulationObject implements Serializable
{
    private static int idCounter = 0;

        private int myHhldSize;
        public int GetHhldSize()
        {
            return myHhldSize;
        }
        public void SetHhldSize(int size)
        {
            myHhldSize = size;
        }
        private int myAge;
        public int GetAge()
        {
            return myAge;
        }
        public void SetAge(int curAge)
        {
            myAge = curAge;
        }
        private int mySex;
        public int GetSex()
        {
            return mySex;
        }
        public void SetSex(int curSex)
        {
            mySex = curSex;
        }
        private int myEducLevel;
        public int GetEducationLevel()
        {
            return myEducLevel;
        }
        public void SetEducationLevel(int eduLvl)
        {
            myEducLevel = eduLvl;
        }

        private String myZoneID;
        public String GetZoneID()
        {
            return myZoneID;
        }
        public void SetZoneID(String id)
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

        public Person(String currZone)
        {
            myZoneID = currZone;
            myID = ++idCounter;
            mySex = Sex.Female;
            myAge = Age.ThirtyFiveToFortyFour;
            myEducLevel = EducationLevel.primary;
            myHhldSize = HouseholdSize2.TwoPersons;
            myType = AgentType.Person;
        }

    @Override
        public String GetNewJointKey(String baseDim)
        {
            String jointKey;
            if (baseDim.equals("Age"))
            {
                jointKey = String.valueOf(mySex)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myHhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myEducLevel);
            }
            else if (baseDim.equals("Sex"))
            {
                jointKey = String.valueOf(myAge)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myHhldSize)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myEducLevel);
            }
            else if (baseDim.equals("HouseholdSize2"))
            {
                jointKey = String.valueOf(myAge)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(mySex)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myEducLevel);
            }
            else if (baseDim.equals("EducationLevel"))
            {
                jointKey = String.valueOf(myAge)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(mySex)
                            + Utils.CONDITIONAL_DELIMITER
                            + String.valueOf(myHhldSize);
            }
            else
            {
                jointKey = "";
            }

            return jointKey;
        }

    @Override
        public SimulationObject CreateNewCopy(String baseDim, int baseDimVal)
        {
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            ObjectOutputStream oos;
            try {
                oos = new ObjectOutputStream(bos);
                oos.writeObject(this);
                oos.flush();
                oos.close();
                bos.close();
                byte [] byteData = bos.toByteArray();

                ByteArrayInputStream bais = new ByteArrayInputStream(byteData);
                try
                {
                    Person myCopy = (Person)new ObjectInputStream(bais).readObject();

                    if (baseDim.equals("Age"))
                    {
                        myCopy.myAge = baseDimVal;
                    }
                    else if (baseDim.equals("Sex"))
                    {
                        myCopy.mySex = baseDimVal;
                    }
                    else if (baseDim.equals("HouseholdSize2"))
                    {
                        myCopy.myHhldSize = baseDimVal;
                    }
                    else if (baseDim.equals("EducationLevel"))
                    {
                        myCopy.myEducLevel = baseDimVal;
                    }
                    else
                    {
                        return null;
                    }
                    return myCopy;

                } catch (ClassNotFoundException ex) {
                    Logger.getLogger(Household.class.getName()).log(Level.SEVERE, null, ex);
                }

            } catch (IOException ex) {
                Logger.getLogger(Household.class.getName()).log(Level.SEVERE, null, ex);
            }
            return null;
        }
}
