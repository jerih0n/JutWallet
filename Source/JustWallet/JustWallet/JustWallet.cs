
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
namespace JustWallet
{
    class JustWallet
    {
        private static List<string> _allowedCommands = new List<string> { "\\help", "\\create",  "\\recover", "\\balance", "\\history", "\\receive", "\\send", "\\exit" };
        
        #region Main
        static void Main(string[] args)
        {
            FirstMessages();
            string input = string.Empty;
            while(true)
            {
                input = Console.ReadLine();
                if(input.ToLower() == "\\exit")
                {
                    Console.WriteLine("Exiting Wallet...");
                    Thread.Sleep(1000);
                    break;
                }
                switch(input.ToLower())
                {
                    case "\\help":
                        GetCommandsInfo();
                        break;
                    case "\\create":
                        CreateWallet();
                        break;
                    default:
                        Console.WriteLine("Unknow Command!\n");
                        break;
                }
            }

        }
        #endregion

        #region Main Methods
        private static void FirstMessages()
        {
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("This is free open source BitCoin wallet. Please back up the .json file of the wallet or your bitcoins will be lost forever!");
            Console.WriteLine();
            Console.WriteLine("The creator of this wallet, does not take ANY responsibility in cases you lost your money and recomend you to use different wallet that this if you trade with real bitcoins");
            Console.WriteLine();
            Console.WriteLine("Enter \\help to get available commands");
            Console.WriteLine();
        }
        private static void GetCommandsInfo()
        {
            Console.WriteLine("Commands: \n");
            Console.WriteLine(string.Join(" ", _allowedCommands));
            Console.WriteLine();
        }
        private static void CreateWallet()
        {
            Network network = null;
            CreateWalletHelper.SelectNetwork(out network);
            string filePath = string.Empty;
            string password = string.Empty;
            do
            {
                Console.WriteLine("Please enter valid folder file path where .json file should be saved");
                filePath = Console.ReadLine();

            } while (!CommonWalletHelper.CheckFilePath(filePath));
            CreateWalletHelper.GetPassword(out password);
        }
        #endregion       
    }
}
