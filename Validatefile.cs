using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using frl_unionimport.util;
using Microsoft.Extensions.Primitives; // Importing AppConfiguration

namespace frl_unionimport
{   
    public class Validatefile
    {
        private static readonly int ExpectedColumnCount;

        // Existing dictionaries
        private static readonly Dictionary<string, int> AccrualIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "Act/360", 1 }, { "30/360", 2 }, { "Act/Act", 3 }
        };
        private static readonly Dictionary<string, int> AccountSystemIDMap = new Dictionary<string, int>
        {
            { "UNK", 0 }, { "AFS", 1 }, { "CML", 2 }, { "CPI", 3 }, { "DSI", 4 }, { "Other Lease", 5 }, { "ALS", 6 }, { "LoanIQ", 7 }
        };
        private static readonly Dictionary<string, int> PrincipalFrequencyIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }
        };
        private static readonly Dictionary<string, int> AmortizationTypeIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }
        };
        private static readonly Dictionary<string, int> AdvanceTypeIDMap = new Dictionary<string, int>
        {
            { "1", 1 }, { "2", 2 }
        };
        private static readonly Dictionary<string, int> CurrencyTypeMap = new Dictionary<string, int>
        {
            { "1", 1 }, { "2", 2 }
        };
        private static readonly Dictionary<string, int> PrePaymentTypeIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }, { "6", 6 }, { "7", 7 }
        };

        // NEW dictionaries for DivisionID and InterestFrequencyID
        private static readonly Dictionary<string, int> DivisionIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 },
            { "5", 5 }, { "6", 6 }, { "7", 7 }, { "8", 8 }, { "9", 9 }
        };

        private static readonly Dictionary<string, int> InterestFrequencyIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }
        };

        private string[] m_lines = Array.Empty<string>();
    
        public string[] FileData
        {
            get => m_lines;
            set => m_lines = value;
        }

        static Validatefile()
        {
            ExpectedColumnCount = int.TryParse(AppConfiguration.GetPropertyValue("numberofcolumns"), out int value)
                ? value 
                : 43;
        }

        public bool LoadFile(string fileName)
        {
            try
            {
                if (!ValidateFile(fileName, out var lines))
                {
                    return false;
                }

                // Optionally call ValidateAllRows if you want deeper row-by-row data checks:
                if (!ValidateAllRows(lines))
                {
                    Console.WriteLine("Row validation failed.");

                    return false;
                }

                foreach (var line in lines)
                {
                Console.WriteLine(line);
                }

                Console.WriteLine("File validation passed.");

                m_lines = lines;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                return false;
            }
        }

        private bool ValidateFile(string fileName, out string[] lines)
        {
            lines = Array.Empty<string>();

        try 
        {
                if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
                {
                    Console.WriteLine("Invalid file path.");
                    return false;
                }

                lines = File.ReadAllLines(fileName);

                if (lines.Length <= 1)
                {
                    Console.WriteLine("File must contain more than one row of data.");
                    return false;
                }

                if (!ValidateNumberOfColumns(lines))
                {
                    Console.WriteLine("Column validation failed.");
                    return false;
                }

                if (!ValidateAllRows(lines))
                {
                    Console.WriteLine("Row validation failed.");
                    return false;
                }
        }
            catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                    return false;
                }

            return true;
        }

    private bool ValidateAllRows(string[] lines)
    {
        // Start at index = 1 to skip the header row (assuming index 0 is headers).
        for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
        {
            var line = lines[rowIndex];
            var columns = line.Split(',');

            // Double-check we have the expected column count (47).
            if (columns.Length != ExpectedColumnCount)
            {
                Console.WriteLine(
                    $"Row {rowIndex} has an incorrect number of columns: {columns.Length}. " +
                    $"Expected {ExpectedColumnCount}."
                );
                return false;
            }

            try
            {
                string hexID = ID_Convert(columns[0]); // HexID

                int accrualID = AccrualID_Convert(columns[1]); // AccrualID

                int acctSystemID = AccountSystemID_Convert(columns[2]); // AcctSystemID

                float allInRate = AllInRate_Convert(columns[3]); // AllInRate

                int amortTerm = AmortizationTermMonths_Convert(columns[4]); // AmortTerm

                int amortTypeID = AmortizationTypeID_Convert(columns[5]); // AmortTypeID

                DateTime? closingDt = DocumentDate_Convert(columns[6]); // ClosingDt (Lock-in Date)

                string costCenter = CostCenter_Convert(columns[7]); // CostCenter

                string customerName = CustomerName_Convert(columns[8]); // CustomerName

                int divisionID = DivisionID_Convert(columns[9]); // DivisionID

                DateTime? firstPmtDt = FirstPaymentDate_Convert(columns[10]); // FirstPmtDt

                int fixedTerm = FixedTermMonths_Convert(columns[11]); // FixedTerm

                float forwardCharge = ForwardCharge_Convert(columns[12]); // ForwardCharge

                int forwardSettlement = ForwardSettlement_Convert(columns[13]); // ForwardSettlement

                float fundingAmt = FundingAmount_Convert(columns[14]); // FundingAmt

                int interestFreqID = InterestFrequencyID_Convert(columns[15]); // InterestFreqID

                float leaseResidual = LeaseResidual_Convert(columns[16]); // LeaseResidual

                int loanTerm = LoanTermMonths_Convert(columns[17]); // LoanTerm

                DateTime? maturityDate = MaturityDate_Convert(columns[18]); // MaturityDate

                float mmcof = MMCOF_Convert(columns[19]); // MMCOF

                string notes = Notes_Convert(columns[20]); // Notes

                string phone = Phone_Convert(columns[21]); // Phone

                float ppRiskPremium = PPRiskPremium_Convert(columns[22]); // PPRiskPremium

                float ppRollOver = PPRollOver_Convert(columns[23]); // PPRollOver

                int principalFreqID = PrincipalFrequencyID_Convert(columns[24]); // PrincipalFreqID

                // This is not really used, we can ask them to remove it.
                string dsiLease = DSILease_Convert(columns[25]); // DSILease

                DateTime? rePriceDt = RePriceDate_Convert(columns[26]); // RePriceDt

                string requestorID = RequestorID_Convert(columns[27]); // RequestorID

                string approver = ApproverInitials_Convert(columns[28]); // Approver

                string originator = Requestor_Convert(columns[29]); // Originator

                float spread = Spread_Convert(columns[30]); // Spread

                int advanceTypeID = AdvanceTypeID_Convert(columns[31]); // AdvanceTypeID

                int currencyType = CurrencyType_Convert(columns[32]); // CurrencyType

                int liquidityID = LiquidityID_Convert(columns[33]); // LiquidityID

                string confirmationInitials = ConfirmationInitials_Convert(columns[34]); // ConfirmationInitials

                int indemnityAgreement = IndemnityAgreement_Convert(columns[35]); // IndemnityAgreement

                string lenderName = LenderName_Convert(columns[36]); // LenderName

                int cip = IsCommunityInvestmentProgram_Convert(columns[37]); // CIP

                int prePaymentWaiver = IsPrePaymentWaiver_Convert(columns[38]); // PrePaymentWaiver

                int prePaymentTypeID = PrePaymentTypeID_Convert(columns[39]); // PrePaymentTypeID

                int partialPrePaymentID = PartialPrePaymentID_Convert(columns[40]); // PartialPrePayID

                int prePaymentWaiverAck = PrePaymentWaiverAcknowledgement_Convert(columns[41]); // PrePaymentWaiverAcknowledgement

                string requestorEmail = RequestorEmail_Convert(columns[42]); // RequestorEmail

                string lenderID = LenderID_Convert(columns[43]); // LenderID

                int isCommercialMortgageEligible = IsCommercialMortgageEligible_Convert(columns[44]); // IsCommercialMortgageEligible


                
            }
            catch (FormatException ex)
            {
                // If any conversion (e.g., phone/email) threw a FormatException, 
                // log and return false so validation stops.
                Console.WriteLine($"Row {rowIndex} data parse error: {ex.Message}");
                return false;
            }
        }

        // If we got here, every row validated successfully
        return true;
    }


        private bool ValidateNumberOfColumns(string[] lines)
        {
            foreach (var line in lines.Skip(1)) // Skip header row
            {
                var columns = line.Split(',');
                if (columns.Length != ExpectedColumnCount)
                {
                    Console.WriteLine("Incorrect number of columns in data row.");
                    return false;
                }
            }
            return true;
        }


        public int FixedTermMonths_Convert(string value) => int.TryParse(value, out int result) ? result : 0;
    
        // can the above be writtien using =>
        public string ID_Convert(string value)
            => string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^UB\d{3}$")
                ? throw new FormatException("Invalid ID format")
                : value;    
        public int AccrualID_Convert(string value)
            => AccrualIDMap.TryGetValue(value, out int result) ? result : 0;

        public int AccountSystemID_Convert(string value)
            => AccountSystemIDMap.TryGetValue(value, out int result) ? result : 0;

        public int PrincipalFrequencyID_Convert(string value)
            => PrincipalFrequencyIDMap.TryGetValue(value, out int result) ? result : 0;

        public int AmortizationTypeID_Convert(string value)
            => AmortizationTypeIDMap.TryGetValue(value, out int result) ? result : 0;

        public int AdvanceTypeID_Convert(string value)
            => AdvanceTypeIDMap.TryGetValue(value, out int result) ? result : 0;

        public int CurrencyType_Convert(string value)
            => CurrencyTypeMap.TryGetValue(value, out int result) ? result : 0;

        public int PrePaymentTypeID_Convert(string value)
            => PrePaymentTypeIDMap.TryGetValue(value, out int result) ? result : 0;

        public float AllInRate_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public string Phone_Convert(string value)
            => !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(value, "^\\d{3}-\\d{3}-\\d{4}$")
                ? value
                : throw new FormatException("Invalid phone format");

        public string RequestorEmail_Convert(string value)
            => !string.IsNullOrWhiteSpace(value) && value.Contains("@")
                ? value
                : throw new FormatException("Invalid email format");

        public float FundingAmount_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public DateTime? DocumentDate_Convert(string value)
            => DateTime.TryParse(value, out DateTime result) ? result : (DateTime?)null;

        public DateTime? FirstPaymentDate_Convert(string value)
            => DateTime.TryParse(value, out DateTime result) ? result : (DateTime?)null;

        public DateTime? MaturityDate_Convert(string value)
            => DateTime.TryParse(value, out DateTime result) ? result : (DateTime?)null;

        public float LeaseResidual_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public int LoanTermMonths_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public string CostCenter_Convert(string value)
            => value ?? string.Empty;

        public string CustomerName_Convert(string value)
            => value ?? string.Empty;

        public string LenderName_Convert(string value)
            => value ?? string.Empty;

        public string LenderID_Convert(string value)
             => value ?? string.Empty;

        public string Notes_Convert(string value)
            => value ?? string.Empty;

        public float PPRiskPremium_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public float PPRollOver_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public string ApproverInitials_Convert(string value)
            => value ?? string.Empty;

        public string Requestor_Convert(string value)
            => value ?? string.Empty;

        public float Spread_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public int LiquidityID_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public string ConfirmationInitials_Convert(string value)
            => value ?? string.Empty;

        public int IsCommunityInvestmentProgram_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public int IsPrePaymentWaiver_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public int PartialPrePaymentID_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public int PrePaymentWaiverAcknowledgement_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public int IsCommercialMortgageEligible_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        // NEW conversions

        /// <summary>
        /// Amortization Term Months (integer)
        /// </summary>
        public int AmortizationTermMonths_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        /// <summary>
        /// Division ID with dictionary mapping
        /// </summary>
        public int DivisionID_Convert(string value)
            => DivisionIDMap.TryGetValue(value, out int result) ? result : 0;

        /// <summary>
        /// Forward Charge (float)
        /// </summary>
        public float ForwardCharge_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        /// <summary>
        /// Forward Settlement (integer)
        /// </summary>
        public int ForwardSettlement_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        /// <summary>
        /// Interest Frequency ID with dictionary mapping
        /// </summary>
        public int InterestFrequencyID_Convert(string value)
            => InterestFrequencyIDMap.TryGetValue(value, out int result) ? result : 0;

        /// <summary>
        /// MMCOF (float)
        /// </summary>
        public float MMCOF_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        /// <summary>
        /// RePriceDate in mm/dd/yyyy format
        /// </summary>
        public DateTime? RePriceDate_Convert(string value)
            => DateTime.TryParse(value, out DateTime dt) ? dt : (DateTime?)null;

        /// <summary>
        /// RequestorID (preferred ID of user)
        /// </summary>
        public string RequestorID_Convert(string value)
            => value ?? string.Empty;   


        /// <summary>
        /// IndemnityAgreement (1 or 0)
        /// </summary>
        public int IndemnityAgreement_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public string DSILease_Convert(string value)
            => value ?? string.Empty;   

    }
}