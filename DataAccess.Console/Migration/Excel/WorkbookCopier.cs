using System;
using System.Linq;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.Excel;

namespace DataAccess.Console.Migration.Excel
{
   

    public class WorkbookCopier : ExcelCopier
    {

        public WorkbookCopier(string file) : base(file)
        {
            
        }

        public override void Begin()
        {
            using (var context = new SiteResourceEntities())
            {
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.leadpersonal"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.leadgroup"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.leadpriority"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.leadshiftinfo"));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.leadpersonal", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.leadpriority", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.leadshiftinfo", 0));

                System.Console.WriteLine("Reading workbook...");
                CopyLeadGroup(context);
                context.SaveChanges();

                CopyLeadPersonal(context);
                CopyLeadPriority(context);
                CopyLeadShift(context);

                System.Console.WriteLine("Saving workbook...");
                context.SaveChanges();
            }
        }

        private void CopyLeadGroup(SiteResourceEntities context)
        {
            Copy<LeadGroup> copy = delegate(ref LeadGroup entity, int i, int j, string value, string sheet)
            {
                switch (j)
                {
                    case 1:
                        var parse = (LeadGroups)Enum.Parse(typeof(LeadGroups), value);
                        entity.Id = (byte) parse;
                        entity.Group = value;
                        break;
                    case 2:
                        entity.LeadTarget = Convert.ToInt32(value);
                        break;
                    case 3:
                        entity.LeadStop = Convert.ToInt32(value);
                        break;
                }

                return true;
            };

            Save<LeadGroup> save = entity => context.LeadGroups.Add(entity);
            Read("Lead Priorities", "F3", "H6", copy, save);
        }

        private void CopyLeadPersonal(SiteResourceEntities context)
        {
            Copy<LeadPersonal> copy = delegate(ref LeadPersonal entity, int i, int j, string value, string sheet)
            {
                switch (j)
                {
                    case 2:
                        entity.Initial = value;
                        break;
                    case 3:
                        entity.Name = value;
                        break;
                    case 4:
                        entity.GroupId = (byte) ((LeadGroups)Enum.Parse(typeof (LeadGroups), value));
                        break;
                }

                return true;
            };

            Save<LeadPersonal> save = entity => context.LeadPersonals.Add(entity);
                
            Read("Lead Priorities", "B76", "Q106", copy, save);
        }

        private void CopyLeadPriority(SiteResourceEntities context)
        {
            Copy<LeadPriority> copy = delegate(ref LeadPriority entity, int i, int j, string value, string sheet)
            {
                switch (j)
                {
                    case 1:
                        int number;
                        var isNumber = int.TryParse(value, out number);

                        if (!isNumber)
                            return false;

                        entity.Distance = number;
                        break;
                    case 2:
                        if (value.StartsWith("G"))
                            entity.Role = (byte)(LeadGroups.GGM | LeadGroups.GM | LeadGroups.ROP);
                        else
                            entity.Role = (byte) LeadGroups.BD;
                        break;
                    case 3:
                        entity.Priority = value;
                        break;
                }

                return true;
            };

            Save<LeadPriority> save = delegate(LeadPriority entity)
            {
                if (
                    context.LeadPriorities.Any(
                        x => x.Distance == entity.Distance && x.Priority == entity.Priority && x.Role == entity.Role))
                    return;

                context.LeadPriorities.Add(entity);
                context.SaveChanges();
            };

            Read("Lead Priorities", "B3", "D66", copy, save);
        }

        private void CopyLeadShift(SiteResourceEntities context)
        {
            Copy<LeadShiftInfo> copy = delegate(ref LeadShiftInfo entity, int i, int j, string value, string sheet)
            {
                switch (i)
                {
                    case 1:
                        entity.RecordPerShift = int.Parse(value);
                        break;
                    case 2:
                        entity.LeadPerShift = int.Parse(value);
                        break;
                    case 3:
                        entity.ContactRatePer3HrShift = int.Parse(value);
                        break;
                    case 4:
                        entity.CallCycleWeeks = int.Parse(value);
                        break;
                    case 5:
                        entity.PaCalledWks = int.Parse(value);
                        break;
                }

                return true;
            };

            Save<LeadShiftInfo> save = entity => context.LeadShiftInfos.Add(entity);

            ReadVertical("Lead Priorities", "AD67", "AD71", copy, save);            
        }
    }
}
