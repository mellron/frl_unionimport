SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[FixedRateLock_INS] 
(
    @ReferenceAlpha VARCHAR(10),  
    @AccrualID INT,
    @AcctSystemID INT,
    @AdvanceTypeID SMALLINT = NULL,
    @AllInRate DECIMAL(28,2) = NULL,
    @AmortTerm SMALLINT = NULL,
    @AmortTypeID INT,
    @Approver VARCHAR(35) = NULL,
    @CIP BIT,
    @ClosingDt DATETIME,
    @ConfirmationInitials VARCHAR(3),
    @CostCenter CHAR(11),
    @CurrencyType SMALLINT,
    @CustomerName VARCHAR(60),
    @DSILease CHAR(13) = NULL,
    @FirstPmtDt DATETIME,
    @FixedTerm SMALLINT,
    @ForwardCharge MONEY = NULL,
    @ForwardSettlement SMALLINT = NULL,
    @FundingAmt MONEY,
    @IndemnityAgreement SMALLINT = NULL,
    @InterestFreqID INT,
    @IsCommercialMortgageEligible BIT,
    @LastUpdateDT DATETIME = NULL,
    @LeaseResidual DECIMAL(5,2) = NULL,
    @LenderID VARCHAR(7) = NULL,
    @LenderName VARCHAR(50),
    @LiquidityID SMALLINT,
    @LoanTerm SMALLINT = NULL,
    @MaturityDate DATETIME,
    @MMCOF DECIMAL(28,2) = NULL,
    @Notes VARCHAR(2048) = NULL,
    @Originator VARCHAR(35),
    @PartialPrePayID TINYINT = NULL,
    @Phone VARCHAR(12),
    @PPRiskPremium MONEY = NULL,
    @PPRollOver MONEY = NULL,
    @PrePaymentTypeID INT = NULL,
    @PrePaymentWaiver BIT,
    @PrePaymentWaiverAcknowledgement BIT,
    @PrincipalFreqID INT,
    @RePriceDt DATETIME = NULL,
    @RequestorEmail VARCHAR(50) = NULL,
    @RequestorID VARCHAR(7),
    @Spread DECIMAL(5,2) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Error INT, @IdentityID INT, @ReferenceNumber VARCHAR(10);

    BEGIN TRY
        BEGIN TRAN;
        
        -- Insert into Fixed Rate Lock table
        INSERT INTO FRLI_FixedRateLock 
        (
            AccrualID, AcctSystemID, AdvanceTypeID, AllInRate, AmortTerm, AmortTypeID, Approver, CIP, ClosingDt, ConfirmationInitials, CostCenter, CurrencyType, CustomerName, 
            DSILease, FirstPmtDt, FixedTerm, ForwardCharge, ForwardSettlement, FundingAmt, IndemnityAgreement, InterestFreqID, IsCommercialMortgageEligible, LastUpdateDT, LeaseResidual, 
            LenderID, LenderName, LiquidityID, LoanTerm, MaturityDate, MMCOF, Notes, Originator, PartialPrePayID, Phone, PPRiskPremium, PPRollOver, PrePaymentTypeID, PrePaymentWaiver, 
            PrePaymentWaiverAcknowledgement, PrincipalFreqID, ReferenceAlpha, RePriceDt, RequestorEmail, RequestorID, Spread
        )
        VALUES
        (
            @AccrualID, @AcctSystemID, @AdvanceTypeID, @AllInRate, @AmortTerm, @AmortTypeID, @Approver, @CIP, @ClosingDt, @ConfirmationInitials, @CostCenter, @CurrencyType, @CustomerName, 
            @DSILease, @FirstPmtDt, @FixedTerm, @ForwardCharge, @ForwardSettlement, @FundingAmt, @IndemnityAgreement, @InterestFreqID, @IsCommercialMortgageEligible, @LastUpdateDT, @LeaseResidual, 
            @LenderID, @LenderName, @LiquidityID, @LoanTerm, @MaturityDate, @MMCOF, @Notes, @Originator, @PartialPrePayID, @Phone, @PPRiskPremium, @PPRollOver, @PrePaymentTypeID, @PrePaymentWaiver, 
            @PrePaymentWaiverAcknowledgement, @PrincipalFreqID, @ReferenceAlpha, @RePriceDt, @RequestorEmail, @RequestorID, @Spread
        );
        
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        SELECT @Error = ERROR_NUMBER();
        RETURN @Error;
    END CATCH

    RETURN 0;
END;