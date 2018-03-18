
using HBitcoin.KeyManagement;
using JustWallet.Helpers;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
namespace JustWallet
{
    class JutWallet
    {
        private static List<string> _allowedCommands = new List<string> { "\\help", "\\create",  "\\recover", "\\balance", "\\history", "\\receive", "\\send", "\\exit", "\\clear - clear the console" };
        
        #region Main
        static void Main(string[] args)
        {
            Safe safe = null;
            FirstMessages();
            string input = string.Empty;
            while(true)
            {
                input = Console.ReadLine();
                if(input.ToLower() == "\\exit")
                {
                    Console.WriteLine("Exiting Wallet...");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
                }                
                switch(input.ToLower())
                {
                    case "\\help":
                        GetCommandsInfo();
                        break;
                    case "\\create":
                        CreateWallet(out safe);
                        break;     
                    default:
                        if(!CommonWalletHelper.CheckForSpecialCommand(input))
                        {
                            Console.WriteLine("Unknow Command!\n");
                        }
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
        private static void CreateWallet(out Safe safe)
        {
            Network network = null;
            safe = null;
            CreateWalletHelper.SelectNetwork(out network);
            string filePath = string.Empty;
            string password = string.Empty;
            bool shouldContinue = true;
            do
            {
                Console.WriteLine("Please enter valid folder file path where .json file should be saved\n");
                filePath = Console.ReadLine();
                CommonWalletHelper.CheckForSpecialCommand(filePath.ToLower());
                shouldContinue = !CommonWalletHelper.CheckDirectory(filePath);
            } while (shouldContinue);
            CreateWalletHelper.GetPassword(out password);            
            Mnemonic mnemonic;
            CreateWalletHelper.CreateJsonFile(password, filePath, network, out mnemonic, out safe);
            Console.WriteLine("File Successfuly Created!\nAfter 2 second full information will be presented\n");
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("Wallet file location: {0}\n",filePath);
            Console.WriteLine("Password: {0}\n", password);
            Console.WriteLine("Please write down the mnemonic phrases!\nIf you lose your .json file, the only way to recover it is mnemonic + password!\n");
            Console.WriteLine("Mnemonic {0}\n",mnemonic);
            var address = CommonWalletHelper.GenerateBitcointAddress(safe);
            var privateKey = CommonWalletHelper.GetPrivateKey(safe, address);
            Console.WriteLine("IMPORTANT! This is your private key! Do not lose it\nand do not show it to anyone!If your private key\nis compromised, you will lose all of your bitcoins!\n");
            Console.WriteLine("PRIVATE KEY: {0}\n",privateKey);
            Console.WriteLine("This is your first bitcoin address! : {0}\n", address);
            Console.WriteLine("Network : {0}\n",safe.Network);
            var publicKey = safe.GetBitcoinExtPubKey();
            Console.WriteLine("This is your public key: {0}",publicKey);
        }
        #endregion       
    }
}
