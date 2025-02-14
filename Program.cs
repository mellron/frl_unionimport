using System;
using System.IO;
using frl_unionimport.util; // Importing AppConfiguration

namespace frl_unionimport
{
    class Program
    {


        static void Main(string[] args)
        {
            ImportStage _oImportStage = new();

            string inputfilename = AppConfiguration.GetPropertyValue("inputfile");

            _oImportStage.LoadFile(inputfilename);
            
        }
    }
}


