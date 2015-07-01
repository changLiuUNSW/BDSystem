using System.Data.Entity.Migrations;
using DataAccess.EntityFramework.DbContexts;
using DataAccess.EntityFramework.Models.BD.Lead;

namespace DataAccess.EntityFramework.Migrations
{
    internal static class LeadInitializer
    {
        public static void Start(SiteResourceEntities context)
        {
            StatusInit(context);
        }

        private static void StatusInit(SiteResourceEntities context)
        {
            context.LeadStatus.AddOrUpdate(
                    l => l.Name,
                    new LeadStatus
                    {
                        Name = "new",
                        Description = "Please contact to make an appointment",
                        Order = 1
                    }, new LeadStatus
                    {
                        Name = "toBeCalled",
                        Description = "Contact not successfully, please contact again",
                        Order = 2
                    },
                    new LeadStatus
                    {
                        Name = "callback",
                        Description = "Please make a callback on: ",
                        Order = 3
                    },
                    new LeadStatus
                    {
                        Name = "appointment",
                        Description = "Please make a site visit on: ",
                        Order = 4
                    },
                    new LeadStatus
                    {
                        Name = "visited",
                        Description = "Please click following buttons to do future actions",
                        Order = 5
                    },
                    new LeadStatus
                    {
                        Name = "estimation",
                        Description = null,
                        Order = 6,
                    },
                    new LeadStatus
                    {
                        Name = "quoted",
                        Description = null,
                        Order = 7,
                        Hidden = true
                    },
                    new LeadStatus
                    {
                        Name = "cancel",
                        Description = null,
                        Order = 8,
                        Hidden = true
                    });
        }
    }
}
