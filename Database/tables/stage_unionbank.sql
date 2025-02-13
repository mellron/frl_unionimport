DROP TABLE IF EXISTS stage_unionbank;
GO
CREATE TABLE stage_unionbank (
    -- ID
    ID VARCHAR(500),
    -- Accrual Basis
    AccrualID VARCHAR(500),
    -- Accounting System
    AccountSystemID VARCHAR(500),
    -- All In Rate
    AllInRate VARCHAR(500),
    -- Amortization Term (Months)
    AmortizationTermMonths VARCHAR(500),
    -- Advance Type
    AmortizationTypeID VARCHAR(500),
    -- Closing Date
    DocumentDate VARCHAR(500),
    -- Cost Center
    CostCenter VARCHAR(500),
    -- Customer Name
    CustomerName VARCHAR(500),
    -- Division ID
    DivisionID VARCHAR(500),
    -- First Payment Date
    FirstPaymentDate VARCHAR(500),
    -- Fixed Term (Months)
    FixedTermMonths VARCHAR(500),
    -- Forward Charge (Admin)
    ForwardCharge VARCHAR(500),
    -- Forward Settlement (Months)
    ForwardSettlementMonths VARCHAR(500),
    -- Funding Amount
    FundingAmount VARCHAR(500),
    -- Interest Frequency ID
    InterestFrequencyID VARCHAR(500),
    -- Lease Residual (Either Lease Residual or Amortization must be used)
    LeaseResidual VARCHAR(500),
    -- Loan Term (Months)
    LoanTermMonths VARCHAR(500),
    -- Maturity Date
    MaturityDate VARCHAR(500),
    -- MMCOF (Admin)
    MMCOF VARCHAR(500),
    -- Notes
    Notes VARCHAR(500),
    -- Phone
    Phone VARCHAR(500),
    -- PP Risk Premium
    PPRiskPremium VARCHAR(500),
    -- PP Roll Over
    PPRollOver VARCHAR(500),
    -- Principal Frequency ID
    PrincipalFrequencyID VARCHAR(500),
    -- DSI Lease
    DSILease VARCHAR(500),
    -- Reprice Date
    RePriceDate VARCHAR(500),
    -- Requestor ID
    RequestorID VARCHAR(500),
    -- Approver Initials (Admin)
    ApproverInitials VARCHAR(500),
    -- Requestor
    Requestor VARCHAR(500),
    -- Spread (Either All In Rate or Spread must be used)
    Spread VARCHAR(500),
    -- Advance Type ID
    AdvanceTypeID VARCHAR(500),
    -- Currency Type
    CurrencyType VARCHAR(500),
    -- Liquidity ID (aka Term Issuance)
    LiquidityID VARCHAR(500),
    -- Confirmation Initials
    ConfirmationInitials VARCHAR(500),
    -- Indemnity Agreement
    IndemnityAgreement VARCHAR(500),
    -- Lender Name
    LenderName VARCHAR(500),
    -- Is Community Investment Program
    IsCommunityInvestmentProgram VARCHAR(500),
    -- Is PrePayment Waiver
    IsPrePaymentWaiver VARCHAR(500),
    -- PrePayment Type ID
    PrePaymentTypeID VARCHAR(500),
    -- Partial PrePayment ID
    PartialPrePaymentID VARCHAR(500),
    -- PrePayment Waiver Acknowledgement
    PrePaymentWaiverAcknowledgement VARCHAR(500),
    -- Requestor Email
    RequestorEmail VARCHAR(500),
    -- Lender ID
    LenderID VARCHAR(500),
    -- Is Commercial Mortgage Eligible
    IsCommercialMortgageEligible VARCHAR(500)
);
