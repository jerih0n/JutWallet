
using HBitcoin.KeyManagement;
using JustWallet.Helpers;
using JutWallet.Classes;
using JutWallet.Helpers;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Threading;
namespace JustWallet
{
    class JutWallet
    {
        private static List<string> _allowedCommands = new List<string> { "\\help", "\\create", "\\get-address", "\\load","\\balance", "\\history", "\\receive", "\\send", "\\exit", "\\clear - clear the console" };
        private static Safe _safe;
        private static WalletConfig _config;
        #region Main
        static void Main(string[] args)
        {
            var a = Console.ReadLine();
            var ss = CryptoHelper.GetSHA256(a);
            Console.WriteLine(ss);
            GetConfig(out _config);
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
                    case "\\get-address":
                        break;
                    case "\\load":
                        LoadWallet(out safe);
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
        private static void GetConfig(out WalletConfig config)
        {
            config = null;
            if(!CommonWalletHelper.TryToOpenWalletConfig(out config))
            {
                CommonWalletHelper.CreateWalletConfigFile();
                config = new WalletConfig();
            }

        }
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
            var sha256PublicKey = CryptoHelper.GetSHA256(safe.GetBitcoinExtPubKey().ToString());
            CommonWalletHelper.AddNewRecordInWalletConfig(_config, sha256PublicKey);
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
                var sha256Public = CryptoHelper.GetSHA256(safe.GetBitcoinExtPubKey().ToString());
                if(!CommonWalletHelper.IsWalletSavedInConfig(_config, sha256Public))
                {
                    CommonWalletHelper.AddNewRecordInWalletConfig(_config, sha256Public);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Recovery FAILED!");
            }

        }
        public static void LoadWallet(out Safe safe)
        {
            safe = null;
            string path;
            bool @continue = true;
            string password;
            Console.WriteLine();
            RecoverWalletHelper.GetPassword(out password);
            Console.WriteLine(password);
            do
            {
                Console.WriteLine("Please enter the full file path of the wallet json file:");
                 path = Console.ReadLine();
                @continue = !CommonWalletHelper.CheckDirectory(path);
            } while (@continue);
            Console.WriteLine("Please enter the name of the wallet: ");
            var name = Console.ReadLine();
            RecoverWalletHelper.LoadFromExistingWalletFile(password, string.Format("{0}\\{1}", path, name), out safe);
            _safe = safe;           
        }
        private static void GetAddress()
        {
           
        }
        
        #endregion       
    }
}
