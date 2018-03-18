using HBitcoin.KeyManagement;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JustWallet.Helpers
{
    public static class CommonWalletHelper
    {
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
    }
    
}
