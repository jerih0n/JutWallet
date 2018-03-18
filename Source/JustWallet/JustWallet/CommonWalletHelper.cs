using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JustWallet
{
    public static class CommonWalletHelper
    {
        public static bool CheckFilePath(string path)
        {
            return File.Exists(path);
        }
    }
}
