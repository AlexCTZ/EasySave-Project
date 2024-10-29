using System;
using System.Collections.Generic;
using System.Text;

namespace test_projet
{
    using System.Security.Cryptography;
    using System.IO;
    public class ChiffrementXOR
    {

        public static byte[] LoadXorKey(string keyFilePath)
        {
            try
            {
                return File.ReadAllBytes(keyFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XOR key: {ex.Message}");
                Environment.Exit(1);
                return null; // This line is not actually reachable, but required for compilation
            }
        }

        public static void EncryptFile(string filePath, byte[] xorKey)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);

                for (int i = 0; i < fileBytes.Length; i++)
                {
                    fileBytes[i] ^= xorKey[i % xorKey.Length];
                }

                File.WriteAllBytes(filePath, fileBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encrypting file {filePath}: {ex.Message}");
            }
        }
    }
}
