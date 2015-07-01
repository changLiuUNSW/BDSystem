using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.Quote.Specification;
using DateAccess.Services.Excel;

namespace DataAccess.Console.Migration.Excel
{
    public class SpecCopier : ExcelCopier
    {
        public IDictionary<string, string[]> Sheets { get; set; }
        public SpecCopier(string file) : base(file)
        {

            Sheets = new Dictionary<string, string[]>
            {
                {"Spec Generic", new[] {"B11", "C104"}},
                {"Spec Aged_Care", new[] {"B11", "C64"}},
                {"Spec Generic_Commerical_Office", new[] {"B11", "C105"}},
                {"Spec Prop_Man_Tenancy", new[] {"B11", "C175"}},
                {"Spec Hospital", new[] {"B11", "C82"}},
                {"Spec Industrial", new[] {"B11", "C138"}},
                {"Spec Nursing_Home", new[] {"B11", "C127"}},
                {"Spec Hostel", new[] {"B11", "C63"}},
                {"Spec Meatworks", new[] {"B11", "C51"}},
                {"Spec School", new[] {"B11", "C86"}},
                {"Spec Hotel_Motel", new[] {"B11", "C123"}},
                {"Spec Large_Retail", new[] {"B11", "C95"}},
                {"Spec Aquatic", new[] {"B11", "C29"}},
                {"Spec Club", new[] {"B11", "C146"}},
                {"Spec Gym", new[] {"B11", "C124"}},
                {"Spec Leisure_Centres", new[] {"B11", "C124"}}
            };
        }

        public override void Begin()
        {
            using (var context = new SiteResourceEntities())
            {
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("ims_test.quote.SpecItem"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("ims_test.quote.SpecTitle"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("ims_test.quote.Spec"));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.quote.SpecItem", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.quote.SpecTitle", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.quote.Spec", 0));

                try
                {
                    Copy(context);
                }
                catch (DbEntityValidationException ex)
                {
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    throw;
                }
            }
        }


        private void Copy(SiteResourceEntities context)
        {
            const string pattern = @"[a-z].*";
            SpecTitle title = null;
            
            Copy<CleaningSpec> copy = (ref CleaningSpec entity, int i, int j, string value, string sheet) =>
            {
                if (string.IsNullOrEmpty(value))
                    return false;

                if (string.IsNullOrEmpty(entity.Name))
                {
                    var name = sheet.Replace("Spec ", "");
                    name = name.Replace('_', ' ');
                    entity.Name = name;                    
                }

                var match = Regex.Match(value, pattern);

                if (!match.Success)
                {
                    title = new SpecTitle
                    {
                        Description = value
                    };

                    if (entity.Titles == null)
                        entity.Titles = new Collection<SpecTitle>();

                    entity.Titles.Add(title);
                }
                else
                {
                    if (title != null)
                    {
                        switch (j)
                        {
                            case 1:
                                if (title.Items == null)
                                    title.Items = new Collection<SpecItem>();

                                title.Items.Add(new SpecItem
                                {
                                    Description = value
                                });
                                break;
                            case 2:
                                var item = title.Items.LastOrDefault();
                                if (item != null)
                                    item.Frequency = value;
                                break;
                        }
                    }
                }

                return true;
            };

            foreach (var sheet in Sheets)
            {
                var entity = ReadSingle(sheet.Key, sheet.Value[0], sheet.Value[1], copy);
                context.CleaningSpecs.Add(entity);
                context.SaveChanges();
            }
        }
    }
}
