namespace DataAccess.Console.Migration.DB.Mappers
{
    internal abstract class ConfigurableMapper
    {
        public MigrationConfiguration Configuration { get; set; }

        protected ConfigurableMapper(MigrationConfiguration config)
        {
            Configuration = config;
        }
    }
}
