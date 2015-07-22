namespace DataAccess.Console.Migration.DB
{
    internal sealed class MigrationCodeConverter : StringConverter
    {
        [StringConvertion(Target = "BLDMAN")]
        internal enum Bldman
        {
            BMS
        }

        /// <summary>
        /// list of codes will be mapped to GOVMNT
        /// </summary>
        [StringConvertion(Target = "GOVMNT")]
        internal enum Govmnt
        {
            GON,
            GOV
        }

        /// <summary>
        /// list of codes will be mapped to TSCALL
        /// </summary>
        [StringConvertion(Target = "TSCALL")]
        internal enum TsCall
        {
            DBL,
            DBO,
            DBT,
            ESL,
            EST,
            FRL,
            FRO,
            FRT,
            ISL,
            ISO,
            IST,
            JPL,
            JPO,
            JPT,
            LCT,
            MCT,
            OAL,
            OAT,
            OOA,
            OP,
            OP1,
            OPM,
            OPS,
            PAL,
            PAT,
            QA,
            QT,
            TBL,
            TBT,
        }

        [StringConvertion(Target = "TSGRP")]
        internal enum Tsgrp
        {
            GNC
        }

        [StringConvertion(Target = "TSGRP1")]
        internal enum Tsgrp1
        {
            G1
        }

        [StringConvertion(Target = "TSGRP2")]
        internal enum Tsgrp2
        {
            G,
            G2
        }

        [StringConvertion(Target = "TSGRP3")]
        internal enum Tsgrp3
        {
            G3
        }

        [StringConvertion(Target = "TSPMAN")]
        internal enum Tspman
        {
            LPM
        }

        [StringConvertion(Target = "TSPMAS")]
        internal enum Tspmas
        {
            PMS
        }

        [StringConvertion(Target = "TSREAL")]
        internal enum Tsreal
        {
            REA
        }

        [StringConvertion(Target = "DBARGP")]
        internal enum Dbargp
        {
            DBP
        }

        [StringConvertion(Target = "DBARSB")]
        internal enum Dbarsb
        {
            DB            
        }

        [StringConvertion(Target = "DHUDPM")]
        internal enum Dhudpm
        {
            DHP
        }

        [StringConvertion(Target = "DHUDSB")]
        internal enum Dhudsb
        {
            dh
        }

        [StringConvertion(Target = "FRENPM")]
        internal enum Frenpm
        {
            FRP
        }
        
        [StringConvertion(Target = "FRENSB")]
        internal enum Frensb
        {
            FR
        }

        [StringConvertion(Target = "ISAGPM")]
        internal enum Isagpm
        {
            ISP        
        }

        [StringConvertion(Target = "ISAGSB")]
        internal enum Isagsb
        {
            IS
        }

        [StringConvertion(Target = "JPOWPM")]
        internal enum Jpowpm
        {
            JPP
        }

        [StringConvertion(Target = "JPOWSB")]
        internal enum Jpowsb
        {
            JP
        }

        [StringConvertion(Target = "COMPET")]
        internal enum Compet
        {
            COM
        }

        [StringConvertion(Target = "DELETE")]
        internal enum Delete
        {
            D
        }

        [StringConvertion(Target = "QUAJOB")]
        internal enum Quajob
        {
            Q
        }

        [StringConvertion(Target = "TENANT")]
        internal enum Tenant
        {
            T
        }

        [StringConvertion(Target = "UND08K")]
        internal enum Und08k
        {
            NR
        }

        [StringConvertion(Target = "VACANT")]
        internal enum Vacant
        {
            VAC
        }

        public MigrationCodeConverter()
        {
            Add<Bldman>();
            Add<Govmnt>();
            Add<TsCall>();
            Add<Tsgrp>();
            Add<Tsgrp1>();
            Add<Tsgrp2>();
            Add<Tsgrp3>();
            Add<Tspman>();
            Add<Tspmas>();
            Add<Tsreal>();
            Add<Dbargp>();
            Add<Dbarsb>();
            Add<Dhudpm>();
            Add<Dhudsb>();
            Add<Frenpm>();
            Add<Frensb>();
            Add<Isagpm>();
            Add<Isagsb>();
            Add<Jpowpm>();
            Add<Jpowsb>();
            Add<Compet>();
            Add<Delete>();
            Add<Quajob>();
            Add<Tenant>();
            Add<Und08k>();
            Add<Vacant>();
        }
    }
}
