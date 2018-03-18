using HBitcoin.KeyManagement;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace JustWallet.Helpers
{
    public static class CreateWalletHelper
    {
        
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
        public static bool CheckFilePath(string path)
        {
            return File.Exists(path);
        }
        public static void GetPassword(out string password)
        {
            password = string.Empty;
            string inputPassword = string.Empty;
            string confirmPassword = string.Empty;
            bool shouldContinue = true;
            do
            {
                Console.WriteLine("Plase Enter Password: Password must be at least 6 symbols long .Do not forget the password!\n");
                inputPassword = Console.ReadLine();
                CommonWalletHelper.CheckForSpecialCommand(inputPassword.ToLower());
                shouldContinue = inputPassword.Length > 6;
            } while (shouldContinue);
            Console.WriteLine("Write password again to confirm: ");
            shouldContinue = true;
            do
            {
                confirmPassword = Console.ReadLine();               
                CommonWalletHelper.CheckForSpecialCommand(confirmPassword.ToLower());
                shouldContinue = !inputPassword.Equals(confirmPassword);
                if(shouldContinue)
                {
                    Console.WriteLine("Password does not match !\n");
                }
                else
                {
                    Console.WriteLine("Password set!Do not forget it !\n Console will be cleared after 3 seconds");
                    Thread.Sleep(3000);
                    Console.Clear();
                }
            } while (shouldContinue);
            password = inputPassword;
        }
        public static void CreateJsonFile(string password,string fileDirectory, Network network, out Mnemonic mnemonic, out Safe safe)
        {
            mnemonic = null;
            safe = null;
            bool shouldContinue = true;
            string fileName = string.Empty;
            while(shouldContinue)
            {
                Console.WriteLine("Please Enter .json file name:\n");
                try
                {
                    fileName = Console.ReadLine();
                    safe = Safe.Create(out mnemonic, password, string.Format("{0}{1}", fileDirectory, fileName), network);
                    shouldContinue = false;
                }
                catch(Exception)
                {
                    Console.WriteLine("File with that name already exist!\n");
                    
                }
            }
            
        }
    }
}
