SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.FRLI_FixedRateLock', 'U') IS NOT NULL 
    DROP TABLE [dbo].[FRLI_FixedRateLock];
GO 

CREATE TABLE [dbo].[FRLI_FixedRateLock](
    [ReferenceNumber] [int] IDENTITY(1,1) NOT NULL,
    [HexID] [varchar](10) NULL,
    [CustomerName] [varchar](60) NOT NULL,
    [OfficerCode] [varchar](3) NULL,
    [AccrualID] [int] NOT NULL,
    [AcctSystemID] [int] NOT NULL,
    [AmortTypeID] [int] NOT NULL,
    [DivisionID] [int] NOT NULL,
    [InterestFreqID] [int] NOT NULL,
    [PrincipalFreqID] [int] NOT NULL,
    [PrePaymentTypeID] [int] NULL,
    [MMCOF] [decimal](28, 2) NULL,
    [FundingAmt] [money] NOT NULL,
    [MaturityDate] [datetime] NOT NULL,
    [LoanTerm] [smallint] NULL,
    [PmtDayofMonth] [tinyint] NULL,
    [FirstPmtDt] [datetime] NOT NULL,
    [CostCenter] [char](11) NOT NULL,
    [Notes] [varchar](2048) NULL,
    [PrePayPenalty] [bit] NULL,
    [Originator] [varchar](35) NOT NULL,
    [Approver] [varchar](35) NULL,
    [LockInDt] [datetime] NOT NULL,
    [AmortTerm] [smallint] NULL,
    [FixedTerm] [smallint] NOT NULL,
    [AllInRate] [decimal](28, 2) NULL,
    [ForwardCharge] [money] NULL,
    [PPRiskPremium] [money] NULL,
    [PPRollOver] [money] NULL,
    [ClosingDt] [datetime] NOT NULL,
    [RePriceDt] [datetime] NULL,
    [ProcessingCode] [varchar](6) NULL,
    [PartialPrePayID] [tinyint] NULL,
    [LeaseResidual] [decimal](5, 2) NULL,
    [Spread] [decimal](5, 2) NULL,
    [Phone] [varchar](12) NOT NULL,
    [RequestorID] [varchar](7) NOT NULL,
    [StatusID] [char](1) NOT NULL,
    [ForwardSettlement] [smallint] NULL,
    [RecordLockDt] [datetime] NULL,
    [DSILease] [char](13) NULL,
    [AdvanceTypeID] [smallint] NULL,
    [CurrencyType] [smallint] NOT NULL,
    [ReferenceAlpha] [varchar](10) NOT NULL,
    [LiquidityID] [smallint] NOT NULL,
    [ConfirmationInitials] [varchar](3) NOT NULL,
    [IndemnityAgreement] [smallint] NULL,
    [LastUpdateDT] [datetime] NOT NULL,
    [PrePaymentWaiverAcknowledgement] [bit] NOT NULL,
    [ForwardSettlementChargeAcknowledgement] [bit] NOT NULL,
    [ForwardSettlementCreditAcknowledgement] [bit] NOT NULL,
    [ForwardSettlementPeriodAcknowledgement] [bit] NOT NULL,
    [LenderName] [varchar](50) NOT NULL,
    [CIP] [bit] NOT NULL,
    [PrePaymentWaiver] [bit] NOT NULL,
    [RequestorEmail] [varchar](50) NULL,
    [LenderID] [varchar](7) NULL,
    [IsCommercialMortgageEligible] [bit] NOT NULL,
 CONSTRAINT [PK_FixedRateLock] PRIMARY KEY CLUSTERED 
(
    [ReferenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) 
) 
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IDX_FixedRateLock_LockInDtRequestorID] ON [dbo].[FRLI_FixedRateLock]
(
    [LockInDt] ASC,
    [RequestorID] ASC,
    [CustomerName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) 
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IDX_FixedRateLock_StatusIDRecordLockDt] ON [dbo].[FRLI_FixedRateLock]
(
    [StatusID] ASC,
    [RecordLockDt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) 
GO
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IXN_FRLI_FixedRateLock_ReferenceAlpha] ON [dbo].[FRLI_FixedRateLock]
(
    [ReferenceAlpha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] ADD  CONSTRAINT [DF_FixedRateLock_LockInDt]  DEFAULT (getdate()) FOR [LockInDt]
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] ADD  CONSTRAINT [DF_FixedRateLock_StatusID]  DEFAULT ('P') FOR [StatusID]
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] ADD  CONSTRAINT [DF_FixedRateLock_CurrencyType]  DEFAULT (1) FOR [CurrencyType]
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] ADD  CONSTRAINT [DF_FixedRateLock_LiquidityID]  DEFAULT (0) FOR [LiquidityID]
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] ADD  CONSTRAINT [DF_FixedRateLock_Indemnity]  DEFAULT (0) FOR [IndemnityAgreement]
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] ADD  CONSTRAINT [DF_FixedRateLock_IsCommercialMortgageEligible]  DEFAULT ((0)) FOR [IsCommercialMortgageEligible]
GO
 

 
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock]  WITH CHECK ADD  CONSTRAINT [CHK_FixedRateLock_RecordLockDt] CHECK  (([RecordLockDt] is null or [RecordLockDt] <= getdate()))
GO
ALTER TABLE [dbo].[FRLI_FixedRateLock] CHECK CONSTRAINT [CHK_FixedRateLock_RecordLockDt]
GO
