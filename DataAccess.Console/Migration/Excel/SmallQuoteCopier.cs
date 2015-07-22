using System;
using System.Data.Entity.Validation;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Extensions;
using DataAccess.EntityFramework.Models.Quote.Cost;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Labour;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.Excel;

namespace DataAccess.Console.Migration.Excel
{
    public class SmallQuoteCopier : ExcelCopier
    {
        public SmallQuoteCopier(string file) : base(file)
        {
            
        }

        public override void Begin()
        {
            using (var context = new SiteResourceEntities())
            {
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.Equipment"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.Machine"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.ToiletRequisite"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.LabourRate"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.AllowanceRate"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.OnCostRate"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.PublicLiability"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.QuoteSource"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.quote.StandardRegion"));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.quote.LabourRate", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.quote.AllowanceRate", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.quote.OnCostRate", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.quote.PublicLiability", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.quote.QuoteSource", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("IMS_Test.quote.StandardRegion",0));

                System.Console.WriteLine("Reading Machine...");
                CopyMachine(context);
                System.Console.WriteLine("Saving Machie...");

                System.Console.WriteLine("Reading Equipment...");
                CopyEquipment(context);
                System.Console.WriteLine("Saving Equipment...");

                System.Console.WriteLine("Reading Supply...");
                CopySupply(context);
                System.Console.WriteLine("Saving Supply...");

                System.Console.WriteLine("Reading labour rate...");
                CopyLabourRate(context);
                System.Console.WriteLine("Saving labour rate...");

                System.Console.WriteLine("Reading allowance rate...");
                CopyAllowanceRate(context);
                System.Console.WriteLine("Saving allowance rate...");

                System.Console.WriteLine("Reading oncost rate...");
                CopyOnCostRate(context);
                System.Console.WriteLine("Saving oncost rate...");

                System.Console.WriteLine("Reading publicLiability rate...");
                CopyPublicLiability(context);
                System.Console.WriteLine("Saving publicLiability rate...");

                System.Console.WriteLine("Reading QuoteSource...");
                CopyQuoteSource(context);
                System.Console.WriteLine("Saving QuoteSource...");

                System.Console.WriteLine("Reading StandardRegion...");
                CopyStandardRegion(context);
                System.Console.WriteLine("Saving StandardRegion...");
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw;
                }
            }
        }

        private void CopyStandardRegion(SiteResourceEntities context)
        {
            Copy<StandardRegion> copy = (ref StandardRegion entity, int i, int j, string value, string sheet) =>
            {
                entity.Name = value;
                return true;
            };
            Save<StandardRegion> save = entity => context.StandardRegions.Add(entity);
            Read("Small Quote form","L190","L197",copy,save);
        }

        private void CopyQuoteSource(SiteResourceEntities context)
        {
            Copy<QuoteSource> copy = (ref QuoteSource entity, int i, int j, string value, string sheet) =>
            {
                switch (j)
                {
                    case 1:
                        entity.Description = value;
                        break;
                    case 2:
                        return false;
                    case 3:
                        entity.Name = value;
                        break;
                }
                return true;
            };
            Save<QuoteSource> save = entity => context.QuoteSources.Add(entity);
            Read("Small Quote form", "H191", "J199", copy, save);
        }

        private void CopyPublicLiability(SiteResourceEntities context)
        {
            Copy<PublicLiability> copy = (ref PublicLiability entity, int i, int j, string value, string sheet) =>
            {
                switch (j)
                {
                    case 1:
                        entity.Description = value;
                        break;
                    case 2:
                        entity.Precentage = Convert.ToDecimal(value);
                        break;
                }
                return true;
            };
            Save<PublicLiability> save = entity => context.PublicLiabilities.Add(entity);
            Read("Small Quote form","B189","C190",copy,save);
        }

        private void CopyMachine(SiteResourceEntities context)
        {
            Copy<Machine> copy = (ref Machine entity, int i, int j, string value, string sheet) =>
            {
                switch (j)
                {
                    case 1:
                        entity.Type = value;
                        break;
                    case 2:
                        int years;
                        if (int.TryParse(value, out years))
                            entity.YearsAllocated = years;
                        break;
                    case 3:
                        double repairFactor;
                        if (double.TryParse(value, out repairFactor))
                            entity.RepairFactor = repairFactor;
                        break;
                    case 4:
                        int fuel;
                        if (int.TryParse(value, out fuel))
                            entity.Fuels = fuel;
                        break;
                }

                return true;
            };

            Save<Machine> save = entity => context.Machines.Add(entity);
            Read("Equ", "B75", "E124", copy, save);
        }

        private void CopyEquipment(SiteResourceEntities context)
        {
            Copy<Equipment> copy = (ref Equipment entity, int i, int j, string text, string sheet) =>
            {
                var toSave = true;

                switch (j)
                {
                    case 1:
                        entity.EquipmentCode = text;
                        entity.UpdateDate = DateTime.Today;
                        break;
                    case 2:

                        if (string.IsNullOrEmpty(entity.EquipmentCode))
                            toSave = false;

                        entity.Description = text;
                        break;
                    case 3:
                        int cost;
                        if (int.TryParse(text, out cost))
                            entity.Cost = cost;
                        break;
                    case 11:
                        entity.MachineType = text;
                        break;
                    case 17:
                        entity.UserGuide = text;
                        break;
                    case 18:
                        entity.Comment = text;
                        break;
                    case 19:
                        int rate;
                        if (int.TryParse(text, out rate))
                            entity.ProductionRatePerHour = rate;
                        break;
                    case 21:
                        entity.Issues = text;
                        break;
                    case 22:
                        entity.IsLargeEquipment = !string.IsNullOrEmpty(text);
                        break;
                }

                return toSave;
            };

            Save<Equipment> save = entity => context.Equipments.Add(entity);


            Read("Equ", "A12", "V68", copy, save);
        }

        private void CopySupply(SiteResourceEntities context)
        {
            Copy<ToiletRequisite> copy = (ref ToiletRequisite entity, int i, int j, string text, string sheet) =>
            {
                switch (j)
                {
                    case 1:
                        entity.ItemCode = text;
                        break;
                    case 2:
                        entity.Type = text;
                        break;
                    case 3:
                        entity.Description = text;
                        break;
                    case 4:
                        entity.RarelyUsed = !string.IsNullOrEmpty(text); 
                        break;
                    case 5:
                        entity.Ply = text;
                        break;
                    case 6:
                        entity.Size = text;
                        break;
                    case 7:
                        entity.Quality = text;
                        break;
                    case 8:
                        entity.UnitsPerCarton = text;
                        break;
                    case 9:
                        decimal price;
                        decimal.TryParse(text, out price);
                        entity.Price = price;
                        break;
                    case 12:
                        entity.HayesCode = text;
                        break;
                    case 14:
                        decimal cost;
                        decimal.TryParse(text, out cost);
                        entity.Cost = cost;
                        break;
                }

                return true;
            };

            Save<ToiletRequisite> save = entity =>
            {
                if (string.IsNullOrEmpty(entity.ItemCode))
                    return;

                context.ToiletRequisites.Add(entity);
            };

            Read("Toi", "B6", "O127", copy, save);
        }

        private void CopyLabourRate(SiteResourceEntities context)
        {
            Copy<LabourRate> copy = delegate(ref LabourRate entity, int i, int j, string value, string sheet)
            {
                switch (j)
                {
                    case 1:
                        entity.Weekdays = Convert.ToDecimal(value);
                        break;
                    case 2:
                        entity.Saturday = Convert.ToDecimal(value);
                        break;
                    case 3:
                        entity.Sunday = Convert.ToDecimal(value);
                        break;
                    case 4:
                        entity.Holiday = Convert.ToDecimal(value);
                        break;
                }

                if (string.IsNullOrEmpty(entity.Title))
                {
                    switch (i)
                    {
                        case 1:
                            entity.Title = LabourRateOptions.PartTimeDay.GetDescription();
                            break;
                        case 2:
                            entity.Title = LabourRateOptions.PartTimeNight.GetDescription();
                            break;
                        case 3:
                            entity.Title = LabourRateOptions.FullTimeDay.GetDescription();
                            break;
                        case 4:
                            entity.Title = LabourRateOptions.FullTimeNight.GetDescription();
                            break;
                        case 5:
                            entity.Title = LabourRateOptions.CasualNight.GetDescription();
                            break;
                        case 6:
                            entity.Title = LabourRateOptions.FullTimeDayCleanStart.GetDescription();
                            break;
                        case 7:
                            entity.Title = LabourRateOptions.PartTimeDayCleanStart.GetDescription();
                            break;
                        case 8:
                            entity.Title = LabourRateOptions.PartTimeNightCleanStart.GetDescription();
                            break;
                        case 9:
                            entity.Title = LabourRateOptions.Supervisor.GetDescription();
                            break;
                    }    
                }

                return true;
            };

            Save<LabourRate> save = entity => context.LabourRates.Add(entity);
            Read("Quad Labour", "C122", "F130", copy, save);
        }

        private void CopyAllowanceRate(SiteResourceEntities context)
        {
            Copy<AllowanceRate> copy = (ref AllowanceRate entity, int i, int j, string value, string sheet) =>
            {
                switch (i)
                {
                    case 1:
                        entity.State = "NSW";
                        break;
                    case 2:
                        entity.State = "QLD";
                        break;
                    case 3:
                        entity.State = "ACT";
                        break;
                    case 4:
                        entity.State = "VIC";
                        break;
                    case 5:
                        entity.State = "SA";
                        break;
                    case 6:
                        entity.State = "NT";
                        break;
                    case 7:
                        entity.State = "WA";
                        break;
                    case 8:
                        entity.State = "TAS";
                        break;
                }

                switch (j)
                {
                    case 1:
                        entity.ToiletAllowPerShift = Convert.ToDecimal(value);
                        break;
                    case 2:
                        entity.LeadingHandSmallGroup = Convert.ToDecimal(value);
                        break;
                    case 3:
                        entity.LeadingHandLargeGroup = Convert.ToDecimal(value);
                        break;
                    case 5:
                        entity.NumberOfHolidays = Convert.ToInt32(value);
                        break;
                }

                return true;
            };

            Save<AllowanceRate> save = entity => context.AllowanceRates.Add(entity);
            ReadHorizontally("Quad Labour", "C134", "J138", copy, save);
        }

        private void CopyOnCostRate(SiteResourceEntities context)
        {
            Copy<OnCostRate> copy = (ref OnCostRate entity, int i, int j, string value, string sheet) =>
            {
                switch (i)
                {
                    case 1:
                        entity.State = "NSW";
                        break;
                    case 2:
                        entity.State = "QLD";
                        break;
                    case 3:
                        entity.State = "ACT";
                        break;
                    case 4:
                        entity.State = "VIC";
                        break;
                    case 5:
                        entity.State = "SA";
                        break;
                    case 6:
                        entity.State = "NT";
                        break;
                    case 7:
                        entity.State = "WA";
                        break;
                    case 8:
                        entity.State = "TAS";
                        break;
                }

                switch (j)
                {
                    case 1:
                        entity.HolidayPay = Convert.ToDecimal(value);
                        break;
                    case 2:
                        entity.SickPay = Convert.ToDecimal(value);
                        break;
                    case 3:
                        entity.WorkerCompensation = Convert.ToDecimal(value);
                        break;
                    case 4:
                        entity.Superannuation = Convert.ToDecimal(value);
                        break;
                    case 5:
                        entity.PayrollTax = Convert.ToDecimal(value);
                        break;
                    case 6:
                        entity.LongService = Convert.ToDecimal(value);
                        break;
                    case 7:
                        entity.IncomeProtection = Convert.ToDecimal(value);
                        break;
                    case 9:
                        entity.Materials = Convert.ToDecimal(value);
                        break;
                }


                return true;
            };

            Save<OnCostRate> save = entity => context.OnCostRates.Add(entity);
            ReadHorizontally("Quad Labour", "E142", "L150", copy, save);
        }
    }
}
