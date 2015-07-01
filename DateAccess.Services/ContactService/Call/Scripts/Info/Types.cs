namespace DateAccess.Services.ContactService.Call.Scripts.Info
{
    public enum ScriptType
    {
        OPR,
        GRP,
        LPM,
        BMS,
        PMS,
        Quali_Question,
        Quali_Email,
        Quali_Security,
        Quali_Maintenance,
        Cln_QaQt,
        Cln_Inhouse,
        Cln_Security,
        Cln_Only
    }

    public enum ActionType
    {
        UpdateNextCallDate,
        NewCleaningLead,
        NewSecurityLead,
        NewMaintenanceLead,
        ReceptionNotPutThrough,
        TaskForDH,
        UpdateContactInfo
    }

    public enum BranchTypes
    {
        PropertyMananger,
        ContctPerson
    }
}
