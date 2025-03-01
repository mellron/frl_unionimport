using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using frl_unionimport.models;

namespace frl_unionimport
{
    public static class LoadFile
    {        
        public static List<UnionBankData> ReadCsv(string sfilename)
        {
            List<UnionBankData> records = new List<UnionBankData>();
            string[] lines = File.ReadAllLines(sfilename);

            if (lines.Length <= 1)
            {
                return records;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] fields = line.Split(',');

                UnionBankData data = new UnionBankData
                {
                    ReferenceAlpha = fields[0].Trim(),
                    AccrualID = getAccrualID(fields[1].Trim()),
                    AcctSystemID = getAcctSystemID(fields[2].Trim()),
                    AllInRate = decimal.TryParse(fields[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal allInRate) ? allInRate : (decimal?)null,
                    AmortTerm = int.TryParse(fields[4].Trim(), out int amortTerm) ? amortTerm : (int?)null,
                    AmortTypeID = int.TryParse(fields[5].Trim(), out int amortTypeId) ? amortTypeId : (int?)null,
                    MaturityDate = DateTime.TryParse(fields[6].Trim(), out DateTime maturityDate) ? maturityDate : (DateTime?)null,
                    CostCenter = fields[7].Trim(),
                    CustomerName = fields[8].Trim(),
                    DivisionID = int.TryParse(fields[9].Trim(), out int divisionId) ? divisionId : (int?)null,
                    FirstPmtDt = DateTime.TryParse(fields[10].Trim(), out DateTime firstPmtDt) ? firstPmtDt : (DateTime?)null,
                    FixedTerm = int.TryParse(fields[11].Trim(), out int fixedTerm) ? fixedTerm : (int?)null,
                    ForwardCharge = fields[12].Trim(),
                    ForwardSettlement = int.TryParse(fields[13].Trim(), out int forwardSettlement) ? forwardSettlement : (int?)null,
                    FundingAmt = decimal.TryParse(fields[14].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal fundingAmt) ? fundingAmt : (decimal?)null,
                    InterestFreqID = int.TryParse(fields[15].Trim(), out int interestFreqId) ? interestFreqId : (int?)null,
                    LeaseResidual = decimal.TryParse(fields[16].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal leaseResidual) ? leaseResidual : (decimal?)null,
                    LoanTerm = int.TryParse(fields[17].Trim(), out int loanTerm) ? loanTerm : (int?)null,
                    MMCOF = fields[18].Trim(),
                    Notes = fields[19].Trim(),
                    Phone = fields[20].Trim(),
                    PPRiskPremium = decimal.TryParse(fields[21].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ppRiskPremium) ? ppRiskPremium : (decimal?)null,
                    PPRollOver = decimal.TryParse(fields[22].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal ppRollOver) ? ppRollOver : (decimal?)null,
                    PrincipalFreqID = int.TryParse(fields[23].Trim(), out int principalFreqId) ? principalFreqId : (int?)null,
                    DSILease = fields[24].Trim(),
                    RePriceDt = DateTime.TryParse(fields[25].Trim(), out DateTime rePriceDt) ? rePriceDt : (DateTime?)null,
                    RequestorID = fields[26].Trim(),
                    Approver = fields[27].Trim(),
                    Originator = fields[28].Trim(),
                    Spread = decimal.TryParse(fields[29].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal spread) ? spread : (decimal?)null,
                    AdvanceTypeID = int.TryParse(fields[30].Trim(), out int advanceTypeId) ? advanceTypeId : (int?)null,
                    CurrencyType = int.TryParse(fields[31].Trim(), out int currencyType) ? currencyType : (int?)null,
                    LiquidityID = int.TryParse(fields[32].Trim(), out int liquidityId) ? liquidityId : (int?)null,
                    ConfirmationInitials = fields[33].Trim(),
                    IndemnityAgreement = int.TryParse(fields[34].Trim(), out int indemnityAgreement) ? indemnityAgreement : (int?)null,
                    LenderName = fields[35].Trim(),
                    CIP = bool.TryParse(fields[36].Trim(), out bool cip) ? cip : false,
                    PrePaymentWaiver = bool.TryParse(fields[37].Trim(), out bool prePaymentWaiver) ? prePaymentWaiver : false,
                    PrePaymentTypeID = int.TryParse(fields[38].Trim(), out int prePaymentTypeId) ? prePaymentTypeId : (int?)null,
                    PartialPrePayID = int.TryParse(fields[39].Trim(), out int partialPrePayId) ? partialPrePayId : (int?)null,
                    PrePaymentWaiverAcknowledgement = bool.TryParse(fields[40].Trim(), out bool prePaymentWaiverAcknowledgement) ? prePaymentWaiverAcknowledgement : false,
                    RequestorEmail = fields[41].Trim(),
                    LenderID = fields[42].Trim(),
                    IsCommercialMortgageEligible = bool.TryParse(fields[43].Trim(), out bool isCommercialMortgageEligible) ? isCommercialMortgageEligible : false
                };
                records.Add(data);
            }
            return records;

        }
        public static int getAccrualID(string value)
        {
         
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            string input = value.Trim().ToLower();

            if (input.StartsWith("act/act"))
                return 3;  // Act/Act (Govt or Syndication ONLY)
            else if (input.StartsWith("act/360"))
                return 1;  // Act/360m
            else if (input.StartsWith("30/360"))
                return 2;  // 30/360 (Lease Only)
            else
                return 0;
        }

        public static int getAcctSystemID(string value)
        {
         
            if (string.IsNullOrWhiteSpace(value))
                return 0;

        
            string input = value.Trim().ToLower();

            switch (input)
            {
                case "afs":
                    return 1;
                case "cml":
                    return 2;
                case "cpi":
                    return 3;
                case "dsi":
                    return 4;
                case "other lease":
                    return 5;
                case "als":
                    return 6;
                case "loaniq":
                    return 7;
                default:
                    return 0;
            }
        }



    }
}
