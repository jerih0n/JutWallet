﻿
using HBitcoin.KeyManagement;
using JustWallet.Helpers;
using JutWallet.Helpers;
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
        private static Safe _safe;
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
                    case "\\recover":
                        RecoverWallet(out safe);
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
            CommonWalletHelper.SelectNetwork(out network);
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
            _safe = safe;
            Thread.Sleep(2000);
            Console.Clear();
            CreateWalletHelper.PrintInformation(safe, filePath, mnemonic, password);
        }
        private static void RecoverWallet(out Safe safe)
        {
            safe = null;
            try
            {   
                
                Mnemonic mnemonic;
                string password;
                byte selectedWay;
                Console.WriteLine("IMPORTANT!: The wallet cannot check if the password you passed is correct", ConsoleColor.Red);
                Console.WriteLine("Recovery required valid password and mnemonic phrase");
                RecoverWalletHelper.GetPassword(out password);
                RecoverWalletHelper.GetSelectedWayOfMnemonicInput(out selectedWay);
                if (selectedWay == 0)
                {
                    RecoverWalletHelper.LoadMnemonicFromFile(out mnemonic);
                }
                else
                {
                    RecoverWalletHelper.LoadMnemonicFromInput(out mnemonic);
                }
                Network network;
                CommonWalletHelper.SelectNetwork(out network);
                string directoryPath;
                CommonWalletHelper.GetDirectoryPath(out directoryPath);
                RecoverWalletHelper.TryRecoverWallet(password, mnemonic, directoryPath, network, out safe);
                Console.WriteLine("Wallet successfuly recoverd!");
                _safe = safe;
            }
            catch(Exception)
            {
                Console.WriteLine("Recovery FAILED!");
            }

        }
        #endregion       
    }
}
