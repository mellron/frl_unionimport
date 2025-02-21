using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using frl_unionimport.models; // <-- So we can use UnionBankData
using frl_unionimport.util; // Importing AppConfiguration


namespace frl_unionimport
{
    public class Validatefile
    {
        private static readonly int ExpectedColumnCount;

        // --- (1) All your existing dictionaries ---
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

        private static readonly Dictionary<string, int> DivisionIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 },
            { "5", 5 }, { "6", 6 }, { "7", 7 }, { "8", 8 }, { "9", 9 }
        };

        private static readonly Dictionary<string, int> InterestFrequencyIDMap = new Dictionary<string, int>
        {
            { "0", 0 }, { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 }
        };

        // Store raw lines here (if you still want them)
        private string[] m_lines = Array.Empty<string>();

        // --- (2) A new List to store your UnionBankData rows ---
        public List<UnionBankData> UnionBankDataRows { get; private set; } = new List<UnionBankData>();


        public string[] FileData
        {
            get => m_lines;
            set => m_lines = value;
        }

        static Validatefile()
        {
            // This tries to read the "numberofcolumns" from AppConfiguration; fallback is 43
            ExpectedColumnCount = int.TryParse(AppConfiguration.GetPropertyValue("numberofcolumns"), out int value) 
                ? value 
                : 43;
        }

        /// <summary>
        /// Main method to load and validate a file. If valid, populates UnionBankDataRows.
        /// </summary>
        public bool LoadFile(string fileName)
        {
            try
            {
                if (!ValidateFile(fileName, out var lines))
                {
                    return false;
                }

                // Once validated at a high level, do row-by-row parsing into the model
                if (!ValidateAllRows(lines))
                {
                    Console.WriteLine("Row validation failed.");
                    return false;
                }

                Console.WriteLine("File validation passed.");
                m_lines = lines; // store raw lines if needed

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate each row AND map it to a UnionBankData object.  
        /// If any row fails, we stop and return false.
        /// </summary>
        private bool ValidateAllRows(string[] lines)
        {
            // Clear any previous data
            UnionBankDataRows.Clear();

            // Start at index = 1 to skip the header row (assuming row 0 is headers).
            for (int rowIndex = 1; rowIndex < lines.Length; rowIndex++)
            {
                var line = lines[rowIndex];
                var columns = line.Split(',');

                // Check for the correct column count
                if (columns.Length != ExpectedColumnCount)
                {
                    Console.WriteLine($"Row {rowIndex} has {columns.Length} columns. Expected {ExpectedColumnCount}.");
                    return false;
                }

                try
                {
                    // --- (3) Perform existing conversions/validations ---
                    string referenceAlpha = ID_Convert(columns[0]);
                    int accrualID = AccrualID_Convert(columns[1]);
                    int acctSystemID = AccountSystemID_Convert(columns[2]);
                    float allInRateFloat = AllInRate_Convert(columns[3]);
                    int amortTerm = AmortizationTermMonths_Convert(columns[4]);
                    int amortTypeID = AmortizationTypeID_Convert(columns[5]);
                    DateTime? documentDate = DocumentDate_Convert(columns[6]);
                    string costCenter = CostCenter_Convert(columns[7]);
                    string customerName = CustomerName_Convert(columns[8]);
                    int divisionID = DivisionID_Convert(columns[9]);
                    DateTime? firstPmtDt = FirstPaymentDate_Convert(columns[10]);
                    int fixedTerm = FixedTermMonths_Convert(columns[11]);
                    float forwardChargeFloat = ForwardCharge_Convert(columns[12]);
                    int forwardSettlement = ForwardSettlement_Convert(columns[13]);
                    float fundingAmtFloat = FundingAmount_Convert(columns[14]);
                    int interestFreqID = InterestFrequencyID_Convert(columns[15]);
                    float leaseResidualFloat = LeaseResidual_Convert(columns[16]);
                    int loanTerm = LoanTermMonths_Convert(columns[17]);
                    DateTime? maturityDate = MaturityDate_Convert(columns[18]);
                    float mmcofFloat = MMCOF_Convert(columns[19]);
                    string notes = Notes_Convert(columns[20]);
                    string phone = Phone_Convert(columns[21]);
                    float ppRiskPremiumFloat = PPRiskPremium_Convert(columns[22]);
                    float ppRollOverFloat = PPRollOver_Convert(columns[23]);
                    int principalFreqID = PrincipalFrequencyID_Convert(columns[24]);
                    string dsiLease = DSILease_Convert(columns[25]);
                    DateTime? rePriceDt = RePriceDate_Convert(columns[26]);
                    string requestorID = RequestorID_Convert(columns[27]);
                    string approverIntials = ApproverInitials_Convert(columns[28]);
                    string requestorName = Requestor_Convert(columns[29]);
                    float spreadFloat = Spread_Convert(columns[30]);
                    int advanceTypeID = AdvanceTypeID_Convert(columns[31]);
                    int currencyType = CurrencyType_Convert(columns[32]);
                    int liquidityID = LiquidityID_Convert(columns[33]);
                    string confirmationInitials = ConfirmationInitials_Convert(columns[34]);
                    int indemnityAgreement = IndemnityAgreement_Convert(columns[35]);
                    string lenderName = LenderName_Convert(columns[36]);
                    int isCommercialMortgageEligible = IsCommunityInvestmentProgram_Convert(columns[37]);
                    int prePaymentWaiver = IsPrePaymentWaiver_Convert(columns[38]);
                    int prePaymentTypeID = PrePaymentTypeID_Convert(columns[39]);
                    int partialPrePaymentID = PartialPrePaymentID_Convert(columns[40]);
                    int prePaymentWaiverAck = PrePaymentWaiverAcknowledgement_Convert(columns[41]);
                    string requestorEmail = RequestorEmail_Convert(columns[42]);
                    string lenderID = LenderID_Convert(columns[43]);
         

                    // --- (4) Create a new UnionBankData object and fill it ---
                    UnionBankData ubData = new()
                    {
                        ReferenceAlpha = referenceAlpha,
                        AccrualID = accrualID == 0 ? (int?)null : accrualID,
                        AcctSystemID = acctSystemID == 0 ? (int?)null : acctSystemID,
                        AllInRate = (decimal)allInRateFloat,  // or cast to decimal?
                        AmortTerm = amortTerm == 0 ? (int?)null : amortTerm,
                        AmortTypeID = amortTypeID == 0 ? (int?)null : amortTypeID,
                        DocumentDate = documentDate,
                        CostCenter = costCenter,
                        CustomerName = customerName,
                        DivisionID = divisionID == 0 ? (int?)null : divisionID,
                        FirstPmtDt = firstPmtDt,
                        FixedTerm = fixedTerm == 0 ? (int?)null : fixedTerm,
                        ForwardCharge = forwardChargeFloat.ToString(), // or keep as string?
                        ForwardSettlement = forwardSettlement == 0 ? (int?)null : forwardSettlement,
                        FundingAmt = (decimal)fundingAmtFloat, // cast to decimal
                        InterestFreqID = interestFreqID == 0 ? (int?)null : interestFreqID,
                        LeaseResidual = (decimal)leaseResidualFloat,
                        LoanTerm = loanTerm == 0 ? (int?)null : loanTerm,
                        MaturityDate = maturityDate,
                        MMCOF = mmcofFloat.ToString(),        // or keep as string?
                        Notes = notes,
                        Phone = phone,
                        PPRiskPremium = (decimal)ppRiskPremiumFloat,
                        PPRollOver = (decimal)ppRollOverFloat,
                        PrincipalFreqID = principalFreqID == 0 ? (int?)null : principalFreqID,
                        DSILease = dsiLease,
                        RePriceDt = rePriceDt,
                        RequestorID = requestorID,
                        ApproverInitials = approverIntials,
                        RequestorName = requestorName,
                        Spread = (decimal)spreadFloat,
                        AdvanceTypeID = advanceTypeID == 0 ? (int?)null : advanceTypeID,
                        CurrencyType = currencyType == 0 ? (int?)null : currencyType,
                        LiquidityID = liquidityID == 0 ? (int?)null : liquidityID,
                        ConfirmationInitials = confirmationInitials,
                        IndemnityAgreement = indemnityAgreement == 0 ? (int?)null : indemnityAgreement,
                        LenderName = lenderName,
                        IsCommercialMortgageEligible = (isCommercialMortgageEligible == 1),
                        IsPrePaymentWaiver = (prePaymentWaiver == 1),
                        PrePaymentTypeID = prePaymentTypeID == 0 ? (int?)null : prePaymentTypeID,
                        PartialPrePayID = partialPrePaymentID == 0 ? (int?)null : partialPrePaymentID,
                        PrePaymentWaiverAcknowledgement = (prePaymentWaiverAck == 1),
                        RequestorEmail = requestorEmail,
                        LenderID = lenderID
                    };

                    // --- (5) Add the validated object to our list ---
                    UnionBankDataRows.Add(ubData);
                }
                catch (FormatException ex)
                {
                    // If any conversion or format fails, log and return false
                    Console.WriteLine($"Row {rowIndex} parse error: {ex.Message}");
                    return false;
                }
            }

            // If we reach here, all rows have been parsed successfully
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

        // --- (6) YOUR EXISTING CONVERSION METHODS, using expression bodies where possible ---

        public int FixedTermMonths_Convert(string value) => int.TryParse(value, out int result) ? result : 0;

        public string ID_Convert(string value)
            => string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^UB\d{3}$")
                ? throw new FormatException("Invalid ID format")
                : value;

        public int AccrualID_Convert(string value)
            => AccrualIDMap.TryGetValue(value.ToUpper(), out int result) ? result : 0;

        public int AccountSystemID_Convert(string value)
            => AccountSystemIDMap.TryGetValue(value.ToUpper(), out int result) ? result : 0;

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
            => !string.IsNullOrWhiteSpace(value) ? value : throw new FormatException("Value cannot be null or blank");


        public string RequestorEmail_Convert(string value)
            => value ?? string.Empty;
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

        public string CostCenter_Convert(string value) => value ?? string.Empty;

        public string CustomerName_Convert(string value) => value ?? string.Empty;

        public string LenderName_Convert(string value) => value ?? string.Empty;

        public string LenderID_Convert(string value) => value ?? string.Empty;

        public string Notes_Convert(string value) => value ?? string.Empty;

        public float PPRiskPremium_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public float PPRollOver_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public string ApproverInitials_Convert(string value) => value ?? string.Empty;

        public string Requestor_Convert(string value) => value ?? string.Empty;

        public float Spread_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public int LiquidityID_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public string ConfirmationInitials_Convert(string value) => value ?? string.Empty;

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

        public int AmortizationTermMonths_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public int DivisionID_Convert(string value)
            => DivisionIDMap.TryGetValue(value, out int result) ? result : 0;

        public float ForwardCharge_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public int ForwardSettlement_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public int InterestFrequencyID_Convert(string value)
            => InterestFrequencyIDMap.TryGetValue(value, out int result) ? result : 0;

        public float MMCOF_Convert(string value)
            => float.TryParse(value, out float result) ? result : 0.0f;

        public DateTime? RePriceDate_Convert(string value)
            => DateTime.TryParse(value, out DateTime dt) ? dt : (DateTime?)null;

        public string RequestorID_Convert(string value)
            => value ?? string.Empty;

        public int IndemnityAgreement_Convert(string value)
            => int.TryParse(value, out int result) ? result : 0;

        public string DSILease_Convert(string value)
            => value ?? string.Empty;
    }
}
