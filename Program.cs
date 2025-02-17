using System;
using System.IO;
using System.Collections.Generic;
using frl_unionimport.util; // Importing AppConfiguration
using frl_unionimport.models;

namespace frl_unionimport
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputfilename = AppConfiguration.GetPropertyValue("inputfile");

            List<UnionBankData> records = LoadFile.ReadCsv(inputfilename);

            Console.WriteLine($"Loaded {records.Count} records from {inputfilename}");

            // _oImportStage.LoadFile(inputfilename);
        }
    }
}
