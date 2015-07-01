using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.Quad;
using DateAccess.Services.Excel;

namespace DataAccess.Console.Migration.Excel
{
    public class PhoneBookCopier : ExcelCopier
    {
        public PhoneBookCopier(string file) : base(file)
        {
           
        }

        public override void Begin()
        {
            using (var context = new SiteResourceEntities())
            {
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.Quad.PhoneBook"));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.Quad.PhoneBook", 0));
                System.Console.WriteLine("Reading Excel...");
                CopyPhoneBook(context);
                System.Console.WriteLine("Saving to DB...");
                context.SaveChanges();
            }
        }

        private void CopyPhoneBook(SiteResourceEntities context)
        {
            Copy<QuadPhoneBook> copy = (ref QuadPhoneBook entity, int i, int j, string text, string sheet) =>
            {
                switch (j)
                {
                    case 1:
                        entity.LoginName = text;
                        break;
                    case 2:
                        entity.Intial = text; 
                        break;
                    case 3:
                        entity.Firstname = text;
                        break;
                    case 4:
                        entity.Lastname = text;
                        break;
                    case 5:
                        entity.ManagerInitial = text;
                        break;
                    case 6:
                        entity.DirectNumber = text;
                        break;
                    case 7:
                        entity.Ext = text;
                        break;
                    case 8:
                        entity.SpeedDail = text;
                        break;
                    case 9:
                        entity.Mobile = text;
                        break;
                    case 10:
                        entity.State = text;
                        break;
                    case 11:
                        entity.Group = text;
                        break;
                    case 12:
                        entity.Position = text;
                        break;
                    case 13:
                        entity.QuadArea = text;
                        break;
                    case 14:
                        entity.LeadArea = text;
                        break;
                    case 15:
                        entity.Fax = text;
                        break;
                    case 16:
                        entity.Email = text;
                        break;
                }
                return true;
            };

            Save<QuadPhoneBook> save = entity => context.QuadPhoneBook.Add(entity);
            Read("Staff Phone Listing", "A2", "P109", copy, save);
        }
    }
}