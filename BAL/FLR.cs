using System;
using System.Data;
using System.Data.SqlClient;
using Utility.Database.MSSQL; // Or your actual namespace for CreateConnection
using frl_unionimport.DAL;
using frl_unionimport.models; // Assuming this is where your UnionBankData class is located


namespace frl_unionimport.BAL
{
    public static class FLR
    {
        public static bool InsertFixedRateLock(UnionBankData unionBankData)
        {
            return DAL.FLR.InsertFixedRateLock(unionBankData);
        }
    }
}