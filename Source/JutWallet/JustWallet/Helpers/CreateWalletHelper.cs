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
        
        
        public static bool CheckFilePath(string path)
        {
            return File.Exists(path);
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
                    safe = Safe.Create(out mnemonic, password, string.Format(@"{0}\{1}", fileDirectory, fileName), network);
                    shouldContinue = false;
                }
                catch(Exception)
                {
                    Console.WriteLine("File with that name already exist!\n");
                    
                }
            }
            
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
                while (true)
                {
                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    inputPassword += key.KeyChar;
                }                
                CommonWalletHelper.CheckForSpecialCommand(inputPassword.ToLower());
                shouldContinue = inputPassword.Length > 6;
            } while (shouldContinue);
            Console.WriteLine("Write password again to confirm: ");
            shouldContinue = true;
            do
            {
                while (true)
                {
                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    confirmPassword += key.KeyChar;
                }
                CommonWalletHelper.CheckForSpecialCommand(confirmPassword.ToLower());
                shouldContinue = !inputPassword.Equals(confirmPassword);
                if (shouldContinue)
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
        public static void PrintInformation(Safe safe, string filePath, Mnemonic mnemonic, string password)
        {
            Console.WriteLine("Wallet file location: {0}\n", filePath);
            Console.WriteLine("Password: {0}\n", password);
            Console.WriteLine("Please write down the mnemonic phrases!If you lose your .json file\nthe only way to recover it is mnemonic + password!");
            Console.WriteLine("Mnemonic: {0}\n", mnemonic);
            var address = CommonWalletHelper.GenerateBitcointAddress(safe);
            var privateKey = CommonWalletHelper.GetPrivateKey(safe, address);
            Console.WriteLine("IMPORTANT! This is your private key! Do not lose it\nand do not show it to anyone!If your private key\nis compromised, you will lose all of your bitcoins!\n");
            Console.WriteLine("PRIVATE KEY: {0}\n", privateKey);
            Console.WriteLine("This is your first bitcoin address! : {0}\n", address);
            Console.WriteLine("Network : {0}\n", safe.Network);
            var publicKey = safe.GetBitcoinExtPubKey();
            Console.WriteLine("This is your public key: {0}", publicKey);
        }
    }
}
