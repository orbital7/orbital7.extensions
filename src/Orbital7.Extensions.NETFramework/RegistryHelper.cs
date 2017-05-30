using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace Orbital7.Extensions.NETFramework
{
    public static class RegistryHelper
    {
        public static string GetLocalMachineRegistryValue(string root, string path, string name)
        {
            return GetRegistryKeyValue(GetLocalMachineRegistryKey(root, path), name);
        }

        public static RegistryKey GetLocalMachineRegistryKey(string root, string path)
        {
            return GetRegistryKey(Registry.LocalMachine, root, path);
        }

        public static string GetCurrentUserRegistryValue(string root, string path, string name)
        {
            return GetRegistryKeyValue(GetCurrentUserRegistryKey(root, path), name);
        }

        public static RegistryKey GetCurrentUserRegistryKey(string root, string path)
        {
            return GetRegistryKey(Registry.CurrentUser, root, path);
        }

        private static RegistryKey GetRegistryKey(RegistryKey home, string root, string path)
        {
            RegistryKey registryKey = home.OpenSubKey(Path.Combine(root, path));
            if (registryKey == null) registryKey = home.OpenSubKey(Path.Combine(root, "Wow6432Node", path));

            return registryKey;
        }

        private static string GetRegistryKeyValue(RegistryKey registryKey, string name)
        {
            string value = String.Empty;

            if (registryKey != null)
            {
                object oValue = registryKey.GetValue(name);
                if (oValue != null) value = oValue.ToString();
            }

            return value;
        }
    }
}
