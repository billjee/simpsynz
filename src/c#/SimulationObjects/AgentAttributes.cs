/*
 * created by: b farooq, poly montreal
 * on: 22 october, 2013
 * last edited by: b farooq, poly montreal
 * on: 22 october, 2013
 * summary: 
 * comments:
 */

namespace SimulationObjects
{
    public enum AgentType
    {
        Household = 0,
        Person = 1
    }

    /// <summary>
    /// Household attributes
    /// </summary>

    public enum HouseholdSize
    {
        SingleMale = 0,
        SingleFemale = 1,
        TwoPersons = 2,
        ThreePersons = 3,
        FourPersons = 4,
        FiveNPlusPersons = 5
    };

    public enum DwellingType
    {
        Separate = 0,
        SemiAttached = 1,
        Attached = 2,
        Apartments = 3
    };

    public enum NumOfCars
    {
        NoCar = 0,
        OneCar = 1,
        TwoCars = 2,
        ThreeOrMore = 3
    };

    public enum NumOfWorkers
    {
        None = 0,
        One = 1,
        TwoOrMore = 2
    };

    public enum NumOfKids
    {
        None = 0,
        One = 1,
        TwoOrMore = 2
    };

    public enum NumWithUnivDeg
    {
        None = 0,
        One = 1,
        TwoOrMore = 2
    }

    public enum IncomeLevel
    {
        ThirtyOrLess = 0,
        ThirtyToSevetyFive = 1,
        SeventyFiveToOneTwentyFive = 2,
        OneTwentyFiveToTwoHundred = 3,
        TwohundredOrMore = 4
    }

    /// <summary>
    /// Person attributes
    /// </summary>

    public enum HouseholdType
    {
        SinglePersonHhld = 0,
        FamilyHhld = 1,
        NonFamilyHhld = 2,
        Collectivehhld = 3
    };

    public enum HouseholdSize2
    {
        SinglePerson = 0,
        TwoPersons = 1,
        ThreePersons = 2,
        FourPersons = 3,
        FivePersons = 4,
        SixNPlusPersons = 5
    };

    public enum Sex
    {
        Male = 0,
        Female = 1
    };

    public enum MaritalStatus
    {
        Single = 0,
        Married = 1,
        Widowed = 2,
        Divorced = 3
    };

    public enum EducationLevel
    {
        none = 0,
        primary = 1,
        secondary = 2,
        tertiary = 3
    };

    public enum EmploymentStatus
    {
        Employed = 0,
        Unemployed = 1,
        Inactive = 2,
        UnderFifteen = 3
    };

    public enum Age
    {
        LessThanFifteen = 0,
        FifteenToTwentyFour = 1,
        TwentyFiveToThirtyFour = 2,
        ThirtyFiveToFortyFour = 3,
        FortyFiveToFiftyFour = 4,
        FiftyFiveToSixtyFour = 5,
        SixtyFiveToSeventyFour = 6,
        SeventyFiveAndMore = 7
    };
}