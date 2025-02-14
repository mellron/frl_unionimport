using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using frl_unionimport.util; // Importing AppConfiguration

namespace frl_unionimport
{   
    public class ImportStage
    {     
        public bool LoadFile(string fileName)
        {
            Validatefile _oValidator = new();

            string[] lines = [];

            if(_oValidator.LoadFile(fileName))
            {
                lines = _oValidator.FileData;

                foreach (var line in lines)
                {
                  Console.WriteLine(line);
                }
                
                return true;
            }
            else
            {
                return false;
            }
          
        }    
    }
}