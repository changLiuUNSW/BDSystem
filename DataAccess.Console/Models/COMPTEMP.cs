using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Console.Models
{
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

        public bool IN_HOUSE { get; set; }

        public DateTime? TENDER_NEX { get; set; }

        public DateTime? NEWMANDATE { get; set; }

        public bool MANBYAGENT { get; set; }

        [StringLength(49)]
        public string AGENT_COMP { get; set; }

        [StringLength(50)]
        public string AGENT_MANA { get; set; }

        [StringLength(12)]
        public string AGENTPHONE { get; set; }

        public DateTime? DATE_START { get; set; }

        public bool Q_LOST_JOB { get; set; }

        [StringLength(60)]
        public string WHYLOSTJOB { get; set; }

        public DateTime? DATE_LOST { get; set; }

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
        [Key]
        public string KEY { get; set; }

        [StringLength(3)]
        public string SALES_REP { get; set; }

        public bool NOT_DIRMAI { get; set; }

        public DateTime? DEADQUOTE { get; set; }

        public bool NO_QUOTE { get; set; }

        public DateTime? SEND_DATE { get; set; }

        public string QUOTE_CHK { get; set; }

        public bool STAY_CURR { get; set; }

        public bool LOSTJOBCHK { get; set; }

        public bool DEAD_Q_CHK { get; set; }

        public bool SALES_ATT { get; set; }

        public bool PRICE_COMP { get; set; }

        public bool CONSI_AGAI { get; set; }

        public DateTime? DQ_CHKDATE { get; set; }

        public bool V_INTEREST { get; set; }

        public bool GET_QUOTE { get; set; }

        public bool NOT_GETQUO { get; set; }

        public bool NO_QUO_INF { get; set; }

        public bool GIVEN_SR { get; set; }

        public DateTime? NO_TS_DM { get; set; }

        public double? EMP_IN_OFF { get; set; }

        
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

        
        public bool SE_LIST { get; set; }

        [StringLength(50)]
        public string SE_EMAIL { get; set; }

        [StringLength(15)]
        public string SE_MOBILE { get; set; }

        
        public bool SE_QUADJOB { get; set; }

        public DateTime? SE_TENDER { get; set; }

        
        public bool SE_CHKTEND { get; set; }

        public short? SE_CALLCYC { get; set; }

        
        public bool SE_PROSPEC { get; set; }

        
        public bool SE_NODIRMA { get; set; }

        public DateTime? SE_Q_DATE { get; set; }

        public double? SE_PR_PA { get; set; }

        
        public bool SE_CUR_QUO { get; set; }

        [StringLength(5)]
        public string SE_SAL_QUO { get; set; }

        public double? SE_QUO_NO { get; set; }

        [StringLength(20)]
        public string SE_Q_SOURC { get; set; }

        
        public bool SE_NODMAIL { get; set; }

        public DateTime? SEDEADQUOT { get; set; }

        [StringLength(50)]
        public string SEWHYLOST { get; set; }

        
        public bool SELOSTJOB { get; set; }

        public DateTime? SEDATELOST { get; set; }

        
        public bool GEN_ISS_RE { get; set; }

        
        public bool SE_GEN_ISS { get; set; }

        public string ISSRESNOTE { get; set; }

        public string SE_IR_NOTE { get; set; }

        public double? RETURN { get; set; }

        public double? SERETURNPW { get; set; }

        
        public bool SITE_SP_RE { get; set; }

        
        public bool SE_S_SP_RE { get; set; }

        public string BUILD_NOTE { get; set; }

        public string BUILD_ID { get; set; }

        
        public bool BUILD_MAN { get; set; }

        
        public bool SECU_CONT { get; set; }

        
        public bool CLEAN_CONT { get; set; }

        public string SE_CT_MEMO { get; set; }

        [StringLength(40)]
        public string CUR_SC { get; set; }

        [StringLength(40)]
        public string CUR_CLN { get; set; }

        
        public bool SC_GUAD { get; set; }

        
        public bool SC_MOB_PAT { get; set; }

        
        public bool SC_B2BMON { get; set; }

        
        public bool SC_CCTV { get; set; }

        
        public bool SC_MAINTEN { get; set; }

        
        public bool CL_GOLF { get; set; }

        
        public bool CL_FISH { get; set; }

        
        public bool CL_RUGBY { get; set; }

        
        public bool CL_LEAGUE { get; set; }

        
        public bool CL_SOCCER { get; set; }

        
        public bool CL_BFAST { get; set; }

        
        public bool CL_LUNCH { get; set; }

        [StringLength(40)]
        public string CL_OTHER { get; set; }

        [StringLength(10)]
        public string ASSO_KEY { get; set; }

        
        public bool TSTOCALL { get; set; }

        
        public bool MULTI_REC { get; set; }

        [StringLength(10)]
        public string ASSO_KEY_C { get; set; }

        
        public bool DIFF_CL_SE { get; set; }

        
        public bool MAINT_CONT { get; set; }

        [StringLength(3)]
        public string SE_SAL_REP { get; set; }

        
        public bool SETSTOCALL { get; set; }

        public DateTime? SENO_TS_DM { get; set; }

        [StringLength(20)]
        public string MD_CALL { get; set; }

        
        public bool HEAD_OFFIC { get; set; }

        [StringLength(20)]
        public string CONT_QUOTE { get; set; }

        [StringLength(10)]
        public string CONTRT_LEN { get; set; }

        
        public bool DA_Q_CHKED { get; set; }

        
        public bool DA_DQ_CHK { get; set; }

        
        public bool SEDA_Q_CHK { get; set; }

        public bool SEDA_DQCHK { get; set; }

        public DateTime? LOSTJOB_D { get; set; }

        
        public bool STAY_INCUM { get; set; }

        [StringLength(20)]
        public string UNSU_OPTIO { get; set; }

        public double? PR_WONAT { get; set; }

        [StringLength(7)]
        public string JOB_NO { get; set; }

        
        public bool DA_JUNTOCL { get; set; }

        
        public bool OVER_350K { get; set; }

        
        public bool OVER_750K { get; set; }

        [StringLength(20)]
        public string SITE_QUALI { get; set; }

        [StringLength(20)]
        public string CHG_BDTS { get; set; }

        [StringLength(8)]
        public string HO_PM_KEY { get; set; }

        
        public bool CONCIERGE { get; set; }

        [StringLength(30)]
        public string GM_NAME { get; set; }

        [StringLength(3)]
        public string ASK_MAINT { get; set; }

        
        public bool PM_SITE { get; set; }

        
        public bool SE_BD_PROP { get; set; }

        
        public bool SE_NOTSUI { get; set; }

        
        public bool RECP_NAME { get; set; }

        public DateTime? UPDATEDATE { get; set; }

        [StringLength(8)]
        public string UPDATETIME { get; set; }

        [StringLength(3)]
        public string SETS_PERSO { get; set; }

        public DateTime? SE_UP_DATE { get; set; }

        [StringLength(8)]
        public string SE_UP_TIME { get; set; }

        
        public bool SE_RECP_NA { get; set; }

        
        public bool SE_RECP_NP { get; set; }

        [StringLength(20)]
        public string SE_UNSU_OP { get; set; }

        
        public bool SE_Q_GUARD { get; set; }

        
        public bool SE_Q_CONCI { get; set; }

        
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

        
        public bool MD_PROSPEC { get; set; }

        public short? MD_CALLCYC { get; set; }

        public DateTime? MDLASTCALL { get; set; }

        public DateTime? MDNEXTCALL { get; set; }

        
        public bool SE_STYINCU { get; set; }

        
        public bool CL_LK_GOOD { get; set; }

        
        public bool SE_LK_GOOD { get; set; }

        
        public bool HP_W_INCUM { get; set; }

        
        public bool QUONEW_MAN { get; set; }

        public DateTime? QUO_CONTAC { get; set; }

        public string QUO_COMM { get; set; }

        [StringLength(80)]
        public string CATEGORY { get; set; }

        
        public bool HAP_INCUM { get; set; }

        
        public bool QUO_NEW_MG { get; set; }

        public DateTime? QUO_CONT_D { get; set; }

        public string QUO_MEMO { get; set; }

        [StringLength(7)]
        public string SE_JOB_NO { get; set; }

        
        public bool SE_HAP_INC { get; set; }

        
        public bool SE_QUO_MG { get; set; }

        public DateTime? SE_QUOCONT { get; set; }

        public string SE_QUOMEMO { get; set; }

        
        public bool NEW_JOB_PD { get; set; }

        
        public bool SE_NEW_JOB { get; set; }

        [StringLength(20)]
        public string SE_LJ_CONT { get; set; }

        
        public bool BD_PROSPEC { get; set; }

        public double? SE_PR_WON { get; set; }

        public DateTime? SE_S_DATE { get; set; }

        [StringLength(100)]
        public string WEB_ADDRES { get; set; }

        [StringLength(3)]
        public string TMP_SALREP { get; set; }

        public DateTime? DQ_CHK { get; set; }

        
        public bool DA_CHK_QC { get; set; }

        [StringLength(15)]
        public string F_NAME_BK { get; set; }

        [StringLength(22)]
        public string L_NAME_BK { get; set; }

        
        public bool CK_NAMECHG { get; set; }

        
        public bool CL_MORELOC { get; set; }

        
        public bool SE_MORELOC { get; set; }

        public string M_TENANT1 { get; set; }

        public string M_TENANT2 { get; set; }

        public short? QUOTE_CPW { get; set; }

        
        public bool SAME_CONTA { get; set; }

        [StringLength(15)]
        public string DB_SOURCE { get; set; }

        
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

        
        public bool SEND_EMAIL { get; set; }

        
        public bool NEED_INFO { get; set; }

        
        public bool SE_CAS_GUA { get; set; }

        
        public bool SE_GM_DNQ { get; set; }

        public DateTime? TSSKPCYC { get; set; }

        
        public bool NO_EMAIL { get; set; }

        [StringLength(50)]
        public string ASSO_COMP { get; set; }

        [StringLength(30)]
        public string SUB_STATE { get; set; }

        
        public bool SAME_CONT { get; set; }

        public double? QUALI_NO { get; set; }

        public string RELATIONSH { get; set; }

        
        public bool VERI_EMAIL { get; set; }

        [StringLength(25)]
        public string REGION { get; set; }

        
        public bool SE_NOEMAIL { get; set; }
    }
}
