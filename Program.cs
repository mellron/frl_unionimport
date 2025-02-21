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
            
            Validatefile _validatefile = new Validatefile();

            if(_validatefile.LoadFile(inputfilename))
            {
                Console.WriteLine($"File {inputfilename} is valid.");

                BAL.FLR.InsertFixedRateLock(_validatefile.UnionBankDataRows[0]);
                
            }
            else
            {
                Console.WriteLine($"File {inputfilename} is not valid.");
                return;     
            }

        }
    }
}
