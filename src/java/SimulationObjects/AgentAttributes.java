package SimulationObjects;

/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

public class AgentAttributes
{
    public static class AgentType
    {
        public static int Household = 0;
        public static int Person = 1;
    }

    public static class HouseholdSize
    {
        public static int SingleMale = 0;
        public static int SingleFemale = 1;
        public static int TwoPersons = 2;
        public static int ThreePersons = 3;
        public static int FourPersons = 4;
        public static int FiveNPlusPersons = 5;
    }

    public static class DwellingType
    {
        public static int Separate = 0;
        public static int SemiAttached = 1;
        public static int Attached = 2;
        public static int Apartments = 3;
    }

    public static class NumOfCars
    {
        public static int NoCar = 0;
        public static int OneCar = 1;
        public static int TwoCars = 2;
        public static int ThreeOrMore = 3;
    }

    public static class NumOfWorkers
    {
        public static int None = 0;
        public static int One = 1;
        public static int TwoOrMore = 2;
    }

    public static class NumOfKids
    {
        public static int None = 0;
        public static int One = 1;
        public static int TwoOrMore = 2;
    }

    public static class NumWithUnivDeg
    {
        public static int None = 0;
        public static int One = 1;
        public static int TwoOrMore = 2;
    }

    public static class IncomeLevel
    {
        public static int ThirtyOrLess = 0;
        public static int ThirtyToSevetyFive = 1;
        public static int SeventyFiveToOneTwentyFive = 2;
        public static int OneTwentyFiveToTwoHundred = 3;
        public static int TwohundredOrMore = 4;
    }

    /// <summary>
    /// Person attributes
    /// </summary>
    public static class HouseholdType
    {
        public static int SinglePersonHhld = 0;
        public static int FamilyHhld = 1;
        public static int NonFamilyHhld = 2;
        public static int Collectivehhld = 3;
    }

    public static class HouseholdSize2
    {
        public static int SinglePerson = 0;
        public static int TwoPersons = 1;
        public static int ThreePersons = 2;
        public static int FourPersons = 3;
        public static int FivePersons = 4;
        public static int SixNPlusPersons = 5;
    }

    public static class Sex
    {
        public static int Male = 0;
        public static int Female = 1;
    }

    public static class MaritalStatus
    {
        public static int Single = 0;
        public static int Married = 1;
        public static int Widowed = 2;
        public static int Divorced = 3;
    }

    public static class EducationLevel
    {
        public static int none = 0;
        public static int primary = 1;
        public static int secondary = 2;
        public static int tertiary = 3;
    }

    public static class EmploymentStatus
    {
        public static int Employed = 0;
        public static int Unemployed = 1;
        public static int Inactive = 2;
        public static int UnderFifteen = 3;
    }

    public static class Age
    {
        public static int LessThanFifteen = 0;
        public static int FifteenToTwentyFour = 1;
        public static int TwentyFiveToThirtyFour = 2;
        public static int ThirtyFiveToFortyFour = 3;
        public static int FortyFiveToFiftyFour = 4;
        public static int FiftyFiveToSixtyFour = 5;
        public static int SixtyFiveToSeventyFour = 6;
        public static int SeventyFiveAndMore = 7;
    }
}
