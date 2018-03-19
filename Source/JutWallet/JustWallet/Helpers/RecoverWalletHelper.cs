using HBitcoin.KeyManagement;
using JustWallet.Helpers;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
namespace JutWallet.Helpers
{
    public static class RecoverWalletHelper
    {
        public static void LoadMnemonicFromInput(out Mnemonic mnemonic)
        {
            mnemonic = null;
            Console.WriteLine("IMPORTANT!The mnemonic phrases will be readed as they are entered. If you make\n changes of the mnemonic phrase the recovery will fail");
            var mnemonicAsString = Console.ReadLine();
            mnemonic = new Mnemonic(mnemonicAsString);
        }
        /// <summary>
        /// Get Entered Password and encryp with SHA256
        /// </summary>
        /// <param name="password"></param>
        public static void GetPassword(out string password)
        {
            password = string.Empty;
            string inputPassword = string.Empty;
            string confirmPassword = string.Empty;
            bool shouldContinue = true;
            do
            {
                Console.WriteLine("Plase Enter Password: ");
                while (true)
                {
                    var key = System.Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                        break;
                    inputPassword += key.KeyChar;
                }
                CommonWalletHelper.CheckForSpecialCommand(inputPassword.ToLower());
                shouldContinue = inputPassword.Length > 6;
                if(shouldContinue)
                {
                    Console.WriteLine("Incorect password. Passwords for this wallet are at least 6 symbols log");
                }
            } while (shouldContinue);
            
            password =  CryptoHelper.GetSHA256(inputPassword);
        }
        public static void GetSelectedWayOfMnemonicInput(out byte option)
        {
            option = byte.MaxValue;
            Console.WriteLine("Please select - 0 to load mnemonic from .txt file or 1 to enter as and imput in console");
            bool shouldContinue = true;
            do
            {
                var inputOptions = Console.ReadLine();
                try
                {
                    option = byte.Parse(inputOptions);
                    if(option >= 0 && option <= 1)
                    {
                        shouldContinue = false;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input!");
                }
                
            
            } while (shouldContinue);
        }
        public static void LoadMnemonicFromFile(out Mnemonic mnemonic)
        {
            mnemonic = null;
            bool shouldContinue = true;
            string fullTxtPath;
            string mnemonicAsStringInput = string.Empty;
            Console.WriteLine("IMPORTANT!The mnemonic phrases will be readed as they are in the file. If you make\n changes of the mnemonic phrase the recovery will fail");
            do
            {
                Console.WriteLine("\nPlease provide full file path to the .txt mnemonic file");
                fullTxtPath = Console.ReadLine();
                if(CommonWalletHelper.CheckFilePath(fullTxtPath) && Path.GetExtension(fullTxtPath) == ".txt")
                {
                    shouldContinue = false;
                    using(StreamReader reader = new StreamReader(fullTxtPath))
                    {
                        mnemonicAsStringInput = reader.ReadToEnd();
                        mnemonic = new Mnemonic(mnemonicAsStringInput);                        
                    }
                }
            }
            while (shouldContinue);
            

        }
        public static void TryRecoverWallet(string password, Mnemonic mnemonic, string filePath, Network network, out Safe safe)
        {
            safe = null;
            var guid = Guid.NewGuid();
            filePath += string.Format("\\wallet_{0}", guid); // the file name, using guid to avoid collitions
            safe = Safe.Recover(mnemonic, password, filePath, network);
        }
        public static void GetAddress(int index)
        {
           
        }
        public static void LoadFromExistingWalletFile(string sha256Password, string walletFile, out Safe safe)
        {
            Console.WriteLine(walletFile);
            safe = null;
            try
            {
                safe = Safe.Load(sha256Password, walletFile);
                Console.WriteLine();
                Console.WriteLine("Wallet Load Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL ERROR! Load FAILD!");
            }
        }

    }
}
