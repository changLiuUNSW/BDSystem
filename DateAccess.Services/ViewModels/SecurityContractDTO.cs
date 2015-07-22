using System;

namespace DateAccess.Services.ViewModels
{
    public class SecurityContractDTO
    {
        public int SiteId { get; set; }
        public string Contarctor { get; set; }
        public DateTime? ReviewDate { get; set; }
        public bool GuardingPersonnel { get; set; }
        public bool MobilePatrol { get; set; }
        public bool Conceirge { get; set; }
        public bool ElectronicInstallation { get; set; }
        public bool BackToBaseMonitoring { get; set; }
        public bool SecurityMaintenance { get; set; }
    }
}
