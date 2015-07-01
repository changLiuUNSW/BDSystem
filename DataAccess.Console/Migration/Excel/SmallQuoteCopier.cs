using System;
using System.Data.Entity.Validation;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.Quote.Cost.Equipment;
using DataAccess.EntityFramework.Models.Quote.Cost.Supply;
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

                System.Console.WriteLine("Reading Machine...");
                CopyMachine(context);
                System.Console.WriteLine("Saving Machie...");

                System.Console.WriteLine("Reading Equipment...");
                CopyEquipment(context);
                System.Console.WriteLine("Saving Equipment...");

                System.Console.WriteLine("Reading Supply...");
                CopySupply(context);
                System.Console.WriteLine("Saving Supply...");

                
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
    }
}
