using System;
using System.IO;
using System.Management;
using System.Security.Cryptography;

namespace WeightCore.GameSystem.GameHelper
{
    public class CPUHelper
    {
        public static string GetCpuID()
        {
            try
            {
                string cpuInfo = ""; //cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }

                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch(Exception exception)
            {
                LoggerHelper.Debug(exception);
                return "unknow";
            }
            finally
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlainText"></param>
        /// <returns></returns>
        public static string Encrypt(string PlainText)
        {
            string KEY_64 = "dafei250";
            string IV_64 = "DAFEI500";
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst =
                new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cst);
            sw.Write(PlainText);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
    }
}