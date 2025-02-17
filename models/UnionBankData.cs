namespace frl_unionimport.models
{
    public class UnionBankData
    {
        public string ReferenceAlpha { get; set; }
        public int? AccrualID { get; set; }
        public int? AcctSystemID { get; set; }
        public decimal? AllInRate { get; set; }
        public int? AmortTerm { get; set; }
        public int? AmortTypeID { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string CostCenter { get; set; }
        public string CustomerName { get; set; }
        public int? DivisionID { get; set; }
        public DateTime? FirstPmtDt { get; set; }
        public int? FixedTerm { get; set; }
        public string ForwardCharge { get; set; }
        public int? ForwardSettlement { get; set; }
        public decimal? FundingAmt { get; set; }
        public int? InterestFreqID { get; set; }
        public decimal? LeaseResidual { get; set; }
        public int? LoanTerm { get; set; }
        public string MMCOF { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public decimal? PPRiskPremium { get; set; }
        public decimal? PPRollOver { get; set; }
        public int? PrincipalFreqID { get; set; }
        public string DSILease { get; set; }
        public DateTime? RePriceDt { get; set; }
        public string RequestorID { get; set; }
        public string Approver { get; set; }
        public string Originator { get; set; }
        public decimal? Spread { get; set; }
        public int? AdvanceTypeID { get; set; }
        public int? CurrencyType { get; set; }
        public int? LiquidityID { get; set; }
        public string ConfirmationInitials { get; set; }
        public int? IndemnityAgreement { get; set; }
        public string LenderName { get; set; }
        public bool CIP { get; set; } = false;
        public bool PrePaymentWaiver { get; set; } = false;
        public int? PrePaymentTypeID { get; set; }
        public int? PartialPrePayID { get; set; }
        public bool PrePaymentWaiverAcknowledgement { get; set; } = false;
        public string RequestorEmail { get; set; }
        public string LenderID { get; set; }
        public bool IsCommercialMortgageEligible { get; set; } = false;
    }
}
