using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using DataAccess.EntityFramework.DbContexts;

namespace DataAccess.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SiteResourceEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SiteResourceEntities context)
        {
            try
            {
                //  This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
                //  to avoid creating duplicate seed data. E.g.
                //
                //    context.People.AddOrUpdate(
                //      p => p.FullName,
                //      new Person { FullName = "Andrew Peters" },
                //      new Person { FullName = "Brice Lambson" },
                //      new Person { FullName = "Rowan Miller" }
                //    );
                //

                //Lead Predefined status
                LeadInitializer.Start(context);
                

                //Quote Predefined status list
                QuoteInitializer.Start(context);


                //for Lead development
//                var addr = new Address
//                {
//                    Number = "1",
//                    Unit = "Unit 10",
//                    Street = "Canal Road",
//                    Suburb = "ST PETERS"
//
//                };
//                context.Leads.AddOrUpdate(
//                   l => l.BusinessTypeId,
//                   new Lead
//                   {
//                       TelesaleId = 1,
//                       BusinessTypeId = 2,
//                       CreatedDate = new DateTime(2015, 4, 10),
//                       LastUpdateDate = new DateTime(2015, 4, 10),
//                       ContactId = 1,
//                       LeadPersonalId = 1,
//                       LeadStatusId = 1,
//                       State = "NSW",
//                       Postcode = "2044",
//                       Phone = "0295578723",
//                       Address = addr
//                   });
//                context.Leads.AddOrUpdate(
//                  l => l.BusinessTypeId,
//                  new Lead
//                  {
//                      TelesaleId = 1,
//                      BusinessTypeId = 1,
//                      CreatedDate = new DateTime(2015, 4, 10),
//                      LastUpdateDate = new DateTime(2015, 4, 10),
//                      ContactId = 1,
//                      LeadPersonalId = 1,
//                      LeadStatusId = 1,
//                      State = "NSW",
//                      Postcode = "2044",
//                      Phone = "0295578723",
//                      Address = addr
//                  });
//                context.Leads.AddOrUpdate(
//                  l => l.BusinessTypeId,
//                  new Lead
//                  {
//                      TelesaleId = 1,
//                      BusinessTypeId = 3,
//                      CreatedDate = new DateTime(2015, 4, 10),
//                      LastUpdateDate = new DateTime(2015, 4, 10),
//                      ContactId = 1,
//                      LeadPersonalId = 1,
//                      LeadStatusId = 1,
//                      State = "NSW",
//                      Postcode = "2044",
//                      Phone = "0295578723",
//                      Address = addr
//                  });
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (DbValidationError ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}