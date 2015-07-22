using System.Collections.ObjectModel;
using DataAccess.Console.Models;
using DataAccess.EntityFramework.Models.BD.Contact;
using DataAccess.EntityFramework.Models.BD.Lead;
using DataAccess.EntityFramework.Models.BD.Site;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal class ContactMapper : ConfigurableMapper, IMigrationMapper
    {
        public ContactMapper(MigrationConfiguration config) : base(config) {}

        /// <summary>
        /// map the row into a list of contacts
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public virtual object Map(COMPTEMP row)
        {
            return new Contact
            {
                NewManagerDate = row.NEWMANDATE,
                DaToCheck = row.DATA_UPDAT,
                DaToCheckInfo = row.INFO_UPDAT,
                ExtManagement = row.NEED_INFO,
                ReceptionName = row.RECP_NAME,
                Code = Configuration.MigrationCodeConverter.Convert(row.SALES_REP),
                Leads = new Collection<Lead>(),
                CallLines = new Collection<CallLine>()
            };
        }
    }
}
