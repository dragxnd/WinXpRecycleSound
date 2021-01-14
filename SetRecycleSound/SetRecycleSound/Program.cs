using Microsoft.Win32;
using System;
using System.IO;

namespace SetRecycleSound
{
    class Program
    {

        static void ErrorMessage(string message)
        {
            Console.WriteLine($"> {message} > [ERROR]");
            Console.WriteLine($"> Please try again");
            Console.ReadLine();
            Environment.Exit(0);
        }

        static void OkMessage(string message)
        {
            Console.WriteLine($"> {message} > [OK]");
        }

        static void EndMessage()
        {
            Console.WriteLine($"> Complete, press any key");
        }

        static void Main(string[] args)
        {

            string WinMediaPath = Environment.GetEnvironmentVariable("windir") + "\\media\\";
            string SoundFileName = "WinRecycleBin.wav";

            try
            {
                File.WriteAllBytes(WinMediaPath + SoundFileName, Properties.Resources.WinRecycleBin);
                OkMessage("Write sound file");
            }
            catch
            {
                ErrorMessage("Error when write sound file. Try run as admin");
            }

            SetRegistryValue("AppEvents\\Schemes\\Apps\\Explorer\\EmptyRecycleBin\\.Current", RegistryValueKind.String, WinMediaPath + SoundFileName);
            SetRegistryValue("AppEvents\\Schemes\\Apps\\Explorer\\EmptyRecycleBin\\.Default", RegistryValueKind.String, WinMediaPath + SoundFileName);
            SetRegistryValue("AppEvents\\Schemes\\Apps\\Explorer\\EmptyRecycleBin\\.Modified", RegistryValueKind.String, WinMediaPath + SoundFileName);

            EndMessage();
            Console.ReadLine();
        }

        public static void SetRegistryValue(string path, RegistryValueKind registryValueType, string valueString)
        {
            using (RegistryKey myKey = Registry.CurrentUser.OpenSubKey(path, true))
            {
                if (myKey != null)
                {
                    myKey.SetValue("", valueString, registryValueType);
                    myKey.Close();
                    OkMessage(path);
                }
                else
                {
                    ErrorMessage("Registry key is null");
                }
            }
        }

    }
}
