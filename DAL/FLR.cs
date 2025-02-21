using System;
using System.Data;
using System.Data.SqlClient;
using Utility.Database.MSSQL; // Or your actual namespace for CreateConnection
using frl_unionimport.models; // Assuming this is where your UnionBankData class is located


namespace frl_unionimport.DAL
{
    public static class FLR  
    { 
        public static bool InsertFixedRateLock(UnionBankData unionBankData)
        {
            bool isSuccess = false;

            try
            {

                using SqlConnection conn = (SqlConnection)Base.CreateConnection();

                conn.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.FixedRateLock_INS", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Because some parameters are optional (can be NULL),
                    // we use the pattern: (object)parameter ?? DBNull.Value
                    // for those that are nullable.
                    // For mandatory fields, just pass the value directly.

                    cmd.Parameters.AddWithValue("@ReferenceAlpha", unionBankData.ReferenceAlpha);
                    cmd.Parameters.AddWithValue("@AccrualID", unionBankData.AccrualID);
                    cmd.Parameters.AddWithValue("@AcctSystemID", unionBankData.AcctSystemID);

                    cmd.Parameters.AddWithValue("@AllInRate", unionBankData.AllInRate);
                    cmd.Parameters.AddWithValue("@AmortTerm", unionBankData.AmortTerm);
                    cmd.Parameters.AddWithValue("@AmortTypeID", unionBankData.AmortTypeID);
                    cmd.Parameters.AddWithValue("@ClosingDt", unionBankData.DocumentDate);
                    cmd.Parameters.AddWithValue("@CostCenter", unionBankData.CostCenter);
                    cmd.Parameters.AddWithValue("@CustomerName", unionBankData.CustomerName);
                    cmd.Parameters.AddWithValue("@DivisionID", unionBankData.DivisionID);
                    cmd.Parameters.AddWithValue("@FirstPmtDt", unionBankData.FirstPmtDt);
                    cmd.Parameters.AddWithValue("@FixedTerm", unionBankData.FixedTerm);
                    cmd.Parameters.AddWithValue("@ForwardCharge", unionBankData.ForwardCharge);
                    cmd.Parameters.AddWithValue("@ForwardSettlement", unionBankData.ForwardSettlement);
                    cmd.Parameters.AddWithValue("@FundingAmt", unionBankData.FundingAmt);
                    cmd.Parameters.AddWithValue("@InterestFreqID", unionBankData.InterestFreqID);
                    cmd.Parameters.AddWithValue("@LeaseResidual", unionBankData.LeaseResidual);
                    cmd.Parameters.AddWithValue("@LoanTerm", unionBankData.LoanTerm);
                    cmd.Parameters.AddWithValue("@MaturityDate", unionBankData.MaturityDate);
                    cmd.Parameters.AddWithValue("@MMCOF", unionBankData.MMCOF);
                    cmd.Parameters.AddWithValue("@Notes", unionBankData.Notes);
                    cmd.Parameters.AddWithValue("@Phone", unionBankData.Phone);
                    cmd.Parameters.AddWithValue("@PPRiskPremium", unionBankData.PPRiskPremium);
                    cmd.Parameters.AddWithValue("@PPRollOver", unionBankData.PPRollOver);
                    cmd.Parameters.AddWithValue("@PrincipalFreqID", unionBankData.PrincipalFreqID);
                    cmd.Parameters.AddWithValue("@DSILease", unionBankData.DSILease);
                    cmd.Parameters.AddWithValue("@RePriceDt", unionBankData.RePriceDt);
                    cmd.Parameters.AddWithValue("@RequestorID", unionBankData.RequestorID);

                    cmd.Parameters.AddWithValue("@RequestorName", unionBankData.RequestorName);
                    cmd.Parameters.AddWithValue("@Spread", unionBankData.Spread);
                    cmd.Parameters.AddWithValue("@AdvanceTypeID", unionBankData.AdvanceTypeID);
                    cmd.Parameters.AddWithValue("@CurrencyType", unionBankData.CurrencyType);
                    cmd.Parameters.AddWithValue("@LiquidityID", unionBankData.LiquidityID);
                    cmd.Parameters.AddWithValue("@ConfirmationInitials", unionBankData.ConfirmationInitials);
                    cmd.Parameters.AddWithValue("@IndemnityAgreement", unionBankData.IndemnityAgreement);
                    cmd.Parameters.AddWithValue("@LenderName", unionBankData.LenderName);
                    cmd.Parameters.AddWithValue("@CIP", unionBankData.IsCommercialMortgageEligible);
                    cmd.Parameters.AddWithValue("@PrePaymentWaiver", unionBankData.IsPrePaymentWaiver);
                    cmd.Parameters.AddWithValue("@PrePaymentTypeID", unionBankData.PrePaymentTypeID);
                    cmd.Parameters.AddWithValue("@PartialPrePayID", unionBankData.PartialPrePayID);
                    cmd.Parameters.AddWithValue("@PrePaymentWaiverAcknowledgement", unionBankData.PrePaymentWaiverAcknowledgement);
                    cmd.Parameters.AddWithValue("@RequestorEmail", unionBankData.RequestorEmail);
                    cmd.Parameters.AddWithValue("@LenderID", unionBankData.LenderID);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    isSuccess = (rowsAffected > 0);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as appropriate.
                Console.WriteLine("Error inserting FixedRateLock: " + ex.Message);
            }

            return isSuccess;
        }

        public static string FindEmailAddressByID(string value)
        {
            string email = string.Empty;

            try
            {
                using SqlConnection conn = (SqlConnection)Base.CreateConnection();

                conn.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.FindEmailAddressByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@intranetid", value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        email = reader["Email"].ToString();
                    }

                    reader.Close();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as appropriate.
                Console.WriteLine("Error finding email address: " + ex.Message);

                return string.Empty;
            }

            return email;
        }
        public static string FindNameByID(string value)
        {
            string name = string.Empty;

            try
            {
                using SqlConnection conn = (SqlConnection)Base.CreateConnection();

                conn.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.FindNameByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@intranetid", value);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        name = reader["Name"].ToString();
                    }

                    reader.Close();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as appropriate.
                Console.WriteLine("Error finding name: " + ex.Message);

                return string.Empty;
            }

            return name;
        }
    }
}
