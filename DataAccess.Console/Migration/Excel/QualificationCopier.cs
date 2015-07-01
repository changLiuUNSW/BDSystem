using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Extensions.Utilities;
using DataAccess.EntityFramework.Models.BD.Site;
using DataAccess.EntityFramework.TypeLibrary;
using DateAccess.Services.Excel;

namespace DataAccess.Console.Migration.Excel
{
    public class QualificationCopier : ExcelCopier
    {
        public QualificationCopier(string file) : base(file)
        {
        }

        public override void Begin()
        {
            using (var context = new SiteResourceEntities())
            {
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.BuildingQualifyCriteria"));
                context.Database.ExecuteSqlCommand(SqlCmd.EmptyTable("IMS_Test.bd.BuildingType"));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.BuildingQualifyCriteria", 0));
                context.Database.ExecuteSqlCommand(SqlCmd.Reseed("ims_test.bd.BuildingType", 0));

                System.Console.WriteLine("Reading qualification...");
                CopyCriteria(context);
                System.Console.WriteLine("Saving qualification...");
                context.SaveChanges();
            }
        }

        private void CopyCriteria(SiteResourceEntities context)
        {
            Copy<BuildingType> copy = delegate(ref BuildingType entity, int i, int j, string value, string sheet)
            {
                if (entity.Criterias == null)
                    entity.Criterias = new Collection<BuildingQualifyCriteria>();

                switch (j)
                {
                    case 1:
                        entity.Code = value;
                        break;
                    case 3:
                        if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(entity.Type))
                            return false;

                        if (string.Compare(value, "School", StringComparison.OrdinalIgnoreCase) == 0 ||
                            string.Compare(value, "CBD residential Tower", StringComparison.OrdinalIgnoreCase) == 0)
                            return false;

                        entity.Type = value;
                        break;
                    case 5:
                        entity.CriteriaDescription = value;
                        break;
                    case 6:
                        GetCriteria(value, Size.Size120.GetDescription(), ref entity);
                        break;
                    case 7:
                        GetCriteria(value, Size.Size050.GetDescription(), ref entity);
                        break;
                    case 8:
                        GetCriteria(value, Size.Size025.GetDescription(), ref entity);
                        break;
                    case 9:
                        GetCriteria(value, Size.Size008.GetDescription(), ref entity);
                        break;
                }

                return true;
            };

            Save<BuildingType> save = delegate(BuildingType entity)
            {
                context.BuildingTypes.Add(entity);
            };

            Read("New Verticals", "A6", "J34", copy, save);
        }

        private void GetCriteria(string target, string size, ref BuildingType buildingType)
        {
            var criteria = GetCriteriaFromString(target, size);
            if (criteria != null)
                buildingType.Criterias.Add(criteria);
        }

        private BuildingQualifyCriteria GetCriteriaFromString(string target, string size)
        {
            if (Regex.Replace(target, @"\s+", "").IndexOf(@"N/A", StringComparison.OrdinalIgnoreCase) >= 0)
                return null;

            var criteria = new BuildingQualifyCriteria
            {
                Size = size
            };

            if (target.IndexOf("Auto", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                criteria.AutoQualify = true;
            }
            else if (target.IndexOf("over", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                criteria.From = Convert.ToDouble(Regex.Match(target, @"\d+").Value);
            }
            else if (target.IndexOf('-', 0) >= 0)
            {
                var numbers = Regex.Matches(target, @"\d+");
                criteria.From = Convert.ToDouble(numbers[0].Value);
                criteria.To = Convert.ToDouble(numbers[1].Value);
            }
            else /*if (target.IndexOf("under", StringComparison.OrdinalIgnoreCase) >= 0)*/
            {
                criteria.To = Convert.ToDouble(Regex.Match(target, @"\d+").Value);
            }

            return criteria;
        }
    }
}
