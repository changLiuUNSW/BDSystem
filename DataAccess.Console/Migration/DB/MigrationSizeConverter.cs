namespace DataAccess.Console.Migration.DB
{
    internal sealed class MigrationSizeConverter : StringConverter
    {
        [StringConvertion(Target = "008")]
        internal enum Size008
        {
            OPM,
            OPS,
            QT
        }

        [StringConvertion(Target = "025")]
        internal enum Size025
        {
            DBO,
            FRO,
            ISO,
            JPO,
            OOA,
            OP,
            QA,
            OP1
        }

        [StringConvertion(Target = "050")]
        internal enum Size050
        {
            DBT,
            EST,
            FRT,
            IST,
            JPT,
            LCT,
            MCT,
            OAT,
            PAT,
            TBT
        }

        [StringConvertion(Target = "120")]
        internal enum Size120
        {
            DBL,
            ESL,
            FRL,
            ISL,
            JPL,
            OAL,
            PAL,
            TBL
        }

        public MigrationSizeConverter()
        {
            Add<Size008>();
            Add<Size025>();
            Add<Size050>();
            Add<Size120>();
        }
    }
}
