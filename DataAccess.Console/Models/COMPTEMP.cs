namespace DataAccess.Console.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMPTEMP")]
    public partial class COMPTEMP
    {
        [StringLength(65)]
        public string COMPANY { get; set; }

        [StringLength(20)]
        public string UNIT { get; set; }

        [StringLength(10)]
        public string NUMBER { get; set; }

        [StringLength(50)]
        public string STREET { get; set; }

        [StringLength(25)]
        public string SUBURB { get; set; }

        [StringLength(25)]
        public string STATE { get; set; }

        [StringLength(11)]
        public string P_CODE { get; set; }

        [StringLength(15)]
        public string TITLE { get; set; }

        [StringLength(15)]
        public string FIRST_NAME { get; set; }

        [StringLength(22)]
        public string LAST_NAME { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(70)]
        public string POSITION { get; set; }

        [StringLength(15)]
        public string PHONE { get; set; }

        public double? QUOTE_NO { get; set; }

        public DateTime? DATEQUOTED { get; set; }

        [StringLength(15)]
        public string SOURCEQUOT { get; set; }

        public double? PRICE_PA { get; set; }

        [Key]
        [Column(Order = 0)]
        public bool QUOTE_CURR { get; set; }

        [StringLength(10)]
        public string SALWHOQUOT { get; set; }

        [StringLength(80)]
        public string WHYQU_UNSU { get; set; }

        public double? CLEAN_FREQ { get; set; }

        [StringLength(4)]
        public string SALES_PREF { get; set; }

        [StringLength(4)]
        public string SALES_BOX { get; set; }

        public DateTime? LASTCONTAC { get; set; }

        [StringLength(3)]
        public string TELE_PERSO { get; set; }

        public DateTime? NEXT_CALL { get; set; }

        public DateTime? LAST_PROFI { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool IN_HOUSE { get; set; }

        public DateTime? TENDER_NEX { get; set; }

        public DateTime? NEWMANDATE { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool MANBYAGENT { get; set; }

        [StringLength(49)]
        public string AGENT_COMP { get; set; }

        [StringLength(50)]
        public string AGENT_MANA { get; set; }

        [StringLength(12)]
        public string AGENTPHONE { get; set; }

        public DateTime? DATE_START { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool Q_LOST_JOB { get; set; }

        [StringLength(60)]
        public string WHYLOSTJOB { get; set; }

        public DateTime? DATE_LOST { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool DATA_UPDAT { get; set; }

        [StringLength(100)]
        public string INFO_UPDAT { get; set; }

        [StringLength(80)]
        public string POBOX { get; set; }

        [StringLength(16)]
        public string FAX_CLEAN { get; set; }

        public string MAN_MEMO { get; set; }

        [StringLength(2)]
        public string BUILD_TYPE { get; set; }

        [StringLength(60)]
        public string LOC_QUOTE { get; set; }

        [StringLength(8)]
        public string KEY { get; set; }

        [StringLength(3)]
        public string SALES_REP { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool NOT_DIRMAI { get; set; }

        public DateTime? DEADQUOTE { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool NO_QUOTE { get; set; }

        public DateTime? SEND_DATE { get; set; }

        public string QUOTE_CHK { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool STAY_CURR { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool LOSTJOBCHK { get; set; }

        [Key]
        [Column(Order = 9)]
        public bool DEAD_Q_CHK { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool SALES_ATT { get; set; }

        [Key]
        [Column(Order = 11)]
        public bool PRICE_COMP { get; set; }

        [Key]
        [Column(Order = 12)]
        public bool CONSI_AGAI { get; set; }

        public DateTime? DQ_CHKDATE { get; set; }

        [Key]
        [Column(Order = 13)]
        public bool V_INTEREST { get; set; }

        [Key]
        [Column(Order = 14)]
        public bool GET_QUOTE { get; set; }

        [Key]
        [Column(Order = 15)]
        public bool NOT_GETQUO { get; set; }

        [Key]
        [Column(Order = 16)]
        public bool NO_QUO_INF { get; set; }

        [Key]
        [Column(Order = 17)]
        public bool GIVEN_SR { get; set; }

        public DateTime? NO_TS_DM { get; set; }

        public double? EMP_IN_OFF { get; set; }

        [Key]
        [Column(Order = 18)]
        public bool TOO_SMALL { get; set; }

        public short? CALL_ON_PM { get; set; }

        public string NOT_CLNOTE { get; set; }

        [StringLength(45)]
        public string POB_SUBURB { get; set; }

        [StringLength(25)]
        public string POB_STATE { get; set; }

        [StringLength(4)]
        public string POB_PCODE { get; set; }

        public string TS_NOTE { get; set; }

        public DateTime? ADD_DATE { get; set; }

        [StringLength(20)]
        public string ADD_TIME { get; set; }

        [StringLength(12)]
        public string DIR_LINE { get; set; }

        [StringLength(16)]
        public string MOBILE { get; set; }

        public string NOTE_PAD { get; set; }

        [Key]
        [Column(Order = 19)]
        public bool PROSPECT { get; set; }

        [StringLength(35)]
        public string GRP_NAME { get; set; }

        [StringLength(15)]
        public string SE_TITLE { get; set; }

        [StringLength(15)]
        public string SE_F_NAME { get; set; }

        [StringLength(22)]
        public string SE_L_NAME { get; set; }

        [StringLength(40)]
        public string SE_POST { get; set; }

        [StringLength(15)]
        public string SE_DIRLINE { get; set; }

        public string SE_MEMO { get; set; }

        public DateTime? SELASTCALL { get; set; }

        public DateTime? SENEXTCALL { get; set; }

        [Key]
        [Column(Order = 20)]
        public bool SE_LIST { get; set; }

        [StringLength(50)]
        public string SE_EMAIL { get; set; }

        [StringLength(15)]
        public string SE_MOBILE { get; set; }

        [Key]
        [Column(Order = 21)]
        public bool SE_QUADJOB { get; set; }

        public DateTime? SE_TENDER { get; set; }

        [Key]
        [Column(Order = 22)]
        public bool SE_CHKTEND { get; set; }

        public short? SE_CALLCYC { get; set; }

        [Key]
        [Column(Order = 23)]
        public bool SE_PROSPEC { get; set; }

        [Key]
        [Column(Order = 24)]
        public bool SE_NODIRMA { get; set; }

        public DateTime? SE_Q_DATE { get; set; }

        public double? SE_PR_PA { get; set; }

        [Key]
        [Column(Order = 25)]
        public bool SE_CUR_QUO { get; set; }

        [StringLength(5)]
        public string SE_SAL_QUO { get; set; }

        public double? SE_QUO_NO { get; set; }

        [StringLength(20)]
        public string SE_Q_SOURC { get; set; }

        [Key]
        [Column(Order = 26)]
        public bool SE_NODMAIL { get; set; }

        public DateTime? SEDEADQUOT { get; set; }

        [StringLength(50)]
        public string SEWHYLOST { get; set; }

        [Key]
        [Column(Order = 27)]
        public bool SELOSTJOB { get; set; }

        public DateTime? SEDATELOST { get; set; }

        [Key]
        [Column(Order = 28)]
        public bool GEN_ISS_RE { get; set; }

        [Key]
        [Column(Order = 29)]
        public bool SE_GEN_ISS { get; set; }

        public string ISSRESNOTE { get; set; }

        public string SE_IR_NOTE { get; set; }

        public double? RETURN { get; set; }

        public double? SERETURNPW { get; set; }

        [Key]
        [Column(Order = 30)]
        public bool SITE_SP_RE { get; set; }

        [Key]
        [Column(Order = 31)]
        public bool SE_S_SP_RE { get; set; }

        public string BUILD_NOTE { get; set; }

        [StringLength(50)]
        public string BUILD_ID { get; set; }

        [Key]
        [Column(Order = 32)]
        public bool BUILD_MAN { get; set; }

        [Key]
        [Column(Order = 33)]
        public bool SECU_CONT { get; set; }

        [Key]
        [Column(Order = 34)]
        public bool CLEAN_CONT { get; set; }

        public string SE_CT_MEMO { get; set; }

        [StringLength(40)]
        public string CUR_SC { get; set; }

        [StringLength(40)]
        public string CUR_CLN { get; set; }

        [Key]
        [Column(Order = 35)]
        public bool SC_GUAD { get; set; }

        [Key]
        [Column(Order = 36)]
        public bool SC_MOB_PAT { get; set; }

        [Key]
        [Column(Order = 37)]
        public bool SC_B2BMON { get; set; }

        [Key]
        [Column(Order = 38)]
        public bool SC_CCTV { get; set; }

        [Key]
        [Column(Order = 39)]
        public bool SC_MAINTEN { get; set; }

        [Key]
        [Column(Order = 40)]
        public bool CL_GOLF { get; set; }

        [Key]
        [Column(Order = 41)]
        public bool CL_FISH { get; set; }

        [Key]
        [Column(Order = 42)]
        public bool CL_RUGBY { get; set; }

        [Key]
        [Column(Order = 43)]
        public bool CL_LEAGUE { get; set; }

        [Key]
        [Column(Order = 44)]
        public bool CL_SOCCER { get; set; }

        [Key]
        [Column(Order = 45)]
        public bool CL_BFAST { get; set; }

        [Key]
        [Column(Order = 46)]
        public bool CL_LUNCH { get; set; }

        [StringLength(40)]
        public string CL_OTHER { get; set; }

        [StringLength(10)]
        public string ASSO_KEY { get; set; }

        [Key]
        [Column(Order = 47)]
        public bool TSTOCALL { get; set; }

        [Key]
        [Column(Order = 48)]
        public bool MULTI_REC { get; set; }

        [StringLength(10)]
        public string ASSO_KEY_C { get; set; }

        [Key]
        [Column(Order = 49)]
        public bool DIFF_CL_SE { get; set; }

        [Key]
        [Column(Order = 50)]
        public bool MAINT_CONT { get; set; }

        [StringLength(3)]
        public string SE_SAL_REP { get; set; }

        [Key]
        [Column(Order = 51)]
        public bool SETSTOCALL { get; set; }

        public DateTime? SENO_TS_DM { get; set; }

        [StringLength(20)]
        public string MD_CALL { get; set; }

        [Key]
        [Column(Order = 52)]
        public bool HEAD_OFFIC { get; set; }

        [StringLength(20)]
        public string CONT_QUOTE { get; set; }

        [StringLength(10)]
        public string CONTRT_LEN { get; set; }

        [Key]
        [Column(Order = 53)]
        public bool DA_Q_CHKED { get; set; }

        [Key]
        [Column(Order = 54)]
        public bool DA_DQ_CHK { get; set; }

        [Key]
        [Column(Order = 55)]
        public bool SEDA_Q_CHK { get; set; }

        [Key]
        [Column(Order = 56)]
        public bool SEDA_DQCHK { get; set; }

        public DateTime? LOSTJOB_D { get; set; }

        [Key]
        [Column(Order = 57)]
        public bool STAY_INCUM { get; set; }

        [StringLength(20)]
        public string UNSU_OPTIO { get; set; }

        public double? PR_WONAT { get; set; }

        [StringLength(7)]
        public string JOB_NO { get; set; }

        [Key]
        [Column(Order = 58)]
        public bool DA_JUNTOCL { get; set; }

        [Key]
        [Column(Order = 59)]
        public bool OVER_350K { get; set; }

        [Key]
        [Column(Order = 60)]
        public bool OVER_750K { get; set; }

        [StringLength(20)]
        public string SITE_QUALI { get; set; }

        [StringLength(20)]
        public string CHG_BDTS { get; set; }

        [StringLength(8)]
        public string HO_PM_KEY { get; set; }

        [Key]
        [Column(Order = 61)]
        public bool CONCIERGE { get; set; }

        [StringLength(30)]
        public string GM_NAME { get; set; }

        [StringLength(3)]
        public string ASK_MAINT { get; set; }

        [Key]
        [Column(Order = 62)]
        public bool PM_SITE { get; set; }

        [Key]
        [Column(Order = 63)]
        public bool SE_BD_PROP { get; set; }

        [Key]
        [Column(Order = 64)]
        public bool SE_NOTSUI { get; set; }

        [Key]
        [Column(Order = 65)]
        public bool RECP_NAME { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        [StringLength(8)]
        public string UPDATETIME { get; set; }

        [StringLength(3)]
        public string SETS_PERSO { get; set; }

        public DateTime? SE_UP_DATE { get; set; }

        [StringLength(8)]
        public string SE_UP_TIME { get; set; }

        [Key]
        [Column(Order = 66)]
        public bool SE_RECP_NA { get; set; }

        [Key]
        [Column(Order = 67)]
        public bool SE_RECP_NP { get; set; }

        [StringLength(20)]
        public string SE_UNSU_OP { get; set; }

        [Key]
        [Column(Order = 68)]
        public bool SE_Q_GUARD { get; set; }

        [Key]
        [Column(Order = 69)]
        public bool SE_Q_CONCI { get; set; }

        [Key]
        [Column(Order = 70)]
        public bool SE_Q_PATRO { get; set; }

        public double? SE_RATE_PH { get; set; }

        [StringLength(20)]
        public string LJ_CONT { get; set; }

        public DateTime? Q_COMPLETE { get; set; }

        public DateTime? SE_Q_COMPL { get; set; }

        [StringLength(40)]
        public string SE_WHYUNSU { get; set; }

        public string TSLEADMEMO { get; set; }

        [StringLength(3)]
        public string MD_SAL_REP { get; set; }

        [Key]
        [Column(Order = 71)]
        public bool MD_PROSPEC { get; set; }

        public short? MD_CALLCYC { get; set; }

        public DateTime? MDLASTCALL { get; set; }

        public DateTime? MDNEXTCALL { get; set; }

        [Key]
        [Column(Order = 72)]
        public bool SE_STYINCU { get; set; }

        [Key]
        [Column(Order = 73)]
        public bool CL_LK_GOOD { get; set; }

        [Key]
        [Column(Order = 74)]
        public bool SE_LK_GOOD { get; set; }

        [Key]
        [Column(Order = 75)]
        public bool HP_W_INCUM { get; set; }

        [Key]
        [Column(Order = 76)]
        public bool QUONEW_MAN { get; set; }

        public DateTime? QUO_CONTAC { get; set; }

        public string QUO_COMM { get; set; }

        [StringLength(80)]
        public string CATEGORY { get; set; }

        [Key]
        [Column(Order = 77)]
        public bool HAP_INCUM { get; set; }

        [Key]
        [Column(Order = 78)]
        public bool QUO_NEW_MG { get; set; }

        public DateTime? QUO_CONT_D { get; set; }

        public string QUO_MEMO { get; set; }

        [StringLength(7)]
        public string SE_JOB_NO { get; set; }

        [Key]
        [Column(Order = 79)]
        public bool SE_HAP_INC { get; set; }

        [Key]
        [Column(Order = 80)]
        public bool SE_QUO_MG { get; set; }

        public DateTime? SE_QUOCONT { get; set; }

        public string SE_QUOMEMO { get; set; }

        [Key]
        [Column(Order = 81)]
        public bool NEW_JOB_PD { get; set; }

        [Key]
        [Column(Order = 82)]
        public bool SE_NEW_JOB { get; set; }

        [StringLength(20)]
        public string SE_LJ_CONT { get; set; }

        [Key]
        [Column(Order = 83)]
        public bool BD_PROSPEC { get; set; }

        public double? SE_PR_WON { get; set; }

        public DateTime? SE_S_DATE { get; set; }

        [StringLength(100)]
        public string WEB_ADDRES { get; set; }

        [StringLength(3)]
        public string TMP_SALREP { get; set; }

        public DateTime? DQ_CHK { get; set; }

        [Key]
        [Column(Order = 84)]
        public bool DA_CHK_QC { get; set; }

        [StringLength(15)]
        public string F_NAME_BK { get; set; }

        [StringLength(22)]
        public string L_NAME_BK { get; set; }

        [Key]
        [Column(Order = 85)]
        public bool CK_NAMECHG { get; set; }

        [Key]
        [Column(Order = 86)]
        public bool CL_MORELOC { get; set; }

        [Key]
        [Column(Order = 87)]
        public bool SE_MORELOC { get; set; }

        public string M_TENANT1 { get; set; }

        public string M_TENANT2 { get; set; }

        public short? QUOTE_CPW { get; set; }

        [Key]
        [Column(Order = 88)]
        public bool SAME_CONTA { get; set; }

        [StringLength(15)]
        public string DB_SOURCE { get; set; }

        [Key]
        [Column(Order = 89)]
        public bool QUALIFIED { get; set; }

        public string DQ_CALL { get; set; }

        [StringLength(4)]
        public string OP { get; set; }

        [StringLength(4)]
        public string OPS { get; set; }

        [StringLength(4)]
        public string BDT { get; set; }

        public DateTime? TS_EMAIL { get; set; }

        public DateTime? CL_PROSP_D { get; set; }

        public DateTime? SE_PROSP_D { get; set; }

        public DateTime? CL_NO_DM_D { get; set; }

        public DateTime? SE_NO_DM_D { get; set; }

        public DateTime? PRVLSTCALL { get; set; }

        [Key]
        [Column(Order = 90)]
        public bool SEND_EMAIL { get; set; }

        [Key]
        [Column(Order = 91)]
        public bool NEED_INFO { get; set; }

        [Key]
        [Column(Order = 92)]
        public bool SE_CAS_GUA { get; set; }

        [Key]
        [Column(Order = 93)]
        public bool SE_GM_DNQ { get; set; }

        public DateTime? TSSKPCYC { get; set; }

        [Key]
        [Column(Order = 94)]
        public bool NO_EMAIL { get; set; }

        [StringLength(50)]
        public string ASSO_COMP { get; set; }

        [StringLength(30)]
        public string SUB_STATE { get; set; }

        [Key]
        [Column(Order = 95)]
        public bool SAME_CONT { get; set; }

        public double? QUALI_NO { get; set; }

        public string RELATIONSH { get; set; }

        [Key]
        [Column(Order = 96)]
        public bool VERI_EMAIL { get; set; }

        [StringLength(25)]
        public string REGION { get; set; }

        [Key]
        [Column(Order = 97)]
        public bool SE_NOEMAIL { get; set; }
    }
}
