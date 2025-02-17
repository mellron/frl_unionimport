using System;
using System.Data;
using System.Data.SqlClient;
using Utility.Database.MSSQL; // Or your actual namespace for CreateConnection

namespace frl_unionimport.DAL
{
    public class FixedRateLockDAL : Base
    {
        public static bool InsertFixedRateLock(
            string referenceAlpha,            // @ReferenceAlpha VARCHAR(10)
            int accrualID,                    // @AccrualID INT
            int acctSystemID,                 // @AcctSystemID INT
            short? advanceTypeID,             // @AdvanceTypeID SMALLINT = NULL
            decimal? allInRate,               // @AllInRate DECIMAL(28,2) = NULL
            short? amortTerm,                 // @AmortTerm SMALLINT = NULL
            int amortTypeID,                  // @AmortTypeID INT
            string approver,                  // @Approver VARCHAR(35) = NULL
            bool cip,                         // @CIP BIT
            DateTime closingDt,               // @ClosingDt DATETIME
            string confirmationInitials,      // @ConfirmationInitials VARCHAR(3)
            string costCenter,                // @CostCenter CHAR(11)
            short currencyType,               // @CurrencyType SMALLINT
            string customerName,              // @CustomerName VARCHAR(60)
            string dsiLease,                  // @DSILease CHAR(13) = NULL
            DateTime? firstPmtDt,             // @FirstPmtDt DATETIME
            short fixedTerm,                  // @FixedTerm SMALLINT
            decimal? forwardCharge,           // @ForwardCharge MONEY = NULL
            short? forwardSettlement,         // @ForwardSettlement SMALLINT = NULL
            decimal fundingAmt,               // @FundingAmt MONEY
            short? indemnityAgreement,        // @IndemnityAgreement SMALLINT = NULL
            int interestFreqID,               // @InterestFreqID INT
            bool isCommercialMortgageEligible, // @IsCommercialMortgageEligible BIT
            DateTime? lastUpdateDT,           // @LastUpdateDT DATETIME = NULL
            decimal? leaseResidual,           // @LeaseResidual DECIMAL(5,2) = NULL
            string lenderID,                  // @LenderID VARCHAR(7) = NULL
            string lenderName,                // @LenderName VARCHAR(50)
            short liquidityID,                // @LiquidityID SMALLINT
            short? loanTerm,                  // @LoanTerm SMALLINT = NULL
            DateTime maturityDate,            // @MaturityDate DATETIME
            decimal? mmcof,                   // @MMCOF DECIMAL(28,2) = NULL
            string notes,                     // @Notes VARCHAR(2048) = NULL
            string originator,                // @Originator VARCHAR(35)
            byte? partialPrePayID,            // @PartialPrePayID TINYINT = NULL
            string phone,                     // @Phone VARCHAR(12)
            decimal? ppRiskPremium,           // @PPRiskPremium MONEY = NULL
            decimal? ppRollOver,              // @PPRollOver MONEY = NULL
            int? prePaymentTypeID,            // @PrePaymentTypeID INT = NULL
            bool prePaymentWaiver,            // @PrePaymentWaiver BIT
            bool prePaymentWaiverAcknowledgement,  // @PrePaymentWaiverAcknowledgement BIT
            int principalFreqID,              // @PrincipalFreqID INT
            DateTime? rePriceDt,              // @RePriceDt DATETIME = NULL
            string requestorEmail,            // @RequestorEmail VARCHAR(50) = NULL
            string requestorID,               // @RequestorID VARCHAR(7)
            decimal? spread                   // @Spread DECIMAL(5,2) = NULL
        )
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = (SqlConnection)CreateConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.FixedRateLock_INS", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Because some parameters are optional (can be NULL),
                        // we use the pattern: (object)parameter ?? DBNull.Value
                        // for those that are nullable.
                        // For mandatory fields, just pass the value directly.

                        cmd.Parameters.AddWithValue("@ReferenceAlpha", referenceAlpha ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AccrualID", accrualID);
                        cmd.Parameters.AddWithValue("@AcctSystemID", acctSystemID);
                        cmd.Parameters.AddWithValue("@AdvanceTypeID", (object)advanceTypeID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AllInRate", (object)allInRate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AmortTerm", (object)amortTerm ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AmortTypeID", amortTypeID);
                        cmd.Parameters.AddWithValue("@Approver", (object)approver ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CIP", cip);
                        cmd.Parameters.AddWithValue("@ClosingDt", closingDt);
                        cmd.Parameters.AddWithValue("@ConfirmationInitials", confirmationInitials ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CostCenter", costCenter ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CurrencyType", currencyType);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@DSILease", (object)dsiLease ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FirstPmtDt", (object)firstPmtDt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FixedTerm", fixedTerm);
                        cmd.Parameters.AddWithValue("@ForwardCharge", (object)forwardCharge ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ForwardSettlement", (object)forwardSettlement ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FundingAmt", fundingAmt);
                        cmd.Parameters.AddWithValue("@IndemnityAgreement", (object)indemnityAgreement ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@InterestFreqID", interestFreqID);
                        cmd.Parameters.AddWithValue("@IsCommercialMortgageEligible", isCommercialMortgageEligible);
                        cmd.Parameters.AddWithValue("@LastUpdateDT", (object)lastUpdateDT ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@LeaseResidual", (object)leaseResidual ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@LenderID", (object)lenderID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@LenderName", lenderName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LiquidityID", liquidityID);
                        cmd.Parameters.AddWithValue("@LoanTerm", (object)loanTerm ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaturityDate", maturityDate);
                        cmd.Parameters.AddWithValue("@MMCOF", (object)mmcof ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Notes", (object)notes ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Originator", originator ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PartialPrePayID", (object)partialPrePayID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Phone", phone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PPRiskPremium", (object)ppRiskPremium ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PPRollOver", (object)ppRollOver ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PrePaymentTypeID", (object)prePaymentTypeID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PrePaymentWaiver", prePaymentWaiver);
                        cmd.Parameters.AddWithValue("@PrePaymentWaiverAcknowledgement", prePaymentWaiverAcknowledgement);
                        cmd.Parameters.AddWithValue("@PrincipalFreqID", principalFreqID);
                        cmd.Parameters.AddWithValue("@RePriceDt", (object)rePriceDt ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@RequestorEmail", (object)requestorEmail ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@RequestorID", requestorID ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Spread", (object)spread ?? DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        isSuccess = (rowsAffected > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as appropriate.
                Console.WriteLine("Error inserting FixedRateLock: " + ex.Message);
            }

            return isSuccess;
        }
    }
}
