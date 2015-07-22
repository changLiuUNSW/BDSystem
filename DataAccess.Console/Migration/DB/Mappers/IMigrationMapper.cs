using DataAccess.Console.Models;

namespace DataAccess.Console.Migration.DB.Mappers
{
    internal interface IMigrationMapper
    {
        object Map(COMPTEMP row);
    }
}
