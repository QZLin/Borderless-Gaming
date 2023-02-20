using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using BorderlessGaming.Logic.Models;
using BorderlessGaming.Logic.System;
using ProtoBuf;

namespace BorderlessGaming.Logic.Windows
{
    public static class Security
    {
        private static readonly byte[] Salt = { 0x33, 0x92, 0x91, 0x12, 0x28, 0x19 };

        public static byte[] Encrypt(byte[] plainText)
        {
            return ProtectedData.Protect(plainText, Salt, DataProtectionScope.CurrentUser);
        }

        public static byte[] Decrypt(byte[] cipher)
        {
            return ProtectedData.Unprotect(cipher, Salt, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        ///     Encrypts the config file, I've seen a trend of people mining the Borderless Gaming favorites list for heuristics.
        /// </summary>
        /// <param name="instance"></param>
        public static void SaveConfig(Config instance)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(instance, options);
            File.WriteAllText(AppEnvironment.ConfigPath, jsonString);
        }


        public static Config LoadConfigFile()
        {
            try
            {
                var text = File.ReadAllText(AppEnvironment.ConfigPath);
                var config = JsonSerializer.Deserialize<Config>(text);
                return config;
            }
            catch (global::System.Exception)
            {
                File.Delete(AppEnvironment.ConfigPath);
                SaveConfig(new Config());
                return LoadConfigFile();
            }
        }
    }
}