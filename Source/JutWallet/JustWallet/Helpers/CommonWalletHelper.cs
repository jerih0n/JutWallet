using HBitcoin.KeyManagement;
using JutWallet.Classes;
using NBitcoin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace JustWallet.Helpers
{
    public static class CommonWalletHelper
    {
        private static string _walletConfigFileName = "jut_config.json";
        public static bool CheckFilePath(string path)
        {
            return File.Exists(path);
        }
        public static bool CheckDirectory(string path)
        {
            return Directory.Exists(path);
        }
        public static bool CheckForSpecialCommand(string command)
        {
            switch(command)
            {
                case "\\clear":
                    Console.Clear();
                    return true;
                default:
                    return false;
            }
        }
        public static BitcoinAddress GenerateBitcointAddress(Safe safe, int addressIndex=0)
        {
            return safe.GetAddress(0);
        }
        public static BitcoinExtKey GetPrivateKey(Safe safe, BitcoinAddress address)
        {
            return safe.FindPrivateKey(address);
        }
        public static void SelectNetwork(out Network network)
        {
            network = null;
            bool shouldContinue = true;
            Console.WriteLine();
            string bitcoinNetworkInput;
            while (shouldContinue)
            {
                Console.WriteLine("Please enter 0 for test bitcoin network and 1 for real network\n");
                bitcoinNetworkInput = Console.ReadLine();
                CommonWalletHelper.CheckForSpecialCommand(bitcoinNetworkInput.ToLower());
                byte selectedNetwordId = byte.MaxValue;
                if (bitcoinNetworkInput.ToLower() == "\\exit")
                {
                    Console.WriteLine("Exiting Wallet...");
                    Thread.Sleep(1000);
                    break;
                }
                try
                {
                    selectedNetwordId = byte.Parse(bitcoinNetworkInput);
                    switch (selectedNetwordId)
                    {
                        case 0:
                            network = Network.TestNet;
                            shouldContinue = false;
                            Console.WriteLine("You successfuly selected TEST bitcoin network\n", ConsoleColor.DarkGreen);
                            break;
                        case 1:
                            network = Network.Main;
                            Console.WriteLine("You successfuly selected MAIN bitcoin network. Using Main network is at your own risk!\n", ConsoleColor.DarkRed);
                            shouldContinue = false;
                            break;
                        default:
                            Console.WriteLine("Invalid Input!");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input format format!\n");
                }
            }
        }
        public static void GetDirectoryPath(out string directoryPath)
        {
            directoryPath = string.Empty;
            bool shouldContinue = true;
            do
            {
                Console.WriteLine("Please enter valid folder file path where .json file should be saved\n");
                directoryPath = Console.ReadLine();
                CheckForSpecialCommand(directoryPath.ToLower());
                shouldContinue = !CheckDirectory(directoryPath);
            } while (shouldContinue);
        }
        public static bool TryToOpenWalletConfig(out WalletConfig configInfo)
        {
            configInfo = null;
            if (File.Exists(_walletConfigFileName))
            {
                try
                {
                    using (var reader = new StreamReader(_walletConfigFileName))
                    {
                        configInfo = JsonConvert.DeserializeObject<WalletConfig>(reader.ReadToEnd());
                        if(configInfo.WalletLastAddressIndexUsed == null)
                        {
                            configInfo.WalletLastAddressIndexUsed = new Dictionary<string, int>();
                        }
                        return true;
                    }
                }
                catch(Exception)
                {
                    return false;
                }
                
            }
            return false; 
        }
        public static void CreateWalletConfigFile()
        {
            using(var writter = new StreamWriter(_walletConfigFileName))
            {
                var newWallet = new WalletConfig()
                {
                    WalletLastAddressIndexUsed = new Dictionary<string, int>()
                };
                writter.WriteLine(JsonConvert.SerializeObject(newWallet));
            }
        }
        public static void AddNewRecordInWalletConfig(WalletConfig config, string sha256PublicKey)
        { 
            if (IsWalletSavedInConfig(config, sha256PublicKey))
            {
                config.WalletLastAddressIndexUsed[sha256PublicKey] += 1;
            }
            else
            {
                config.WalletLastAddressIndexUsed.Add(sha256PublicKey, 1);
            }
            TryUpdateConfig(config);
        }
        public static void TryUpdateConfig(WalletConfig config)
        {
            if (File.Exists(_walletConfigFileName))
            {
                File.Delete(_walletConfigFileName);
                using (var writter = new StreamWriter(_walletConfigFileName))
                {
                    writter.WriteLine(JsonConvert.SerializeObject(config));
                }
            }
        }
        public static bool IsWalletSavedInConfig(WalletConfig config,string sha256PublicKey)
        {
            return config.WalletLastAddressIndexUsed.ContainsKey(sha256PublicKey);
        }
        

    }
    
}
