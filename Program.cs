using System;
using System.IO;

namespace frl_unionimport
{
    class Program
    {
        private static util.ConsoleQ _consoleQ = new();
        static async Task Main(string[] args)
        {
            
            await _consoleQ.WaitForCompletionAsync();
        }
    }
}


