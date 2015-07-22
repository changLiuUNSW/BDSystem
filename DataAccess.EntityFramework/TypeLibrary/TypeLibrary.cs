using System;
using System.ComponentModel;

namespace DataAccess.EntityFramework.TypeLibrary
{
    [Flags]
    public enum LeadGroups
    {
        GM = 1,
        GGM = GM << 1,
        BD = GM << 2,
        ROP = GM << 3
    }

    [Flags]
    public enum CallLineStatus
    {
        Attempted = 1,
        Contacted = Attempted << 1,
        NotAnswered = Attempted << 2
    }

    public enum CallType
    {
        BD,
        Telesale
    }

    public enum Size
    {
        [Description("008")]
        Size008,
        [Description("025")]
        Size025,
        [Description("050")]
        Size050,
        [Description("120")]
        Size120
    }

    //TODO: BusinessType Table, Please always sync 
    public enum BusinessTypes
    {
        Cleaning = 1,
        Security = 2,
        Maintenance = 3,
        Others = 4
    }


    //TODO: LeadStatus Table, Please always sync 
    public enum LeadStatusTypes
    {
        New = 1,
        ToBeCalled = 2,
        CallBack=3,
        Appointment = 4,
        Visited = 5,
        Estimation=6,
        Quoted = 7,
        Cancelled = 8,
        
    }


    //TODO: Quote.Status Table, Please always sync 
    public enum QuoteStatusTypes
    {
        New = 1,
        Estimation=2,
        WPReview = 3,
        QPReview=4,
        BDReview=5,
        PreFinalReview=6,
        FinalReview=7,
        Print=8,
        PresentToClient=9,
        WPIssues=10,
        QPIssues = 11,
        Current=12,
        Cancel=13,
        Dead=14
    }


    public enum QuoteQuestionType
    {
        ToCurrent=1,
        NotCalled=2,
        Dead=3,
        NoDead=4,
        NoAdjust=5,
        NoEmail=6
    }

    public enum QuoteAnswerType
    {
        None=0,
        Date=1,
        Text=2
    }

    //TODO: Group type in the database should be always lower case
    public enum GroupType
    {
        Building = 1,
        Group = 2
    }

    public enum SqlExceptionType
    {
        PKViolate = 2627
    }

    public enum CostStatus
    {
        Draft = 0,
        Finalize
    }


    public enum CostType
    {
        Upload=1,
        System=2
    }

    public enum LabourRateOptions
    {
        [Description("Part Time Day")]
        PartTimeDay = 1,
        [Description("Part Time Night")]
        PartTimeNight = 2,
        [Description("Full Time Day")]
        FullTimeDay = 3,
        [Description("Full Time Night")]
        FullTimeNight = 4,
        [Description("Casual Night")]
        CasualNight = 5,
        [Description("F/T Day Cleanstart")]
        FullTimeDayCleanStart = 6,
        [Description("F/T Day Cleanstart")]
        PartTimeDayCleanStart = 7,
        [Description("F/T Night Cleanstart")]
        PartTimeNightCleanStart = 8,
        [Description("Supervisor")]
        Supervisor = 9
    }
    
    //for most cost tables that needs to access price per state
    public enum States
    {
        NSW = 1,
        QLD = 2,
        ACT = 3,
        VIC = 4,
        SA = 5,
        NT = 6,
        WA = 7,
        TAS = 8
    }
}
