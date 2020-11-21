using System.Collections.Generic;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SpamCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = string.Empty;
            if (args.Length !=  2 )
            {
                Console.WriteLine("Must enter arguments : spam <root directory> <number of mails to send>");
            }
            else
            {
                CLIEngine cliSpammer = new CLIEngine(args[0], "log.txt", "Senders.txt", "Recivers.txt","Headers.txt",int.Parse(args[1]));
                Console.WriteLine("Starting spammer....");
                cliSpammer.Run();
            }
            Console.WriteLine("Press any key to exit.... ");
            Console.ReadLine();
        }
    }


}

