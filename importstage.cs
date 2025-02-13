using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using frl_unionimport.util; // Importing AppConfiguration

public class ImportStage
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

    static ImportStage()
    {
        ExpectedColumnCount = int.TryParse(AppConfiguration.GetPropertyValue("ExpectedColumnCount"), out int value)
            ? value 
            : 47;
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
            // if (!ValidateAllRows(lines))
            // {
            //     return false;
            // }

            Console.WriteLine("File validation passed.");
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
            string id = ID_Convert(columns[0]);
            
            int accrualId = AccrualID_Convert(columns[1]);
            
            int accountSystemId = AccountSystemID_Convert(columns[2]);
            
            float allInRate = AllInRate_Convert(columns[3]);
            
            int amortTermMonths = AmortizationTermMonths_Convert(columns[4]);
            
            int amortTypeId = AmortizationTypeID_Convert(columns[5]);
            
            DateTime? documentDate = DocumentDate_Convert(columns[6]);

            string costCenter = CostCenter_Convert(columns[7]);

            string customerName = CustomerName_Convert(columns[8]);

            int divisionId = DivisionID_Convert(columns[9]);
            
            DateTime? firstPaymentDate = FirstPaymentDate_Convert(columns[10]);
 
            
            float forwardCharge = ForwardCharge_Convert(columns[12]);
            
            int forwardSettlement = ForwardSettlement_Convert(columns[13]);
                        
            float fundingAmount = FundingAmount_Convert(columns[14]);
            
            int interestFreqId = InterestFrequencyID_Convert(columns[15]);
            
            float leaseResidual = LeaseResidual_Convert(columns[16]);
            
            int loanTermMonths = LoanTermMonths_Convert(columns[17]);
            
            DateTime? maturityDate = MaturityDate_Convert(columns[18]);
             
            float mmcof = MMCOF_Convert(columns[19]);

            string notes = Notes_Convert(columns[20]);

            string phone = Phone_Convert(columns[21]);
            
            float ppRiskPremium = PPRiskPremium_Convert(columns[22]);
            
            float ppRollOver = PPRollOver_Convert(columns[23]);
            
            int principalFreqId = PrincipalFrequencyID_Convert(columns[24]);
            
            DateTime? rePriceDate = RePriceDate_Convert(columns[25]);
            
            int requestorId = RequestorID_Convert(columns[26]);

            string approverInitials = ApproverInitials_Convert(columns[27]);
                                    
            string requestor = Requestor_Convert(columns[28]);
            
            float spread = Spread_Convert(columns[29]);
            
            int advanceTypeId = AdvanceTypeID_Convert(columns[30]);
            
            int currencyType = CurrencyType_Convert(columns[31]);
            
            int liquidityId = LiquidityID_Convert(columns[32]);
            
            string confirmationInitials = ConfirmationInitials_Convert(columns[33]);

            int indemnityAgreement = IndemnityAgreement_Convert(columns[34]);

            string lenderName = LenderName_Convert(columns[35]);

            int isCommunityInvestmentProgram = IsCommunityInvestmentProgram_Convert(columns[36]);
             
            int isPrePaymentWaiver = IsPrePaymentWaiver_Convert(columns[37]);

            int prePaymentTypeId = PrePaymentTypeID_Convert(columns[38]);
       
            int partialPrePaymentId = PartialPrePaymentID_Convert(columns[39]);
          
            int prePaymentWaiverAck = PrePaymentWaiverAcknowledgement_Convert(columns[40]);

            string requestorEmail = RequestorEmail_Convert(columns[41]);

            int lenderId = LenderID_Convert(columns[42]);
          
            int isCommercialMortgageEligible = IsCommercialMortgageEligible_Convert(columns[43]);
 
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

    public int LenderID_Convert(string value)
        => int.TryParse(value, out int result) ? result : 0;

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
    public int RequestorID_Convert(string value)
        => int.TryParse(value, out int result) ? result : 0;

    /// <summary>
    /// IndemnityAgreement (1 or 0)
    /// </summary>
    public int IndemnityAgreement_Convert(string value)
        => int.TryParse(value, out int result) ? result : 0;
}
