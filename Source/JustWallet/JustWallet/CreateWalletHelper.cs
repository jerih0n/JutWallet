using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace JustWallet
{
    public static class CreateWalletHelper
    {
        private static string _passWordRegex = @"^(?=.*[A - Za - z])(?=.*\d)[A - Za - z\d]{8,}$"; // at least 8 lenght, at least 1 digit and 1 char
        public static void SelectNetwork(out Network network)
        {
            network = null;
            bool shouldContinue = true;
            Console.WriteLine();
            string bitcoinNetworkInput;
            while (shouldContinue)
            {
                Console.WriteLine("Please enter 0 for test bitcoin network and 1 for real network");
                bitcoinNetworkInput = Console.ReadLine();
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
                            Console.WriteLine("You successfuly selected MAIN bitcoin network", ConsoleColor.DarkGreen);
                            break;
                        case 1:
                            network = Network.Main;
                            Console.WriteLine("You successfuly selected MAIN bitcoin network. Using Main network is at your own risk!", ConsoleColor.DarkRed);
                            shouldContinue = false;
                            break;
                        default:
                            Console.WriteLine("Invalid Input!");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input format format!");
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
            Console.WriteLine("Plase Enter Password: Password must be at least 8 symbols long and to contain \nat least 1 digit and 1 letter.Do not forget the password!");
            do
            {
                inputPassword = Console.ReadLine();
                Console.WriteLine("Plase Enter Password: Password must be at least 8 symbols long and to contain \nat least 1 digit and 1 letter.Do not forget the password!");
            } while (Regex.IsMatch(inputPassword, _passWordRegex));
            Console.WriteLine("Write password again to confirm: ");
            do
            {
                confirmPassword = Console.ReadLine();
                Console.WriteLine("Plase Enter Password: Password must be at least 8 symbols long and to contain \nat least 1 digit and 1 letter.Do not forget the password!");

            } while (inputPassword.Equals(confirmPassword));
            password = inputPassword;
        }
    }
}
