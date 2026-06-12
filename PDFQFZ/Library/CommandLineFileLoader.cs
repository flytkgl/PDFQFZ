using System;
using System.Collections.Generic;
using System.IO;

namespace PDFQFZ.Library
{
    internal static class CommandLineFileLoader
    {
        public static List<string> CollectExistingPdfFiles(IEnumerable<string> args)
        {
            List<string> files = new List<string>();
            if (args == null)
            {
                return files;
            }

            foreach (string arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }

                string fullPath = Path.GetFullPath(arg);
                if (!File.Exists(fullPath))
                {
                    continue;
                }

                if (!string.Equals(Path.GetExtension(fullPath), ".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                files.Add(fullPath);
            }

            return files;
        }
    }
}
